using System;
using System.Threading.Tasks;

namespace VanArsdel.Inventory.Services
{
    public interface IContext
    {
        int ViewID { get; }

        void Initialize(int viewID, object dispatcher);

        Task RunAsync(Action action);
    }
}
