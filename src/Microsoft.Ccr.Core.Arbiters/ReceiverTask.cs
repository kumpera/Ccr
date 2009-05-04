//
// ReceiverTask.cs
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

namespace Microsoft.Ccr.Core.Arbiters {

	public abstract class ReceiverTask : TaskCommon
	{
		protected ReceiverTask ()
		{
		}

		protected ReceiverTask (ITask taskToRun)
		{
			UserTask = taskToRun;
		}

		public override string ToString ()
		{
			return String.Format ("receiver {0} of task {1}", GetType (), UserTask);
		}

		public virtual void Cleanup ()
		{
			State = ReceiverTaskState.CleanedUp;
		}

		public abstract void Cleanup (ITask taskToCleanup);

		public abstract void Consume (IPortElement item);

		public abstract bool Evaluate (IPortElement messageNode, ref ITask deferredTask);

		public override IEnumerator<ITask> Execute ()
		{
			Arbiter = null;
			return null;
		}

		public override ITask PartialClone ()
		{
			throw new NotImplementedException ();
		}

		public override int PortElementCount
		{
			get
			{
				ITask t = UserTask;
				return t == null ? 0 : t.PortElementCount;
			}
		}

		public override IPortElement this[int index]
		{
			get { return UserTask [index]; }
			set { UserTask [index] = value; }
		}

		public ReceiverTaskState State { get; set; }
		public Object ArbiterContext { get; set; }
		public virtual IArbiterTask Arbiter { get; set; }
		protected ITask UserTask { get; set; }	
	}
}