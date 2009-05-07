//
// PortSetTest.cs
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
	public class PortSetTest
	{
		public class ExposeFieldsPortSet : PortSet
		{
			public 	ExposeFieldsPortSet (Type[] type) : base (type) {}
			
			
			public PortSetMode _ModeInternal { get { return ModeInternal; } }
			public IPort[] _PortsTable { get { return PortsTable; } }
			public Port<Object> _SharedPortInternal { get { return SharedPortInternal; } }
			public Type[] _Types { get { return Types; } }
		}

		[Test]
		public void BadCtors ()
		{
			try {
				new PortSet (null);
				Assert.Fail ("#1");
			} catch (ArgumentNullException) {}

			try {
				new PortSet (new Type [0]);
				Assert.Fail ("#2");
			} catch (ArgumentOutOfRangeException) {}
			
			try {
				new PortSet (new Type [1]);
				Assert.Fail ("#3");
			} catch (ArgumentNullException) {}
		}

		[Test]
		public void ProtectedFieldsAfterCtor ()
		{
			Type tint = typeof (int);
			Type tstr = typeof (string);
			Type[] arr = new Type[] { tint, tstr };
			var ps = new ExposeFieldsPortSet (arr);

			Assert.AreEqual (PortSetMode.Default, ps._ModeInternal, "#1");
			Assert.AreEqual (2, ps._PortsTable.Length, "#2");
			Assert.IsTrue (ps._PortsTable[0] is Port<int>, "#3");
			Assert.IsTrue (ps._PortsTable[1] is Port<string>, "#4");
			Assert.IsNull (ps._SharedPortInternal, "#5");
			Assert.AreEqual (arr, ps._Types, "#6");

		}

		[Test]
		public void CtorSideEffects ()
		{
			Type tint = typeof (int);
			Type tstr = typeof (string);
			var ps = new PortSet (new Type[] { tint, tstr });

			Assert.IsNotNull (ps [tint], "#1");
			Assert.IsNotNull (ps [tstr], "#2");
			Assert.IsNull (ps [typeof (object)], "#3");

			Assert.AreEqual (PortSetMode.Default, ps.Mode, "#4");
			var ports = ps.Ports;
			Assert.AreEqual (2, ports.Count, "#5");
			Assert.IsTrue (ports.Contains (ps [tint]), "#6");
			Assert.IsTrue (ports.Contains (ps [tstr]), "#7");
			Assert.IsTrue (ports.IsReadOnly, "#8");

			Assert.IsNull (ps.SharedPort, "#9");
		}

		[Test]
		public void GenericTestMethod ()
		{
			Type tint = typeof (int);
			Type tstr = typeof (string);
			var ps = new PortSet (new Type[] { tint, tstr });

			ps[tint].PostUnknownType (10);
			ps[tint].PostUnknownType (20);
			ps[tstr].PostUnknownType ("hello");

			Assert.AreEqual (10, ps.Test<int>(), "#1"); 
			Assert.AreEqual ("hello", ps.Test<string>(), "#2"); 
			Assert.AreEqual (20, ps.Test<int>(), "#3");
			try { 
				ps.Test<int> (); //LAMEIMPL fail that bad instead of returning default(T)? LAME
				Assert.Fail ("#4");
			} catch (NullReferenceException) {}

			Assert.AreEqual (null, ps.Test<string>(), "#5"); 

			try {
				ps.Test<object> ();
				Assert.Fail ("#6");
			} catch (PortNotFoundException) {}

			try {
				ps.Test<double> ();
				Assert.Fail ("#7");
			} catch (PortNotFoundException) {}
		}
	}
}
