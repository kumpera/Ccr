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
		Evaluate
			Under different States
			Cleanup
			LinkedInterator
			Null task (with/out arbiter)
		Execute
		Consume
		Cleanup
	*/
	[TestFixture]
	public class ReceiverTest
	{
		public class TrivialArbiter : Task, IArbiterTask
		{
			bool res;
			ITask task;

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

			public ITask UserTask { get { return base.UserTask; } }
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
			Assert.AreEqual (task, outTask, "#2");
			Assert.AreEqual (portElem, r [0], "#3");
		}	

		[Test]
		public void EvaluateWithArbiter ()
		{
			int cnt = 0;
			Task<int> task = new Task<int> ((a) => cnt += a);
			Port<int> port = new Port<int> ();
			Receiver r = new Receiver (port, task);
			r.Arbiter = new TrivialArbiter (false);

			IPortElement portElem = new PortElement<int> (10);
			ITask outTask = null;

			Assert.IsFalse (r.Evaluate (portElem, ref outTask), "#1");
			Assert.AreEqual (task, outTask, "#2");

			r = new Receiver (port, task);
			r.Arbiter = new TrivialArbiter (true);

			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#3");
			Assert.AreEqual (task, outTask, "#4");

			//Test what happens if the arbiter set's the resulting task
			Task<int> otherTask = new Task<int> ((a) => cnt = 99);
			r = new Receiver (port, task);
			r.Arbiter = new TrivialArbiter (true, otherTask);

			Assert.IsTrue (r.Evaluate (portElem, ref outTask), "#5");
			Assert.AreEqual (otherTask, outTask, "#6");
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

		class VoidDispatcherQueue : DispatcherQueue
		{
			public int queuedTasks;

			public override bool Enqueue (ITask task)
			{
				++queuedTasks;
				return true;
			}
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
		}
	}
}
