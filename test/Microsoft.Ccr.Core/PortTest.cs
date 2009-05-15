//
// PortTest.cs
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
using System.Reflection;
using Microsoft.Ccr.Core.Arbiters;

using NUnit.Framework;

namespace Microsoft.Ccr.Core {

	[TestFixture]
	public class PortTest
	{
		[Test]
		public void PortStatusAfterConstruction ()
		{
			var p = new Port<int> ();
			int val = 0;

			Assert.IsFalse (p.Test (out val), "#1");

			IPortReceive rec = p;
			Assert.AreEqual (0, rec.GetItems ().Length, "#r1");
			Assert.AreEqual (0, rec.GetReceivers ().Length, "#r2");
			Assert.IsNull (rec.Test (), "#r3");
			Assert.AreEqual (0, rec.ItemCount, "#r4");

			IPortArbiterAccess paa = p;
			Assert.AreEqual (PortMode.Default, paa.Mode, "#p1");
		}

		[Test]
		public void PostThenReceive ()
		{
			var p = new Port<int> ();
			p.Post (10);

			int res;
			Assert.IsTrue (p.Test (out res), "#1");
			Assert.AreEqual (10, res, "#2");
		}

		[Test]
		public void PostAndReceiveOrdering ()
		{
			var p = new Port<int> ();
			p.Post (10);
			p.Post (20);
			p.Post (30);

			int res;
			Assert.IsTrue (p.Test (out res), "#1");
			Assert.AreEqual (10, res, "#2");

			Assert.IsTrue (p.Test (out res), "#3");
			Assert.AreEqual (20, res, "#4");

			Assert.IsTrue (p.Test (out res), "#5");
			Assert.AreEqual (30, res, "#6");
		}

		[Test]
		public void TestWithEmptyPort ()
		{
			var p = new Port<int> ();

			int res = 99;
			Assert.IsFalse (p.Test (out res), "#1");
			Assert.AreEqual (0, res, "#2");
		}

		[Test]
		public void NonGenericTest ()
		{
			var p = new Port<int> ();
			p.Post (10);

			IPortReceive ir = p;
			Assert.AreEqual (10, ir.Test (), "#1");			
			Assert.AreEqual (null, ir.Test (), "#1");			
		}

		class MyTask : ITask
		{
			public ITask PartialClone ()
			{
				Console.WriteLine ("PartialClone"); 
				throw new NotImplementedException ();
				return null;
			}

			public IEnumerator<ITask> Execute ()
			{
				Console.WriteLine ("executing task");
				return null;
			}

			public int PortElementCount
			{
				get { Console.WriteLine ("GET count"); throw new NotImplementedException (); }
			}
	
			public IPortElement this[int index]
			{
				get { Console.WriteLine ("GET idx"); throw new NotImplementedException (); }
				set { Console.WriteLine ("SET idx"); throw new NotImplementedException (); }
			}

			public Handler ArbiterCleanupHandler
			{
				get { Console.WriteLine ("GET handler"); throw new NotImplementedException (); }
				set { Console.WriteLine ("SET handler"); throw new NotImplementedException (); }
			}
			public Object LinkedIterator
			{
				get { Console.WriteLine ("GET LINKINT"); throw new NotImplementedException (); }
				set { Console.WriteLine ("SET LINKINT"); throw new NotImplementedException (); }
			}
			public DispatcherQueue TaskQueue 
			{
				get { Console.WriteLine ("GET taskK"); throw new NotImplementedException (); }
				set { Console.WriteLine ("SET taskK"); throw new NotImplementedException (); }
			}
		}

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

		class NullTask : TaskCommon
		{
			public override ITask PartialClone ()
			{
				return null;
			}

			public override IEnumerator<ITask> Execute ()
			{
				return null;
			}
			public override IPortElement this[int index]
			{
				get { return null; }
				set { }
			}

			public override int PortElementCount
			{
				get { return 0; }
			}
		}

		class EvalTask : ReceiverTask
		{
			public bool eval;
			public int tested;
			public ITask task;

			public EvalTask (bool eval) {  this.eval = eval; }
			public EvalTask (bool eval, ITask task) {  this.eval = eval; this.task = task; }

