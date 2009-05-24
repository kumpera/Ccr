//
// MultipleItemGatherTest.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Ccr.Core.Arbiters;

using NUnit.Framework;

namespace Microsoft.Ccr.Core {

	[TestFixture]
	public class MultipleItemGatherTest
	{
		class SerialDispatchQueue : DispatcherQueue
		{
			public int count;
			public bool exec = true;

			public override bool Enqueue (ITask task)
			{
				++count;
				if (exec)
					task.Execute ();
				return true;
			}
		}

		[Test]
		public void CtorBadArgs ()
		{
			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { new Port<int>(), new Port<string> () };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => {}; 
			new MultipleItemGather (types, ports, count, handler);
			try {
				new MultipleItemGather (null, ports, count, handler);
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {}

			try {
				new MultipleItemGather (types, null, count, handler);
				Assert.Fail ("#2");
			} catch (ArgumentNullException) {}

			try {
				new MultipleItemGather (types, ports, count, null);
				Assert.Fail ("#3");
			} catch (ArgumentNullException) {}

			try {
				new MultipleItemGather (new Type[0], ports, count, handler);
				Assert.Fail ("#4");
			} catch (ArgumentOutOfRangeException) {}

			try {
				new MultipleItemGather (types, new IPortReceive [0], count, handler);
				Assert.Fail ("#5");
			} catch (ArgumentOutOfRangeException) {}

			try {
				new MultipleItemGather (new Type [0], new IPortReceive [0], count, handler);
				Assert.Fail ("#6");
			} catch (ArgumentOutOfRangeException) {}

			try {
				new MultipleItemGather (new Type[] { typeof (int) }, ports, count, handler);
				Assert.Fail ("#7");
			} catch (ArgumentOutOfRangeException) {}
		}


		[Test]
		public void Consume ()
		{
			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { new Port<int>(), new Port<string> () };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => {}; 
			var mig = new MultipleItemGather (types, ports, count, handler);

			try {
				mig.Consume (new PortElement<int> (10));
				Assert.Fail ("#1");
			} catch (InvalidOperationException) {}
		}

		[Test]
		public void Evaluate ()
		{
			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { new Port<int>(), new Port<string> () };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => {}; 
			var mig = new MultipleItemGather (types, ports, count, handler);

			try {
				ITask res = null;
				mig.Evaluate (new PortElement<int> (10), ref res);
				Assert.Fail ("#1");
			} catch (InvalidOperationException) {}
		}

		[Test]
		public void Execute ()
		{
			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { new Port<int>(), new Port<string> () };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => {}; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;
			
			Assert.AreEqual (0, ports [0].GetReceivers ().Length, "#1");
			Assert.AreEqual (0, ports [1].GetReceivers ().Length, "#2");

			mig.Execute ();

			Assert.AreEqual (2, ports [0].GetReceivers ().Length, "#3");
			Assert.AreEqual (2, ports [1].GetReceivers ().Length, "#4");

			Assert.AreEqual (ReceiverTaskState.Persistent, ports [0].GetReceivers ()[0].State, "#5");	
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [0].GetReceivers ()[1].State, "#6");	
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [1].GetReceivers ()[0].State, "#7");	
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [1].GetReceivers ()[1].State, "#8");	

