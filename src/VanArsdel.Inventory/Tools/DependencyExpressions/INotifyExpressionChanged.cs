using System;
using System.ComponentModel;

using Windows.UI.Xaml;

namespace VanArsdel.Inventory
{
    public interface INotifyExpressionChanged : INotifyPropertyChanged
    {
        long RegisterPropertyChangedCallback(DependencyProperty dp, DependencyPropertyChangedCallback callback);
        void RaisePropertyChanged(string propertyName);
    }
}
