using System;
using System.Collections.Concurrent;

namespace Animaonline.Threading
{
    public class BlockingContextActionQueue : ConcurrentQueue<Action>
    {
        #region Public Constructors

        public BlockingContextActionQueue(BlockingContext context)
        {
            this.context = context;
        }

        #endregion

        #region Private Fields

        private readonly BlockingContext context;

        #endregion

        #region Public Methods

        public new void Enqueue(Action item)
        {
            base.Enqueue(item);

            if (context != null)
                context.OnEnqueue();
        }

        #endregion
    }
}