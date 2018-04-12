using System;
using System.Linq;
using System.Collections.Generic;

namespace VanArsdel.Inventory.Services
{
    public class MessageService : IMessageService
    {
        private object _sync = new Object();

        private List<Subscriber> _subscribers = new List<Subscriber>();

        public void Subscribe<TSender>(object target, Action<object, string, object> action) where TSender : class
        {
            lock (_sync)
            {
                var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
                if (subscriber == null)
                {
                    subscriber = new Subscriber(target);
                    _subscribers.Add(subscriber);
                }
                subscriber.AddSubscription(typeof(TSender), action);
            }
        }

        public void Unsubscribe<TSender>(object target) where TSender : class
        {
            lock (_sync)
            {
                var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
                if (subscriber != null)
                {
                    subscriber.RemoveSubscription(typeof(TSender));
                    if (subscriber.IsEmpty)
                    {
                        _subscribers.Remove(subscriber);
                    }
                }
            }
        }
        public void Unsubscribe(object target)
        {
            lock (_sync)
            {
                var subscriber = _subscribers.Where(r => r.Target == target).FirstOrDefault();
                if (subscriber != null)
                {
                    _subscribers.Remove(subscriber);
                }
            }
        }

        public void Send<TSender>(TSender sender, string message, object args) where TSender : class
        {
            foreach (var subscriber in GetSubscribersSnapshot())
            {
                subscriber.TryInvoke(sender, message, args);
            }
        }

        private Subscriber[] GetSubscribersSnapshot()
        {
            lock (_sync)
            {
                return _subscribers.ToArray();
            }
        }

        class Subscriber
        {
            private WeakReference _reference = null;

            private Dictionary<Type, Action<object, string, object>> _subscriptions;

            public Subscriber(object target)
            {
                _reference = new WeakReference(target);
                _subscriptions = new Dictionary<Type, Action<object, string, object>>();
            }

            public object Target => _reference.Target;

            public bool IsEmpty => _subscriptions.Count == 0;

            public void AddSubscription(Type type, Action<object, string, object> action)
            {
                _subscriptions.Add(type, action);
            }

            public void RemoveSubscription(Type type)
            {
                _subscriptions.Remove(type);
            }

            public void TryInvoke(object sender, string message, object args)
            {
                var senderType = sender.GetType();
                var type = _subscriptions.Keys.FirstOrDefault(r => r.IsAssignableFrom(senderType));
                if (type != null)
                {
                    var action = _subscriptions[type];
                    var target = _reference.Target;
                    if (_reference.IsAlive)
                    {
                        action?.Invoke(sender, message, args);
                    }
                }
            }
        }
    }
}
