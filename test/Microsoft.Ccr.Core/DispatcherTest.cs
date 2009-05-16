//
// DispatcherTest.cs
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
	public class DispatcherTest
	{
		public class SerialDispatcherQueue : DispatcherQueue
		{
			public int queuedTasks;
			public ITask lastQueuedTask;

			public SerialDispatcherQueue (string str, Dispatcher disp) : base (str, disp) {}

			public override bool Enqueue (ITask task)
			{
				++queuedTasks;
				lastQueuedTask = task;
				return true;
			}
		}

		[Test]
		public void EmptyCtorSideEffects ()
		{
			var disp = new Dispatcher ();
		}

		[Test]
		public void UnhandledExceptionPort ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				DispatcherQueue dq = new DispatcherQueue ("foo", d);
				var evt = new AutoResetEvent (false);
				int dispEx = 0;
				int queueEx = 0;
				d.UnhandledException += delegate { ++dispEx; };
				dq.UnhandledException += delegate { ++queueEx; };

				dq.Enqueue (Arbiter.FromHandler (() => { evt.Set (); throw new Exception (); }));

				Assert.IsTrue (evt.WaitOne (2000), "#1");
				Assert.AreEqual (0, dispEx, "#2"); 
				Assert.AreEqual (1, queueEx, "#3"); 

			}
		}

		[Test]
		public void UnhandledExceptionPort2 ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				DispatcherQueue dq = new DispatcherQueue ("foo", d);
				var evt = new AutoResetEvent (false);
				int dispEx = 0;
				d.UnhandledException += delegate { ++dispEx; };

				dq.Enqueue (Arbiter.FromHandler (() => { evt.Set (); throw new Exception (); }));

				Assert.IsTrue (evt.WaitOne (2000), "#1");
				Assert.AreEqual (1, dispEx, "#2"); 
			}
		}
	}
}
