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
		
		public static Receiver<T> Receive<T> (bool persist, Port<T> port, Handler<T> handler)
		{
			return new Receiver<T> (persist, port, null, new Task<T> (handler));
		}

		public static Receiver<T> Receive<T> (bool persist, Port<T> port, Handler<T> handler, Predicate<T> predicate)
		{
			return new Receiver<T> (persist, port, predicate, new Task<T> (handler));
		}
	}

}
