//
// TaskTest.cs
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
	public class TaskTest
	{
		[Test]
		[Category ("NotDotNet")]
		public void NullHandlerFailsCtor ()
		{
			try {
				new Task (null);
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {}
		}
		
		[Test]
		public void Execute ()
		{
			int cnt = 0;
			Task tk = new Task (() => ++cnt);

			Assert.IsNull (tk.Execute (), "#1");
			Assert.AreEqual (1, cnt, "#2");
		}

		[Test]
		public void CtorSetHandlerProperty ()
		{
			int cnt = 0;
			Handler h = () => ++cnt;
			Task tk = new Task (h);
			Assert.AreEqual (h, tk.Handler, "#1");  
		}

		[Test]
		public void PartialCloneDoesntCloneHandler ()
		{
			int cnt = 0;
			Handler h = () => ++cnt;
			Task tk = new Task (h);
			ITask it = tk.PartialClone ();
			Assert.IsTrue (it is Task, "#1");

			Task tk2 = (Task)it;
			Assert.AreEqual (h, tk2.Handler, "#2");
		}

		[Test]
		public void PortElementCountAndItemProperties ()
		{
			int cnt = 0;
			Task tk = new Task (() => ++cnt);
		
			Assert.AreEqual (0, tk.PortElementCount, "#1");
			try {
				tk [0] = new PortElement<int> (10);
				Assert.Fail ("#2");
			} catch (NotSupportedException) {}

			try {
				object f = tk [0];
				Assert.Fail ("#3");
			} catch (NotSupportedException) {}

		}
	}
}
