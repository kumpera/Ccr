//
// DispatcherQueueTest.cs
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
using System.Threading;

using Microsoft.Ccr.Core.Arbiters;
using NUnit.Framework;

namespace Microsoft.Ccr.Core {

	[TestFixture]
	public class DispatcherQueueTest
	{
		[Test]
		public void EmptyCtor ()
		{
			var dq = new DispatcherQueue ();
			Assert.IsNotNull (dq.Name, "#1");
			Assert.IsNull (dq.Dispatcher, "#2");
		}

		[Test]
		public void NameDispatcherCtor ()
		{
			try {
				new DispatcherQueue ("tst", null);
				Assert.Fail ("#1");
			} catch (ArgumentNullException){}

			try {
				new DispatcherQueue (null, new Dispatcher ());
				Assert.Fail ("#2");
			} catch (ArgumentNullException){}

			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp);

			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
		}

		[Test]
		public void PropertiesAfterZeroArgCtor ()
		{
			var dq = new DispatcherQueue ();

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.IsNotNull (dq.Name, "#3");
			Assert.IsNull (dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsTrue (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (0, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (0, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.Unconstrained, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void PropertiesAfterTwoArgCtor ()
		{
			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp);

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsFalse (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (0, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (1, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.Unconstrained, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void PropertiesAfterFourArgDoubleCtor1 ()
		{
			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp, TaskExecutionPolicy.Unconstrained, 20.0);

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsFalse (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (0, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (20, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.Unconstrained, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void PropertiesAfterFourArgDoubleCtor2 ()
		{
			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainSchedulingRateThrottleExecution, 20.0);

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsFalse (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (0, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (20, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.ConstrainSchedulingRateThrottleExecution, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void PropertiesAfterFourArgDoubleCtor3 ()
		{
			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainSchedulingRateDiscardTasks, 20.0);

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsFalse (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (0, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (20, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.ConstrainSchedulingRateDiscardTasks, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void BadFourArgDoubleCtor ()
		{
			Dispatcher disp = new Dispatcher ();
			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainQueueDepthDiscardTasks, 10.0);
				Assert.Fail ("#1");
			} catch (ArgumentException) {}

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainQueueDepthThrottleExecution, 10.0);
				Assert.Fail ("#2");
			} catch (ArgumentException) {}

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.Unconstrained, 0.0);
			} catch (ArgumentException) { Assert.Fail ("#3"); }

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainSchedulingRateDiscardTasks, 0.0);
				Assert.Fail ("#4");
			} catch (ArgumentException) {}

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainSchedulingRateDiscardTasks, -1.1);
				Assert.Fail ("#5");
			} catch (ArgumentException) {}
		}

		[Test]
		public void PropertiesAfterFourArgIntCtor1 ()
		{
			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp, TaskExecutionPolicy.Unconstrained, 20);

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsFalse (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (20, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (0, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.Unconstrained, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void PropertiesAfterFourArgIntCtor2 ()
		{
			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainQueueDepthDiscardTasks, 20);

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsFalse (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (20, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (0, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.ConstrainQueueDepthDiscardTasks, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void PropertiesAfterFourArgIntCtor3 ()
		{
			Dispatcher disp = new Dispatcher ();
			var dq = new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainQueueDepthThrottleExecution, 20);

			Assert.AreEqual (0, dq.Count, "#1");
			Assert.AreEqual (0, dq.CurrentSchedulingRate, "#2");
			Assert.AreEqual ("dd", dq.Name, "#3");
			Assert.AreEqual (disp, dq.Dispatcher, "#4");
			Assert.IsNull (dq.ExecutionPolicyNotificationPort, "#5");
			Assert.IsFalse (dq.IsDisposed, "#6");
			Assert.IsFalse (dq.IsSuspended, "#7");
			Assert.IsFalse (dq.IsUsingThreadPool, "#8");
			Assert.AreEqual (20, dq.MaximumQueueDepth, "#9");
			Assert.AreEqual (0, dq.MaximumSchedulingRate, "#10");
			Assert.AreEqual (TaskExecutionPolicy.ConstrainQueueDepthThrottleExecution, dq.Policy, "#11");
			Assert.AreEqual (0, dq.ScheduledTaskCount, "#12");
			/*default is 10ms*/
			Assert.AreEqual (new TimeSpan (0,0,0,0,10), dq.ThrottlingSleepInterval, "#13");
			Assert.AreEqual (1, dq.Timescale, "#14");
			Assert.IsNull (dq.UnhandledExceptionPort, "#15");
		}

		[Test]
		public void BadFourArgIntCtor ()
		{
			Dispatcher disp = new Dispatcher ();
			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainSchedulingRateDiscardTasks, 10);
				Assert.Fail ("#1");
			} catch (ArgumentException) {}

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainSchedulingRateThrottleExecution, 10);
				Assert.Fail ("#2");
			} catch (ArgumentException) {}

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.Unconstrained, 0);
			} catch (ArgumentException) { Assert.Fail ("#3"); }

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainQueueDepthThrottleExecution, 0);
				Assert.Fail ("#4");
			} catch (ArgumentException) {}

			try {
				new DispatcherQueue ("dd", disp, TaskExecutionPolicy.ConstrainQueueDepthThrottleExecution, -10);
				Assert.Fail ("#4");
			} catch (ArgumentException) {}
		}

		[Test]
		public void SimpleEnqueue ()
		{
			int cnt = 0;
			int tpThreads = 0;
			const int TOTAL = 10;
			AutoResetEvent evt = new AutoResetEvent (false);

			Task t = new Task(() => {
				if (Thread.CurrentThread.IsThreadPoolThread)
					Interlocked.Increment (ref tpThreads);
				if (Interlocked.Increment (ref cnt) == TOTAL)
					evt.Set ();
			});
			DispatcherQueue dq = new DispatcherQueue ();

			for (int i = 0; i < TOTAL; ++i)
				Assert.IsTrue (dq.Enqueue (t), "#t-"+i);
			Assert.AreEqual (0, dq.Count, "#1");

			evt.WaitOne ();
			Assert.AreEqual (TOTAL, cnt, "#2");
			Assert.AreEqual (TOTAL, tpThreads, "#3");
			Assert.AreEqual (TOTAL, dq.ScheduledTaskCount, "#4");
		}

		public class RecordPartialCloneTask : Task
		{
			public bool cloneCalled;

			public RecordPartialCloneTask (Handler h) : base (h) {
			}

			public override ITask PartialClone ()
			{
				cloneCalled = true;
				return base.PartialClone ();
			}
		}

		[Test]
		public void SimpleEnqueueDoesntClonesTask ()
		{
			AutoResetEvent evt = new AutoResetEvent (false);

			var task = new RecordPartialCloneTask(() => evt.Set ());
			var dq = new DispatcherQueue ();

			Assert.IsTrue (dq.Enqueue (task), "#1");
			evt.WaitOne ();
			Assert.IsFalse (task.cloneCalled, "#2");
		}

		[Test]
		public void SettingIsUsingThreadPoolIsUseless ()
		{
			DispatcherQueue dq = new DispatcherQueue ();
			Assert.IsTrue (dq.IsUsingThreadPool, "#1");
			dq.IsUsingThreadPool = false;
			Assert.IsTrue (dq.IsUsingThreadPool, "#2");
			dq.IsUsingThreadPool = true;
			Assert.IsTrue (dq.IsUsingThreadPool, "#3");


			dq = new DispatcherQueue ("foo", new Dispatcher ());
			Assert.IsFalse (dq.IsUsingThreadPool, "#4");
			try {
				dq.IsUsingThreadPool = false;
				Assert.Fail ("#5");
			} catch (InvalidOperationException) {}

			dq.IsUsingThreadPool = true;
			Assert.IsFalse (dq.IsUsingThreadPool, "#6");
		}

		[Test]
		public void SuspendDoesntWorkWithCLRThreadPool ()
		{
			int doneThreads = 0;
			AutoResetEvent evt = new AutoResetEvent (false);

			Task t = new Task(() => {
				Interlocked.Increment (ref doneThreads);
				evt.Set ();
			});
			DispatcherQueue dq = new DispatcherQueue ();

			Assert.IsTrue (dq.Enqueue (t), "#1");
			Assert.AreEqual (0, dq.Count, "#2");
			evt.WaitOne ();
			Assert.AreEqual (1, doneThreads, "#3");

			dq.Suspend ();
			Assert.IsTrue (dq.IsSuspended, "#4");
			Assert.IsTrue (dq.Enqueue (t), "#5");
			Assert.IsTrue (evt.WaitOne (10000, false), "#6");

			Assert.AreEqual (2, doneThreads, "#7");
			Assert.AreEqual (0, dq.Count, "#8");
		}

		[Test]
		public void ResumeClearSuspendedState ()
		{
			DispatcherQueue dq = new DispatcherQueue ();
			Assert.IsFalse (dq.IsSuspended, "#1");
			dq.Suspend ();
			Assert.IsTrue (dq.IsSuspended, "#2");
			dq.Resume ();
			Assert.IsFalse (dq.IsSuspended, "#3");
			
		}

		[Test]
		public void TaskExceptionsPostOnTheUnhandledExceptionEvent ()
		{
			AutoResetEvent evt = new AutoResetEvent (false);
			int count = 0;
			object obj = null; 
			Exception origException = null;
			UnhandledExceptionEventArgs args = null;
			bool wasTPThread = false;

			Task t = new Task(() => { origException = new Exception ("hello"); throw origException; });
			
			DispatcherQueue dq = new DispatcherQueue ();
			dq.UnhandledException += (_obj, _args) => {
				++count;
				obj = _obj;
				args = _args;
				wasTPThread = Thread.CurrentThread.IsThreadPoolThread;
				evt.Set ();
			};
			Assert.IsTrue (dq.Enqueue (t), "#1");
			Assert.IsTrue (evt.WaitOne (5000, false), "#2");

			Assert.AreEqual (1, count, "#3");
			Assert.AreEqual (dq, obj, "#4");
			Assert.AreEqual (origException, args.ExceptionObject, "#5");
			Assert.IsFalse (args.IsTerminating, "#6");
			Assert.IsTrue (wasTPThread, "#7");
		}

		[Test]
		public void TaskExceptionDontBlowUpIfNoEventIsRegistered ()
		{
			AutoResetEvent evt = new AutoResetEvent (false);
			int count = 0;
			object obj = null; 
			Exception origException = null;
			UnhandledExceptionEventArgs args = null;
			bool wasTPThread = false;

			Task t = new Task(() => { origException = new Exception ("hello"); throw origException; });
			
			DispatcherQueue dq = new DispatcherQueue ();
			Assert.IsTrue (dq.Enqueue (t), "#1");
		}

		class NakedTaskQueueTask : Task, ITask {
			public bool getQueue, setQueue;
			public DispatcherQueue queue;

			public NakedTaskQueueTask () : base (() => {}) {}

			DispatcherQueue ITask.TaskQueue {
				get { getQueue = true; return queue; }
				set { queue = value; setQueue = true; }
			}
		}

		[Test]
		public void EnqueueSetTheTaskQueueProperty ()
		{
			DispatcherQueue dq = new DispatcherQueue ();
			var task = new NakedTaskQueueTask ();
			dq.Enqueue (task);
			Assert.IsTrue (task.setQueue, "#1");
			Assert.AreEqual (dq, task.queue, "#2");
		}

		class NakedDispatcher : DispatcherQueue
		{
			public override bool Enqueue (ITask task)
			{
				//Console.WriteLine ("\n---Enqueue---\n");
				throw new Exception ("not now");
			}

			public override bool TryDequeue (out ITask task)
			{
				 //Console.WriteLine ("\n---TryDequeue2 {0} -- {1}---\n", DateTime.Now.Ticks, new StackTrace (1));
				 return base.TryDequeue (out task);
			}

			public override void Suspend ()
			{
				Console.WriteLine ("\n---Suspend---\n");
				throw new Exception ("not now");
			}

			public override void Resume ()
			{
				Console.WriteLine ("\n---Resume---\n");
				throw new Exception ("not now");
			}

			public NakedDispatcher (Dispatcher d) : base ("bla", d) {}

			public bool _Enqueue (ITask task)
			{
				return base.Enqueue (task);
			}
		}

		[Test]
		public void HowTheDispatcherWork ()
		{
			AutoResetEvent evt = new AutoResetEvent (false);
			using (Dispatcher d = new Dispatcher ()) {
				var disp = new  NakedDispatcher (d); 
				disp._Enqueue (new Task (() => { evt.Set (); }));
				Assert.IsTrue (evt.WaitOne (2000), "#1");
			}
		}

		class MyReceiver : Receiver<int> {
			Port<int> port;
			public MyReceiver (Port<int> p) : base (p, null, (Task<int>)null) { this.port = p; }

			public override IEnumerator<ITask> Execute ()
			{
				Console.WriteLine ("execute handler {0} arbiter {1} ll {2}", ArbiterCleanupHandler, Arbiter, LinkedIterator);
				Console.WriteLine (new StackTrace ());
				//((IPortReceive)port).RegisterReceiver (this);
				return null;
			}

			public bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
			{
				Console.WriteLine ("eval handler {0} arbiter {1} ll {2}", ArbiterCleanupHandler, Arbiter, LinkedIterator);
				Console.WriteLine (new StackTrace ());
				deferredTask = null;
				return false;
			}
		}

		Port<int> iterPort;
		AutoResetEvent iterEvent;
		int iterRes;

		IEnumerator<ITask> SimpleTaskIterator ()
		{
			for (int i = 0; i < 5; ++i) {
				yield return iterPort.Receive ();
				int val = iterPort;
				iterRes += val;
			}
			iterEvent.Set ();
		}

		[Test]
		public void DispatchOfIterativeTask ()
		{
			iterPort = new Port<int> ();
			iterEvent = new AutoResetEvent (false);
			iterRes = 0;

			using (Dispatcher d = new Dispatcher ()) {
				var disp = new DispatcherQueue ("bla", d); 
				disp.Enqueue (new IterativeTask (this.SimpleTaskIterator));
				for (int i = 0; i < 5; ++i)
					iterPort.Post (i * 10);
				Assert.IsTrue (iterEvent.WaitOne (2000), "#1");
				Assert.AreEqual (100, iterRes, "#2");
			}
		}

		[Test]
		public void SuspendWithDispatcher ()
		{
			var evt = new AutoResetEvent (false);
			int res = 0;

			using (Dispatcher d = new Dispatcher ()) {
				var disp = new DispatcherQueue ("bla", d);
				ITask task = null;
				disp.Suspend ();

				Assert.IsTrue (disp.Enqueue (Arbiter.FromHandler (() => { ++res; evt.Set(); })), "#1");
				Assert.IsFalse (disp.TryDequeue (out task), "#2");
				Assert.IsNull (task, "#3");
				Assert.AreEqual (0, res, "#4");

				disp.Resume ();
				Assert.IsTrue (evt.WaitOne (2000),"#5");
				Assert.AreEqual (1, res, "#6");
			}
		}

		[Test]
		public void EnqueueAfterDispose ()
		{
			using (Dispatcher d = new Dispatcher ()) {
				var disp = new DispatcherQueue ("bla", d);
				disp.Dispose ();

				try {
					disp.Enqueue (Arbiter.FromHandler (() => { }));
					Assert.Fail ("#1");
				} catch (ObjectDisposedException) {}

				try {
					ITask task = null;
					disp.TryDequeue (out task);
					Assert.Fail ("#2");
				} catch (ObjectDisposedException) {}
			}
		}


		[Test]
		[Category ("NotDotNet")]
		public void EnqueueAfterDispose2 ()
		{
			var disp = new DispatcherQueue ();
			disp.Dispose ();

			try {
				disp.Enqueue (Arbiter.FromHandler (() => { }));
				Assert.Fail ("#1");
			} catch (ObjectDisposedException) {}

			try {
				ITask tk = null;
				disp.TryDequeue (out tk);
				Assert.Fail ("#2");
			} catch (ObjectDisposedException) {}
		}

		[Test]
		public void FixedQueueAndDiscardConstraint ()
		{
			var evt = new AutoResetEvent (false);
			int res = 0;
			using (Dispatcher d = new Dispatcher ()) {
				var disp = new DispatcherQueue ("bla", d, TaskExecutionPolicy.ConstrainQueueDepthDiscardTasks, 2);
				ITask task = null;
				disp.Suspend ();

				Assert.IsTrue (disp.Enqueue (Arbiter.FromHandler (() => { ++res; })), "#1");
				Assert.IsTrue (disp.Enqueue (Arbiter.FromHandler (() => { ++res; })), "#2");
				Assert.IsFalse (disp.Enqueue (Arbiter.FromHandler (() => { ++res; })), "#3");

				disp.Resume ();
				Thread.Sleep (10);
				Assert.IsTrue (disp.Enqueue (Arbiter.FromHandler (() => { evt.Set (); })), "#4");
				Assert.IsTrue (evt.WaitOne (2000), "#5");
				Assert.AreEqual (2, res, "#6");
			}
		}
	}
}
