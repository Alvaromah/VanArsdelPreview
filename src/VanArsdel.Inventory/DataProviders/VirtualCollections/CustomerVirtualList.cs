using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Providers
{
    public class CustomerVirtualList : VirtualList<CustomerModel>
    {
        private PageRequest<Customer> _request = null;

        public CustomerVirtualList(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        private int _count = 0;
        public override int Count => _count;

        private CustomerModel _defaultItem = CustomerModel.CreateEmpty();
        protected override CustomerModel DefaultItem => _defaultItem;

        public async Task InitializeAsync(PageRequest<Customer> request)
        {
            _request = request;
            using (var provider = ProviderFactory.CreateDataProvider())
            {
                _count = await provider.GetCustomersCountAsync(_request);
                _ranges[0] = await FetchDataAsync(0, RangeSize);
            }
        }

        protected override async Task<IList<CustomerModel>> FetchDataAsync(int pageIndex, int pageSize)
        {
            _request.PageIndex = pageIndex;
            _request.PageSize = pageSize;
            using (var provider = ProviderFactory.CreateDataProvider())
            {
                return (await provider.GetCustomersAsync(_request)).Items;
            }
        }
    }
}
