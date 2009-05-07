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
using System.Collections;
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

			public Port<T> _AllocatePort<T> ()
			{
				return AllocatePort<T> ();
			}
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

		class A {}
		class B : A {}
		class C : A {}

		[Test]
		public void PostUnknownType ()
		{
			Type tint = typeof (int);
			Type tb = typeof (B);
			Type tc = typeof (A);
			var ps = new PortSet (new Type[] { tint, tb, tc });
			object obj;

			Port<int> p0 = (Port<int>)ps [tint]; 
			Port<B> p1 = (Port<B>)ps [tb]; 
			Port<A> p2 = (Port<A>)ps [tc]; 

			try {
				ps.PostUnknownType ("hello");
				Assert.Fail ("#1");
			} catch (PortNotFoundException) {}

			try {
				ps.PostUnknownType ((short)10);
				Assert.Fail ("#2");
			} catch (PortNotFoundException) {}

			try {
				ps.PostUnknownType (new object ());
				Assert.Fail ("#3");
			} catch (PortNotFoundException) {}

			ps.PostUnknownType (10);
			Assert.AreEqual (10, (int)p0, "#4");

			ps.PostUnknownType (obj = new B ());
			Assert.AreEqual (obj, p1.Test (), "#5");

			ps.PostUnknownType (obj = new A ());
			Assert.AreEqual (obj, p2.Test (), "#6");

			ps.PostUnknownType (obj = new C ());
			Assert.AreEqual (obj, p2.Test (), "#7");
		}

		[Test]
		public void PostUnknownTypeNull ()
		{
			Type tint = typeof (int);
			Type tstr = typeof (string);
		
			var ps = new PortSet (new Type[] { tint, tstr });

			IPortReceive p0 = (IPortReceive)ps [tint]; 
			IPortReceive p1 = (IPortReceive)ps [tstr]; 

			ps.PostUnknownType (null);
			Assert.AreEqual (0, p0.GetItems ().Length, "#1");
			Assert.AreEqual (1, p1.GetItems ().Length, "#2");
		}

		[Test]
		public void PostUnknownTypeNull2 ()
		{
			Type tstr = typeof (string);
			Type tobj = typeof (object);
		
			var ps = new PortSet (new Type[] { tstr, tobj });

			IPortReceive p0 = (IPortReceive)ps [tstr]; 
			IPortReceive p1 = (IPortReceive)ps [tobj]; 

			ps.PostUnknownType (null);
			Assert.AreEqual (1, p0.GetItems ().Length, "#1");
			Assert.AreEqual (0, p1.GetItems ().Length, "#2");
		}
		
		[Test]
		public void PostUnknownTypeOrdering1 ()
		{
			Type tobj = typeof (object);
			Type ta = typeof (A);

			var ps = new PortSet (new Type[] { tobj, ta });

			Port<object> p0 = (Port<object>)ps [tobj]; 
			Port<A> p1 = (Port<A>)ps [ta]; 

			object obj;

			ps.PostUnknownType (obj = new B ());
			Assert.AreEqual (obj, p0.Test (), "#1");
			Assert.AreEqual (null, p1.Test (), "#2");

			ps.PostUnknownType (obj = new A ());
			Assert.AreEqual (null, p0.Test (), "#3");
			Assert.AreEqual (obj, p1.Test (), "#4");
		}

		[Test]
		public void PostUnknownTypeOrdering2 ()
		{
			Type ta = typeof (A);
			Type tobj = typeof (object);

			var ps = new PortSet (new Type[] { ta, tobj });

			Port<A> p0 = (Port<A>)ps [ta]; 
			Port<object> p1 = (Port<object>)ps [tobj]; 

			object obj;

			ps.PostUnknownType (obj = new B ());
			Assert.AreEqual (obj, p0.Test (), "#1");
			Assert.AreEqual (null, p1.Test (), "#2");

			ps.PostUnknownType (obj = new A ());
			Assert.AreEqual (obj, p0.Test (), "#3");
			Assert.AreEqual (null, p1.Test (), "#4");
		}

		[Test]
		public void PostUnknownTypeOrdering3 ()
		{
			Type tcol = typeof (ICollection);
			Type tlist = typeof (IList);

			var ps = new PortSet (new Type[] { tcol, tlist });

			IPortReceive p0 = (IPortReceive)ps [tcol]; 
			IPortReceive p1 = (IPortReceive)ps [tlist]; 

			object obj;

			ps.PostUnknownType (obj = new ArrayList ());
			Assert.AreEqual (1, p0.GetItems ().Length, "#1");
			Assert.AreEqual (0, p1.GetItems ().Length, "#2");

			ps.PostUnknownType (obj = new int [10]);
			Assert.AreEqual (2, p0.GetItems ().Length, "#3");
			Assert.AreEqual (0, p1.GetItems ().Length, "#4");
		}

		[Test]
		public void PostUnknownTypeOrdering4 ()
		{
			Type tcol = typeof (ICollection);
			Type tlist = typeof (ArrayList);

			var ps = new PortSet (new Type[] { tcol, tlist });

			IPortReceive p0 = (IPortReceive)ps [tcol]; 
			IPortReceive p1 = (IPortReceive)ps [tlist]; 

			object obj;

			ps.PostUnknownType (obj = new ArrayList ());
			Assert.AreEqual (0, p0.GetItems ().Length, "#1");
			Assert.AreEqual (1, p1.GetItems ().Length, "#2");

			ps.PostUnknownType (obj = new int [10]);
			Assert.AreEqual (1, p0.GetItems ().Length, "#3");
			Assert.AreEqual (1, p1.GetItems ().Length, "#4");
		}

		[Test]
		public void PostUnknownTypeOrdering5 ()
		{
			Type tcol = typeof (ICollection);
			Type tobj = typeof (object);

			var ps = new PortSet (new Type[] { tcol, tobj });

			IPortReceive p0 = (IPortReceive)ps [tcol]; 
			IPortReceive p1 = (IPortReceive)ps [tobj]; 

			object obj;

			ps.PostUnknownType (obj = new ArrayList ());
			Assert.AreEqual (1, p0.GetItems ().Length, "#1");
			Assert.AreEqual (0, p1.GetItems ().Length, "#2");

			ps.PostUnknownType (obj = new int [10]);
			Assert.AreEqual (2, p0.GetItems ().Length, "#3");
			Assert.AreEqual (0, p1.GetItems ().Length, "#4");
		}

		[Test]
		public void PostUnknownTypeOrdering6 ()
		{
			Type tcol = typeof (ICollection);
			Type tobj = typeof (object);

			var ps = new PortSet (new Type[] { tobj, tcol });

			IPortReceive p0 = (IPortReceive)ps [tobj]; 
			IPortReceive p1 = (IPortReceive)ps [tcol]; 

			object obj;

			ps.PostUnknownType (obj = new ArrayList ());
			Assert.AreEqual (1, p0.GetItems ().Length, "#1");
			Assert.AreEqual (0, p1.GetItems ().Length, "#2");

			ps.PostUnknownType (obj = new int [10]);
			Assert.AreEqual (2, p0.GetItems ().Length, "#3");
			Assert.AreEqual (0, p1.GetItems ().Length, "#4");
		}

		[Test]
		public void FindTypeFromRuntimeTypeWithNull ()
		{
			Type res;
			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (null, new Type[] { typeof (object) }, out res), "#1");
			Assert.AreEqual (typeof (object), res, "#2");

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (null, new Type[] { typeof (ICollection<int>) }, out res), "#2");
			Assert.AreEqual (typeof (ICollection<int>), res, "#3");

			Assert.IsFalse (PortSet.FindTypeFromRuntimeType (null, new Type[] { typeof (int) }, out res), "#3");
			Assert.IsFalse (PortSet.FindTypeFromRuntimeType (null, new Type[] { typeof (PortSetMode) }, out res), "#4");

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (null, new Type[] { typeof (Enum) }, out res), "#5");
			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (null, new Type[] { typeof (ValueType) }, out res), "#6");
		}

		[Test]
		public void FindTypeFromRuntime ()
		{
			//LAMEIMPL this method is completely useless and broken
			Type res;
			Type[] types = new Type[] { typeof (ICollection<int>), typeof (object) };

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (new int[0], types, out res), "#1");
			Assert.AreEqual (typeof (int[]), res, "#2");

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (10, types, out res), "#3");
			Assert.AreEqual (typeof (int), res, "#4");

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType ("hello", types, out res), "#5");
			Assert.AreEqual (typeof (string), res, "#6");

			types = new Type[] { typeof (int), typeof (object) };

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (new int[0], types, out res), "#7");
			Assert.AreEqual (typeof (int[]), res, "#8");

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType (10, types, out res), "#9");
			Assert.AreEqual (typeof (int), res, "#10");

			Assert.IsTrue (PortSet.FindTypeFromRuntimeType ("hello", types, out res), "#11");
			Assert.AreEqual (typeof (string), res, "#12");
		}

		[Test]
		public void TryPostUnknownType1 ()
		{
			Type tint = typeof (int);
			Type tb = typeof (B);
			Type tc = typeof (A);
			var ps = new PortSet (new Type[] { tint, tb, tc });
			object obj;

			Port<int> p0 = (Port<int>)ps [tint]; 
			Port<B> p1 = (Port<B>)ps [tb]; 
			Port<A> p2 = (Port<A>)ps [tc]; 

			Assert.IsFalse (ps.TryPostUnknownType ("hello"), "#1");
			Assert.IsFalse (ps.TryPostUnknownType ((short)10), "#2");
			Assert.IsFalse (ps.TryPostUnknownType (new object ()), "#3");

			Assert.IsTrue (ps.TryPostUnknownType (10), "#4");
			Assert.AreEqual (10, (int)p0, "#5");

			Assert.IsTrue (ps.TryPostUnknownType (obj = new B ()), "#6");
			Assert.AreEqual (obj, p1.Test (), "#7");

			Assert.IsTrue (ps.TryPostUnknownType (obj = new A ()), "#8");
			Assert.AreEqual (obj, p2.Test (), "#9");

			Assert.IsTrue (ps.TryPostUnknownType (obj = new C ()), "#10");
			Assert.AreEqual (obj, p2.Test (), "#11");
		}

		[Test]
		public void TryPostUnknownTypeNull ()
		{
			var ps = new PortSet (new Type[] { typeof (int), typeof (double) });
			Assert.IsFalse (ps.TryPostUnknownType ("hello"), "#1");
			Assert.IsFalse (ps.TryPostUnknownType (null), "#2");
		}

		[Test]
		public void PostUnknownTypeValueType ()
		{
			var ps = new PortSet (new Type[] { typeof (string), typeof (object) });
			Assert.IsTrue (ps.TryPostUnknownType (10), "#1");
			Assert.IsTrue (ps.TryPostUnknownType (DateTime.Now), "#2");

			IPortReceive port = (IPortReceive)ps[typeof(object)];
			ps.PostUnknownType (10);
			Assert.AreEqual (3, port.GetItems ().Length, "#3");  
			ps.PostUnknownType (DateTime.Now);
			Assert.AreEqual (4, port.GetItems ().Length, "#4");  
		}

		[Test]
		public void AllocatePort ()
		{
			ExposeFieldsPortSet p = new ExposeFieldsPortSet (new Type[] {typeof (int), typeof (bool) });
			Assert.IsTrue (p._AllocatePort<int> () is Port<int>, "#1");
		}

		[Test]
		public void SharedPortModeSwitching ()
		{
			var ps = new ExposeFieldsPortSet (new Type[] { typeof (int), typeof (string) });

			Assert.IsNull (ps.SharedPort, "#1");
			Assert.IsNull (ps._SharedPortInternal, "#2");
			Assert.AreEqual (PortSetMode.Default, ps.Mode, "#3");
			Assert.AreEqual (2, ps.Ports.Count, "#4");

			ps.Mode = PortSetMode.SharedPort;
			Assert.AreEqual (PortSetMode.SharedPort, ps.Mode, "#5");
			Assert.AreEqual (1, ps.Ports.Count, "#6");
			Assert.IsNotNull (ps.SharedPort, "#7");
			Assert.IsNotNull (ps._SharedPortInternal, "#8");

			ps.Mode = PortSetMode.Default;
			Assert.IsNull (ps.SharedPort, "#9");
			Assert.IsNull (ps._SharedPortInternal, "#10");
			Assert.AreEqual (PortSetMode.Default, ps.Mode, "#11");
			Assert.AreEqual (2, ps.Ports.Count, "#12");
		}

		[Test]
		public void SharedPortShape1 ()
		{
			var ps = new ExposeFieldsPortSet (new Type[] { typeof (int), typeof (string) });
			ps.Mode = PortSetMode.SharedPort;

			var sh = ps.SharedPort;
			Assert.AreEqual (0, sh.ItemCount, "#1");
			Assert.AreEqual (PortMode.Default, sh.Mode, "#2");

			IPortReceive pr = sh;
			Assert.AreEqual (0, pr.GetReceivers ().Length, "#3");

			try {
				var p0 = ps [typeof (int)];
				Assert.Fail ("#4");
			} catch (InvalidOperationException) {}
		}

		[Test]
		public void SharedPortShape2 ()
		{
			var ps = new ExposeFieldsPortSet (new Type[] { typeof (int), typeof (string) });
			var p0 = (IPortReceive)ps [typeof (int)];
			Assert.AreEqual (0, p0.GetReceivers ().Length, "#1");

			ps.Mode = PortSetMode.SharedPort;

			var sh = ps.SharedPort;
			Assert.AreEqual (0, sh.ItemCount, "#2");
			Assert.AreEqual (PortMode.Default, sh.Mode, "#3");

			IPortReceive pr = sh;
			Assert.AreEqual (0, pr.GetReceivers ().Length, "#4");
			Assert.AreEqual (0, p0.GetReceivers ().Length, "#5");
		}

		[Test]
		public void PortsAllocation ()
		{
			var ps = new ExposeFieldsPortSet (new Type[] { typeof (int), typeof (string) });
			var a = ps [typeof (int)];
			var b = ps [typeof (int)];
			Assert.AreSame (a, b, "#1");

			ps.Mode = PortSetMode.SharedPort;
			Assert.IsNotNull (ps._PortsTable [0], "#2");
			
			ps.Mode = PortSetMode.Default;
			b = ps [typeof (int)];
			Assert.AreSame (a, b, "#3");
		}

		[Test]
		public void AllocatePortUnderSharedMode ()
		{
			ExposeFieldsPortSet p = new ExposeFieldsPortSet (new Type[] {typeof (int), typeof (bool) });
			p.Mode = PortSetMode.SharedPort;
			try {
				p._AllocatePort<int> ();
				Assert.Fail ("#4");
			} catch (InvalidOperationException) {}				
		}

		[Test]
		public void AllocatePortMultipleTimes ()
		{
			ExposeFieldsPortSet p = new ExposeFieldsPortSet (new Type[] {typeof (int), typeof (bool) });
			var a = p._AllocatePort<int> ();			
			var b = p._AllocatePort<int> ();
			Assert.AreSame (a, b, "#1");
			Assert.AreSame (a, p [typeof (int)], "#2");
			Assert.IsNull (p._AllocatePort<DateTime> (), "#3");
			Assert.IsNotNull (p._AllocatePort<bool> (), "#4");
		}

		[Test]
		public void SharedPortCreatesPortEverytime ()
		{
			var ps = new ExposeFieldsPortSet (new Type[] { typeof (int), typeof (string) });
			ps.Mode = PortSetMode.SharedPort;
			var pa = ps.SharedPort;
			ps.Mode = PortSetMode.Default;
			ps.Mode = PortSetMode.SharedPort;
			var pb = ps.SharedPort;
			Assert.AreNotSame (pa, pb, "#1");
		}

		[Test]
		public void PostUnknownTypeUnderSharedMode ()
		{
			var ps = new ExposeFieldsPortSet (new Type[] { typeof (int), typeof (string) });
			ps.Mode = PortSetMode.SharedPort;
			var sh = ps.SharedPort;

			ps.PostUnknownType (DateTime.Now);
			Assert.AreEqual (1, sh.ItemCount, "#1");
			Assert.IsTrue (ps.TryPostUnknownType (DateTime.Now), "#2");
			Assert.AreEqual (2, sh.ItemCount, "#3");

			ps.Mode = PortSetMode.Default;

			try {
				ps.PostUnknownType (DateTime.Now);
				Assert.Fail ("#4");
			} catch (PortNotFoundException) {}
		}

		[Test]
		public void TestUnderSharedMode ()
		{
			var ps = new ExposeFieldsPortSet (new Type[] { typeof (int), typeof (string) });
			ps.Mode = PortSetMode.SharedPort;
			var sh = ps.SharedPort;
			sh.Post (10);

			Assert.AreEqual (10, ps.Test <int> (), "#1");

			sh.Post (10);
			try {
				ps.Test <string> ();
				Assert.Fail ("#2");
			} catch (InvalidCastException) {}
			sh.Post (10);

			Assert.AreEqual (10, ps.Test <object> (), "3");
		}
	}
}
