//
// PortSet.cs
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

	public class PortSet : IPortSet, IPort
	{
		protected PortSetMode ModeInternal;
		protected IPort[] PortsTable;
		[MonoTODO ("add support for shared port mode")]
		protected Port<Object> SharedPortInternal;
		protected Type[] Types;

		protected PortSet ()
		{
		}

		public PortSet (Type[] types)
		{
			if (types == null)
				throw new ArgumentNullException ("types");
			if (types.Length == 0)
				throw new ArgumentOutOfRangeException ("types");
			foreach (var t in types)
				if (t == null)
					throw new ArgumentNullException ("types");

			this.Types = types;
			this.PortsTable = new IPort [types.Length];
			for (int i = 0; i < types.Length; ++i)
				this.PortsTable[i] = (IPort)Activator.CreateInstance (typeof (Port<>).MakeGenericType (types [i]));
		}

		public void PostUnknownType (object item)
		{
			throw new NotImplementedException ();
		}

		public bool TryPostUnknownType (object item)
		{
			throw new NotImplementedException ();
		}

		public T Test<T> ()
		{
			IPortReceive p = (IPortReceive)this[typeof(T)];
			if (p == null)
				throw new PortNotFoundException(typeof (T).ToString ());
			return (T)p.Test ();
		}

		public IPort this[Type portItemType]
		{
			get
			{
				for (int i = 0; i < Types.Length; ++i)
					if (Types [i] == portItemType)
						return PortsTable [i];
				return null;
			}
		}

		public PortSetMode Mode
		{
			get { return ModeInternal; }
			set { ModeInternal = value; }
		}

		public ICollection<IPort> Ports
		{
			get { return Array.AsReadOnly (PortsTable); }
		}

		public Port<Object> SharedPort
		{
			get { return SharedPortInternal; }
		}

	}
}
