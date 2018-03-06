﻿using System;
using System.Linq;
using System.Collections.Generic;

using Windows.ApplicationModel.Core;

namespace VanArsdel.Inventory
{
    public class ViewModelBase : ModelBase
    {
        public event EventHandler UpdateView;

        public bool IsMainView => CoreApplication.GetCurrentView().IsMain;

        virtual public string Title => String.Empty;

        public void NavigateTo(NavigationItem item)
        {
            NavigationService.Shell.Navigate(item.Page, item.Label);
        }

        public void GoBack()
        {
            if (NavigationService.Shell.CanGoBack)
            {
                NavigationService.Shell.GoBack();
            }
        }

        public void RaiseUpdateView() => UpdateView?.Invoke(this, EventArgs.Empty);
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
