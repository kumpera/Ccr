//
// DispatcherQueue.cs
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
using System.Xml.Serialization;
using System.Threading;
using Stopwatch=System.Diagnostics.Stopwatch;

namespace Microsoft.Ccr.Core
{
	class IteratorData
	{
		IEnumerator<ITask> iterator;

		internal IteratorData (IEnumerator<ITask> iterator)
		{
			this.iterator = iterator;
		}

		internal void Begin (DispatcherQueue queue)
		{
			try {
				if (iterator.MoveNext ()) {
					ITask task = iterator.Current;
					task.LinkedIterator = this;
					task.TaskQueue = queue;
					task.Execute ();
				}
			} catch (Exception ex) {
				this.iterator.Dispose ();
				this.iterator = null;
				Console.WriteLine (ex);
				//TODO post it somewhere
			}
		}

		internal void Step (ITask task, DispatcherQueue queue)
		{
			try {
				if (iterator.MoveNext ()) {
					task = iterator.Current;
					task.LinkedIterator = this;
					task.TaskQueue = queue;
					task.Execute ();
				}
			}  catch (Exception ex) {
				this.iterator.Dispose ();
				this.iterator = null;
				Console.WriteLine (ex);
				//TODO post it somewhere
			}
		}
	}

	public class DispatcherQueue : IDisposable
	{
		Dispatcher dispatcher;
		bool suspended;
		long scheduledItems;
		LinkedList<ITask> queue = new LinkedList<ITask> ();
		object _lock = new object ();
		volatile bool isDisposed;
		double currentRate;
		Stopwatch watch;
		volatile int waitingProducers;

		internal object DispatcherObject { get; set; }

		void ConfigureDefaults ()
		{
			ThrottlingSleepInterval = new TimeSpan (0,0,0,0,10);
			Timescale = 1;
		}

		DispatcherQueue (string name, Dispatcher dispatcher, TaskExecutionPolicy policy)
		{
			if (name == null)
				throw new ArgumentNullException ("name");

			if (dispatcher == null)
				throw new ArgumentNullException ("dispatcher");

			this.dispatcher = dispatcher;

			Name = name;

			ThrottlingSleepInterval = new TimeSpan (0,0,0,0,10);
			Timescale = 1;
	
			Policy = policy;
			dispatcher.Register (this);
		}


		public DispatcherQueue () : this ("Unnamed queue using Threadpool") {}

		public DispatcherQueue (string name) {
			if (name == null)
				throw new ArgumentNullException ("name");

			Name = name;
			ThrottlingSleepInterval = new TimeSpan (0,0,0,0,10);
			Timescale = 1;
		}

		public DispatcherQueue (string name, Dispatcher dispatcher) : this (name, dispatcher, TaskExecutionPolicy.Unconstrained)
		{
			MaximumSchedulingRate = 1;
		}

		public DispatcherQueue (string name, Dispatcher dispatcher, TaskExecutionPolicy policy, int maximumQueueDepth) : this (name, dispatcher, policy) 
		{
			if (policy == TaskExecutionPolicy.ConstrainSchedulingRateDiscardTasks || policy == TaskExecutionPolicy.ConstrainSchedulingRateThrottleExecution)
				throw new ArgumentException ("schedulingRate");
			if (maximumQueueDepth < 1 && policy != TaskExecutionPolicy.Unconstrained)
				throw new ArgumentException ("maximumQueueDepth");

			MaximumQueueDepth = maximumQueueDepth;
		}

		public DispatcherQueue (string name, Dispatcher dispatcher, TaskExecutionPolicy policy, double schedulingRate) : this (name, dispatcher, policy)
		{
			if (policy == TaskExecutionPolicy.ConstrainQueueDepthDiscardTasks || policy == TaskExecutionPolicy.ConstrainQueueDepthThrottleExecution)
				throw new ArgumentException ("maximumQueueDepth");
			if (schedulingRate < 1 && policy != TaskExecutionPolicy.Unconstrained)
				throw new ArgumentException ("schedulingRate");

			MaximumSchedulingRate = schedulingRate;
			watch = Stopwatch.StartNew ();
		}

		~DispatcherQueue ()
		{
			Dispose (false);
		}

		[MonoTODO ("Implement ignore on dispose Dispatcher option")]
		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
			lock (_lock) {
				isDisposed = true;
				Monitor.PulseAll (_lock);
			}	
		
			if (disposing)
				GC.SuppressFinalize (this);
		}

		internal void UpdateSchedulingRate ()
		{
			//MS's behavior is pretty odd, it seens to calculate the rate over the whole execution time
			//It should a more stocastic model of storing the last 1,5,15 secs and be done with it.
			//Otherwise it's method is very fragile against bursts.
			currentRate = scheduledItems / (double)watch.Elapsed.TotalSeconds;
		}

