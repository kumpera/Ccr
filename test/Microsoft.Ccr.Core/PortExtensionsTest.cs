//
// PortExtensionsTest.cs
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
	public class PortExtensionsTest
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

		[Test]
		public void TestTrivialReceiver ()
		{
			Port<int> port = new Port<int> ();
			var receiver = port.Receive ();	
			var dq = new VoidDispatcherQueue ();	
			receiver.TaskQueue = dq;

			Assert.IsNull (receiver.Arbiter, "#1");
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
	}
}
