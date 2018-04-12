using System;
using System.Threading.Tasks;

using Windows.UI.Core;

namespace VanArsdel.Inventory.Services
{
    public class Context : IContext
    {
        private CoreDispatcher _dispatcher = null;

        public int ViewID { get; private set; }

        public void Initialize(int viewID, object dispatcher)
        {
            ViewID = viewID;
            _dispatcher = dispatcher as CoreDispatcher;
        }

        public async Task RunAsync(Action action)
        {
            await _dispatcher.RunIdleAsync((e) => action());
        }
    }
}
