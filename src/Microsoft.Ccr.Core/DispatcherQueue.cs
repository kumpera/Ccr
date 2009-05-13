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

namespace Microsoft.Ccr.Core {

	public class DispatcherQueue : IDisposable
	{
		Dispatcher dispatcher;
		bool suspended;
		long scheduledItems;
		LinkedList<ITask> queue = new LinkedList<ITask> ();
		object _lock = new object ();

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

		public DispatcherQueue(string name, Dispatcher dispatcher, TaskExecutionPolicy policy, int maximumQueueDepth) : this (name, dispatcher, policy) 
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
		}

		~DispatcherQueue ()
		{
			Dispose (false);
		}

		[MonoTODO ("Implement proper dispose semantics and checks")]
		public void Dispose ()
		{
			Dispose (true);
		}

		protected virtual void Dispose (bool disposing)
		{
			IsDisposed = true;
			if (disposing)
				GC.SuppressFinalize (this);
		}

		void DispatchTask (ITask task)
		{
			try {
				if (task.Execute () != null)
					throw new Exception ("Iterative tasks not supported");
			} catch (Exception e) {
				if (UnhandledException != null)
					UnhandledException (this, new UnhandledExceptionEventArgs (e, false));
			}
		}

		[MonoTODO ("doesn't work with constrained policies")]
		public virtual bool Enqueue (ITask task)
		{
			task.TaskQueue = this;
			if (dispatcher == null) {
				Handler<ITask> x = DispatchTask;
				
				++scheduledItems;
				return x.BeginInvoke (task, null, null) != null;
			} else {
				lock (_lock) {
					queue.AddLast (task);
				}
				dispatcher.Notify (this);
				return true;//?
			}
		}

		[MonoTODO ("doesn't work constrained policies")]
		public virtual bool TryDequeue (out ITask task)
		{
			lock (_lock) {
				if (queue.Count == 0) {
					task = null;
					return false;
				}
				task = queue.First.Value;
				queue.RemoveFirst ();
				return true;
			}
		}


		[MonoTODO ("doesn't work with dispatcher or constrained policies")]
		public virtual void Suspend ()
		{
			suspended = true;
		}

		[MonoTODO ("doesn't work with dispatcher or constrained policies")]
		public virtual void Resume ()
		{
			suspended = false;
		}

		public int Count { get; set; }
		public double CurrentSchedulingRate { get; set; }
		public string Name { get; set; }

		[XmlIgnoreAttribute]
		public Port<ITask> ExecutionPolicyNotificationPort { get; set; }
		
		[MonoTODO ("having a setter is VERY weird, must test behavior")]
		public bool IsDisposed { get; set; }

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
			set { scheduledItems = value; }
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