			public override void Cleanup (ITask taskToCleanup)
			{
			}
	
			public override void Consume (IPortElement item)
			{
			}

			public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
			{
				++tested;
				if (task != null)
					deferredTask = task;
				return eval;
			}
		}

		[Test]
		[Category ("NotDotNet")]
		public void ReceiverThatReturnsDeferredTaskAndHaveNoTaskQueue ()
		{
			var p = new Port<int> ();
			IPortReceive ipr = p;
			ipr.RegisterReceiver (new EvalTask (true, new NullTask ()));
		}

		[Test]
		public void OneShotReceiverReturnsTask ()
		{
			var p = new Port<int> ();
			IPortReceive ipr = p;
			VoidDispatcherQueue dq = new VoidDispatcherQueue ();
			ReceiverTask rt = new EvalTask (true, new NullTask ());
			rt.State = ReceiverTaskState.Onetime;
			rt.TaskQueue = dq;
			ipr.RegisterReceiver (rt);

			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#1");
			p.Post (10);
			Assert.AreEqual (1, dq.queuedTasks, "#2");
			Assert.AreEqual (0, ipr.GetReceivers ().Length, "#3");
		}

		[Test]
		public void PersistentReceiverReturnsTask ()
		{
			var p = new Port<int> ();
			IPortReceive ipr = p;
			VoidDispatcherQueue dq = new VoidDispatcherQueue ();
			ReceiverTask rt = new EvalTask (true, new NullTask ());
			rt.State = ReceiverTaskState.Persistent;
			rt.TaskQueue = dq;
			ipr.RegisterReceiver (rt);

			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#1");
			p.Post (10);
			Assert.AreEqual (1, dq.queuedTasks, "#2");
			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#3");
			p.Post (20);
			Assert.AreEqual (2, dq.queuedTasks, "#4");
			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#5");
		}


		[Test]
		public void CleanedUpReceiverReturnsTask ()
		{
			var p = new Port<int> ();
			IPortReceive ipr = p;
			VoidDispatcherQueue dq = new VoidDispatcherQueue ();
			ReceiverTask rt = new EvalTask (true, new NullTask ());
			rt.State = ReceiverTaskState.CleanedUp;
			rt.TaskQueue = dq;
			ipr.RegisterReceiver (rt);

			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#1");
			p.Post (10);
			Assert.AreEqual (1, dq.queuedTasks, "#2");
			Assert.AreEqual (0, ipr.GetReceivers ().Length, "#3");
		}

		[Test]
		public void RegisterReceiverChangesGetReceivers ()
		{
			IPortReceive ipr = new Port<int> ();	
			ReceiverTask rt = new EvalTask (true);

			Assert.AreEqual (0, ipr.GetReceivers ().Length, "#1");
			ipr.RegisterReceiver (rt);			
			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#2");
			ipr.RegisterReceiver (rt);			
			Assert.AreEqual (2, ipr.GetReceivers ().Length, "#3");
		}

		[Test]
		[Category ("NotDotNet")]
		public void UnRegisterThrowsOnNull ()
		{
			try {
				var p = new Port<int> ();
				IPortReceive ipr = p;
				ipr.UnregisterReceiver (null);
				Assert.Fail ("#1");
			} catch (ArgumentNullException){}
		}


		[Test]
		public void UnRegisterReceiverChangesGetReceivers ()
		{
			IPortReceive ipr = new Port<int> ();	
			ReceiverTask rt = new EvalTask (true);

			Assert.AreEqual (0, ipr.GetReceivers ().Length, "#1");
			ipr.RegisterReceiver (rt);			
			ipr.RegisterReceiver (rt);			
			Assert.AreEqual (2, ipr.GetReceivers ().Length, "#3");
			ipr.UnregisterReceiver (rt);
			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#4");
			ipr.UnregisterReceiver (rt);
			Assert.AreEqual (0, ipr.GetReceivers ().Length, "#5");
		}
		
		[Test]
		public void UnRegisterReceiverIgnoreNonRegistered ()
		{
			IPortReceive ipr = new Port<int> ();	
			ReceiverTask rt = new EvalTask (true);
			ipr.UnregisterReceiver (rt);
		}

