//
// MultipleItemReceiverTest.cs
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

	[TestFixture]
	public class MultipleItemReceiverTest
	{
		class SerialDispatchQueue : DispatcherQueue
		{
			public override bool Enqueue (ITask task)
			{
				task.Execute ();
				return true;
			}
		}

		[Test]
		public void ConstructorSideEffects ()
		{
			int cnt = 0;
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			var mr = new MultipleItemReceiver (Arbiter.FromHandler (() => { ++cnt; }), pa, pb);

			Assert.AreEqual (0, pa.GetReceivers ().Length, "#1");
			Assert.AreEqual (0, pb.GetReceivers ().Length, "#2");
			Assert.AreEqual (0, cnt, "#3");
		}

		[Test]
		public void Consume ()
		{
			int cnt = 0;
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			var mr = new MultipleItemReceiver (Arbiter.FromHandler (() => { ++cnt; }), pa, pb);

			try {
				mr.Consume (new PortElement<int> (10));
				Assert.Fail ("#1");
			} catch (NotImplementedException) {}
		}

		[Test]
		public void Evaluate ()
		{
			int cnt = 0;
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			var mr = new MultipleItemReceiver (Arbiter.FromHandler (() => { ++cnt; }), pa, pb);

			try {
				ITask res = null;
				mr.Evaluate (new PortElement<int> (10), ref res);
				Assert.Fail ("#1");
			} catch (NotImplementedException) {}
		}

		[Test]
		public void Execute ()
		{
			int cnt = 0;
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			var mr = new MultipleItemReceiver (Arbiter.FromHandler (() => { ++cnt; }), pa, pb);
			var dq = new SerialDispatchQueue ();
			mr.TaskQueue = dq;

			Assert.AreEqual (0, pa.GetReceivers ().Length, "#1");
			Assert.AreEqual (0, pb.GetReceivers ().Length, "#2");

			mr.Execute ();

			Assert.AreEqual (1, pa.GetReceivers ().Length, "#3");
			Assert.AreEqual (1, pb.GetReceivers ().Length, "#4");

			Assert.AreEqual (ReceiverTaskState.Onetime, pa.GetReceivers ()[0].State, "#5");
		}

		[Test]
		public void ExecutePersistent ()
		{
			int cnt = 0;
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			var mr = new MultipleItemReceiver (Arbiter.FromHandler (() => { ++cnt; }), pa, pb);
			var dq = new SerialDispatchQueue ();
			mr.TaskQueue = dq;
			mr.State = ReceiverTaskState.Persistent;

			Assert.AreEqual (0, pa.GetReceivers ().Length, "#1");
			Assert.AreEqual (0, pb.GetReceivers ().Length, "#2");

			mr.Execute ();

			Assert.AreEqual (1, pa.GetReceivers ().Length, "#3");
			Assert.AreEqual (1, pb.GetReceivers ().Length, "#4");

			Assert.AreEqual (ReceiverTaskState.Onetime, pa.GetReceivers ()[0].State, "#5");
		}

		[Test]
		public void Execute2 ()
		{
			int cnt = 0;
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			ITask task = new Task<int, string> ( (i, s) => { cnt += i + s.Length; });
			var mr = new MultipleItemReceiver (task, pa, pb);
			var dq = new SerialDispatchQueue ();
			mr.TaskQueue = dq;

			mr.Execute ();

			var a = (Port<int>)pa;
			var b = (Port<string>)pb;

			a.Post (10);
			Assert.AreEqual (0, pa.ItemCount, "#1");
			Assert.AreEqual (0, cnt, "#2");

			b.Post ("hello");
			Assert.AreEqual (15, cnt, "#3");

			//it's a one time thing
			Assert.AreEqual (0, pa.GetReceivers ().Length, "#4");
			Assert.AreEqual (0, pb.GetReceivers ().Length, "#5");

			a.Post (1);
			Assert.AreEqual (15, cnt, "#6");
			b.Post ("x");
			Assert.AreEqual (15, cnt, "#7");
		}

		[Test]
		public void Execute3 ()
		{
			int cnt = 0;
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			ITask task = new Task<int, string> ( (i, s) => { cnt += i + s.Length; });
			var mr = new MultipleItemReceiver (task, pa, pb);
			var dq = new SerialDispatchQueue ();
			mr.TaskQueue = dq;

			mr.Execute ();

			var a = (Port<int>)pa;
			var b = (Port<string>)pb;

			b.Post ("hello");
			Assert.AreEqual (0, cnt, "#1");
			a.Post (10);

			Assert.AreEqual (15, cnt, "#2");
		}

		[Test]
		public void PostToPortSetUserTaskItem ()
		{
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			ITask task = new Task<int, string> ((i, s) => { });
			var mr = new MultipleItemReceiver (task, pa, pb);
			var dq = new SerialDispatchQueue ();
			mr.TaskQueue = dq;

			mr.Execute ();
			((Port<int>)pa).Post (10);

			Assert.AreEqual (10, task [0].Item, "#1");
		}

		[Test]
		public void CleanupTask ()
		{
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			ITask task = new Task<int, string> ((i, s) => { });
			var mr = new MultipleItemReceiver (task, pa, pb);
			var dq = new SerialDispatchQueue ();
			mr.TaskQueue = dq;

			mr.Execute ();
			((Port<int>)pa).Post (10);
			Assert.AreEqual (0, pa.ItemCount, "#1");
			mr.Cleanup (task);

			Assert.AreEqual (1, pa.ItemCount, "#2");
			Assert.AreEqual (0, pb.ItemCount, "#3");
		}

		[Test]
		public void Cleanup ()
		{
			IPortReceive pa = new Port <int> ();
			IPortReceive pb = new Port <string> ();
			ITask task = new Task<int, string> ((i, s) => { });
			var mr = new MultipleItemReceiver (task, pa, pb);
			var dq = new SerialDispatchQueue ();
			mr.TaskQueue = dq;

			mr.Execute ();
			Assert.AreEqual (1, pa.GetReceivers ().Length, "#1");
			Assert.AreEqual (1, pb.GetReceivers ().Length, "#2");

			mr.Cleanup ();
			Assert.AreEqual (0, pa.GetReceivers ().Length, "#3");
			Assert.AreEqual (0, pb.GetReceivers ().Length, "#4");

			Assert.AreEqual (0, pa.ItemCount, "#5");
			Assert.AreEqual (0, pb.ItemCount, "#6");
		}
	}
}
