using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using VanArsdel.Data;
using VanArsdel.Inventory.Models;

namespace VanArsdel.Inventory.Providers
{
    public class CustomerCollection : VirtualCollection<CustomerModel>
    {
        private DataRequest<Customer> _dataRequest = null;

        public CustomerCollection(IDataProviderFactory providerFactory)
        {
            ProviderFactory = providerFactory;
        }

        public IDataProviderFactory ProviderFactory { get; }

        private CustomerModel _defaultItem = CustomerModel.CreateEmpty();
        protected override CustomerModel DefaultItem => _defaultItem;

        public async Task RefreshAsync(DataRequest<Customer> dataRequest)
        {
            _dataRequest = dataRequest;
            using (var provider = ProviderFactory.CreateDataProvider())
            {
                Count = await provider.GetCustomersCountAsync(_dataRequest.Query, _dataRequest.Where);
                Ranges[0] = await FetchDataAsync(0, RangeSize);
            }
        }

        protected override async Task<IList<CustomerModel>> FetchDataAsync(int pageIndex, int pageSize)
        {
            var pageRequest = new PageRequest<Customer>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = _dataRequest.Query,
                Where = _dataRequest.Where,
                OrderBy = _dataRequest.OrderBy,
                OrderByDesc = _dataRequest.OrderByDesc
            };
            using (var provider = ProviderFactory.CreateDataProvider())
            {
                return (await provider.GetCustomersAsync(pageRequest)).Items;
            }
        }
    }
}