		[Test]
		[Category ("NotDotNet")]
		public void RegisterThrowsOnNull ()
		{
			try {
				var p = new Port<int> ();
				IPortReceive ipr = p;
				ipr.RegisterReceiver (null);
				Assert.Fail ("#1");
			} catch (ArgumentNullException){}
		}

		[Test]
		public void PostOnlyEvalUntilOneAccept ()
		{
			var p = new Port<int> ();
			IPortReceive ipr = p;
			EvalTask a,b,c;
			ipr.RegisterReceiver (a = new EvalTask (false));
			ipr.RegisterReceiver (b = new EvalTask (true));
			ipr.RegisterReceiver (c = new EvalTask (false));

			p.Post (10);
			Assert.AreEqual (1, a.tested, "#1");
			Assert.AreEqual (1, b.tested, "#2");
			Assert.AreEqual (0, c.tested, "#3");
		}

		[Test]
		public void RegisterDuplicateTask ()
		{
			var p = new Port<int> ();
			IPortReceive ipr = p;
			EvalTask a,b,c;
			ipr.RegisterReceiver (a = new EvalTask (false));
			ipr.RegisterReceiver (a);	
			ipr.RegisterReceiver (b = new EvalTask (true));

			p.Post (10);
			Assert.AreEqual (2, a.tested, "#1");
			Assert.AreEqual (1, b.tested, "#2");
		}

		[Test]
		public void IfReceiverReturnsTrueMessageIsNotEnqueued ()
		{
			var p = new Port<int> ();
			int tmp;
			IPortReceive ipr = p;
			ipr.RegisterReceiver (new EvalTask (false));

			p.Post (10);
			Assert.IsTrue (p.Test (out tmp), "#1");

			p = new Port <int> ();
			ipr = p;
			ipr.RegisterReceiver (new EvalTask (true));
			p.Post (10);
			Assert.IsFalse (p.Test (out tmp), "#2");
		}

		[Test]
		public void ImplicitCastRemovesElement ()
		{
			var p = new Port<int> ();
			int x;
			p.Post (10);
			x = p;
			Assert.AreEqual (10, x, "#1");
			x = p;
			Assert.AreEqual (0, x, "#2");
		}

		[Test]
		public void PostUnknownType ()
		{
			var p = new Port<int> ();
			int tmp;

			p.PostUnknownType (10);
			Assert.AreEqual (1, p.ItemCount, "#1");
			p.Post (20);
			Assert.AreEqual (2, p.ItemCount, "#2");
			Assert.AreEqual (10, p.Test (), "#3");
			p.Test (out tmp);
			Assert.AreEqual (20, tmp, "#4");
		}

		[Test]
		public void PostUnknownTypeThrowsOnBadType ()
		{
			var p = new Port<int> ();
			object obj = new object ();
			try {
				p.PostUnknownType (obj);
				Assert.Fail ("#1");
			} catch (InvalidCastException ex) {} /*LAMEDOCS LAMEIMPL*/
			/*} catch (PortNotFoundException ex) {
				Assert.AreEqual (p, ex.Port);
				Assert.AreEqual (obj, ex.ObjectPosted);
				Assert.AreEqual ("zzz", ex.Message);
			}*/
		}

		[Test]
		public void PostUnknownTypeThrowsOnNull ()
		{
			var p = new Port<int> ();
			try {
				p.PostUnknownType (null);
				Assert.Fail ("#1");
			} catch (NullReferenceException ex) {} /*LAMEDOCS LAMEIMPL*/
		}

		[Test]
		public void TryPostUnknownType ()
		{
			var p = new Port<int> ();
			int tmp;

			Assert.IsTrue (p.TryPostUnknownType (10), "#1");
			Assert.AreEqual (1, p.ItemCount, "#2");
			Assert.IsTrue (p.TryPostUnknownType (20), "#3");
			Assert.AreEqual (2, p.ItemCount, "#4");
			Assert.IsFalse (p.TryPostUnknownType ("oi"), "#5");
			Assert.AreEqual (2, p.ItemCount, "#6");
			try {
				p.TryPostUnknownType (null);
				Assert.Fail ("#7");
			} catch (NullReferenceException ex) {} /*LAMEDOCS LAMEIMPL*/

		}

