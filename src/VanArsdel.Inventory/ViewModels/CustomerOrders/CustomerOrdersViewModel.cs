using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.ViewModels
{
    public partial class CustomerOrdersViewModel : ViewModelBase
    {
        public event EventHandler UpdateBindings;

        public CustomerOrdersViewModel(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        public async Task LoadAsync()
        {
            using (var dataProvider = ProviderFactory.CreateDataProvider())
            {
                await RefreshCustomersAsync(dataProvider);
            }
        }

        private void RaiseUpdateBindings() => UpdateBindings?.Invoke(this, EventArgs.Empty);
    }
}
