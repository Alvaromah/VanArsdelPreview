﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomerDetailsViewModel : DetailsViewModel<CustomerModel>
    {
        public CustomerDetailsViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
            : base(providerFactory, serviceManager)
        {
        }

        override public string Title => (Item?.IsNew ?? true) ? TitleNew : TitleEdit;
        public string TitleNew => "New Customer";
        public string TitleEdit => Item == null ? "Customer" : $"{Item.FullName}";

        public override bool IsNewItem => Item?.IsNew ?? false;

        protected override void ItemUpdated()
        {
            NotifyPropertyChanged(nameof(Title));
        }

        public async Task LoadAsync(CustomerViewState state)
        {
            if (state.CustomerID > 0)
            {
                using (var dp = ProviderFactory.CreateDataProvider())
                {
                    var item = await dp.GetCustomerAsync(state.CustomerID);
                    Item = item ?? new CustomerModel { CustomerID = state.CustomerID, IsDeleted = true };
                }
            }
            else
            {
                Item = new CustomerModel();
                IsEditMode = true;
            }
        }

        public void Subscribe()
        {
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
        }

        protected override async Task SaveItemAsync(CustomerModel model)
        {
            StatusMessage("Saving customer...");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await Task.Delay(100);
                await dataProvider.UpdateCustomerAsync(model);
                NotifyPropertyChanged(nameof(Title));
            }

            stopwatch.Stop();
            StatusMessage($"Customer saved ({stopwatch.Elapsed.TotalSeconds:#0.00} seconds)");
        }

        protected override async Task DeleteItemAsync(CustomerModel model)
        {
            StatusMessage("Deleting customer...");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await Task.Delay(100);
                await dataProvider.DeleteCustomerAsync(model);
            }

            stopwatch.Stop();
            StatusMessage($"Customer deleted ({stopwatch.Elapsed.TotalSeconds:#0.00} seconds)");
        }

        protected override async Task<bool> ConfirmDeleteAsync()
        {
            return await DialogService.ShowAsync("Confirm Delete", "Are you sure you want to delete current customer?", "Ok", "Cancel");
        }

        override protected IEnumerable<IValidationConstraint<CustomerModel>> ValidationConstraints
        {
            get
            {
                yield return new RequiredConstraint<CustomerModel>("First Name", m => m.FirstName);
                yield return new RequiredConstraint<CustomerModel>("Last Name", m => m.LastName);
                yield return new RequiredConstraint<CustomerModel>("Email Address", m => m.EmailAddress);
                yield return new RequiredConstraint<CustomerModel>("Address Line 1", m => m.AddressLine1);
                yield return new RequiredConstraint<CustomerModel>("City", m => m.City);
                yield return new RequiredConstraint<CustomerModel>("Region", m => m.Region);
                yield return new RequiredConstraint<CustomerModel>("Postal Code", m => m.PostalCode);
                yield return new RequiredConstraint<CustomerModel>("Country", m => m.CountryCode);
            }
        }
    }
}
