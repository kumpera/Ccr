//
// Causality.cs
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

namespace Microsoft.Ccr.Core {

	public class Causality : ICausality
	{
		string name;
		IPort exceptionPort;
		IPort coordinationPort;

		public Causality (string name, Guid guid, IPort exceptionPort, IPort coordinationPort)
		{
			this.name = name;
			Guid = guid;
			this.exceptionPort = exceptionPort;
			this.coordinationPort = coordinationPort;
		}

		public Causality (Guid guid)
		{
				Guid = guid;
		}

		public Causality (string name)
		{
			this.name = name;
		}

		public Causality (string name, IPort exceptionPort) : this (name, exceptionPort, null)
		{
		}

		public Causality (string name, IPort exceptionPort, IPort coordinationPort)
		{
			this.name = name;
			this.exceptionPort = exceptionPort;
			this.coordinationPort = coordinationPort;
		}

		public IPort CoordinationPort { get { return coordinationPort; } }
		public IPort ExceptionPort { get { return exceptionPort; } }
		public Guid Guid { get; set; }
		public string Name { get { return name; } }

		[MonoTODO ("Figure out how/when to break")]
		public bool BreakOnReceive { get; set; }
	}
}
