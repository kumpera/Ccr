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
		List<T> elements = new List<T> ();
		

		public Port()
		{
		
		}

		//TODO
		public bool Test (out T item)
		{
			item = default (T);
			return false;
		}


		//IPort

		//TODO
		public void PostUnknownType (object item)
		{
		}

		//TODO
		public bool TryPostUnknownType (object item)
		{
			return false;
		}


		//IPortReceive
		//TODO
		public void Clear ()
		{
		}

		object[] IPortReceive.GetItems ()
		{
			object[] res = new object[elements.Count];
			for (int i = 0; i < elements.Count; ++i)
				res [i] = elements [i];

			return res;
		}


		//TODO
		ReceiverTask[] IPortReceive.GetReceivers ()
		{
			return new ReceiverTask [0];
		}

		//TODO
		void IPortReceive.RegisterReceiver (ReceiverTask receiver)
		{
		}

		//TODO
		public object Test ()
		{
			return null;
		}
		
		//TODO
		void IPortReceive.UnregisterReceiver (ReceiverTask receiver)
		{
		}

		public int ItemCount
		{
			get { return elements.Count; }
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