			Assert.AreNotEqual (ports [0].GetReceivers ()[0], ports [0].GetReceivers ()[1], "#9");
			Assert.AreEqual (0, dq.count, "#10");
		}

		[Test]
		public void Execute2 ()
		{
			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { new Port<int>(), new Port<string> () };
			int count = 2;
			Handler<ICollection[]> handler = (cols) => {}; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			Assert.AreEqual (0, ports [0].GetReceivers ().Length, "#1");
			Assert.AreEqual (0, ports [1].GetReceivers ().Length, "#2");

			mig.Execute ();

			Assert.AreEqual (2, ports [0].GetReceivers ().Length, "#3");
			Assert.AreEqual (2, ports [1].GetReceivers ().Length, "#4");

			Assert.AreEqual (ReceiverTaskState.Persistent, ports [0].GetReceivers ()[0].State, "#5");	
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [0].GetReceivers ()[1].State, "#6");	
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [1].GetReceivers ()[0].State, "#7");	
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [1].GetReceivers ()[1].State, "#8");	

			Assert.AreEqual (0, dq.count, "#9");
		}

		[Test]
		public void PostToPortsAfterExecute1 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			int res = 0;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => { res += cols.Length; }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pa.Post (10);

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#1");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#2");
			Assert.AreEqual (2, res, "#3");
			Assert.AreEqual (1, dq.count, "#4");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#5");
		}

		[Test]
		public void PostToPortsAfterExecute2 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			int resA = 0;
			int resB = 0;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => { resA += cols[0].Count ; resB += cols[1].Count; }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pa.Post (10);

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#1");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#2");
			Assert.AreEqual (1, resA, "#3");
			Assert.AreEqual (0, resB, "#4");
			Assert.AreEqual (1, dq.count, "#5");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#6");
		}

		[Test]
		public void PostToPortsAfterExecute3 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			int resA = 0;
			int resB = 0;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => { resA += cols[0].Count ; resB += cols[1].Count; }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pb.Post ("hh");

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#1");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#2");
			Assert.AreEqual (0, resA, "#3");
			Assert.AreEqual (1, resB, "#4");
			Assert.AreEqual (1, dq.count, "#5");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#6");
		}

		[Test]
		public void PostToPortsAfterExecute4 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			int resA = 0;
			int resB = 0;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => { resA += cols[0].Count ; resB += cols[1].Count; }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pb.Post ("hh");

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#1");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#2");
			Assert.AreEqual (0, resA, "#3");
			Assert.AreEqual (1, resB, "#4");
			Assert.AreEqual (1, dq.count, "#5");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#6");
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [0].GetReceivers ()[0].State, "#7");
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [1].GetReceivers ()[0].State, "#8");
			

			pa.Post (10);

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#9");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#10");
			Assert.AreEqual (0, resA, "#11");
			Assert.AreEqual (1, resB, "#12");
			Assert.AreEqual (1, dq.count, "#13");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#14");
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [0].GetReceivers ()[0].State, "#15");
			Assert.AreEqual (ReceiverTaskState.Persistent, ports [1].GetReceivers ()[0].State, "#16");
		}

		[Test]
		public void PostToPortsAfterExecute5 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			int resA = 0;
			int resB = 0;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 2;
			Handler<ICollection[]> handler = (cols) => { resA += cols[0].Count ; resB += cols[1].Count; }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pa.Post (10);
			pa.Post (10);

			Assert.AreEqual (2, resA, "#1");
			Assert.AreEqual (0, resB, "#2");
			Assert.AreEqual (1, dq.count, "#3");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#4");
		}

		[Test]
		public void PostToPortsAfterExecute6 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			int resA = 0;
			int resB = 0;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 2;
			Handler<ICollection[]> handler = (cols) => { resA += cols[0].Count ; resB += cols[1].Count; }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pb.Post ("hw");
			pa.Post (10);

			Assert.AreEqual (1, resA, "#1");
			Assert.AreEqual (1, resB, "#2");
			Assert.AreEqual (1, dq.count, "#3");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#4");
		}

		[Test]
		public void WhatResultDataHas ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			ICollection[] res = null;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 2;
			Handler<ICollection[]> handler = (cols) => res = cols;; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pb.Post ("hw");
			pa.Post (10);

			Assert.IsTrue (res [0] is List<object>, "#1");
			Assert.IsTrue (res [1] is List<object>, "#2");
			var la = res [0] as List<object>; 
			var lb = res [1] as List<object>; 

			Assert.AreEqual (10, la [0], "#3");
			Assert.AreEqual ("hw", lb [0], "#4");
		}

		[Test]
		public void WhatResultDataHas2 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			ICollection[] res = null;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 2;
			Handler<ICollection[]> handler = (cols) => res = cols;; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			pa.Post (10); //result is post order independent
			pb.Post ("hw");

			Assert.IsTrue (res [0] is List<object>, "#1");
			Assert.IsTrue (res [1] is List<object>, "#2");
			var la = res [0] as List<object>; 
			var lb = res [1] as List<object>; 

			Assert.AreEqual (10, la [0], "#3");
			Assert.AreEqual ("hw", lb [0], "#4");
		}

		public class NakedArbiter : Task, IArbiterTask 
		{
			public ITask taskPassed;
			public ReceiverTask receiverPassed;
			public int calls;
			public bool res;

			public NakedArbiter (bool res) : base (()=>{})
			{
				this.res = res;
			}
		
			public bool Evaluate (ReceiverTask receiver, ref ITask deferredTask) {
				++calls;
				taskPassed = deferredTask;
				receiverPassed = receiver;
				return res;
			}

			public ArbiterTaskState ArbiterState { get { return ArbiterTaskState.Created; } }
	
			public Handler ArbiterCleanupHandler { get; set; }
			public Object LinkedIterator { get; set; }
			public DispatcherQueue TaskQueue { get; set; }
		}

		[Test]
		public void ExecuteWithArbiter1 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();
			bool handler_called = false;

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 2;
			Handler<ICollection[]> handler = (cols) => { handler_called = true; }; 
			var mig = new MultipleItemGather (types, ports, count, handler);

			var arbiter = new NakedArbiter (false);
			var dq = new SerialDispatchQueue ();
			arbiter.TaskQueue = dq;
			mig.Arbiter = arbiter;

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#1");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#2");

			pa.Post (10);

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#4");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#3");

			Assert.AreEqual (0, dq.count, "#4");
			Assert.AreEqual (0, arbiter.calls, "#5");
			Assert.AreEqual (ReceiverTaskState.Onetime, mig.State, "#6");
			Assert.IsFalse (handler_called, "#13");
			pa.Post (10);

			Assert.AreEqual (1, ports [0].GetReceivers ().Length, "#7");
			Assert.AreEqual (1, ports [1].GetReceivers ().Length, "#8");

			Assert.AreEqual (1, dq.count, "#9");
			Assert.AreEqual (1, arbiter.calls, "#10");
			Assert.AreEqual (ReceiverTaskState.Onetime, mig.State, "#11");
			Assert.IsTrue (handler_called, "#14");
		}

		[Test]
		public void ExecuteWithArbiter2 ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => { }; 
			var mig = new MultipleItemGather (types, ports, count, handler);

			var arbiter = new NakedArbiter (false);
			var dq = new SerialDispatchQueue ();
			arbiter.TaskQueue = dq;
			mig.Arbiter = arbiter;

			var rec = ports [0].GetReceivers () [0];
			ITask task = null;
			Assert.IsFalse (rec.Evaluate (new PortElement<int> (10), ref task), "#1");
			Assert.AreEqual (1, arbiter.calls, "#2");
			Assert.IsNotNull (task, "#3");
			Assert.AreEqual (ReceiverTaskState.Onetime, mig.State, "#4");
			Assert.AreEqual (ReceiverTaskState.Persistent, rec.State, "#5");
		}

		[Test]
		public void DontCleanupWithArbiter ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => { }; 
			var mig = new MultipleItemGather (types, ports, count, handler);

			var arbiter = new NakedArbiter (true);
			var dq = new SerialDispatchQueue ();
			arbiter.TaskQueue = dq;
			mig.Arbiter = arbiter;

			var rec = ports [0].GetReceivers () [0];
			ITask task = null;
			Assert.IsTrue (rec.Evaluate (new PortElement<int> (10), ref task), "#1");
			Assert.AreEqual (1, arbiter.calls, "#2");
			Assert.IsNotNull (task, "#3");
			Assert.AreEqual (ReceiverTaskState.Onetime, mig.State, "#4");
			Assert.AreEqual (ReceiverTaskState.Persistent, rec.State, "#5");
		}

		[Test]
		public void CleanupDoneInlineWithoutArbiter ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 1;
			Handler<ICollection[]> handler = (cols) => { }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			dq.exec = false;
			mig.TaskQueue = dq;

			mig.Execute ();

			var rec = ports [0].GetReceivers () [0];
			var rec2 = ports [0].GetReceivers () [1];
			ITask task = null;
			Assert.IsTrue (rec.Evaluate (new PortElement<int> (10), ref task), "#1");
			Assert.IsNotNull (task, "#2");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, mig.State, "#3");
			Assert.AreEqual (ReceiverTaskState.Persistent, rec.State, "#4");
			Assert.AreEqual (ReceiverTaskState.Persistent, rec2.State, "#5");
			Assert.AreEqual (0, dq.count, "#6");
		}

		[Test]
		public void CleanupTask ()
		{
			Port<int> pa = new Port<int> ();
			Port<string> pb = new Port<string> ();

			Type[] types = new Type[] { typeof (int), typeof (string) };
			IPortReceive[] ports = new IPortReceive[] { pa, pb };
			int count = 2;
			Handler<ICollection[]> handler = (cols) => { }; 
			var mig = new MultipleItemGather (types, ports, count, handler);
			var dq = new SerialDispatchQueue ();
			mig.TaskQueue = dq;

			mig.Execute ();

			ITask task = null;
			var rec = ports [0].GetReceivers () [0];
			Assert.IsTrue (rec.Evaluate (new PortElement<int> (10), ref task), "#1");
			Assert.IsNull (task, "#2");
			Assert.IsTrue (rec.Evaluate (new PortElement<int> (20), ref task), "#3");
			Assert.IsNotNull (task, "#4");

			mig.Cleanup (task);

			Assert.AreEqual (2, pa.ItemCount, "#4");
			Assert.AreEqual (0, pb.ItemCount, "#5");

			Assert.AreEqual (10, pa.Test (), "#6");
			Assert.AreEqual (20, pa.Test (), "#7");


		}
	}
}
