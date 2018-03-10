using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Controls;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomerDetailsViewModel : DetailsViewModel<CustomerModel>
    {
        public CustomerDetailsViewModel(IDataProviderFactory providerFactory) : base(providerFactory)
        {
        }

        override public string Title => ((Item?.IsNew) ?? false) ? "New Customer" : Item?.FullName ?? String.Empty;

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
                    Item = await dp.GetCustomerAsync(state.CustomerID);
                    IsDeleted = Item == null;
                }
            }
            else
            {
                Item = new CustomerModel();
                IsEditMode = true;
                ToolbarMode = DetailToolbarMode.CancelSave;
            }
        }

        protected override async Task SaveItemAsync(CustomerModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await dataProvider.UpdateCustomerAsync(model);
            }
        }

        protected override async Task DeleteItemAsync(CustomerModel model)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await dataProvider.DeleteCustomerAsync(model);
            }
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
                yield return new RequiredConstraint<CustomerModel>("Country", m => m.CountryCode);
            }
        }
    }
}
