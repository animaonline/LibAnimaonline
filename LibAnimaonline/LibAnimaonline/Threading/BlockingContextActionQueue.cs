using System;
using System.Collections.Concurrent;

namespace Animaonline.Threading
{
    public class BlockingContextActionQueue : ConcurrentQueue<Action>
    {
        #region Public Constructors

        public BlockingContextActionQueue() { }

        public BlockingContextActionQueue(Action onEnqueue)
        {
            _onEnqueue = onEnqueue;
        }

        #endregion

        #region Private Fields

        private readonly Action _onEnqueue;

        #endregion

        #region Public Methods

        public new void Enqueue(Action item)
        {
            base.Enqueue(item);

            if (_onEnqueue != null)
                _onEnqueue();
        }

        #endregion
    }
}