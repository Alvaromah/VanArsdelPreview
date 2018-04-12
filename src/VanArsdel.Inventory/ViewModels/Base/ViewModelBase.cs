using System;

using VanArsdel.Inventory.Services;

namespace VanArsdel.Inventory.ViewModels
{
    public class ViewModelBase : ModelBase
    {
        public ViewModelBase(IContext context)
        {
            Context = context;
        }

        public IContext Context { get; }

        public bool IsMainView => Context.IsMainView;

        virtual public string Title => String.Empty;
    }
}
