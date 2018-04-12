using System;
using System.Threading.Tasks;

using Windows.UI.Core;

namespace VanArsdel.Inventory.Services
{
    public class Context : IContext
    {
        private CoreDispatcher _dispatcher = null;

        public int ViewID { get; private set; }
        public bool IsMainView { get; private set; }

        public void Initialize(object dispatcher, int viewID, bool isMainView)
        {
            _dispatcher = dispatcher as CoreDispatcher;
            ViewID = viewID;
            IsMainView = isMainView;
        }

        public async Task RunAsync(Action action)
        {
            await _dispatcher.RunIdleAsync((e) => action());
        }
    }
}
