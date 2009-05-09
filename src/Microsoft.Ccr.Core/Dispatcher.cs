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
					task.Execute ();
				} catch (Exception e) {
					Console.WriteLine ("please fix me");
				}
			}
		}
	}

	class QueueMediator
	{
		readonly object _lock = new object ();
		readonly DispatcherQueue queue;
		readonly List<CcrWorker> worker = new List<CcrWorker> ();
		bool active = true;
		readonly internal Dispatcher dispatcher;

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

		internal void Start ()
		{
			SpawnWorker ();
		}

		internal void Notify ()
		{
			lock (_lock) {
				Monitor.Pulse (_lock);
			}	
		}

		internal ITask Dequeue ()
		{
			lock (_lock) {
				ITask task = null;
				while (active &&!queue.TryDequeue (out task))
					Monitor.Wait (_lock);
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
				qm.Start ();
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
