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
            CustomerList.UpdateView += OnUpdateView;

            CustomerDetails = new CustomerDetailsViewModel(ProviderFactory);
            CustomerDetails.UpdateView += OnUpdateView;

            CustomerOrders = new OrderListViewModel(ProviderFactory);
            CustomerOrders.UpdateView += OnUpdateView;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public CustomerListViewModel CustomerList { get; set; }
        public CustomerDetailsViewModel CustomerDetails { get; set; }
        public OrderListViewModel CustomerOrders { get; set; }

        public async Task LoadAsync()
        {
            await CustomerList.LoadAsync();
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
                    if (!CustomerList.IsMultipleSelection)
                    {
                        await UpdateDetails(CustomerList.SelectedItem);
                        await UpdateOrders(CustomerList.SelectedItem);
                    }
                    break;
                default:
                    break;
            }
        }

        private async Task UpdateDetails(CustomerModel selected)
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
            CustomerDetails.RaiseUpdateView();
        }

        private async Task UpdateOrders(CustomerModel selectedItem)
        {
            if (selectedItem != null)
            {
                await CustomerOrders.LoadAsync(new OrdersViewState { CustomerID = selectedItem.CustomerID });
            }
        }

        public void BeginEdit()
        {
            CustomerDetails.BeginEdit();
        }

        public void CancelEdit()
        {
            CustomerDetails.CancelEdit();
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            RaiseUpdateView();
        }
    }
}
