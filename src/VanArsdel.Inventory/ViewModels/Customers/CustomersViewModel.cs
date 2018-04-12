using System;
using System.ComponentModel;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Services;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class CustomersViewModel : ViewModelBase
    {
        public CustomersViewModel(IDataProviderFactory providerFactory, IServiceManager serviceManager)
        {
            ProviderFactory = providerFactory;
            MessageService = serviceManager.MessageService;

            CustomerList = new CustomerListViewModel(ProviderFactory, serviceManager);
            CustomerDetails = new CustomerDetailsViewModel(ProviderFactory, serviceManager);
            CustomerOrders = new OrderListViewModel(ProviderFactory, serviceManager);
        }

        public IDataProviderFactory ProviderFactory { get; }
        public IMessageService MessageService { get; }

        public CustomerListViewModel CustomerList { get; set; }
        public CustomerDetailsViewModel CustomerDetails { get; set; }
        public OrderListViewModel CustomerOrders { get; set; }

        public async Task LoadAsync(CustomersViewState state)
        {
            MessageService.Subscribe<CustomerListViewModel>(this, OnMessage);
            await CustomerList.LoadAsync(state);
        }

        public void Unload()
        {
            CustomerList.Unload();
            MessageService.Unsubscribe(this);
        }

        public async Task RefreshAsync()
        {
            await CustomerList.RefreshAsync();
        }

        public void CancelEdit()
        {
            CustomerDetails.CancelEdit();
        }

        private async void OnMessage(object sender, string message, object args)
        {
            if (sender == CustomerList && message == "ItemSelected")
            {
                await Dispatcher.RunIdleAsync((e) =>
                {
                    OnItemSelected();
                });
            }
        }

        private async void OnItemSelected()
        {
            CustomerDetails.CancelEdit();
            CustomerOrders.IsMultipleSelection = false;
            var selected = CustomerList.SelectedItem;
            if (!CustomerList.IsMultipleSelection)
            {
                if (selected != null && !selected.IsEmpty)
                {
                    await PopulateDetails(selected);
                    await PopulateOrders(selected);
                }
            }
            CustomerDetails.Item = selected;
        }

        private async Task PopulateDetails(CustomerModel selected)
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                var model = await dataProvider.GetCustomerAsync(selected.CustomerID);
                selected.Merge(model);
            }
        }

        private async Task PopulateOrders(CustomerModel selectedItem)
        {
            if (selectedItem != null)
            {
                await CustomerOrders.LoadAsync(new OrdersViewState { CustomerID = selectedItem.CustomerID });
            }
        }
    }
}
