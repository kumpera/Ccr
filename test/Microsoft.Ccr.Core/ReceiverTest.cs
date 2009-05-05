//
// ReceiverTest.cs
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

using NUnit.Framework;

namespace Microsoft.Ccr.Core {
	/*
	TODO test the following things:
		Evaluate/Consume
			LinkedInterator
			ArbiterCleanupHandler
			
	*/
	[TestFixture]
	public class ReceiverTest
	{
		public class TrivialArbiter : Task, IArbiterTask
		{
			bool res;
			ITask task;
			public bool evaluateCalled;

			public TrivialArbiter (bool res) : base (() => {}) {
				this.res = res;
			}

			public TrivialArbiter (bool res, ITask task) : base (() => {}) {
				this.res = res;
				this.task = task;
			}

			public bool Evaluate(ReceiverTask receiver, ref ITask deferredTask)
			{
				if (res && task != null)
					deferredTask = task;
				evaluateCalled = true;
				//Console.WriteLine ("EVALUATE {0} --- {1}", receiver, deferredTask);
				return res;
			}

			public ArbiterTaskState ArbiterState { get { throw new NotImplementedException (); } }
		}

		public class ExposeUserTaskReceiver : Receiver
		{
			public ExposeUserTaskReceiver (IPortReceive port, ITask task) : base (port, task)
			{
			}
			public ExposeUserTaskReceiver (bool persist, IPortReceive port, ITask task) : base (persist, port, task)
			{
			}

			public ITask UserTask { get { return base.UserTask; } }
		}

		public class VoidDispatcherQueue : DispatcherQueue
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

