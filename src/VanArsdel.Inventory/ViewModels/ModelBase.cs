using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml;

namespace VanArsdel.Inventory
{
    public class ModelBase : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // TODO: 
        //public DataHelper DataHelper => DataHelper.Current;
        public UIHelper UIHelper => UIHelper.Current;

        protected bool Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                NotifyPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyChanges()
        {
            // Notify all properties changes
            NotifyPropertyChanged("");
        }
    }
}
