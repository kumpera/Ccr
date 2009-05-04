//
// VariableArgumentTask.cs
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
using System.Diagnostics;
using Microsoft.Ccr.Core.Arbiters;

namespace Microsoft.Ccr.Core {
	public class VariableArgumentTask<T> : ITask
	{
		PortElement<T>[] data;
		VariableArgumentHandler<T> handler;

		public VariableArgumentTask (int varArgSize, VariableArgumentHandler<T> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");

			this.data = new PortElement<T>[varArgSize];
			this.handler = handler;
		}
	
		public IEnumerator<ITask> Execute ()
		{
			T[] values = new T[data.Length];
			for (int i = 0; i < data.Length; ++i)
				values [i] = data[i].TypedItem;

			handler (values);
			return null; 
		}

		public ITask PartialClone ()
		{
			return new VariableArgumentTask<T> (data.Length, handler);
		}

		public IPortElement this[int index]
		{
			get { return data [index]; }
			set { data [index] = (PortElement<T>)value; }
		}

		public int PortElementCount
		{
			get { return data.Length; }
		}

		public Handler ArbiterCleanupHandler { get; set; }
		public Object LinkedIterator { get; set; }
		public DispatcherQueue TaskQueue { get; set; }

	}
}
