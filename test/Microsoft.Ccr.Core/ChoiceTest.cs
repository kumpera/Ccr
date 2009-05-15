//
// ChoiceTest.cs
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
using System.Threading;
using System.Diagnostics;
using Microsoft.Ccr.Core.Arbiters;

using NUnit.Framework;

namespace Microsoft.Ccr.Core {

	[TestFixture]
	public class ChoiceTest
	{
		[Test]
		public void CtorWithPersistentTask ()
		{
			var pa = new Port<int> ();
			var pb = new Port<string> ();

			var ra = Arbiter.Receive (false, pa, (i) => {});
			var rb = Arbiter.Receive (false, pb, (s) => {});
			rb.Cleanup ();

			new Choice (ra, rb);
		}

		[Test]
		public void CtorWithCleanedupTaskSilentlyAccepts ()
		{
			var pa = new Port<int> ();
			var pb = new Port<string> ();

			var ra = Arbiter.Receive (true, pa, (i) => {});
			var rb = Arbiter.Receive (false, pb, (s) => {});

			try {
				new Choice (ra, rb);
				Assert.Fail ("#1");
			} catch (ArgumentOutOfRangeException) {}
		}

		[Test]
		public void CtorSideEffects ()
		{
			int count = 0;
			var pa = new Port<int> ();
			var pb = new Port<string> ();

			var ra = Arbiter.Receive (false, pa, (i) => count += i);
			var rb = Arbiter.Receive (false, pb, (s) => count += s.Length);

			var c = new Choice (ra, rb);

			IPortReceive pra = pa;
			IPortReceive prb = pb;

			Assert.AreEqual (ArbiterTaskState.Created, c.ArbiterState, "#1");
			Assert.AreEqual (0, c.PortElementCount, "#2");
			Assert.AreEqual (0, pra.GetReceivers ().Length, "#3");
			Assert.AreEqual (0, prb.GetReceivers ().Length, "#4");
			Assert.IsNull (ra.Arbiter, "#5");
			Assert.IsNull (rb.Arbiter, "#6");
			Assert.IsNull (ra.ArbiterContext, "#7");
			Assert.IsNull (rb.ArbiterContext, "#8");
			Assert.IsNull (ra.ArbiterCleanupHandler, "#9");
			Assert.IsNull (rb.ArbiterCleanupHandler, "#10");
		}

		[Test]
		public void ItemProperty ()
		{
			var pa = new Port<int> ();
			var pb = new Port<string> ();

			var ra = Arbiter.Receive (false, pa, (i) => {});
			var rb = Arbiter.Receive (false, pb, (s) => {});

			var c = new Choice (ra, rb);

			try {
				c [0] = new PortElement<int> (10);
				Assert.Fail ("#1");
			} catch (NotSupportedException) {}		

			try {
				var x = c [1];
				Assert.Fail ("#2");
			} catch (NotSupportedException) {}		
		}

		class NakedReceiver : Receiver
		{
			internal int execute;
			internal int set_arbiter;
			internal int cleanup;
			internal int cleanup_task;
			
			internal NakedReceiver (IPortReceive p, ITask task) : base (p, task) {}

			public override IArbiterTask Arbiter {
				set {
					++set_arbiter;
					base.Arbiter = value;
				}
			}
			
			public override IEnumerator<ITask> Execute ()
			{
				++execute;
				return base.Execute ();
			}

			public override void Cleanup ()
			{
				++cleanup;
				base.Cleanup ();
			}

			public override void Cleanup (ITask task)
			{
				++cleanup_task;
				base.Cleanup (task);
			}
		}

		class SerialDispatchQueue : DispatcherQueue
		{
			public override bool Enqueue (ITask task)
			{
				//Console.WriteLine ("executing {0} from \n{1}", task, new StackTrace ());
				task.Execute ();
				return true;
			}
		}

		[Test]
		public void Execute ()
		{
			var pa = new Port<int> ();
			var pb = new Port<string> ();

			var ra = Arbiter.Receive (false, pa, (i) => {});
			var rb = new NakedReceiver (pb, new Task<string>((s) => {}));

			var c = new Choice (ra, rb);

			IPortReceive pra = pa;
			IPortReceive prb = pb;

			Assert.IsNull (c.Execute (), "#0");

			Assert.AreEqual (ArbiterTaskState.Active, c.ArbiterState, "#1");
			Assert.AreEqual (0, c.PortElementCount, "#2");
			Assert.AreEqual (1, pra.GetReceivers ().Length, "#3");
			Assert.AreEqual (1, prb.GetReceivers ().Length, "#4");
			Assert.AreEqual (c, ra.Arbiter, "#5");
			Assert.AreEqual (c, rb.Arbiter, "#6");
			Assert.IsNull (ra.ArbiterContext, "#7");
			Assert.IsNull (rb.ArbiterContext, "#8");
			Assert.IsNull (ra.ArbiterCleanupHandler, "#9");
			Assert.IsNull (rb.ArbiterCleanupHandler, "#10");

			Assert.AreEqual (0, rb.execute, "#11");
			Assert.AreEqual (1, rb.set_arbiter, "#12");
		}
		[Test]
		public void ExecutePart2 ()
		{
			int count = 3;
			var pa = new Port<int> ();
			var pb = new Port<string> ();

			var ra = Arbiter.Receive (false, pa, (i) => count += i);
			var rb = new NakedReceiver (pb, new Task<string>((s) => count += s.Length));
			var dq = new SerialDispatchQueue ();

			var c = new Choice (ra, rb);
			c.TaskQueue = dq;

			IPortReceive pra = pa;
			IPortReceive prb = pb;

			c.Execute ();
			pa.Post (10);

			Assert.AreEqual (ArbiterTaskState.Done, c.ArbiterState, "#1");
			Assert.AreEqual (0, c.PortElementCount, "#2");
			Assert.AreEqual (0, pra.GetReceivers ().Length, "#3");
			Assert.AreEqual (0, prb.GetReceivers ().Length, "#4");
			Assert.AreEqual (c, ra.Arbiter, "#5");
			Assert.AreEqual (c, rb.Arbiter, "#6");
			Assert.IsNull (ra.ArbiterContext, "#7");
			Assert.IsNull (rb.ArbiterContext, "#8");
			Assert.IsNull (ra.ArbiterCleanupHandler, "#9");
			Assert.IsNull (rb.ArbiterCleanupHandler, "#10");
			Assert.AreEqual (0, rb.execute, "#11");
			Assert.AreEqual (1, rb.set_arbiter, "#12");
			Assert.AreEqual (1, rb.cleanup, "#13");
			Assert.AreEqual (0, rb.cleanup_task, "#14");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, ra.State, "#15");
			Assert.AreEqual (ReceiverTaskState.CleanedUp, rb.State, "#16");

			Assert.AreEqual (13, count, "#17");
		}

	}
}
