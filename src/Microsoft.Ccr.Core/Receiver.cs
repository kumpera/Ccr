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
		ITask task;

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
			throw new NotImplementedException ();
		}

		public override void Consume (IPortElement item)
		{
			throw new NotImplementedException ();
		}

		public override void Cleanup ()
		{
			port.UnregisterReceiver (this);
			base.Cleanup ();
		}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
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
		public Receiver (IPortReceive port, ITask task) : base (port, task)
		{
		}

	
	}
}
