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

	internal static class PortSetHelper
	{
		static IPort ResolvePort (object item, Type[] types, IPort[] ports)
		{
			if (item == null) {
				for (int i = 0; i < types.Length; ++i) {
					if (!types [i].IsValueType)
						return ports [i];
				}
				return null;
			}

			Type item_type = item.GetType ();
			int first_match = -1;

			for (int i = 0; i < types.Length; ++i) {
				if (types [i] == item_type)
					return ports [i];
				if (first_match == -1 && types [i].IsAssignableFrom (item_type))
					first_match = i;
			}	
			return first_match >= 0 ? ports [first_match] : null;
		}


		internal static IPort FindPort (Type portType, IPort[] ports, Type[] types, PortSetMode mode)
		{
			if (mode != PortSetMode.Default)
				throw new InvalidOperationException ("Mode is not Default"); 
			for (int i = 0; i < types.Length; ++i)
				if (types [i] == portType)
					return ports [i];
			return null;
		}
		
		internal static void Post (object item, IPort[] ports, Type[] types, PortSetMode mode, Port<Object> sharedPort)
		{
			if (mode == PortSetMode.SharedPort) {
				sharedPort.Post (item);
				return;
			}

			IPort port = ResolvePort (item, types, ports);
			if (port == null)
				throw new PortNotFoundException ("item");

			port.PostUnknownType (item);
		}

		internal static bool TryPost (object item, IPort[] ports, Type[] types, PortSetMode mode, Port<Object> sharedPort)
		{
			if (mode == PortSetMode.SharedPort) {
				sharedPort.Post (item);
				return true;
			}

			IPort port = ResolvePort (item, types, ports);

			if (port == null)
				return false;

			port.PostUnknownType (item);
			return true;
		}

		internal static T Test<T> (IPort[] ports, Type[] types, PortSetMode mode, Port<Object> sharedPort)
		{
			if (mode == PortSetMode.SharedPort)
				return (T)sharedPort.Test ();

			IPortReceive p = (IPortReceive)FindPort (typeof (T), ports, types, mode);
			if (p == null)
				throw new PortNotFoundException(typeof (T).ToString ());
			return (T)p.Test ();
		}

		internal static ICollection<IPort> GetPorts (IPort[] ports, PortSetMode mode, Port<Object> sharedPort)
		{
			if (mode == PortSetMode.Default)
				return Array.AsReadOnly (ports);

			List<IPort> list = new List<IPort> ();
			list.Add (sharedPort);
			return list.AsReadOnly ();
		}

		internal static Port<T> GetPort<T> (PortSetMode mode, Port<T> port)
		{
			if (mode != PortSetMode.Default)
				throw new InvalidOperationException ("Mode is not Default"); 
			return port;
		} 
	}

	public class PortSet : IPortSet, IPort
	{
		protected PortSetMode ModeInternal;
		protected IPort[] PortsTable;
		protected Port<Object> SharedPortInternal;
		protected Type[] Types;

		protected PortSet ()
		{
		}

		protected Port<TYPE> AllocatePort<TYPE> ()
		{
			return (Port<TYPE>) PortSetHelper.FindPort (typeof (TYPE), PortsTable, Types, ModeInternal);
		}

		public PortSet (params Type[] types)
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
			PortSetHelper.Post (item, PortsTable, Types, ModeInternal, SharedPortInternal);
		}

		public bool TryPostUnknownType (object item)
		{
			return PortSetHelper.TryPost (item, PortsTable, Types, ModeInternal, SharedPortInternal);
		}

		public T Test<T> ()
		{
			return PortSetHelper.Test<T> (PortsTable, Types, ModeInternal, SharedPortInternal);
		}

		public static bool FindTypeFromRuntimeType (object item, Type[] types, out Type portItemType)
		{
			portItemType = null;
			if (item == null) {
				foreach (var t in types) {
					if (!t.IsValueType) {
						portItemType = t;
						return true;
					}
				}
				return false;
			}
			
			portItemType = item.GetType ();
			return true;
		}

		public IPort this[Type portItemType]
		{
			get
			{
				return PortSetHelper.FindPort (portItemType, PortsTable, Types, ModeInternal);
			}
		}

		public PortSetMode Mode
		{
			get { return ModeInternal; }
			set
			{
				ModeInternal = value;
				if (value == PortSetMode.Default) {
					SharedPortInternal = null;
				} else {
					SharedPortInternal = new Port<object> ();
				}
			}
		}

		public ICollection<IPort> Ports
		{
			get
			{
				return PortSetHelper.GetPorts (PortsTable, ModeInternal, SharedPortInternal);
			}
		}

		public Port<Object> SharedPort
		{
			get { return SharedPortInternal; }
		}
	}
}
