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
            _actions = new BlockingContextActionQueue(OnEnqueue);
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

        /// <summary>
        /// Is called when a new item is added to the action queue
        /// </summary>
        private void OnEnqueue()
        {
            //Wake up the executing and carry on with the execution
            lock (sync_lock)
                Monitor.Pulse(sync_lock);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Blocks the executing thread while awaiting for tasks
        /// </summary>
        public void Start()
        {
            //while _keepAlive is true , and Stop() has not been called
            while (_keepAlive)
            {
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

                    //Stop() was called, exit flow
                    if (!_keepAlive)
                        break;

                    //all actions have been executed, the action queue is empty
                    //block the thread
                    Monitor.Wait(sync_lock);
                }
            }
        }

        /// <summary>
        /// Stops processing the queue of actions and releases the context
        /// </summary>
        public void Stop()
        {
            _keepAlive = false;
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