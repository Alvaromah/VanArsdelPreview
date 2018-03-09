using System;
using System.ComponentModel;

namespace VanArsdel.Inventory
{
    public interface INotifyExpressionChanged : INotifyPropertyChanged
    {
        void NotifyPropertyChanged(string propertyName);
    }
}