		[Test]
		public void Clear ()
		{
			var p = new Port<int> ();

			p.Post (10);
			p.Post (10);
			Assert.AreEqual (2, p.ItemCount, "#1");
			p.Clear ();
			Assert.AreEqual (0, p.ItemCount, "#2");
			p.Clear ();
			Assert.AreEqual (0, p.ItemCount, "#3");
		}

		[Test]
		public void PostElementBad ()
		{
			var p = new Port<int> ();
;			var elem = new PortElement<double> (99);
			try {
				p.PostElement (elem);
				Assert.Fail ("#1");
			} catch (InvalidCastException) {} /*LAMEIMPL, stupid exception to throw*/
			try {
				p.PostElement (null);
				Assert.Fail ("#2");
			} catch (NullReferenceException) {} /*LAMEIMPL, stupid exception to throw*/

		}

		[Test]
		public void PostElementThenTestForElement ()
		{
			int tmp;
			var p = new Port<int> ();
;			var elem = new PortElement<int> (99);
			p.PostElement (elem);

			Assert.AreEqual (1, p.ItemCount, "#1");
			var res = p.TestForElement ();
			Assert.AreEqual (elem, res, "#2");
			Assert.AreEqual (0, p.ItemCount, "#3");
			Assert.AreEqual (null, res.Owner, "#4");
		}

		[Test]
		public void PostElementCausesLinking ()
		{
			var p = new Port<int> ();
			p.Post (10);
			p.PostElement (new PortElement<int> (20));
			p.Post (30);

			var r0 = p.TestForElement ();

			var r1 = r0.Next;
			Assert.IsNotNull (r1, "#1");
			var r2 = r1.Next;
			Assert.IsNotNull (r2, "#1");
			Assert.AreEqual (r1, r2.Previous, "#2");
			Assert.AreEqual (r2, r1.Previous, "#3");
		}

		[Test]
		public void PostThenTestForElement ()
		{
			var p = new Port<int> ();
			p.Post (33);
			p.Post (44);

			var res = p.TestForElement ();
			Assert.IsNotNull (res, "#1");
			Assert.IsNull (res.CausalityContext, "#1");
			Assert.AreEqual (33, res.Item,  "#2");
			Assert.IsNotNull (res.Next, "#3");
			Assert.AreEqual (p, res.Owner, "#4");
			Assert.AreEqual (res.Next, res.Previous, "#5");

			Assert.IsTrue (res is IPortElement<int>,  "#6");
			var typed = (IPortElement<int>)res;
			Assert.AreEqual (33, typed.TypedItem, "#7");
		}

		[Test]
		public void TestPortElementNextPropertyWithSinglePost ()
		{
			var p = new Port<int> ();
			p.Post (33);
			var res0 = p.TestForElement ();

			Assert.AreEqual (res0, res0.Next, "#1");
			Assert.AreEqual (res0, res0.Previous, "#2");
		}

		[Test]
		public void TestPortElementNextProperty ()
		{
			var p = new Port<int> ();
			p.Post (33);
			p.Post (44);
			p.Post (55);
			p.Post (66);

			var res0 = p.TestForElement ();
			var res1 = res0.Next;
			var res2 = res1.Next;
			var res3 = res2.Next;

			Assert.AreEqual (33, res0.Item, "#1");
			Assert.AreEqual (res1, res0.Next, "#2");
			Assert.AreEqual (res3, res0.Previous, "#12");

			
			Assert.AreEqual (44, res1.Item, "#3");
			Assert.IsNotNull (res1.Next, "#4");
			Assert.AreEqual (res3, res1.Previous, "#5");

			Assert.AreEqual (55, res2.Item, "#6");
			Assert.IsNotNull (res2.Next, "#7");
			Assert.AreEqual (res1, res2.Previous, "#8");

			Assert.AreEqual (66, res3.Item, "#9");
			Assert.AreEqual (res1, res3.Next, "#10");
			Assert.AreEqual (res2, res3.Previous, "#11");
		}

