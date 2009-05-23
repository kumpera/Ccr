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
		public class FaultyDispatcherQueue : DispatcherQueue
		{
			public FaultyDispatcherQueue (string str, Dispatcher disp) : base (str, disp) {}

			public override bool TryDequeue (out ITask task)
			{
				task = null;
				throw new Exception ();
				return false;
			}
		}

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
				dq.UnhandledException += delegate { ++queueEx; evt.Set (); };

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));

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
				d.UnhandledException += delegate { ++dispEx; evt.Set (); };

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));

				Assert.IsTrue (evt.WaitOne (2000), "#1");
				Assert.AreEqual (1, dispEx, "#2"); 
			}
		}

		[Test]
		public void UnhandledExceptionPort3 ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				var dq = new FaultyDispatcherQueue ("foo", d);
				var evt = new AutoResetEvent (false);

				int dispEx = 0;
				d.UnhandledException += delegate { ++dispEx; };

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Thread.Sleep (100);
				Assert.AreEqual (0, dispEx, "#1"); 
			}
		}

		[Test]
		public void ExceptionPort ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				var dq = new DispatcherQueue ("foo", d);
				var evt = new AutoResetEvent (false);
				var port = new Port<Exception> ();
				d.UnhandledExceptionPort = port;

				int portPost = 0;
				var rec = Arbiter.Receive (true, port, (e) => { ++portPost; evt.Set(); });
				rec.TaskQueue = dq;
				rec.Execute ();

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#1");
				Assert.AreEqual (1, portPost, "#2");

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#3");
				Assert.AreEqual (2, portPost, "#4");
			}
		}

		[Test]
		public void DispatchEventWithDispatcherQueueExceptionPort ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				var dq = new DispatcherQueue ("foo", d);
				var evt = new AutoResetEvent (false);
				var port = new Port<Exception> ();
				dq.UnhandledExceptionPort = port;
				
				int portPost = 0;
				int dispEx = 0;
				d.UnhandledException += delegate { ++dispEx; };

				var rec = Arbiter.Receive (true, port, (e) => { ++portPost; evt.Set(); });
				rec.TaskQueue = dq;
				rec.Execute ();

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#1");
				Assert.AreEqual (1, portPost, "#2");
				Assert.AreEqual (0, dispEx, "#3");

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#4");
				Assert.AreEqual (2, portPost, "#5");
				Assert.AreEqual (0, dispEx, "#6");
			}
		}

		[Test]
		public void ExceptionPortAndEvent ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				var dq = new DispatcherQueue ("foo", d);
				var evt = new AutoResetEvent (false);
				var port = new Port<Exception> ();

				int dispEx = 0;
				int portPost = 0;

				d.UnhandledExceptionPort = port;
				d.UnhandledException += delegate { ++dispEx; };

				var rec = Arbiter.Receive (true, port, (e) => { ++portPost; evt.Set(); });
				rec.TaskQueue = dq;
				rec.Execute ();

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#1");
				Assert.AreEqual (1, portPost, "#2"); 
				Assert.AreEqual (1, dispEx, "#3"); 

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#4");

				Assert.AreEqual (2, portPost, "#5"); 
				Assert.AreEqual (2, dispEx, "#6"); 
			}
		}

		[Test]
		public void ExceptionPortAndEventButWithDQEvent ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				var dq = new DispatcherQueue ("foo", d);
				var evt = new AutoResetEvent (false);
				var port = new Port<Exception> ();

				int dispEx = 0;
				int portPost = 0;
				int queueEx = 0;

				d.UnhandledExceptionPort = port;
				d.UnhandledException += delegate { ++dispEx; };
				dq.UnhandledException += delegate { ++queueEx; evt.Set (); };

				var rec = Arbiter.Receive (true, port, (e) => { ++portPost; });
				rec.TaskQueue = dq;
				rec.Execute ();

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#1");
				Assert.AreEqual (0, portPost, "#2"); 
				Assert.AreEqual (0, dispEx, "#3"); 
				Assert.AreEqual (1, queueEx, "#4"); 

				dq.Enqueue (Arbiter.FromHandler (() => { throw new Exception (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#5");

				Assert.AreEqual (0, portPost, "#6"); 
				Assert.AreEqual (0, dispEx, "#7"); 
				Assert.AreEqual (2, queueEx, "#8"); 
			}
		}

		[Test]
		public void StuffAfterDipose ()
		{
			Dispatcher d = new Dispatcher ();
			var dq = new DispatcherQueue ("foo", d);
			d.Dispose ();
			dq.Dispose ();
			Assert.IsTrue (dq.IsDisposed, "#1");
			try {
				dq.Enqueue (Arbiter.FromHandler( () => { Console.WriteLine ("ff"); }));
				Assert.Fail ("#2");
			} catch (ObjectDisposedException) {}

			d = new Dispatcher (1, ThreadPriority.Normal, DispatcherOptions.SuppressDisposeExceptions, "foo");
			dq = new DispatcherQueue ("foo", d);
			d.Dispose ();
			dq.Dispose ();
			Assert.IsTrue (dq.IsDisposed, "#3");
			Assert.IsFalse (dq.Enqueue (Arbiter.FromHandler( () => {})), "#4");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#5");
		}

	}
}
