//
// CcrServiceBase.cs
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

	public class CcrServiceBase
	{
		protected CcrServiceBase () { }

		protected CcrServiceBase (DispatcherQueue dispatcherQueue)
		{
			TaskQueue = dispatcherQueue;
		}

		protected DispatcherQueue TaskQueue { get; set; }

		protected void Spawn (Handler handler)
		{
			TaskQueue.Enqueue (Arbiter.FromHandler (handler));
		}

		protected void Spawn<T0> (T0 t0, Handler<T0> handler)
		{
			TaskQueue.Enqueue (new Task<T0> (t0, handler));
		}

		protected void Spawn<T0, T1> (T0 t0, T1 t1, Handler<T0, T1> handler)
		{
			TaskQueue.Enqueue (new Task<T0, T1> (t0, t1, handler));
		}

		protected void Spawn<T0, T1, T2> (T0 t0, T1 t1, T2 t2, Handler<T0, T1, T2> handler)
		{
			TaskQueue.Enqueue (new Task<T0, T1, T2> (t0, t1, t2, handler));
		}

		protected void SpawnIterator (IteratorHandler handler)
		{
			TaskQueue.Enqueue (new IterativeTask (handler));
		}

		protected void SpawnIterator<T0> (T0 t0, IteratorHandler<T0> handler)
		{
			TaskQueue.Enqueue (new IterativeTask<T0> (t0, handler));
		}

		protected void SpawnIterator<T0, T1> (T0 t0, T1 t1, IteratorHandler<T0, T1> handler)
		{
			TaskQueue.Enqueue (new IterativeTask<T0, T1> (t0, t1, handler));
		}

		protected void SpawnIterator<T0, T1, T2> (T0 t0, T1 t1, T2 t2, IteratorHandler<T0, T1, T2> handler)
		{
			TaskQueue.Enqueue (new IterativeTask<T0, T1, T2> (t0, t1, t2, handler));
		}

		public void Activate<T> (params T[] tasks) where T : ITask
		{
			foreach (var t in tasks)
				TaskQueue.Enqueue (t);
		}


		public static void EmptyHandler<T> (T message) {}

	}

}
