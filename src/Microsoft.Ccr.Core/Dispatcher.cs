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
		readonly QueueMediator mediator;
		Thread thread;

		internal CcrWorker (QueueMediator mediator)
		{
			this.mediator = mediator;
		}

		internal void Start ()
		{
			thread = new Thread (this.Run);
			thread.Start ();
		}

		[MonoTODO ("support nested iterators")]
		void RunTask (ITask task)
		{
			var obj = task.LinkedIterator;
			var iter = task.Execute ();
			if (obj != null && iter != null)
				Console.WriteLine ("FIX ME PLEASE as I have a nested iterator");

			if (iter != null) {
				IteratorData id = new IteratorData (iter);
				id.Begin (mediator.queue);
			}
			if (obj != null)
				((IteratorData)obj).Step (task, mediator.queue);
		}

		void Run ()
		{
			Dispatcher disp = mediator.dispatcher;
			while (disp.IsAlive) {
				ITask task = null;
				try {
					task = mediator.Dequeue ();
				} catch (Exception e) { //DispatcherQueue is failing, let's with
					//TODO do something with the exception
					Thread.Sleep (500);
				}
				try {
					if (task == null)
						continue;
					RunTask (task);
				} catch (Exception e) {
					Console.WriteLine ("please fix me {0}", e);
					//TODO post it somewhere
				}
			}
		}
	}

	class QueueMediator
	{
		readonly object _lock = new object ();
		internal readonly DispatcherQueue queue;
		readonly List<CcrWorker> worker = new List<CcrWorker> ();
		volatile bool active = true;
		readonly internal Dispatcher dispatcher;
		int pendingTasks;

		const int MAX_WORKERS = 4;

		internal QueueMediator (Dispatcher dispatcher, DispatcherQueue queue)
		{
			this.queue = queue;
			this.dispatcher = dispatcher;
		}

		internal ITask Test ()
		{
			return null;
		}
		
		internal void SpawnWorker ()
		{
			lock (_lock) {
				var w = new CcrWorker (this);
				worker.Add (w);
				w.Start ();
			}
		}


		internal void Notify ()
		{
			lock (_lock) {
				if (worker.Count == 0)
					SpawnWorker ();
				if (pendingTasks > 0 && worker.Count < MAX_WORKERS)
					SpawnWorker ();
				++pendingTasks;
				Monitor.Pulse (_lock);
			}
		}

		internal ITask Dequeue ()
		{
			lock (_lock) {
				ITask task = null;
				while (active && !queue.TryDequeue (out task))
					Monitor.Wait (_lock);
				--pendingTasks;
				return task;
			}
		}

		internal void Shutdown ()
		{
			active = false;	
			lock (_lock) {
				Monitor.PulseAll (_lock);
			}	
		}
		
	}

	public sealed class Dispatcher : IDisposable
	{
		object _lock = new object ();
		List <QueueMediator> queues = new List <QueueMediator> ();
		bool isDisposed;

		public Dispatcher ()
		{
		}

		internal bool IsAlive { get { return !isDisposed; } }

		internal void Notify (DispatcherQueue queue)
		{
			((QueueMediator)queue.DispatcherObject).Notify ();
		}

		internal void Register (DispatcherQueue queue)
		{
			lock (_lock) {
				QueueMediator qm = new QueueMediator (this, queue);
				queue.DispatcherObject = qm;
				queues.Add (qm);
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
				foreach (var qm in queues)
					qm.Shutdown ();
			}
			isDisposed = true;
			if (disposing)
				GC.SuppressFinalize (this);
		}
	}
}
