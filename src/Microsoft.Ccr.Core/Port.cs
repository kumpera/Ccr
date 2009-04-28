//
// Port.cs
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
using Microsoft.Ccr.Core.Arbiters;

namespace Microsoft.Ccr.Core {

	public class Port<T>: IPort, IPortReceive, IPortArbiterAccess
	{
		PortMode mode;
		LinkedList<T> list = new LinkedList<T> ();
		LinkedList<ReceiverTask> receivers = new LinkedList<ReceiverTask> ();
		object _lock = new object ();
		

		public Port ()
		{
		
		}

		public virtual bool Test (out T item)
		{
			lock (_lock) {
				if (list.Count > 0) {
					item = list.First.Value;
					list.RemoveFirst ();
					return true;
				}
				item = default (T);
				return false;
			}
		}

		public virtual void Post (T item)
		{
			lock (_lock) {
				var elem = new PortElement<T> (item);
				foreach (ReceiverTask rt in receivers) {
					ITask task = null;
					if (rt.Evaluate (elem, ref task)) {
						if (task != null) {
							DispatcherQueue dq = rt.TaskQueue;
							if (dq != null)
								dq.Enqueue (task);
						}
						if (rt.State != ReceiverTaskState.Persistent)
							receivers.Remove (rt);
						return;
					}

				}
				list.AddLast (item);
			}
		}

		public override int GetHashCode ()
		{
			return typeof (T).GetHashCode ();
		}

		[MonoTODO ("make this more informative")]
		public override string ToString ()
		{
			lock (_lock) {
				return String.Format ("Port\n\t Type: {0}\n\t Elements: {1}\n\t Receivers: {2}", typeof (T), list.Count, receivers.Count);
			} 
		}

		public static implicit operator T (Port<T> port)
		{
			T t;
			port.Test (out t);
			return t;
		}
		//IPort

		public virtual void PostUnknownType (object item)
		{
			Post ((T)item);
		}

		void IPort.PostUnknownType (object item)
		{
			this.PostUnknownType (item);
		}

		public virtual bool TryPostUnknownType (object item)
		{
			if (item == null)
				throw new NullReferenceException ();
			if (!(item is T))
				return false;
			Post ((T)item);
			return true;
		}

		bool IPort.TryPostUnknownType (object item)
		{
			return this.TryPostUnknownType (item);
		}

		//IPortReceive
		public void Clear ()
		{
			lock (_lock) {
				list.Clear ();
			}
		}

		[MonoTODO]
		protected virtual object[] GetItems ()
		{
			return new object [0];
		}

		object[] IPortReceive.GetItems ()
		{
			return this.GetItems ();
		}

		protected virtual ReceiverTask[] GetReceivers ()
		{
			lock (_lock) {
				ReceiverTask[] res = new ReceiverTask [receivers.Count];
				receivers.CopyTo (res, 0);
				return res;
			}
		}

		ReceiverTask[] IPortReceive.GetReceivers ()
		{
			return this.GetReceivers ();
		}

		protected virtual void RegisterReceiver (ReceiverTask receiver)
		{
			if (receiver == null)
				throw new ArgumentNullException ("receiver");
			lock (_lock) {
				this.receivers.AddLast (receiver);
			}
		}

		void IPortReceive.RegisterReceiver (ReceiverTask receiver)
		{
			this.RegisterReceiver (receiver);
		}

		public virtual object Test ()
		{
			lock (_lock) {
				if (list.Count > 0) {
					object res = list.First.Value;
					list.RemoveFirst ();
					return res;
				}
				return null;
			}
		}

		object IPortReceive.Test ()
		{
			return this.Test ();
		}

		protected virtual void UnregisterReceiver (ReceiverTask receiver)
		{
			if (receiver == null)
				throw new ArgumentNullException ("receiver");
			lock (_lock) {
				this.receivers.Remove (receiver);
			}
		}

		void IPortReceive.UnregisterReceiver (ReceiverTask receiver)
		{
			this.UnregisterReceiver (receiver);
		}


		public virtual int ItemCount
		{
			get { lock (_lock) { return list.Count; } }
		}

		int IPortReceive.ItemCount
		{
			get { lock (_lock) { return list.Count; } }
		}


		//IPortArbiterAccess

		public void PostElement (IPortElement element)
		{
			throw new NotImplementedException ();
		}

		public IPortElement TestForElement ()
		{
			throw new NotImplementedException ();
			return null;
		}

		public IPortElement[] TestForMultipleElements (int count)
		{
			throw new NotImplementedException ();
			return null;
		}

		public PortMode Mode
		{
			get { return mode; }
			set { mode = value; throw new NotImplementedException (); }
		}
	}
}