		[Test]
		[Category ("NotDotNet")]
		public void BadCtor ()
		{
			Task t = new Task (() => {});
			Port<int> p = new Port<int> ();
			try {
				new Receiver (null, t);
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {}
		}

		[Test]
		public void CtorResultingProperties ()
		{
			Task<int> t = new Task<int> ((a) => {});
			Port<int> p = new Port<int> ();
			ExposeUserTaskReceiver r = new ExposeUserTaskReceiver (p, t);

			Assert.AreEqual (t, r.UserTask, "#1");
			Assert.AreEqual (ReceiverTaskState.Onetime, r.State, "#2");
			Assert.AreEqual (1, r.PortElementCount, "#3");
		}

		[Test]
		public void CtorResultingPropertiesNullTask ()
		{
			Task<int> t = new Task<int> ((a) => {});
			Port<int> p = new Port<int> ();
			ExposeUserTaskReceiver r = new ExposeUserTaskReceiver (p, null);

			Assert.IsNull (r.UserTask, "#1");
			Assert.AreEqual (ReceiverTaskState.Onetime, r.State, "#2");
			Assert.AreEqual (0, r.PortElementCount, "#3");
		}
		[Test]

		public void PersistentCtorResultingProperties ()
		{
			Task<int> t = new Task<int> ((a) => {});
			Port<int> p = new Port<int> ();
			ExposeUserTaskReceiver r = new ExposeUserTaskReceiver (true, p, t);

			Assert.AreEqual (t, r.UserTask, "#1");
			Assert.AreEqual (ReceiverTaskState.Persistent, r.State, "#2");
			Assert.AreEqual (1, r.PortElementCount, "#3");
		}

		[Test]
		public void IndexWithNoTask ()
		{
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (port, null);	

			var elem = new PortElement<int> (2);
			try {
				r [0] = elem;
				Assert.Fail ("#1");
			} catch (NullReferenceException) {}
		}

		[Test]
		public void IndexManipulateTaskDirectly ()
		{
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (port, task);	

			var elem = new PortElement<int> (2);
			task [0] = elem;

			Assert.AreEqual (elem, r [0], "#1");
			elem = new PortElement<int> (20);
			r [0] = elem;
			Assert.AreEqual (elem, r [0], "#2");

			try {
				r [0] = new PortElement <double> (2);
				Assert.Fail ("#3");
			} catch (InvalidCastException) {}
		}

		[Test]
		public void EvaluateSimple ()
		{
			int cnt = 0;
			Task<int> task = new Task<int> ((a) => cnt += a);
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (port, task);	

			IPortElement portElem = new PortElement<int> (10);
			ITask outTask = null;

			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#1");
			Assert.AreEqual (task, outTask, "#2"); //outTask
			Assert.AreEqual (portElem, r [0], "#3");
		}	

		[Test]
		public void EvaluateWithArbiter ()
		{
			int cnt = 0;
			Task<int> task = new Task<int> ((a) => cnt += a);
			Port<int> port = new Port<int> ();
			var arb = new TrivialArbiter (false);
			Receiver r = new Receiver (port, task);
			r.Arbiter = arb;

			IPortElement portElem = new PortElement<int> (10);
			ITask outTask = null;

			Assert.IsFalse (arb.evaluateCalled, "#8");
			Assert.IsFalse (r.Evaluate (portElem, ref outTask), "#1");
			Assert.AreEqual (task, outTask, "#2");
			Assert.IsTrue (arb.evaluateCalled, "#9");

			r = new Receiver (port, task);
			r.Arbiter = arb =  new TrivialArbiter (true);

			Assert.IsFalse (arb.evaluateCalled, "#10");

			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#3");
			Assert.AreEqual (task, outTask, "#4");
			Assert.IsTrue (arb.evaluateCalled, "#11");

			//Test what happens if the arbiter set's the resulting task
			Task<int> otherTask = new Task<int> ((a) => cnt = 99);
			r = new Receiver (port, task);
			r.Arbiter = new TrivialArbiter (true, otherTask);

			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#5");
			Assert.AreEqual (otherTask, outTask, "#6");
			Assert.AreEqual (portElem, r [0], "#7");
		}

		public class RecordPartialCloneTask : Task<int>
		{
			public bool cloneCalled;

			public RecordPartialCloneTask () : base ((A) => {}) {
			}

			public override ITask PartialClone ()
			{
				cloneCalled = true;
				return base.PartialClone ();
			}
		}

		[Test]
		public void EvaluatePersistentReceiver ()
		{
			var task = new RecordPartialCloneTask ();
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (true, port, task);
			var dq = new VoidDispatcherQueue ();
			var arb = new TrivialArbiter (false);
			arb.TaskQueue = dq;

			IPortElement portElem = new PortElement<int> (10);
			ITask outTask = null;

			Assert.IsFalse (task.cloneCalled, "#1");
			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#2");
			Assert.IsTrue (task.cloneCalled, "#3");
			Assert.AreNotEqual (task, outTask, "#4");
			Assert.AreEqual (portElem, outTask [0], "#5");
			Assert.IsNull (r [0], "#6");
			Assert.IsNull (task [0], "#7");
			Assert.IsNull (task.TaskQueue, "#8");
		}

		[Test]
		public void ReceiverWithNullTask ()
		{
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (false, port, null);

			IPortElement portElem = new PortElement<int> (10);

			ITask outTask = null;
			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#1");
			Assert.IsNull (outTask, "#2");

			r = new Receiver (true, port, null);
			outTask = null;
			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#3");
			Assert.IsNull (outTask, "#4");
			
		}

		[Test]
		public void UserTaskAfterEvaluate ()
		{
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();
			ExposeUserTaskReceiver r = new ExposeUserTaskReceiver (port, task);

			IPortElement portElem = new PortElement<int> (10);
			ITask outTask = null;

			Assert.AreEqual (task, r.UserTask, "#1");
			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#2");
			Assert.AreEqual (task, outTask, "#3");
			Assert.AreEqual (task, r.UserTask, "#4");
			Assert.AreEqual (portElem, r [0], "#5");
		} 

		[Test]
		public void ArbiterCanBeSetManyTimes ()
		{
			//LAME DOCS/API The API is bulshiting us about that Arbiter can be set only once 
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (port, task);
			r.Arbiter = new TrivialArbiter (false);
			r.Arbiter = new TrivialArbiter (false);
			((ReceiverTask)r).Arbiter = new TrivialArbiter (false);
			((ReceiverTask)r).Arbiter = new TrivialArbiter (false);
		}

		[Test]
		public void PortRegisteringAsSideEffectOfSettingTheArbiter ()
		{
			IPortReceive port = new Port<int> ();
			Receiver r = new Receiver (port, null);
			var dq = new VoidDispatcherQueue ();
			var arb = new TrivialArbiter (false);
			arb.TaskQueue = dq;
			r.Arbiter = arb;

			Assert.AreEqual (1, port.GetReceivers ().Length, "#1"); //LAMEIMPL super weird behavior
			Assert.AreEqual (arb, r.Arbiter, "#2");
			Assert.AreEqual (dq, r.TaskQueue, "3");
		}

		[Test]
		public void VerifyExecuteSideEffects ()
		{
			IPortReceive port = new Port<int> ();
			Receiver r = new Receiver (port, null);
			var dq = new VoidDispatcherQueue ();
			var arb = new TrivialArbiter (false);
			r.TaskQueue = dq;
			r.Arbiter = arb;

			Assert.AreEqual (1, port.GetReceivers ().Length, "#1"); //yeah, weird, look at previous test
			Assert.AreEqual (arb, r.Arbiter, "#2");

			r.Execute ();
			Assert.AreEqual (2, port.GetReceivers ().Length, "#3");
			Assert.AreEqual (r, port.GetReceivers () [0], "#4");
			Assert.AreEqual (r, port.GetReceivers () [1], "#5");
			Assert.IsNull (r.Arbiter, "#6");
		}

		[Test]
		public void VerifyExecuteSideEffectsWithoutAnArbiter ()
		{
			IPortReceive port = new Port<int> ();
			Receiver r = new Receiver (port, null);
			var dq = new VoidDispatcherQueue ();
			r.TaskQueue = dq;

			Assert.AreEqual (0, port.GetReceivers ().Length, "#1");
			Assert.IsNull (r.Execute (), "#2");
			Assert.AreEqual (1, port.GetReceivers ().Length, "#3");
			Assert.AreEqual (r, port.GetReceivers () [0], "#4");
		}

		[Test]
		public void CleanupUnregisterReceiver ()
		{
			IPortReceive port = new Port<int> ();
			Receiver r = new Receiver (port, null);
			var dq = new VoidDispatcherQueue ();
			r.TaskQueue = dq;
			Assert.IsNull (r.Execute (), "#1");
			Assert.AreEqual (1, port.GetReceivers ().Length, "#2");
			r.Cleanup ();
			Assert.AreEqual (0, port.GetReceivers ().Length, "#3");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, r.State, "#4");
		}

