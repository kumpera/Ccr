//
// VariableArgumentTaskTest.cs
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
	public class VariableArgumentTaskTest
	{

		[Test]
		public void CtorWithBadArgs ()
		{
			int cnt = 1;
			try {
				var task = new VariableArgumentTask<int> (-1, (a) => cnt += a.Length);
				Assert.Fail ("#1");
			} catch (OverflowException) {} //LAMEIMPL should throw a smarter exception
		}

		[Test]
		[Category ("NotDotNet")]
		public void CtorWithNullHandler ()
		{
			try {
				var task = new VariableArgumentTask<int> (1, null);
				Assert.Fail ("#2");
			} catch (ArgumentNullException) {}
		}

		[Test]
		public void PortElementCount ()
		{
			int cnt = 1;
			var task = new VariableArgumentTask<int> (3, (a) => cnt += a.Length);

			Assert.AreEqual (3, task.PortElementCount, "#1");
		}

		[Test]
		public void Execute ()
		{
			int cnt = 1;
			var task = new VariableArgumentTask<int> (2, (a) => cnt += a.Length);

			try {
				task.Execute ();
				Assert.Fail ("#1");
			} catch (NullReferenceException) {}

			task [0] = new PortElement<int> (10);
			try {
				task.Execute ();
				Assert.Fail ("#2");
			} catch (NullReferenceException) {}

			task [1] = new PortElement<int> (20);
			Assert.IsNull (task.Execute (), "#3");
			Assert.AreEqual (3, cnt, "#4");
		}

		[Test]
		public void PartialCloneEmptyElements ()
		{
			int cnt = 1;
			var task = new VariableArgumentTask<int> (2, (a) => cnt += a.Length);
			task [0] = new PortElement<int> (10);
			task [1] = new PortElement<int> (20);

			var tk = task.PartialClone ();
			Assert.IsNull (tk [0], "#1");
			Assert.IsNull (tk [0], "#2");
		}

		[Test]
		public void ItemPropertyBounds ()
		{
			int cnt = 1;
			var task = new VariableArgumentTask<int> (5, (a) => cnt += a.Length);

			try {
				task [-1] = new PortElement<int> (10);
				Assert.Fail ("#1");
			} catch (IndexOutOfRangeException) {} 		

			try {
				task [5] = new PortElement<int> (10);
				Assert.Fail ("#2");
			} catch (IndexOutOfRangeException) {} 

			try {
				var obj = task [-1];
				Assert.Fail ("#3");
			} catch (IndexOutOfRangeException) {} 		

			try {
				var obj = task [5];
				Assert.Fail ("#4");
			} catch (IndexOutOfRangeException) {} 
		}

		[Test]
		public void PortElementCountWithBoundFirstArg ()
		{
			double cntA = 1;
			int cntB = 1;
			var task = new VariableArgumentTask<double, int> (1, (a, b) => { cntA += a; cntB += b.Length; });
			Assert.AreEqual (2, task.PortElementCount, "#1");
		}

		[Test]
		public void ExecuteWithBoundFirstArg ()
		{
			double cntA = 1;
			int cntB = 1;
			var task = new VariableArgumentTask<double, int> (1, (a, b) => { cntA += a; cntB += b.Length; });

			task [0] = new PortElement <double> (4);
			task [1] = new PortElement <int> (2);
			Assert.IsNull (task.Execute (), "#1");
			Assert.AreEqual (5, cntA, "#2");
			Assert.AreEqual (2, cntB, "#3");

		}
	}
}
