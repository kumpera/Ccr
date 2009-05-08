//
// PortSet.generared.cs.cs
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

namespace Microsoft.Ccr.Core {

	public class PortSet<T0, T1, T2> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2> port) { return (Port<T2>)port.PortsTable [2]; }

		public static implicit operator T0 (PortSet<T0, T1, T2> port) { return port.Test<T0> (); }

		public static implicit operator T1 (PortSet<T0, T1, T2> port) { return port.Test<T1> (); }

		public static implicit operator T2 (PortSet<T0, T1, T2> port) { return port.Test<T2> (); }

	}

	public class PortSet<T0, T1, T2, T3> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3> port) { return (Port<T3>)port.PortsTable [3]; }

	}

	public class PortSet<T0, T1, T2, T3, T4> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4> port) { return (Port<T4>)port.PortsTable [4]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5> port) { return (Port<T5>)port.PortsTable [5]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6> port) { return (Port<T6>)port.PortsTable [6]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7> port) { return (Port<T7>)port.PortsTable [7]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8> port) { return (Port<T8>)port.PortsTable [8]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9> port) { return (Port<T9>)port.PortsTable [9]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> port) { return (Port<T10>)port.PortsTable [10]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> port) { return (Port<T11>)port.PortsTable [11]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> port) { return (Port<T12>)port.PortsTable [12]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12, Port<T13> parameter13)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T12>)port.PortsTable [12]; }

		public void Post (T13 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T13>)PortsTable [13]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T13> P13 { get { return PortSetHelper.GetPort<T13> (ModeInternal, PortsTable, 13); } }
		public static implicit operator Port<T13> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> port) { return (Port<T13>)port.PortsTable [13]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12, Port<T13> parameter13, Port<T14> parameter14)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T12>)port.PortsTable [12]; }

		public void Post (T13 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T13>)PortsTable [13]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T13> P13 { get { return PortSetHelper.GetPort<T13> (ModeInternal, PortsTable, 13); } }
		public static implicit operator Port<T13> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T13>)port.PortsTable [13]; }

		public void Post (T14 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T14>)PortsTable [14]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T14> P14 { get { return PortSetHelper.GetPort<T14> (ModeInternal, PortsTable, 14); } }
		public static implicit operator Port<T14> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> port) { return (Port<T14>)port.PortsTable [14]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12, Port<T13> parameter13, Port<T14> parameter14, Port<T15> parameter15)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T12>)port.PortsTable [12]; }

		public void Post (T13 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T13>)PortsTable [13]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T13> P13 { get { return PortSetHelper.GetPort<T13> (ModeInternal, PortsTable, 13); } }
		public static implicit operator Port<T13> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T13>)port.PortsTable [13]; }

		public void Post (T14 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T14>)PortsTable [14]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T14> P14 { get { return PortSetHelper.GetPort<T14> (ModeInternal, PortsTable, 14); } }
		public static implicit operator Port<T14> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T14>)port.PortsTable [14]; }

		public void Post (T15 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T15>)PortsTable [15]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T15> P15 { get { return PortSetHelper.GetPort<T15> (ModeInternal, PortsTable, 15); } }
		public static implicit operator Port<T15> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> port) { return (Port<T15>)port.PortsTable [15]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12, Port<T13> parameter13, Port<T14> parameter14, Port<T15> parameter15, Port<T16> parameter16)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T12>)port.PortsTable [12]; }

		public void Post (T13 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T13>)PortsTable [13]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T13> P13 { get { return PortSetHelper.GetPort<T13> (ModeInternal, PortsTable, 13); } }
		public static implicit operator Port<T13> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T13>)port.PortsTable [13]; }

		public void Post (T14 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T14>)PortsTable [14]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T14> P14 { get { return PortSetHelper.GetPort<T14> (ModeInternal, PortsTable, 14); } }
		public static implicit operator Port<T14> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T14>)port.PortsTable [14]; }

		public void Post (T15 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T15>)PortsTable [15]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T15> P15 { get { return PortSetHelper.GetPort<T15> (ModeInternal, PortsTable, 15); } }
		public static implicit operator Port<T15> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T15>)port.PortsTable [15]; }

		public void Post (T16 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T16>)PortsTable [16]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T16> P16 { get { return PortSetHelper.GetPort<T16> (ModeInternal, PortsTable, 16); } }
		public static implicit operator Port<T16> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> port) { return (Port<T16>)port.PortsTable [16]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> (), new Port<T17> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16), typeof(T17) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12, Port<T13> parameter13, Port<T14> parameter14, Port<T15> parameter15, Port<T16> parameter16, Port<T17> parameter17)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> (), new Port<T17> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16), typeof(T17) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T12>)port.PortsTable [12]; }

		public void Post (T13 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T13>)PortsTable [13]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T13> P13 { get { return PortSetHelper.GetPort<T13> (ModeInternal, PortsTable, 13); } }
		public static implicit operator Port<T13> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T13>)port.PortsTable [13]; }

		public void Post (T14 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T14>)PortsTable [14]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T14> P14 { get { return PortSetHelper.GetPort<T14> (ModeInternal, PortsTable, 14); } }
		public static implicit operator Port<T14> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T14>)port.PortsTable [14]; }

		public void Post (T15 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T15>)PortsTable [15]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T15> P15 { get { return PortSetHelper.GetPort<T15> (ModeInternal, PortsTable, 15); } }
		public static implicit operator Port<T15> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T15>)port.PortsTable [15]; }

		public void Post (T16 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T16>)PortsTable [16]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T16> P16 { get { return PortSetHelper.GetPort<T16> (ModeInternal, PortsTable, 16); } }
		public static implicit operator Port<T16> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T16>)port.PortsTable [16]; }

		public void Post (T17 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T17>)PortsTable [17]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T17> P17 { get { return PortSetHelper.GetPort<T17> (ModeInternal, PortsTable, 17); } }
		public static implicit operator Port<T17> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> port) { return (Port<T17>)port.PortsTable [17]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> (), new Port<T17> (), new Port<T18> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16), typeof(T17), typeof(T18) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12, Port<T13> parameter13, Port<T14> parameter14, Port<T15> parameter15, Port<T16> parameter16, Port<T17> parameter17, Port<T18> parameter18)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> (), new Port<T17> (), new Port<T18> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16), typeof(T17), typeof(T18) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T12>)port.PortsTable [12]; }

		public void Post (T13 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T13>)PortsTable [13]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T13> P13 { get { return PortSetHelper.GetPort<T13> (ModeInternal, PortsTable, 13); } }
		public static implicit operator Port<T13> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T13>)port.PortsTable [13]; }

		public void Post (T14 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T14>)PortsTable [14]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T14> P14 { get { return PortSetHelper.GetPort<T14> (ModeInternal, PortsTable, 14); } }
		public static implicit operator Port<T14> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T14>)port.PortsTable [14]; }

		public void Post (T15 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T15>)PortsTable [15]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T15> P15 { get { return PortSetHelper.GetPort<T15> (ModeInternal, PortsTable, 15); } }
		public static implicit operator Port<T15> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T15>)port.PortsTable [15]; }

		public void Post (T16 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T16>)PortsTable [16]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T16> P16 { get { return PortSetHelper.GetPort<T16> (ModeInternal, PortsTable, 16); } }
		public static implicit operator Port<T16> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T16>)port.PortsTable [16]; }

		public void Post (T17 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T17>)PortsTable [17]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T17> P17 { get { return PortSetHelper.GetPort<T17> (ModeInternal, PortsTable, 17); } }
		public static implicit operator Port<T17> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T17>)port.PortsTable [17]; }

		public void Post (T18 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T18>)PortsTable [18]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T18> P18 { get { return PortSetHelper.GetPort<T18> (ModeInternal, PortsTable, 18); } }
		public static implicit operator Port<T18> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> port) { return (Port<T18>)port.PortsTable [18]; }

	}

	public class PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> (), new Port<T17> (), new Port<T18> (), new Port<T19> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16), typeof(T17), typeof(T18), typeof(T19) };
			Mode = mode;
		}

		public PortSet (Port<T0> parameter0, Port<T1> parameter1, Port<T2> parameter2, Port<T3> parameter3, Port<T4> parameter4, Port<T5> parameter5, Port<T6> parameter6, Port<T7> parameter7, Port<T8> parameter8, Port<T9> parameter9, Port<T10> parameter10, Port<T11> parameter11, Port<T12> parameter12, Port<T13> parameter13, Port<T14> parameter14, Port<T15> parameter15, Port<T16> parameter16, Port<T17> parameter17, Port<T18> parameter18, Port<T19> parameter19)
		{
			PortsTable = new IPort [] { new Port<T0> (), new Port<T1> (), new Port<T2> (), new Port<T3> (), new Port<T4> (), new Port<T5> (), new Port<T6> (), new Port<T7> (), new Port<T8> (), new Port<T9> (), new Port<T10> (), new Port<T11> (), new Port<T12> (), new Port<T13> (), new Port<T14> (), new Port<T15> (), new Port<T16> (), new Port<T17> (), new Port<T18> (), new Port<T19> () };
			Types = new Type [] { typeof (T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8), typeof(T9), typeof(T10), typeof(T11), typeof(T12), typeof(T13), typeof(T14), typeof(T15), typeof(T16), typeof(T17), typeof(T18), typeof(T19) };
		}


		public void Post (T0 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T0>)PortsTable [0]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T0> P0 { get { return PortSetHelper.GetPort<T0> (ModeInternal, PortsTable, 0); } }
		public static implicit operator Port<T0> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T0>)port.PortsTable [0]; }

		public void Post (T1 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T1>)PortsTable [1]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T1> P1 { get { return PortSetHelper.GetPort<T1> (ModeInternal, PortsTable, 1); } }
		public static implicit operator Port<T1> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T1>)port.PortsTable [1]; }

		public void Post (T2 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T2>)PortsTable [2]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T2> P2 { get { return PortSetHelper.GetPort<T2> (ModeInternal, PortsTable, 2); } }
		public static implicit operator Port<T2> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T2>)port.PortsTable [2]; }

		public void Post (T3 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T3>)PortsTable [3]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T3> P3 { get { return PortSetHelper.GetPort<T3> (ModeInternal, PortsTable, 3); } }
		public static implicit operator Port<T3> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T3>)port.PortsTable [3]; }

		public void Post (T4 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T4>)PortsTable [4]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T4> P4 { get { return PortSetHelper.GetPort<T4> (ModeInternal, PortsTable, 4); } }
		public static implicit operator Port<T4> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T4>)port.PortsTable [4]; }

		public void Post (T5 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T5>)PortsTable [5]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T5> P5 { get { return PortSetHelper.GetPort<T5> (ModeInternal, PortsTable, 5); } }
		public static implicit operator Port<T5> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T5>)port.PortsTable [5]; }

		public void Post (T6 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T6>)PortsTable [6]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T6> P6 { get { return PortSetHelper.GetPort<T6> (ModeInternal, PortsTable, 6); } }
		public static implicit operator Port<T6> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T6>)port.PortsTable [6]; }

		public void Post (T7 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T7>)PortsTable [7]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T7> P7 { get { return PortSetHelper.GetPort<T7> (ModeInternal, PortsTable, 7); } }
		public static implicit operator Port<T7> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T7>)port.PortsTable [7]; }

		public void Post (T8 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T8>)PortsTable [8]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T8> P8 { get { return PortSetHelper.GetPort<T8> (ModeInternal, PortsTable, 8); } }
		public static implicit operator Port<T8> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T8>)port.PortsTable [8]; }

		public void Post (T9 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T9>)PortsTable [9]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T9> P9 { get { return PortSetHelper.GetPort<T9> (ModeInternal, PortsTable, 9); } }
		public static implicit operator Port<T9> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T9>)port.PortsTable [9]; }

		public void Post (T10 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T10>)PortsTable [10]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T10> P10 { get { return PortSetHelper.GetPort<T10> (ModeInternal, PortsTable, 10); } }
		public static implicit operator Port<T10> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T10>)port.PortsTable [10]; }

		public void Post (T11 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T11>)PortsTable [11]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T11> P11 { get { return PortSetHelper.GetPort<T11> (ModeInternal, PortsTable, 11); } }
		public static implicit operator Port<T11> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T11>)port.PortsTable [11]; }

		public void Post (T12 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T12>)PortsTable [12]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T12> P12 { get { return PortSetHelper.GetPort<T12> (ModeInternal, PortsTable, 12); } }
		public static implicit operator Port<T12> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T12>)port.PortsTable [12]; }

		public void Post (T13 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T13>)PortsTable [13]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T13> P13 { get { return PortSetHelper.GetPort<T13> (ModeInternal, PortsTable, 13); } }
		public static implicit operator Port<T13> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T13>)port.PortsTable [13]; }

		public void Post (T14 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T14>)PortsTable [14]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T14> P14 { get { return PortSetHelper.GetPort<T14> (ModeInternal, PortsTable, 14); } }
		public static implicit operator Port<T14> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T14>)port.PortsTable [14]; }

		public void Post (T15 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T15>)PortsTable [15]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T15> P15 { get { return PortSetHelper.GetPort<T15> (ModeInternal, PortsTable, 15); } }
		public static implicit operator Port<T15> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T15>)port.PortsTable [15]; }

		public void Post (T16 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T16>)PortsTable [16]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T16> P16 { get { return PortSetHelper.GetPort<T16> (ModeInternal, PortsTable, 16); } }
		public static implicit operator Port<T16> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T16>)port.PortsTable [16]; }

		public void Post (T17 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T17>)PortsTable [17]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T17> P17 { get { return PortSetHelper.GetPort<T17> (ModeInternal, PortsTable, 17); } }
		public static implicit operator Port<T17> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T17>)port.PortsTable [17]; }

		public void Post (T18 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T18>)PortsTable [18]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T18> P18 { get { return PortSetHelper.GetPort<T18> (ModeInternal, PortsTable, 18); } }
		public static implicit operator Port<T18> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T18>)port.PortsTable [18]; }

		public void Post (T19 item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T19>)PortsTable [19]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T19> P19 { get { return PortSetHelper.GetPort<T19> (ModeInternal, PortsTable, 19); } }
		public static implicit operator Port<T19> (PortSet<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> port) { return (Port<T19>)port.PortsTable [19]; }

	}
}