		[Test]
		public void TestGetItems ()
		{
			var p = new Port<int> ();
			p.Post (33);
			p.Post (55);

			object[] items = ((IPortReceive)p).GetItems ();

			Assert.IsNotNull (items, "#1");
			Assert.AreEqual (2, items.Length, "#2");
			Assert.IsTrue (items [0] is PortElement<int>, "#3");
			PortElement<int> a = (PortElement<int>)items [0];
			Assert.AreEqual (33, a.TypedItem, "#4");

			//Check if they don't clone the elements
			object[] items2 = ((IPortReceive)p).GetItems ();
			Assert.AreSame (a, items2 [0], "#5");
		}

		[Test]
		public void TestForMultipleElements  ()
		{
			var p = new Port<int> ();
			p.Post (33);
			p.Post (55);
			p.Post (44);

			IPortElement[] res = p.TestForMultipleElements (2);

			Assert.IsNotNull (res, "#1");
			Assert.AreEqual (2, res.Length, "#2");
			Assert.AreEqual (1, p.ItemCount, "#3");
			Assert.AreEqual (33, res [0].Item, "#4");
			Assert.AreEqual (55, res [1].Item, "#5");

			res = p.TestForMultipleElements (2);

			Assert.IsNull (res, "#6");
			Assert.AreEqual (1, p.ItemCount, "#7");
		}

		[Test]
		public void LinkStateAfterATestForMultipleElements ()
		{
			var p = new Port<int> ();
			p.Post (33);
			p.Post (55);
			p.Post (44);
			p.Post (77);


			IPortElement[] res = p.TestForMultipleElements (2);
			IPortElement a = res [0];
			IPortElement b = res [1];
			IPortElement c = (IPortElement)((IPortReceive)p).GetItems ()[0];
			IPortElement d = (IPortElement)((IPortReceive)p).GetItems ()[1];
	
			Assert.AreEqual (b, a.Next, "#1");
			Assert.AreEqual (c, b.Next, "#2");//LAMEIMPL MS doesn't unlink the resulting elements
			Assert.AreEqual (d, c.Next, "#3");
			Assert.AreEqual (c, d.Next, "#4");

			Assert.AreEqual (d, a.Previous, "#5");
			Assert.AreEqual (d, b.Previous, "#6"); //LAMEIMPL now this is SICK, previos always point to the last element
			Assert.AreEqual (d, c.Previous, "#7");
			Assert.AreEqual (c, d.Previous, "#8");
		}
		
		[Test]
		public void PostElementPrependOnTheQueue ()
		{
			Port<int> p = new Port<int> ();

			p.Post (10);
			p.Post (20);
			p.PostElement (new PortElement<int> (30));

			Assert.AreEqual (30, (int)p, "#1");
			Assert.AreEqual (10, (int)p, "#2");
			Assert.AreEqual (20, (int)p, "#3");
		}

		[Test]
		public void LinkStateAfterPostElement ()
		{
			Port<int> p = new Port<int> ();

			p.Post (10);
			p.Post (20);
			p.PostElement (new PortElement<int> (30));
			p.PostElement (new PortElement<int> (40));

			IPortReceive rec = p;
			IPortElement a = (IPortElement)rec.GetItems () [0];
			IPortElement b = (IPortElement)rec.GetItems () [1];
			IPortElement c = (IPortElement)rec.GetItems () [2];
			IPortElement d = (IPortElement)rec.GetItems () [3];


			Assert.AreEqual (b, a.Next, "#1");
			Assert.AreEqual (c, b.Next, "#2");
			Assert.AreEqual (d, c.Next, "#3");
			Assert.AreEqual (a, d.Next, "#4");

			Assert.AreEqual (d, a.Previous, "#5");
			Assert.AreEqual (a, b.Previous, "#6");
			Assert.AreEqual (b, c.Previous, "#7");
			Assert.AreEqual (c, d.Previous, "#8");

			Assert.AreEqual (40, a.Item, "#9");
			Assert.AreEqual (30, b.Item, "#10");
			Assert.AreEqual (10, c.Item, "#11");
			Assert.AreEqual (20, d.Item, "#12");
		}

