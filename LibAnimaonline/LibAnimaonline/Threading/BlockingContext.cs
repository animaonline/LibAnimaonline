using System;
using System.Threading;

namespace Animaonline.Threading
{
    /// <summary>
    /// Blocks the executing thread while awaiting for tasks
    /// </summary>
    public class BlockingContext
    {
        #region Public Constructor

        public BlockingContext()
        {
            this._keepAlive = true;
            _actions = new BlockingContextActionQueue(this);
        }

        #endregion

        #region Destructor

        ~BlockingContext()
        {
            _actions = null;
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// Synchronization lock
        /// </summary>
        private readonly object sync_lock = new object();

        /// <summary>
        /// Indicates whether the blocking context is alive
        /// </summary>
        private bool _keepAlive;

        /// <summary>
        /// A queue of actions to be executed inside this context
        /// </summary>
        private BlockingContextActionQueue _actions;

        private Timer _unblockTimer;

        #endregion

        #region Public Properties

        /// <summary>
        /// A queue of actions to be executed inside this context
        /// </summary>
        public BlockingContextActionQueue ContextActions
        {
            get { return _actions; }
        }

        #endregion

        #region Private Methods

        private void scheduleUnblockCallback(object state)
        {
            if (_unblockTimer != null)
            {
                _unblockTimer.Dispose();
                _unblockTimer = null;
            }

            this.Unblock();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Is called when a new item is added to the action queue
        /// </summary>
        internal void OnEnqueue()
        {
            //Wake up the Block() loop and carry on with the execution
            lock (sync_lock)
                Monitor.Pulse(sync_lock);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Blocks the executing thread while awaiting for tasks
        /// </summary>
        public void Block()
        {
            _keepAlive = true;

            //while _keepAlive is true , and Stop() has not been called
            while (_keepAlive)
                //enter the monitor
                lock (sync_lock)
                {
                    //process all queued actions
                    while (_actions.Count > 0)
                    {
                        //Stop() was called, exit flow
                        if (!_keepAlive)
                            break;

                        Action action;

                        //attempt to dequeue the action
                        var dequeueResult = _actions.TryDequeue(out action);

                        if (dequeueResult && action != null)
                            //successfully dequeued the action
                            action();
                    }

                    //if Stop() was called, exit flow
                    if (!_keepAlive)
                        break;

                    //all actions have been executed, the action queue is empty
                    //block the thread
                    Monitor.Wait(sync_lock);
                }
        }

        /// <summary>
        /// Stops processing the queue of actions and releases the context
        /// </summary>
        public void Unblock()
        {
            lock (sync_lock)
            {
                _keepAlive = false;
                Monitor.Pulse(sync_lock);
            }
        }

        /// <summary>
        /// Calls Unblock after the given amount of milliseconds has passed
        /// </summary>
        /// <param name="milliseconds">Timeout</param>
        public void ScheduleUnblock(int milliseconds)
        {
            if (milliseconds < 1)
                throw new ArgumentException("Invalid time");

            this._unblockTimer = new Timer(scheduleUnblockCallback, null, milliseconds, Timeout.Infinite);
        }

        /// <summary>
        /// Enqueues an action in the action queue
        /// </summary>
        /// <param name="action">The action to enqueue</param>
        public void Enqueue(Action action)
        {
            _actions.Enqueue(action);
        }

        #endregion
    }
}