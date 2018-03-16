using System;
using System.ComponentModel;
using System.Threading.Tasks;

using VanArsdel.Inventory.Models;
using VanArsdel.Inventory.Providers;

namespace VanArsdel.Inventory.ViewModels
{
    public class OrderItemsViewModel : ViewModelBase
    {
        public OrderItemsViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;

            OrderItemList = new OrderItemListViewModel(ProviderFactory);
            OrderItemList.PropertyChanged += OnListPropertyChanged;

            OrderItemDetails = new OrderItemDetailsViewModel(ProviderFactory);
            OrderItemDetails.ItemDeleted += OnItemDeleted;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public OrderItemListViewModel OrderItemList { get; set; }
        public OrderItemDetailsViewModel OrderItemDetails { get; set; }

        public async Task LoadAsync(OrderItemsViewState state)
        {
            await OrderItemList.LoadAsync(state);
        }

        public void Unload()
        {
            OrderItemList.Unload();
        }

        public async Task RefreshAsync(bool resetPageIndex = false)
        {
            await OrderItemList.RefreshAsync(resetPageIndex);
        }

        private async void OnListPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(OrderItemListViewModel.SelectedItem):
                    OrderItemDetails.CancelEdit();
                    await PopulateDetails(OrderItemList.SelectedItem);
                    break;
                default:
                    break;
            }
        }

        private async void OnItemDeleted(object sender, EventArgs e)
        {
            await OrderItemList.RefreshAsync();
        }

        private async Task PopulateDetails(OrderItemModel selected)
        {
            if (selected != null)
            {
                using (var dataProvider = ProviderFactory.CreateDataProvider())
                {
                    var model = await dataProvider.GetOrderItemAsync(selected.OrderID, selected.OrderLine);
                    selected.Merge(model);
                }
            }
            OrderItemDetails.Item = selected;
        }

        public void CancelEdit()
        {
            OrderItemDetails.CancelEdit();
        }
    }
}
