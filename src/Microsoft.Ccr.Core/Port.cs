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
				list.AddLast (item);
				ITask task = null;
				foreach (ReceiverTask rt in receivers) {
					if (rt.Evaluate (elem, ref task))
						break;
					
				}
			}
		}

		//IPort

		//TODO
		void IPort.PostUnknownType (object item)
		{
		}

		//TODO
		bool IPort.TryPostUnknownType (object item)
		{
			return false;
		}


		//IPortReceive
		//TODO
		public void Clear ()
		{
		}

		//TODO
		protected virtual object[] GetItems ()
		{
			return new object [0];
		}

		object[] IPortReceive.GetItems ()
		{
			return this.GetItems ();
		}


		//TODO
		protected virtual ReceiverTask[] GetReceivers ()
		{
			return new ReceiverTask [0];
		}

		ReceiverTask[] IPortReceive.GetReceivers ()
		{
			return this.GetReceivers ();
		}

		//TODO
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

		//TODO
		void IPortReceive.UnregisterReceiver (ReceiverTask receiver)
		{
		}

		int IPortReceive.ItemCount
		{
			get { lock (_lock) { return list.Count; } }
		}



		//IPortArbiterAccess

		//TODO
		public void PostElement (IPortElement element)
		{
		}

		//TODO
		public IPortElement TestForElement ()
		{
			return null;
		}

		//TODO
		public IPortElement[] TestForMultipleElements (int count)
		{
			return null;
		}

		public PortMode Mode
		{
			get { return mode; }
			set { mode = value;} //TODO
		}



	}
}