		[MonoTODO ("support nested iterators")]
		internal void RunTask (ITask task)
		{
			Exception excep = null;
			try {
				var obj = task.LinkedIterator;
				var iter = task.Execute ();
				if (obj != null && iter != null)
					Console.WriteLine ("FIX ME PLEASE as I have a nested iterator");
	
				if (iter != null) {
					IteratorData id = new IteratorData (iter);
					id.Begin (this);
				}
				if (obj != null)
					((IteratorData)obj).Step (task, this);
			} catch (Exception e) {
					excep = e;
				if (UnhandledException != null) {
					UnhandledException (this, new UnhandledExceptionEventArgs (e, false));
					excep = null;
				}
				var port = UnhandledExceptionPort;
				if (port != null) {
					port.Post (e);	
					excep = null;
				}
			} finally {
				dispatcher.TaskDone (task, excep);
			}
		}

		public virtual bool Enqueue (ITask task)
		{
			if (isDisposed)
				throw new ObjectDisposedException (ToString ());
			var res = true;;
			task.TaskQueue = this;
			if (dispatcher == null) {
				Handler<ITask> x = RunTask;
				
				++scheduledItems;
				return x.BeginInvoke (task, null, null) != null;
			} else {
				lock (_lock) {
					var p = Policy;
					switch (p) {
					case TaskExecutionPolicy.ConstrainQueueDepthDiscardTasks:
						ITask tk = null;
						if (queue.Count >= MaximumQueueDepth) {
							queue.RemoveFirst ();
							res = false;
						}
						break;
					case TaskExecutionPolicy.ConstrainQueueDepthThrottleExecution:
						while (!isDisposed && queue.Count >= MaximumQueueDepth) {
							++waitingProducers;
							Monitor.Wait (_lock);
							--waitingProducers;
						}
						if (isDisposed)
							throw new ObjectDisposedException (ToString ());
						break;
					case TaskExecutionPolicy.ConstrainSchedulingRateDiscardTasks:
						UpdateSchedulingRate ();
						if (CurrentSchedulingRate > MaximumSchedulingRate) {
							queue.RemoveFirst ();
							res = false;
						}
						break;
					case TaskExecutionPolicy.ConstrainSchedulingRateThrottleExecution:
						UpdateSchedulingRate ();
						while (!isDisposed && CurrentSchedulingRate > MaximumSchedulingRate) {
							++waitingProducers;
							Monitor.Wait (_lock);
							--waitingProducers;
							UpdateSchedulingRate ();
						}
						if (isDisposed)
							throw new ObjectDisposedException (ToString ());
						break;
					}

					++scheduledItems;
					queue.AddLast (task);
				}
				if (!suspended)
					dispatcher.Notify (this);
				return res;
			}
		}

		public virtual bool TryDequeue (out ITask task)
		{
			if (isDisposed)
				throw new ObjectDisposedException (ToString ());
			if (suspended) {
				task = null;
				return false;
			}
			lock (_lock) {
				if (queue.Count == 0) {
					task = null;
					return false;
				}
				task = queue.First.Value;
				queue.RemoveFirst ();
				if (waitingProducers > 0)
					Monitor.Pulse (_lock);
				return true;
			}
		}


		public virtual void Suspend ()
		{
			lock (_lock) {
				suspended = true;
			}
		}

		public virtual void Resume ()
		{
			bool orig;
			lock (_lock) {
				orig = suspended;
				suspended = false;
				if (waitingProducers > 0)
					Monitor.Pulse (_lock);
			}
			if (orig && dispatcher != null)
				dispatcher.Notify (this);
		}

		public int Count
		{
			get { return queue.Count; }
			set {}
		}

		public double CurrentSchedulingRate
		{
			get { return currentRate; }
			set {} //stupid
		}

		public string Name { get; set; }

		[XmlIgnoreAttribute]
		public Port<ITask> ExecutionPolicyNotificationPort { get; set; }
		
		public bool IsDisposed
		{
			get { return isDisposed; }
		 	set { }
		}

		public bool IsSuspended
		{
			get { return suspended; }
		}

		public bool IsUsingThreadPool
		{
			get { return dispatcher == null; }
			set
			{
				if (dispatcher != null && !value)
					throw new InvalidOperationException ();
			}
		}
			
		public int MaximumQueueDepth { get; set; }
		public double MaximumSchedulingRate { get; set; }
		public TaskExecutionPolicy Policy { get; set; }

		public long ScheduledTaskCount {
			get { return scheduledItems; }
			set { } //stupid setter
		}

		[XmlIgnoreAttribute]
		public TimeSpan ThrottlingSleepInterval { get; set; }

		public double Timescale { get; set; }
		public Port<Exception> UnhandledExceptionPort { get; set; }

		[XmlIgnoreAttribute]
		public Dispatcher Dispatcher
		{
			get { return dispatcher; }
		}

		public event UnhandledExceptionEventHandler UnhandledException;
	}
}
