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
            OrderList.UpdateView += OnUpdateView;

            OrderDetails = new OrderDetailsViewModel(ProviderFactory);
            OrderDetails.UpdateView += OnUpdateView;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public OrderListViewModel OrderList { get; set; }
        public OrderDetailsViewModel OrderDetails { get; set; }

        public async Task LoadAsync(OrdersViewState state)
        {
            await OrderList.LoadAsync(state);
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
                    await UpdateDetails(OrderList.SelectedItem);
                    break;
                default:
                    break;
            }
        }

        private async Task UpdateDetails(OrderModel selected)
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
            OrderDetails.RaiseUpdateView();
        }

        public void BeginEdit()
        {
            OrderDetails.BeginEdit();
        }

        public void CancelEdit()
        {
            OrderDetails.CancelEdit();
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            RaiseUpdateView();
        }
    }
}
