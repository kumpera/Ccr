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
			thread.Name = String.Format ("{0} ThreadPoolThread ID: {1}", dispatcher.Name, this.currentQueue);
			thread.Start ();
		}


		void Run ()
		{
			while (dispatcher.active) {
				ITask task = null;
				DispatcherQueue queue = null;
				try {
					task = dispatcher.Dequeue (ref currentQueue, out queue);
				} catch (Exception e) { //DispatcherQueue is failing, what should we do?
					//dispatcher.TaskDone (task, queue, null);
					Thread.Sleep (500);
				}
				
				if (task == null)
					continue;
				queue.RunTask (task);
			}
		}
	}

	public sealed class Dispatcher : IDisposable
	{
		object _lock = new object ();
		DispatcherQueue[] queues = new DispatcherQueue [0];
		readonly List<CcrWorker> workers = new List<CcrWorker> ();

		long processedTasks;
		volatile int pendingTasks;
		volatile int pendingWorkers;
		internal bool active = true;
		bool isDisposed;
		int maxThreads = 10;

		public Dispatcher ()
		{
			Name = "unnamed";
		}

		public Dispatcher (int threadCount, string threadPoolName)
		{
			maxThreads = threadCount;
			Name = threadPoolName;
		}

		internal void TaskDone (ITask task, Exception e)
		{
			Interlocked.Increment (ref processedTasks);
			if (e != null && UnhandledException != null)
				UnhandledException (this, new UnhandledExceptionEventArgs (e, false));
		}

		internal void SpawnWorker ()
		{
			lock (_lock) {
				if (workers.Count >= maxThreads)
					return;
				var w = new CcrWorker (this, queues.Length);
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

		public List<DispatcherQueue> DispatcherQueues
		{
			get
			{
				DispatcherQueue[] queues = this.queues;
				return new List<DispatcherQueue> (queues);
			}
		}

		public string Name { get; set; }

		public int PendingTaskCount
		{
			get { return pendingTasks; }
			set { } //FIXME what on earth a setter here could do?
		}

		public long ProcessedTaskCount	
		{
			get { return Thread.VolatileRead (ref processedTasks); }
			set { } //FIXME what on earth a setter here could do?
		}

		public int WorkerThreadCount
		{
			get { return workers.Count; }
			set { } //FIXME what on earth a setter here could do?
		}

		public event UnhandledExceptionEventHandler UnhandledException;

	}
}
