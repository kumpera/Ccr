//
// Arbitrer.cs
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

using Microsoft.Ccr.Core.Arbiters;

namespace Microsoft.Ccr.Core {

	public static class Arbiter
	{
		public static void Activate (DispatcherQueue dispatcherQueue, params ITask[] arbiter)
		{
			if (dispatcherQueue == null)
				throw new ArgumentNullException ("dispatcher");
			if (arbiter == null)
				throw new ArgumentNullException ("arbiter");
			foreach (var task in arbiter)
				dispatcherQueue.Enqueue (task);
		}		

		public static ITask FromHandler (Handler handler)
		{
			return new Task (handler);
		}
		
		public static ITask FromIteratorHandler (IteratorHandler handler)
		{
			return new IterativeTask (handler);
		}

		public static Receiver<T> Receive<T> (bool persist, Port<T> port, Handler<T> handler)
		{
			return new Receiver<T> (persist, port, null, new Task<T> (handler));
		}

		public static Receiver<T> Receive<T> (bool persist, Port<T> port, Handler<T> handler, Predicate<T> predicate)
		{
			return new Receiver<T> (persist, port, predicate, new Task<T> (handler));
		}

		public static Receiver<T> ReceiveWithIterator<T> (bool persist, Port<T> port, IteratorHandler<T> handler)
		{
			return new Receiver<T> (persist, port, null, new IterativeTask<T> (handler));
		}

		public static Receiver<T> ReceiveWithIterator<T> (bool persist, Port<T> port, IteratorHandler<T> handler, Predicate<T> predicate)
		{
			return new Receiver<T> (persist, port, predicate, new IterativeTask<T> (handler));
		}

		public static Receiver<T> ReceiveFromPortSet<T> (bool persist, IPortSet portSet, Handler<T> handler)
		{
			return new Receiver<T> (persist, (IPortReceive)portSet [typeof (T)], null, new Task<T> (handler));
		}

		public static Receiver<T> ReceiveFromPortSet<T> (bool persist, IPortSet portSet, Handler<T> handler, Predicate<T> predicate)
		{
			return new Receiver<T> (persist, (IPortReceive)portSet [typeof (T)], predicate, new Task<T> (handler));
		}

		public static Receiver<T> ReceiveWithIteratorFromPortSet<T> (bool persist, IPortSet portSet, IteratorHandler<T> handler)
		{
			return new Receiver<T> (persist, (IPortReceive)portSet [typeof (T)], null, new IterativeTask<T> (handler));
		}

		public static Receiver<T> ReceiveWithIteratorFromPortSet<T> (bool persist, IPortSet portSet, IteratorHandler<T> handler, Predicate<T> predicate)
		{
			return new Receiver<T> (persist, (IPortReceive)portSet [typeof (T)], predicate, new IterativeTask<T> (handler));
		}

		public static Choice Choice (params ReceiverTask[] receivers)
		{
			return new Choice (receivers);
		}

		public static Choice Choice<T0, T1> (PortSet<T0, T1> resultPort, Handler<T0> handler0, Handler<T1> handler1)
		{
			return new Choice (
				Receive (false, resultPort.P0, handler0),
				Receive (false, resultPort.P1, handler1));
		}

		public static Choice Choice (IPortSet portSet)
		{
			ICollection<IPort> ports = portSet.Ports;
			ReceiverTask[] receivers = new ReceiverTask [ports.Count];
			int idx = 0;
			foreach (var p in ports)
				receivers[idx++] = (p as IPortReceive).ReceiveForIterator ();

			return new Choice (receivers);
		}

		public static MultipleItemReceiver MultipleItemReceive<T> (VariableArgumentHandler<T> handler, params Port<T>[] ports)
		{
			return new MultipleItemReceiver (new VariableArgumentTask<T> (ports.Length, handler), ports);
		}

		public static MultipleItemGather MultipleItemReceive<T0, T1> (PortSet<T0, T1> portSet, int totalItemCount, Handler<ICollection<T0>, ICollection<T1>> handler)
		{
			//public MultipleItemGather (Type[] types, IPortReceive[] ports, int itemCount, Handler<ICollection[]> handler)
			Type[] types = new Type [] { typeof (T0), typeof (T1) };
			IPortReceive[] ports = new IPortReceive[] { portSet.P0, portSet.P1 };
			Handler<ICollection[]> inner_handler = (col) => {
				//LAMEIMPL MS impl of MultipleItemGather passes an array of List<object>.
				List<T0> l0 = new List<T0> (col [0].Count);
				List<T1> l1 = new List<T1> (col [1].Count);
				foreach (var o in col [0])
					l0.Add ((T0)o);
				foreach (var o in col [1])
					l1.Add ((T1)o);
				handler (l0, l1);
			}; 
			return new MultipleItemGather (types, ports, totalItemCount, inner_handler);
		}
	}
}
