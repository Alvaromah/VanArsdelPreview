using System;

namespace VanArsdel.Inventory.Services
{
    public interface IMessageService
    {
        void Subscribe<TSender>(object target, Action<object, string, object> action) where TSender : class;
        void Unsubscribe(object target);
        void Unsubscribe<TSender>(object target) where TSender : class;

        void Send<TSender>(TSender sender, string message, object args) where TSender : class;
    }
}
