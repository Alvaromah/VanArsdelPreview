using System;
using System.ComponentModel;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrdersViewModel : ViewModelBase
    {
        public OrdersViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;

            OrderList = new OrderListViewModel(ProviderFactory);
            OrderList.PropertyChanged += OnListPropertyChanged;

            OrderDetails = new OrderDetailsViewModel(ProviderFactory);
            OrderDetails.ItemDeleted += OnItemDeleted;

            OrderItemList = new OrderItemListViewModel(ProviderFactory);
        }

        public IDataProviderFactory ProviderFactory { get; }

        public OrderListViewModel OrderList { get; set; }
        public OrderDetailsViewModel OrderDetails { get; set; }
        public OrderItemListViewModel OrderItemList { get; set; }

        public async Task LoadAsync(OrdersViewState state)
        {
            await OrderList.LoadAsync(state);
        }

        public void SaveState()
        {
            OrderList.Unload();
        }

        public async Task RefreshAsync(bool resetPageIndex = false)
        {
            await OrderList.RefreshAsync(resetPageIndex);
        }

        private async void OnListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OrderListViewModel.SelectedItem):
                    OrderDetails.CancelEdit();
                    OrderItemList.IsMultipleSelection = false;
                    await PopulateDetails(OrderList.SelectedItem);
                    await PopulateOrderItems(OrderList.SelectedItem);
                    break;
                default:
                    break;
            }
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await OrderList.RefreshAsync();
        }

        private async Task PopulateDetails(OrderModel selected)
        {
            if (selected != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    var model = await dataProvider.GetOrderAsync(selected.OrderID);
                    selected.Merge(model);
                }
            }
            OrderDetails.Item = selected;
        }

        private async Task PopulateOrderItems(OrderModel selected)
        {
            if (selected != null)
            {
                await OrderItemList.LoadAsync(new OrderItemsViewState { OrderID = selected.OrderID });
            }
        }

        public void CancelEdit()
        {
            OrderDetails.CancelEdit();
        }
    }
}
