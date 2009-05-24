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

	public static class PortSetExtensions 
	{
		public static Receiver Receive<T0, T1> (this PortSet<T0, T1> portSet, Handler<T0> handler)
		{
			return Arbiter.Receive (false, portSet.P0, handler);
		}

		public static Receiver Receive<T0, T1> (this PortSet<T0, T1> portSet, Handler<T1> handler)
		{
			return Arbiter.Receive (false, portSet.P1, handler);
		}

		public static Choice Choice<T0, T1> (this PortSet<T0, T1> portSet)
		{
			return Arbiter.Choice (portSet);
		}

		public static Choice Choice<T0, T1, T2> (this PortSet<T0, T1, T2> portSet)
		{
			return Arbiter.Choice (portSet);
		}

		public static Choice Choice<T0, T1> (this PortSet<T0, T1> portSet, Handler<T0> handler0, Handler<T1> handler1)
		{
			return Arbiter.Choice (portSet, handler0, handler1);
		}

		public static Choice Choice<T0, T1, T2> (this PortSet<T0, T1, T2> portSet, Handler<T0> handler0, Handler<T1> handler1, Handler<T2> handler2)
		{
			return new Choice (
				Arbiter.Receive (false, portSet.P0, handler0),
				Arbiter.Receive (false, portSet.P1, handler1),
				Arbiter.Receive (false, portSet.P2, handler2));
		}

		public static MultipleItemGather MultipleItemReceive<T0, T1> (this PortSet<T0, T1> portSet, int totalItemCount, Handler<ICollection<T0>, ICollection<T1>> handler)
		{
			return Arbiter.MultipleItemReceive (portSet, totalItemCount, handler);
		}


	}
}
