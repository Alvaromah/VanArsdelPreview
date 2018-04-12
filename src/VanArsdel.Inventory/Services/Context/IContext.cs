using System;
using System.Threading.Tasks;

namespace VanArsdel.Inventory.Services
{
    public interface IContext
    {
        int ViewID { get; }

        bool IsMainView { get; }

        void Initialize(object dispatcher, int viewID, bool isMainView);

        Task RunAsync(Action action);
    }
}
