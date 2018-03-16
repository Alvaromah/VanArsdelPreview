using System;
using System.Linq;
using System.Collections.Generic;

using Windows.ApplicationModel.Core;

using VanArsdel.Inventory.Views;
using VanArsdel.Inventory.ViewModels;

namespace VanArsdel.Inventory
{
    public class ViewModelBase : ModelBase
    {
        public bool IsMainView => CoreApplication.GetCurrentView().IsMain;

        virtual public string Title => String.Empty;

        // TODO: Move to Navigation
        public void NavigateTo(NavigationItem item)
        {
            switch (item.Page.Name)
            {
                case nameof(CustomersView):
                    NavigationService.Shell.Navigate(item.Page, CustomersViewState.CreateDefault());
                    break;
                case nameof(OrdersView):
                    NavigationService.Shell.Navigate(item.Page, OrdersViewState.CreateDefault());
                    break;
                default:
                    NavigationService.Shell.Navigate(item.Page, item.Label);
                    break;
            }
        }
    }

    public class ViewModelBase<T> : ViewModelBase where T : ModelBase
    {
        virtual protected IEnumerable<IValidationConstraint<T>> ValidationConstraints => Enumerable.Empty<IValidationConstraint<T>>();

        public Result Validate(T model)
        {
            foreach (var constraint in ValidationConstraints)
            {
                if (!constraint.Validate(model))
                {
                    return Result.Error("Validation Error", $"{constraint.Message} Please, correct the error and try again.");
                }
            }
            return Result.Ok();
        }
    }
}
