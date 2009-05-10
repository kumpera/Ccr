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
		LinkedList<PortElement<T>> list = new LinkedList<PortElement<T>> ();
		LinkedList<ReceiverTask> receivers = new LinkedList<ReceiverTask> ();
		object _lock = new object ();
		
		public Port ()
		{
		}

		void Push (bool front, PortElement<T> elem)
		{
			lock (_lock) {
				if (mode == PortMode.OptimizedSingleReissueReceiver) {
					receivers.First.Value.Consume (elem);
					return;
				}

				for (var node = receivers.First; node != null; node = node.Next) {
					ReceiverTask rt = node.Value;

					ITask task = null;
					bool res = rt.Evaluate (elem, ref task);
					if (task != null) {
						DispatcherQueue dq = rt.TaskQueue;
						if (dq != null)
							dq.Enqueue (task);
					}
					if (res && rt.State != ReceiverTaskState.Persistent)
						receivers.Remove (node);
					if (res)
						return;
				}

				if (list.Count > 0) {
					PortElement<T> first = list.First.Value;
					PortElement<T> last = list.Last.Value;
	
					first.Previous = elem;
					last.Next = elem;
					elem.Next = first;
					elem.Previous = last;
				} else {
					elem.Next = elem.Previous = elem;
				}
				if (front)
					list.AddFirst (elem);				
				else
					list.AddLast (elem);
			}
		} 

		PortElement<T> Pop ()
		{
			lock (_lock) {
				if (list.Count > 0) {
					PortElement<T> res = list.First.Value;
					list.RemoveFirst ();

					if (list.Count > 0) {
						PortElement<T> first = list.First.Value;
						PortElement<T> last = list.Last.Value;
		
						first.Previous = last;
						last.Next = first;
					}
			
					return res;
				}
			}
			return null;
		}


		public virtual bool Test (out T item)
		{
			PortElement<T> res = Pop ();
			if (res != null) {
				item = res.TypedItem;
				return true;
			}
			item = default (T);
			return false;
		}

		public virtual void Post (T item)
		{
			Push (false, new PortElement<T> (item, this));
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


		public static implicit operator Receiver<T> (Port<T> port)
		{
			Receiver<T> res = null;
			Task<T> task = new Task<T> ((_unused) => res.Cleanup ());
			res = new WeirdReceiver<T> (port, task);
			return res;
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

		protected virtual object[] GetItems ()
		{
			lock (_lock) {
				object[] res = new object [list.Count];
				int idx = 0;
				foreach (var o in list)
					res [idx++] = o;
				return res;
			}
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
			PortElement<T> res = Pop ();
			return res == null ? null : res.Item;
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
			Push (true, (PortElement<T>)element);
		}

		public IPortElement TestForElement ()
		{
			return Pop ();
		}

		public IPortElement[] TestForMultipleElements (int count)
		{
			lock (_lock) {
				if (list.Count < count)
					return null;
				var res = new IPortElement [count];
				for (int i = 0; i < count; ++i) {
					res [i] = list.First.Value;
					res [i].Previous = list.Last.Value;
					list.RemoveFirst ();
				}

				if (list.Count > 0) {
					PortElement<T> first = list.First.Value;
					PortElement<T> last = list.Last.Value;
	
					first.Previous = last;
					last.Next = first;
				}
				return res;
			}
		}

		public PortMode Mode
		{
			get { return mode; }
			set { mode = value; }
		}
	}
}
