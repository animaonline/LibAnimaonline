/*
LibAnimaonline - A set of useful cross platform helper classes to use with .NET, written in C#
Copyright (C) 2007-2014  Roman Alifanov - animaonline@gmail.com - http://www.animaonline.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see http://www.gnu.org/licenses/
 */
using System;
using System.Threading;

namespace Animaonline.Threading
{
    /// <summary>
    /// Blocks the executing thread (similar to AutoResetEvent) while awaiting for tasks
    /// </summary>
    public class BlockingContext
    {
        #region Public Constructor

        public BlockingContext()
        {
            _keepAlive = true;
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
        private readonly object _syncLock = new object();

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

        private void ScheduleUnblockCallback(object state)
        {
            if (_unblockTimer != null)
            {
                _unblockTimer.Dispose();
                _unblockTimer = null;
            }

            Unblock();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Is called when a new item is added to the action queue
        /// </summary>
        internal void OnEnqueue()
        {
            //Wake up the Block() loop and carry on with the execution
            lock (_syncLock)
                Monitor.Pulse(_syncLock);
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
                lock (_syncLock)
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
                    Monitor.Wait(_syncLock);
                }
        }

        /// <summary>
        /// Stops processing the queue of actions and releases the context
        /// </summary>
        public void Unblock()
        {
            lock (_syncLock)
            {
                _keepAlive = false;
                Monitor.Pulse(_syncLock);
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

            _unblockTimer = new Timer(ScheduleUnblockCallback, null, milliseconds, Timeout.Infinite);
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