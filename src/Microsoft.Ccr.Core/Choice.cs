//
// Choice.cs
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

	public class Choice :  IArbiterTask, ITask
	{
		ReceiverTask[] branches;
		object _lock = new object ();
		ArbiterTaskState state;
		bool done = false;

		public Choice (params ReceiverTask[] branches)
		{
			foreach (var b in branches) {
				if (b.State == ReceiverTaskState.Persistent)
					throw new ArgumentOutOfRangeException ("branches", "Cannot use with Choise a Receiver in persistent mode");
			}
			this.branches = branches;
		}
	
		public IEnumerator<ITask> Execute ()
		{
			lock (_lock) {
				foreach (var rt in branches)
					rt.Arbiter = this;
				state = ArbiterTaskState.Active;
			}
			return null;
		}

		public ITask PartialClone ()
		{
			return new Choice (branches);
		}

		void Finish (ITask winner)
		{
			TaskQueue.Enqueue (winner);
			foreach (var rt in branches)
				rt.Cleanup ();
		}

		public bool Evaluate (ReceiverTask receiver, ref ITask deferredTask)
		{
			lock (_lock) {
				if (done) {
					return false;
				} else {
					state = ArbiterTaskState.Done;
					deferredTask = new Task<ITask> (deferredTask, this.Finish);
					done = true;
					return true;
				}
			}
		}

		public ArbiterTaskState ArbiterState
		{
			get { return state; }
		}

		public IPortElement this[int index]
		{
			get { throw new NotSupportedException (); }
			set { throw new NotSupportedException (); }
		}

		public int PortElementCount
		{
			get { return 0; }
		}

		public Handler ArbiterCleanupHandler { get; set; }
		public Object LinkedIterator { get; set; }
		public DispatcherQueue TaskQueue { get; set; }
	}
}
