//
// MultipleItemGather .cs
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
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Ccr.Core.Arbiters;

namespace Microsoft.Ccr.Core {

	class MultiItemReceiverSurrogate : Receiver {
		MultipleItemGather receiver;
		int number;

		internal MultiItemReceiverSurrogate (IPortReceive port, MultipleItemGather receiver, int number) : base (port, null)
		{
			this.receiver = receiver;
			this.number = number;
		}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
			return receiver.Process (messageNode, number, ref deferredTask);
		}
	}

	public class MultipleItemGather : ReceiverTask, ITask
	{
		readonly Type[] types;
		readonly IPortReceive[] ports;
		readonly List<object>[] results;
		readonly Handler<ICollection[]> handler;
		readonly object _lock = new object ();
		readonly int itemCount;
		ReceiverTask[] receivers;
		int remaining;

		//FIXME what's the use of the types array?
		public MultipleItemGather (Type[] types, IPortReceive[] ports, int itemCount, Handler<ICollection[]> handler)
		{
			if (types == null)
				throw new ArgumentNullException("types");
			if (ports == null)
				throw new ArgumentNullException("ports");
			if (handler == null)
				throw new ArgumentNullException("handler");
			if (types.Length == 0)
				throw new ArgumentOutOfRangeException ("types");
			if (ports.Length == 0)
				throw new ArgumentOutOfRangeException ("ports");
			if (types.Length != ports.Length)
				throw new ArgumentOutOfRangeException ("types");

			this.types = types;
			this.ports = ports;
			this.itemCount = itemCount;
			this.handler = handler;
			this.results = new List<object>[ports.Length];
			for (int i = 0; i < ports.Length; ++i)
				this.results [i] = new List<object> ();
		}

		internal bool Process (IPortElement messageNode, int number, ref ITask deferredTask)
		{
			int rem = Interlocked.Decrement (ref remaining);
			if (rem < 0)
				return false;
			lock (_lock) {
				results [number].Add (messageNode.Item);
			}
			if (rem == 0) {
				ITask task = new Task<ICollection[]> (results, handler);
				task.LinkedIterator = LinkedIterator;
				deferredTask = task;
				var arb = Arbiter;
				if (arb != null)
					return arb.Evaluate (this, ref deferredTask);
				Cleanup ();
			}
			return true;
		}

		void RegisterReceivers ()
		{
			receivers = new ReceiverTask [ports.Length];
			remaining = itemCount;

			for (int i = 0; i < ports.Length; ++i) {
				var rec = new MultiItemReceiverSurrogate (ports [i], this, i);
				rec.TaskQueue = TaskQueue;
				rec.State = ReceiverTaskState.Persistent;
				receivers [i] = rec;
				ports [i].RegisterReceiver (rec);
				if (remaining == 0)
					break;
			}

		}

		public override void Cleanup (ITask taskToCleanup)
		{
			ICollection[] data = (ICollection[])taskToCleanup [0].Item;
			for (int i = 0; i < ports.Length; ++i) {
				foreach (var o in data [i])
					(ports [i] as IPort).PostUnknownType (o);
			}
		}

		public override void Cleanup ()
		{
			base.Cleanup ();
			for (int i = 0; i < ports.Length; ++i)
				ports [i].UnregisterReceiver (receivers [i]);
		}

		public sealed override ITask PartialClone ()
		{
			return new MultipleItemGather (types, ports, itemCount, handler);
		}

		ITask ITask.PartialClone ()
		{
			return new MultipleItemGather (types, ports, itemCount, handler);
		}
		
		public override void Consume (IPortElement item)
		{
			throw new InvalidOperationException ();
		}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
			throw new InvalidOperationException ();
		}

		public override IEnumerator<ITask> Execute ()
		{
			RegisterReceivers ();
			return base.Execute ();
		}

		public override IArbiterTask Arbiter
		{
			set
			{
				base.Arbiter = value;
				if (TaskQueue == null && value != null)
					TaskQueue = value.TaskQueue;
				RegisterReceivers ();
			}
		}
	}
}
