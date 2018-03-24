using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class ListViewState : ViewStateBase
    {
        public ListViewState()
        {
            PageIndex = 0;
            PageSize = 20;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string Query { get; set; }
    }
}
