//
// PortExtensions.cs
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
	class WeirdReceiver<T0> : Receiver<T0>
	{
		internal WeirdReceiver (Port<T0> port, Task<T0> task): base (port, null, task) {}

		public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
		{
			base.Evaluate (messageNode, ref deferredTask);
			return false;
		}
	}


	public static class PortExtensions 
	{

		public static Receiver Receive<T> (this Port<T> port)
		{
			Receiver<T> res = null;
			Task<T> task = new Task<T> ((_unused) => res.Cleanup ());
			res = new WeirdReceiver<T> (port, task);
			return res;
		}
	}
}