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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Animaonline.Events
{
    /// <summary>
    /// A simple SmartEvent implementation, works for methods without signature.
    /// </summary>
    public sealed class SmartEvent
    {
        #region Public Constructors

        /// <summary>
        /// Creates a new SmartEvent
        /// </summary>
        public SmartEvent()
        {
            _subscribers = new List<Action>();
        }

        #endregion

        #region Private Fields

        private readonly List<Action> _subscribers;

        #endregion

        #region Public Properties

        /// <summary>
        /// A read-only collection of subscribers.
        /// Use Subscribe / Unsubscribe to alter the collection.
        /// </summary>
        public ReadOnlyCollection<Action> Subscribers
        {
            get
            {
                return _subscribers.AsReadOnly();
            }
        }

        /// <summary>
        /// Compatibility event, if you want to handle the subscription/unsubscription the old way.
        /// </summary>
        public event Action CompatEvent
        {
            add
            {
                this.Subscribe(value);
            }
            remove
            {
                this.Unsubscribe(value);
            }
        }

        #endregion

        #region Private Methods

        private void _trigger()
        {
            foreach (var subscriber in Subscribers.ToArray())
            {
                if (subscriber != null)

                    subscriber.Invoke();
            }
        }

        private void _triggerAsync()
        {
            foreach (var subscriber in Subscribers)
            {
                if (subscriber != null)
                    subscriber.BeginInvoke(null, null);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Subscribes the given action to this SmartEvent.
        /// </summary>
        /// <param name="action">The action to execute when this event is triggered.</param>
        public void Subscribe(Action action)
        {
            this._subscribers.Add(action);
        }

        /// <summary>
        /// Unsubscribes the given action from this SmartEvent.
        /// </summary>
        /// <param name="action">The action to unsubscribes.</param>
        public void Unsubscribe(Action action)
        {
            lock (this)
            {
                var subscribersMatch = this._subscribers.Where(a => a.Equals(action));

                if (_subscribers != null)
                    foreach (var smartEventSubscriber in subscribersMatch.ToArray())
                        _subscribers.Remove(smartEventSubscriber);
            }
        }

        /// <summary>
        /// Unsubscribes all actions from this SmartEvent.
        /// </summary>
        public void UnsubscribeAll()
        {
            this._subscribers.Clear();
        }

        /// <summary>
        /// Synchronously triggers the event passing an empty instance of <![CDATA[<signature>]]>, signaling to all subscribers.
        /// </summary>
        public void Trigger()
        {
            _trigger();
        }

        /// <summary>
        /// Asynchronously triggers the event passing an empty instance of <![CDATA[<signature>]]>, signaling to all subscribers.
        /// </summary> 
        public void TriggerAsync()
        {
            _triggerAsync();
        }

        #endregion
    }

    /// <summary>
    /// SmartEvent provides an easy way to work with generic events.
    /// 
    /// Support for subscribing / unsubscribing events and a / synchronous execution.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SmartEvent<T>
    {
        #region Public Constructors

        /// <summary>
        /// Creates a new SmartEvent
        /// </summary>
        public SmartEvent()
        {
            _subscribers = new List<SmartEventSubscriber>();
        }

        #endregion

        #region Private Fields

        private readonly List<SmartEventSubscriber> _subscribers;

        #endregion

        #region Public Properties

        /// <summary>
        /// A read-only collection of subscribers.
        /// Use Subscribe / Unsubscribe to alter the collection.
        /// </summary>
        public ReadOnlyCollection<SmartEventSubscriber<T>> Subscribers
        {
            get
            {
                return _subscribers.OfType<SmartEventSubscriber<T>>().ToList().AsReadOnly();
            }
        }

        /// <summary>
        /// Compatibility event, if you want to handle the subscription/unsubscription the old way.
        /// </summary>
        public event Action<SmartEventSubscriber, T> CompatEvent
        {
            add
            {
                this.Subscribe(value);
            }
            remove
            {
                this.Unsubscribe(value);
            }
        }

        #endregion

        #region Private Methods

        private void _trigger(T arguments)
        {
            foreach (var subscriber in Subscribers.ToArray())
            {
                if (subscriber != null)

                    subscriber.Callback.Invoke(subscriber, arguments);
            }
        }

        private void _triggerAsync(T arguments)
        {
            foreach (var subscriber in Subscribers)
            {
                if (subscriber != null)
                    subscriber.Callback.BeginInvoke(subscriber, arguments, null, null);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Subscribes the given action to this SmartEvent.
        /// </summary>
        /// <param name="action">The action to execute when this event is triggered.</param>
        public void Subscribe(Action<SmartEventSubscriber<T>, T> action)
        {
            var eventSubscriber = new SmartEventSubscriber<T>(this, action);

            this._subscribers.Add(eventSubscriber);
        }

        /// <summary>
        /// Unsubscribes the given action from this SmartEvent.
        /// </summary>
        /// <param name="action">The action to unsubscribes.</param>
        public void Unsubscribe(Action<SmartEventSubscriber<T>, T> action)
        {
            lock (this)
            {
                var subscribersMatch = this._subscribers.OfType<SmartEventSubscriber<T>>().Where(a => a.Callback.Equals(action));

                if (_subscribers != null)
                    foreach (var smartEventSubscriber in subscribersMatch.ToArray())
                        _subscribers.Remove(smartEventSubscriber);
            }
        }

        /// <summary>
        /// Unsubscribes all actions from this SmartEvent.
        /// </summary>
        public void UnsubscribeAll()
        {
            this._subscribers.Clear();
        }

        /// <summary>
        /// Synchronously triggers the event passing an empty instance of <![CDATA[<signature>]]>, signaling to all subscribers.
        /// </summary>
        public void Trigger()
        {
            var emptyInstance = default(T);

            _trigger(emptyInstance);
        }

        /// <summary>
        /// Asynchronously triggers the event passing an empty instance of <![CDATA[<signature>]]>, signaling to all subscribers.
        /// </summary> 
        public void TriggerAsync()
        {
            var emptyInstance = default(T);

            _triggerAsync(emptyInstance);
        }

        /// <summary>
        /// Synchronously triggers the event, signaling to all subscribers.
        /// </summary>
        /// <param name="arguments">Input parameters</param>
        public void Trigger(T arguments)
        {
            _trigger(arguments);
        }

        /// <summary>
        /// Asynchronously triggers the event, signaling to all subscribers.
        /// </summary>
        /// <param name="arguments"></param>
        public void TriggerAsync(T arguments)
        {
            _triggerAsync(arguments);
        }

        #endregion
    }
}