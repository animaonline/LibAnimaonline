using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Animaonline.Events;

namespace LibAnimaonline.Console.Tests.Events
{
    public class SmartEventTest : ITest
    {
        public void StartTest()
        {
            var messageReceiver = new SmartEvent<string>();
            messageReceiver.Subscribe(onMessageReceived);
            messageReceiver.Subscribe(onMessageReceived2);
            messageReceiver.Trigger("Hello world");
        }

        private void onMessageReceived(SmartEventSubscriber<string> subscriber, object x)
        {
            subscriber.Unsubscribe();
        }

        private void onMessageReceived2(SmartEventSubscriber<string> subscriber, object x)
        {
            subscriber.Unsubscribe();
        }
    }
}
