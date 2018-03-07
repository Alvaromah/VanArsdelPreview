using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class ViewStateBase
    {
        public ViewStateBase Clone()
        {
            return MemberwiseClone() as ViewStateBase;
        }
    }
}
