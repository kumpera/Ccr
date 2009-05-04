//
// Task.cs
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
using System.Diagnostics;
using Microsoft.Ccr.Core.Arbiters;
namespace Microsoft.Ccr.Core {

	public class Task : TaskCommon
	{
		Handler handler;

		public Task (Handler handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");
			this.handler = handler;
		}

		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		public override IEnumerator<ITask> Execute ()
		{
			handler ();
			return null;
		}

		public override ITask PartialClone ()
		{
			return new Task (this.handler);
		}

		public override IPortElement this [int index]
		{
			get { throw new NotSupportedException (); }
			set { throw new NotSupportedException (); }
		}

		public override int PortElementCount
		{
			get { return 0; }
		}

		public Handler Handler
		{
			get { return handler; }
		}
	}

	public class Task<T0> : TaskCommon
	{
		protected PortElement<T0> Param0;
		Handler<T0> handler;

		public Task (T0 t0, Handler<T0> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");
			this.handler = handler;
			this.Param0 = new PortElement<T0> (t0);
		}

		public Task (Handler<T0> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");
			this.handler = handler;
		}

		public override string ToString ()
		{
			return String.Format ("{0} with param0 {1}", typeof (Task<T0>), Param0);
		}

		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		public override IEnumerator<ITask> Execute ()
		{
			handler (Param0.TypedItem);
			return null;
		}

		public override ITask PartialClone ()
		{
			return new Task<T0> (this.handler);
		}

		public override IPortElement this [int index]
		{
			get
			{
				if (index != 0)
					throw new ArgumentException ("index out of range", "index");
				return Param0;
			}
			set
			{
				if (index != 0)
					throw new ArgumentException ("index out of range", "index");
				Param0 = (PortElement<T0>)value;
			}
		}

		public override int PortElementCount
		{
			get { return 1; }
		}
	}

	public class Task<T0, T1> : TaskCommon
	{
		protected PortElement<T0> Param0;
		protected PortElement<T1> Param1;

		Handler<T0, T1> handler;

		public Task (T0 t0, T1 t1, Handler<T0, T1> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");

			this.handler = handler;
			this.Param0 = new PortElement<T0> (t0);
			this.Param1 = new PortElement<T1> (t1);
		}

		public Task (Handler<T0, T1> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");
			this.handler = handler;
		}

		public override string ToString ()
		{
			return String.Format ("{0} with param0 {1} param1 {2}", typeof (Task<T0>), Param0, Param1);
		}

		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		public override IEnumerator<ITask> Execute ()
		{
			handler (Param0.TypedItem, Param1.TypedItem);
			return null;
		}

		public override ITask PartialClone ()
		{
			return new Task<T0, T1> (this.handler);
		}

		public override IPortElement this [int index]
		{
			get
			{
				switch (index) {
				case 0:
					return Param0;
				case 1:
					return Param1;
				default:
					throw new ArgumentException ("index out of range", "index");
				}
			}
			set
			{
				switch (index) {
				case 0:
					Param0 = (PortElement<T0>)value;
					break;
				case 1:
					Param1 = (PortElement<T1>)value;
					break;
				default:
					throw new ArgumentException ("index out of range", "index");
				}
			}
		}

		public override int PortElementCount
		{
			get { return 2; }
		}
	}

	public class Task<T0, T1, T2> : TaskCommon
	{
		protected PortElement<T0> Param0;
		protected PortElement<T1> Param1;
		protected PortElement<T2> Param2;

		Handler<T0, T1, T2> handler;

		public Task (T0 t0, T1 t1, T2 t2, Handler<T0, T1, T2> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");

			this.handler = handler;
			this.Param0 = new PortElement<T0> (t0);
			this.Param1 = new PortElement<T1> (t1);
			this.Param2 = new PortElement<T2> (t2);
		}

		public Task (Handler<T0, T1, T2> handler)
		{
			if (handler == null)
				throw new ArgumentNullException ("handler");
			this.handler = handler;
		}

		public override string ToString ()
		{
			return String.Format ("{0} with param0 {1} param1 {2} param2 {3}", typeof (Task<T0>), Param0, Param1, Param2);
		}

		[DebuggerStepThrough]
		[DebuggerNonUserCode]
		public override IEnumerator<ITask> Execute ()
		{
			handler (Param0.TypedItem, Param1.TypedItem, Param2.TypedItem);
			return null;
		}

		public override ITask PartialClone ()
		{
			return new Task<T0, T1, T2> (this.handler);
		}

		public override IPortElement this [int index]
		{
			get
			{
				switch (index) {
				case 0:
					return Param0;
				case 1:
					return Param1;
				case 2:
					return Param2;
				default:
					throw new ArgumentException ("index out of range", "index");
				}
			}
			set
			{
				switch (index) {
				case 0:
					Param0 = (PortElement<T0>)value;
					break;
				case 1:
					Param1 = (PortElement<T1>)value;
					break;
				case 2:
					Param2 = (PortElement<T2>)value;
					break;
				default:
					throw new ArgumentException ("index out of range", "index");
				}
			}
		}

		public override int PortElementCount
		{
			get { return 3; }
		}
	}

}
