using System;
using System.ComponentModel;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;

            CustomerList = new CustomerListViewModel(ProviderFactory);
            CustomerList.PropertyChanged += OnListPropertyChanged;

            CustomerDetails = new CustomerDetailsViewModel(ProviderFactory);
            CustomerDetails.ItemDeleted += OnItemDeleted;
            CustomerOrders = new OrderListViewModel(ProviderFactory);
        }

        public IDataProviderFactory ProviderFactory { get; }

        public CustomerListViewModel CustomerList { get; set; }
        public CustomerDetailsViewModel CustomerDetails { get; set; }
        public OrderListViewModel CustomerOrders { get; set; }

        public async Task LoadAsync(CustomersViewState state)
        {
            await CustomerList.LoadAsync(state);
        }

        public void SaveState()
        {
            CustomerList.Unload();
        }

        public async Task RefreshAsync(bool resetPageIndex = false)
        {
            await CustomerList.RefreshAsync(resetPageIndex);
        }

        private async void OnListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CustomerListViewModel.SelectedItem):
                    CustomerDetails.CancelEdit();
                    CustomerOrders.IsMultipleSelection = false;
                    if (!CustomerList.IsMultipleSelection)
                    {
                        await PopulateDetails(CustomerList.SelectedItem);
                        await PopulateOrders(CustomerList.SelectedItem);
                    }
                    break;
                default:
                    break;
            }
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await CustomerList.RefreshAsync();
        }

        private async Task PopulateDetails(CustomerModel selected)
        {
            if (selected != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    var model = await dataProvider.GetCustomerAsync(selected.CustomerID);
                    selected.Merge(model);
                }
            }
            CustomerDetails.Item = selected;
        }

        private async Task PopulateOrders(CustomerModel selectedItem)
        {
            if (selectedItem != null)
            {
                await CustomerOrders.LoadAsync(new OrdersViewState { CustomerID = selectedItem.CustomerID });
            }
        }

        public void CancelEdit()
        {
            CustomerDetails.CancelEdit();
        }
    }
}
