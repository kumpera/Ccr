//
// TaskCommon.cs
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
using Microsoft.Ccr.Core.Arbiters;

namespace Microsoft.Ccr.Core {

	public class Receiver : ReceiverTask
	{
		IPortReceive port;

		public Receiver (IPortReceive port, ITask task) : this (false, port, task) {}

		public Receiver (bool persist, IPortReceive port, ITask task) : base (task)
		{
			if (port == null)
				throw new ArgumentNullException ("port");
			this.port = port;
			if (persist)
				State = ReceiverTaskState.Persistent;
		}

		public override void Cleanup (ITask taskToCleanup)
		{
			((IPortArbiterAccess)port).PostElement (taskToCleanup [0]);
		}

		public override void Consume (IPortElement item)
		{
			if (State == ReceiverTaskState.CleanedUp)
				return;
			ITask res = UserTask.PartialClone ();
			res [0] = item;
			TaskQueue.Enqueue (res);
		}

		public override void Cleanup ()
		{
			base.Cleanup ();
			port.UnregisterReceiver (this);
		}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
			if (State == ReceiverTaskState.CleanedUp)
				return false;
			ITask task = UserTask;
			IArbiterTask arbiter = Arbiter;
			if (task != null) {
				if (State == ReceiverTaskState.Persistent)
					task = task.PartialClone ();
				task [0] = messageNode;
			}
			deferredTask = task;

			if (arbiter != null && !arbiter.Evaluate (this, ref deferredTask))
				return false;

			return true;
		}

		public override IArbiterTask Arbiter
		{
			set {
				base.Arbiter = value;
				if (TaskQueue == null && value != null)
					TaskQueue = value.TaskQueue;
				port.RegisterReceiver (this);
			}
		}
	}


	public class Receiver<T> : Receiver
	{
		public Receiver (IPortReceive port, Predicate<T> predicate, Task<T> task) : base (port, task)
		{
			Predicate = predicate;
		}

		public Receiver (IPortReceive port, Predicate<T> predicate, IterativeTask<T> task) : base (port, task)
		{
			Predicate = predicate;
		}

		public Receiver (bool persist, IPortReceive port, Predicate<T> predicate, Task<T> task) : base (persist, port, task)
		{
			Predicate = predicate;
		}

		public Receiver (bool persist, IPortReceive port, Predicate<T> predicate, IterativeTask<T> task) : base (persist, port, task)
		{
			Predicate = predicate;
		}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
			Predicate<T> pred = Predicate;
			if (pred != null && !pred ((T)messageNode.Item))
				return false;
			return base.Evaluate (messageNode, ref deferredTask);
		}

		public Predicate<T> Predicate { get; set; }
	}
}
