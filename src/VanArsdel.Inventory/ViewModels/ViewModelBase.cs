using System;

namespace VanArsdel.Inventory
{
    public class ViewModelBase : ModelBase
    {
        public void NavigateTo(NavigationItem item)
        {
            NavigationService.Shell.Navigate(item.Page, item.Label);
        }
    }
}