		[Test]
		public void ImplicitCastPortToReceiver ()
		{
			var port = new Port<int> ();
			var receiver = (Receiver<int>)port;
			var dq = new VoidDispatcherQueue ();

			receiver.TaskQueue = dq;

			Assert.IsNull (receiver.Arbiter, "#1");
			Assert.IsNull (receiver.ArbiterCleanupHandler, "#1.1");
			Assert.AreEqual (ReceiverTaskState.Onetime, receiver.State, "#2");
			Assert.IsNull (receiver.Execute (), "#3");
			Assert.AreEqual (ReceiverTaskState.Onetime, receiver.State, "#4");

			IPortReceive rec = port;
			Assert.AreEqual (1, rec.GetReceivers ().Length, "#5");

			ITask res = null;
			Assert.IsFalse (receiver.Evaluate (new PortElement<int>(10), ref res), "#6");
			Assert.IsNotNull (res, "#7");
			Assert.IsNull (res.Execute (), "#8");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, receiver.State, "#9");
			Assert.AreEqual (0, rec.GetReceivers ().Length, "#10");
		}

		[Test]
		public void ScheduleAllTaskReturnedByEvaluate ()
		{
			Task tk0 = new Task(()=>{});
			Task tk1 = new Task(()=>{});
			EvalTask rv0 = new EvalTask (false, tk0); 
			VoidDispatcherQueue dq0 = new VoidDispatcherQueue ();
			rv0.TaskQueue = dq0; 
			EvalTask rv1 = new EvalTask (false, tk1); 
			VoidDispatcherQueue dq1 = new VoidDispatcherQueue ();
			rv1.TaskQueue = dq1; 

			Port<int> port = new Port<int> ();
			IPortReceive ipr = port;

			ipr.RegisterReceiver (rv0);
			ipr.RegisterReceiver (rv1);

			port.Post (10);

			Assert.AreEqual (1, dq0.queuedTasks, "#1");
			Assert.AreEqual (1, dq1.queuedTasks, "#2");
			Assert.AreEqual (2, ipr.GetReceivers ().Length, "#3");
		}

		[Test]
		public void DontRemoveOnetimeReceiversThatReturnFalse ()
		{
			Task tk0 = new Task(()=>{});
			Task tk1 = new Task(()=>{});
			EvalTask rv0 = new EvalTask (true, tk0); 
			VoidDispatcherQueue dq0 = new VoidDispatcherQueue ();
			rv0.TaskQueue = dq0; 
			EvalTask rv1 = new EvalTask (false, tk1); 
			VoidDispatcherQueue dq1 = new VoidDispatcherQueue ();
			rv1.TaskQueue = dq1; 

			Port<int> port = new Port<int> ();
			IPortReceive ipr = port;

			ipr.RegisterReceiver (rv0);

			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#1");
			port.Post (10);
			Assert.AreEqual (1, dq0.queuedTasks, "#2");
			Assert.AreEqual (0, ipr.GetReceivers ().Length, "#3");

			ipr.RegisterReceiver (rv1);
			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#4");
			port.Post (10);
			Assert.AreEqual (1, dq1.queuedTasks, "#5");
			Assert.AreEqual (1, ipr.GetReceivers ().Length, "#6");
		}


		class MyReceiver : ReceiverTask
		{
			ITask task;
			public int consume = 0;
			public int eval = 0;
			bool eval_res;

			public MyReceiver (ITask task) : this (true, task) {}
			public MyReceiver (bool eval_res, ITask task)
			{
				this.task = task;
				this.eval_res = eval_res;
			}
	
			public override void Cleanup (ITask taskToCleanup)
			{
			}
	
			public override void Consume (IPortElement item)
			{
				++consume;
			}
	
			public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
			{
				++eval;
				return eval_res;
			}
		}

