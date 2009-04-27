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

	}
}
