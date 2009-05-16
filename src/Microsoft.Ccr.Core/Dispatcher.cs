//
// Dispatcher.cs
//
// Author:
//   Rodrigo Kumpera  <kumpera@gmail.com>
//
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Ccr.Core
{
	class IteratorData
	{
		IEnumerator<ITask> iterator;

		internal IteratorData (IEnumerator<ITask> iterator)
		{
			this.iterator = iterator;
		}

		internal void Begin (DispatcherQueue queue)
		{
			try {
				if (iterator.MoveNext ()) {
					ITask task = iterator.Current;
					task.LinkedIterator = this;
					task.TaskQueue = queue;
					task.Execute ();
				}
			} catch (Exception ex) {
				this.iterator.Dispose ();
				this.iterator = null;
				Console.WriteLine (ex);
				//TODO post it somewhere
			}
		}

		internal void Step (ITask task, DispatcherQueue queue)
		{
			try {
				if (iterator.MoveNext ()) {
					task = iterator.Current;
					task.LinkedIterator = this;
					task.TaskQueue = queue;
					task.Execute ();
				}
			}  catch (Exception ex) {
				this.iterator.Dispose ();
				this.iterator = null;
				Console.WriteLine (ex);
				//TODO post it somewhere
			}
		}
	}

	class CcrWorker
	{
		Thread thread;
		Dispatcher dispatcher;
		int currentQueue;

		internal CcrWorker (Dispatcher dispatcher, int currentQueue)
		{
			this.dispatcher = dispatcher;
			this.currentQueue = currentQueue;
		}

		internal void Start ()
		{
			thread = new Thread (this.Run);
			thread.Start ();
		}

		[MonoTODO ("support nested iterators")]
		void RunTask (ITask task, DispatcherQueue queue)
		{
			var obj = task.LinkedIterator;
			var iter = task.Execute ();
			if (obj != null && iter != null)
				Console.WriteLine ("FIX ME PLEASE as I have a nested iterator");

			if (iter != null) {
				IteratorData id = new IteratorData (iter);
				id.Begin (queue);
			}
			if (obj != null)
				((IteratorData)obj).Step (task, queue);
		}

		void Run ()
		{
			while (dispatcher.active) {
				ITask task = null;
				DispatcherQueue queue = null;
				try {
					task = dispatcher.Dequeue (ref currentQueue, out queue);
				} catch (Exception e) { //DispatcherQueue is failing, what should we do?
					//TODO do something with the exception
					Thread.Sleep (500);
				}
				try {
					if (task == null)
						continue;
					RunTask (task, queue);
				} catch (Exception e) {
					Console.WriteLine ("please fix me {0}", e);
					//TODO post it somewhere
				}
			}
		}
	}

	public sealed class Dispatcher : IDisposable
	{
		object _lock = new object ();
		DispatcherQueue[] queues = new DispatcherQueue [0];
		readonly List<CcrWorker> workers = new List<CcrWorker> ();

		volatile int pendingTasks;
		volatile int pendingWorkers;
		internal bool active = true;
		bool isDisposed;
		int maxThreads = 10;

		public Dispatcher ()
		{
		}

		internal void SpawnWorker ()
		{
			lock (_lock) {
				if (workers.Count >= maxThreads)
					return;
				var w = new CcrWorker (this, queues.Length == 0 ? 0 : workers.Count % queues.Length);
				workers.Add (w);
				w.Start ();
			}
		}

		internal ITask Dequeue (ref int start, out DispatcherQueue queue)
		{
			DispatcherQueue[] queues = this.queues;
			ITask task  = null;
			int qlen = queues.Length;

			while (active) {
				for (int i = 0; i < qlen; ++i) {
					int idx = (i + start) % qlen;
					if (queues [idx].TryDequeue (out task)) {
						queue = queues [idx];
						start = (idx + 1) % qlen;
						Interlocked.Decrement (ref pendingTasks);
						return task;
					}
				}
	
				lock (_lock) {
					for (int i = 0; i < qlen; ++i) {
						int idx = (i + start) % qlen;
						if (queues [idx].TryDequeue (out task)) {
							queue = queues [idx];
							start = (idx + 1) % qlen;
							Interlocked.Decrement (ref pendingTasks);
							return task;
						}
					}
					Interlocked.Increment (ref pendingWorkers);
					Monitor.Wait (_lock);
					Interlocked.Decrement (ref pendingWorkers);
				}
			}
			queue = null;
			return null;
		}

		internal void Notify (DispatcherQueue queue)
		{
			int curPending = Interlocked.Increment (ref pendingTasks);

			if (workers.Count == 0 || (curPending > 0 && workers.Count < maxThreads)) {
				SpawnWorker ();
				return;
			}

			lock (_lock) {
				if (pendingWorkers != 0)
					Monitor.Pulse (_lock);
			}
		}

		internal void Register (DispatcherQueue queue)
		{
			lock (_lock) {
				DispatcherQueue[] res = new DispatcherQueue [queues.Length + 1];
				queues.CopyTo (res, 0);
				res [queues.Length] = queue;
				queues = res;
			}
		}

		~Dispatcher ()
		{
			Dispose (false);
		}

		[MonoTODO ("Implement proper dispose semantics and checks")]
		public void Dispose ()
		{
			Dispose (true);
		}

		void Dispose (bool disposing)
		{
			if (isDisposed)
				return;
			lock (_lock) {
				active = false;
				Monitor.PulseAll (_lock);
				foreach (var qm in queues)
					qm.Dispose ();
			}
			isDisposed = true;
			if (disposing)
				GC.SuppressFinalize (this);
		}
	}
}
