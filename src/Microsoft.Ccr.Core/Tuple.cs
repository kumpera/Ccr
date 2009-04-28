//
// Tuple.cs
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

	public class Tuple<ITEM0>
	{
		public Tuple ()
		{
		}

		public Tuple (ITEM0 item0)
		{
			this.Item0 = item0;
		}

		public ITEM0 ToItem0 ()
		{
			return Item0;
		}

		public static implicit operator ITEM0 (Tuple<ITEM0> tuple)
		{
			return tuple.Item0;
		}

		public ITEM0 Item0 { get; set; }
	}

	public class Tuple<ITEM0, ITEM1>
	{
		public Tuple ()
		{
		}

		public Tuple (ITEM0 item0, ITEM1 item1)
		{
			this.Item0 = item0;
			this.Item1 = item1;
		}

		public ITEM0 ToItem0 ()
		{
			return Item0;
		}

		public ITEM1 ToItem1 ()
		{
			return Item1;
		}

		public static implicit operator ITEM0 (Tuple<ITEM0, ITEM1> tuple)
		{
			return tuple.Item0;
		}

		public static implicit operator ITEM1 (Tuple<ITEM0, ITEM1> tuple)
		{
			return tuple.Item1;
		}

		public ITEM0 Item0 { get; set; }
		public ITEM1 Item1 { get; set; }
	}
}
