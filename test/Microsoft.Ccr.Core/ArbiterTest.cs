//
// ArbiterTest.cs
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
using System.Threading;
using Microsoft.Ccr.Core.Arbiters;

using NUnit.Framework;

namespace Microsoft.Ccr.Core {

	[TestFixture]
	public class ArbiterTest
	{
		class VoidDispatcherQueue : DispatcherQueue
		{
			public int queuedTasks;
			public ITask lastQueuedTask;

			public override bool Enqueue (ITask task)
			{
				++queuedTasks;
				lastQueuedTask = task;
				return true;
			}
		}

		class SerialDispatchQueue : DispatcherQueue
		{
			public override bool Enqueue (ITask task)
			{
				task.Execute ();
				return true;
			}
		}

		[Test]
		public void OneTimeReceive3 ()
		{
			int executed = 1;
			var port = new Port<int> ();
			Handler<int> handler = (a) => { executed += a; };

			var rec = Arbiter.Receive (false, port, handler);
			rec.TaskQueue = new SerialDispatchQueue ();

			Assert.AreEqual (0, ((IPortReceive)port).GetReceivers ().Length, "#1");
			Assert.IsNull (rec.Arbiter, "#2");
			Assert.IsNull (rec.Predicate, "#3");
			Assert.AreEqual (ReceiverTaskState.Onetime, rec.State, "#4");

			rec.Consume (new PortElement<int> (10));
			Assert.AreEqual (11, executed, "#5");
		}

		[Test]
		public void Activate ()
		{
			int exec = 0;
			var task = new Task (() =>{ ++exec; });
			DispatcherQueue dq = new SerialDispatchQueue ();

			Assert.IsNull (task.TaskQueue, "#1");
			Arbiter.Activate (dq, task);
			Assert.AreEqual (1, exec, "#4");
			Arbiter.Activate (dq, task, task, task);
			Assert.AreEqual (4, exec, "#5");
		}

		[Test]
		public void ActivateBadArgs ()
		{
			var task = new Task (() =>{ });
			DispatcherQueue dq = new SerialDispatchQueue ();
			try {
				Arbiter.Activate (null, task);
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {}

			try {
				Arbiter.Activate (dq, null);
				Assert.Fail ("#2");
			} catch (ArgumentNullException) {}
		}


		[Test]
		public void FromHandler ()
		{
			int cnt = 0;
			var task = Arbiter.FromHandler (()=> { ++cnt; });
			task.Execute ();
			Assert.AreEqual (1, cnt, "#1");
		}
	}
}