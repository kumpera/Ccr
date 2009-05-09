//
// IterativeTaskTest.cs
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
	public class IterativeTaskTest
	{
		[Test]
		[Category ("NotDotNet")]
		public void NullHandlerFailsCtor ()
		{
			try {
				new IterativeTask (null);
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {}
		}

		[Test]
		public void CtorSideEffect ()
		{
			IteratorHandler handler = () => null;
			var task = new IterativeTask (handler);

			Assert.IsNull (task.ArbiterCleanupHandler, "#1");
			try {
				var x = task [0];
				Assert.Fail ("#2");
			} catch (NotSupportedException) {}
			try {
				task [0] = new PortElement<int> (10);
				Assert.Fail ("#2");
			} catch (NotSupportedException) {}

			Assert.AreEqual (handler, task.Handler, "#3");
			Assert.IsNull (task.LinkedIterator, "#4");
			Assert.IsNull (task.LinkedIterator, "#5");
			Assert.IsNull (task.TaskQueue, "#6");
		}

		[Test]
		public void Execute ()
		{
			IEnumerable <ITask> list = new List<ITask> ();
			var iter = list.GetEnumerator ();
			IteratorHandler handler = () => iter;
			var task = new IterativeTask (handler);

			Assert.AreEqual (iter, task.Execute (), "#1");
			Assert.IsNull (task.LinkedIterator, "#2");
		}
	}
}
