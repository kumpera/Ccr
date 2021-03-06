//
// Handlers.cs
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

namespace Microsoft.Ccr.Core {
	public delegate void Handler();
	public delegate void Handler<T>(T parameter0);
	public delegate void Handler<T0, T1>(T0 parameter0, T1 parameter1);
	public delegate void Handler<T0, T1, T2>(T0 parameter0, T1 parameter1, T2 parameter2);

	public delegate IEnumerator<ITask> IteratorHandler ();
	public delegate IEnumerator<ITask> IteratorHandler<T0> (T0 parameter0);
	public delegate IEnumerator<ITask> IteratorHandler<T0, T1> (T0 parameter0, T1 parameter1);
	public delegate IEnumerator<ITask> IteratorHandler<T0, T1, T2> (T0 parameter0, T1 parameter1, T2 parameter2);

	public delegate void VariableArgumentHandler<T> (params T[] t);
	public delegate void VariableArgumentHandler<T0, T> (T0 t0,	params T[] t);



}
