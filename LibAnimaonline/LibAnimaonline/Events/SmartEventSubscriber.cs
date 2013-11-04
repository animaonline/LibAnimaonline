using System;

namespace Animaonline.Events
{
    public abstract class SmartEventSubscriber
    {
        public abstract void Unsubscribe();
    }

    public class SmartEventSubscriber<T> : SmartEventSubscriber
    {
        public SmartEventSubscriber(SmartEvent<T> _event, Action<SmartEventSubscriber<T>, T> _callback)
        {
            this.Event = _event;
            this.Callback = _callback;
        }

        public SmartEvent<T> Event { get; private set; }
        public Action<SmartEventSubscriber<T>, T> Callback { get; private set; }

        #region Implementation

        public override void Unsubscribe()
        {
            Event.Unsubscribe(Callback);
        }

        #endregion
    }
}
