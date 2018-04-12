using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class ListViewState : ViewStateBase
    {
        public bool IsEmpty { get; set; }
        public string Query { get; set; }
    }
}