		[Test]
		public void CleanupTask ()
		{
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (port, task);

			IPortElement portElem = new PortElement<int> (10);
			Task<int> other = new Task<int> ((a) => {});
			other [0] = portElem;

			Assert.AreEqual (0, port.ItemCount, "#1");
			r.Cleanup (other);
			Assert.AreEqual (1, port.ItemCount, "#2");

			other = new Task<int> ((a) => {});
			try {
				r.Cleanup (other); //Port will fail due to null PortElement
				Assert.Fail ("#3");
			} catch (NullReferenceException) {}
		}

		[Test]
		public void SimpleConsume ()
		{
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();
			var dq = new VoidDispatcherQueue ();

			Receiver r = new Receiver (port, task);
			r.TaskQueue = dq;

			IPortElement portElem = new PortElement<int> (10);

			r.Consume (portElem);
			Assert.AreEqual (1, dq.queuedTasks, "#1");
			Assert.AreNotEqual (task, dq.lastQueuedTask, "#2"); //cloned
			Assert.AreEqual (portElem, dq.lastQueuedTask [0], "#3");
			Assert.AreNotEqual (portElem, task [0], "#4");
		}

		[Test]
		public void ConsumeAfterCleanup ()
		{
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();
			var dq = new VoidDispatcherQueue ();

			Receiver r = new Receiver (port, task);
			r.TaskQueue = dq;
			r.Cleanup ();

			IPortElement portElem = new PortElement<int> (10);

			r.Consume (portElem);
			Assert.AreEqual (0, dq.queuedTasks, "#1");
			Assert.IsNull (r [0], "#2");
		}

		[Test]
		public void ConsumeIgnoredArbiter ()
		{
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();
			var dq = new VoidDispatcherQueue ();
			var arb = new TrivialArbiter (false);

			Receiver r = new Receiver (port, task);

			r.TaskQueue = dq;
			r.Arbiter = arb;

			IPortElement portElem = new PortElement<int> (10);

			r.Consume (portElem);
			Assert.AreEqual (1, dq.queuedTasks, "#1");
			Assert.AreNotEqual (task, dq.lastQueuedTask, "#2");
			Assert.AreEqual (portElem, dq.lastQueuedTask [0], "#3");
			Assert.IsFalse (arb.evaluateCalled, "#3");
		}

		[Test]
		public void ConsumeFailsWithoutTaskQueue ()
		{
			Task<int> task = new Task<int> ((a) => {});
			Port<int> port = new Port<int> ();

			Receiver r = new Receiver (port, task);

			IPortElement portElem = new PortElement<int> (10);

			try {
				r.Consume (portElem);
				Assert.Fail ("#1");
			} catch (NullReferenceException) {}
		}
	}
}