		[Test]
		public void OptimizedSingleReissueReceiver ()
		{
			Port<int> p = new Port <int> ();
			p.Mode = PortMode.OptimizedSingleReissueReceiver;
			var rec = new MyReceiver (Arbiter.FromHandler (() =>{}));
			rec.State = ReceiverTaskState.Persistent;

			((IPortReceive)p).RegisterReceiver (rec);
			p.Post (10);
			Assert.AreEqual (0, rec.eval, "#1");
			Assert.AreEqual (1, rec.consume, "#2");
			Assert.AreEqual (0, p.ItemCount, "#3");
		}

		[Test]
		public void NewReceiverGetAChancewithCurrentItems1 ()
		{
			Port<int> p = new Port <int> ();
			var receiver = new MyReceiver ( Arbiter.FromHandler (() =>{}));
			p.Post (10);
			p.Post (20);
			p.Post (30);

			IPortReceive pr = p;
			pr.RegisterReceiver (receiver); 

			Assert.AreEqual (1, receiver.eval, "#1");
			Assert.AreEqual (2, p.ItemCount, "#2");
		}
		
		[Test]
		public void NewReceiverGetAChancewithCurrentItems2 ()
		{
			Port<int> p = new Port <int> ();
			var receiver = new MyReceiver ( Arbiter.FromHandler (() =>{}));
			receiver.State = ReceiverTaskState.Persistent;

			p.Post (10);
			p.Post (20);
			p.Post (30);

			IPortReceive pr = p;
			pr.RegisterReceiver (receiver); 

			Assert.AreEqual (3, receiver.eval, "#1");
			Assert.AreEqual (0, p.ItemCount, "#2");
		}

		[Test]
		public void NewReceiverGetAChancewithCurrentItems3 ()
		{
			Port<int> p = new Port <int> ();
			var receiver = new MyReceiver (false, Arbiter.FromHandler (() =>{}));
			receiver.State = ReceiverTaskState.Persistent;

			p.Post (10);
			p.Post (20);
			p.Post (30);

			IPortReceive pr = p;
			pr.RegisterReceiver (receiver);

			Assert.AreEqual (3, receiver.eval, "#1");
			Assert.AreEqual (3, p.ItemCount, "#2");
		}

		[Test]
		public void NewReceiverGetAChancewithCurrentItems4 ()
		{
			Port<int> p = new Port <int> ();
			var r0 = new MyReceiver (false, Arbiter.FromHandler (() =>{}));
			r0.State = ReceiverTaskState.Persistent;
			IPortReceive pr = p;
			pr.RegisterReceiver (r0);


			p.Post (10);
			p.Post (20);
			p.Post (30);

			Assert.AreEqual (3, p.ItemCount, "#1");

			var r1 = new MyReceiver (false, Arbiter.FromHandler (() =>{}));
			r1.State = ReceiverTaskState.Persistent;
			pr.RegisterReceiver (r1);

			Assert.AreEqual (3, r0.eval, "#2");
			Assert.AreEqual (3, r1.eval, "#3");
			Assert.AreEqual (3, p.ItemCount, "#4");
		}

		[Test]
		public void PostWithOneTimeReceiverDoesNotCleanup ()
		{
			var p = new Port<int> ();
			var recv = Arbiter.Receive (false, p, (i) => {});
			recv.TaskQueue = new VoidDispatcherQueue ();

			recv.Execute ();

			Assert.AreEqual (ReceiverTaskState.Onetime, recv.State);
			p.Post (10);
			Assert.AreEqual (ReceiverTaskState.Onetime, recv.State);
		}

		class WeirdReceiver : Receiver
		{
			IPortReceive port;
			internal WeirdReceiver(IPortReceive port) : base (port, null)
			{
				this.port = port;
				port.RegisterReceiver (this);
			}

			public override bool Evaluate (IPortElement messageNode, ref ITask deferredTask)
			{
				port.UnregisterReceiver (this);
				return true;
			}
		}

		[Test]
		public void ReceiverCanUnregisterOnEvaluate ()
		{
			IPortReceive p = new Port<int> ();
			var rec = new WeirdReceiver (p);
			Assert.AreEqual (1, p.GetReceivers ().Length, "#1");
			((IPort)p).PostUnknownType (10);
			Assert.AreEqual (0, p.GetReceivers ().Length, "#2");
		}
	}
}
