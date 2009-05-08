def create_port_set types
	inst = "T0"
	(1..(types - 1)).each { |t| inst = inst + ", T#{t}" }

	init_ports = "PortsTable = new IPort [] { new Port<T0> ()"
	init_types = "Types = new Type [] { typeof (T0)"
	init_params = "Port<T0> parameter0"

	(1..(types - 1)).each { |t|
		init_ports += ", new Port<T#{t}> ()"
		init_types += ", typeof(T#{t})"
		init_params += ", Port<T#{t}> parameter#{t}"
 	}

 	init_ports += " };"
 	init_types += " };"

	print """
	public class PortSet<#{inst}> : PortSet
	{
		public PortSet () : this (PortSetMode.Default) {}

		public PortSet (PortSetMode mode)
		{
			#{init_ports}
			#{init_types}
			Mode = mode;
		}

		public PortSet (#{init_params})
		{
			#{init_ports}
			#{init_types}
		}

"
	(0..(types - 1)).each { |t|
			print """
		public void Post (T#{t} item)
		{
			if (ModeInternal == PortSetMode.Default)
				((Port<T#{t}>)PortsTable [#{t}]).Post (item);
			else
				SharedPortInternal.Post (item);
		}

		public Port<T#{t}> P#{t} { get { return PortSetHelper.GetPort<T#{t}> (ModeInternal, PortsTable, #{t}); } }
		public static implicit operator Port<T#{t}> (PortSet<#{inst}> port) { return (Port<T#{t}>)port.PortsTable [#{t}]; }
"}

	if (types == 3)
		(0..(types - 1)).each { |t|
			print """
		public static implicit operator T#{t} (PortSet<#{inst}> port) { return port.Test<T#{t}> (); }
"}

	end
	print "\n\t}\n"
end


def print_header file_name
	print """//
// #{file_name}.cs
//
// Author:
//   Rodrigo Kumpera  <kumpera@gmail.com>
//
//
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// \"Software\"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND,
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
"
end

def print_footer
	print "}\n"
end

print_header "PortSet.generared.cs"
(3..20).each {|n| create_port_set n }
#create_port_set 3
print_footer
