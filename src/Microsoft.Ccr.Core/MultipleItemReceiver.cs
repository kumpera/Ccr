//
// MultipleItemReceiver.cs
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

using Microsoft.Ccr.Core.Arbiters;

namespace Microsoft.Ccr.Core {

	class ReceiverSurrogate : Receiver {
		MultipleItemReceiver receiver;
		int number;

		internal ReceiverSurrogate (IPortReceive port, MultipleItemReceiver receiver, int number) : base (port, null)
		{
			this.receiver = receiver;
			this.number = number;
		}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
			return receiver.Process (messageNode, number, ref deferredTask);
		}
	}

	public class MultipleItemReceiver : ReceiverTask
	{
		ITask userTask;
		IPortReceive[] ports;
		int remaining;

		public MultipleItemReceiver (ITask userTask, params IPortReceive[] ports)
		{
			this.userTask = userTask;
			this.ports = ports;
		}

		internal bool Process (IPortElement messageNode, int number, ref ITask deferredTask)
		{
			userTask [number] = messageNode;
			deferredTask = null;
			if (Interlocked.Decrement (ref remaining) == 0)
				deferredTask = userTask;
			return true;
		}

		public override void Cleanup (ITask taskToCleanup)
		{
			throw new NotImplementedException ();
		}

		public override void Consume (IPortElement item)
		{
			throw new NotImplementedException ();
		}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
			throw new NotImplementedException ();
		}

		public override IEnumerator<ITask> Execute ()
		{
			return base.Execute ();
		}

		public ITask PartialClone ()
		{
			return new MultipleItemReceiver (userTask, ports);
		}

		public override IArbiterTask Arbiter
		{
			set
			{
				base.Arbiter = value;
				if (TaskQueue == null && value != null)
					TaskQueue = value.TaskQueue;
				remaining = ports.Length;
				for (int i = 0; i < ports.Length; ++i) {
					Receiver rec = new ReceiverSurrogate (ports [i], this, i);
					rec.TaskQueue = this.TaskQueue;
					ports [i].RegisterReceiver (rec);
				}
			}
		}
	}
}
