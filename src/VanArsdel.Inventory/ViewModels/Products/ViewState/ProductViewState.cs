using System;

namespace VanArsdel.Inventory.ViewModels
{
    public class ProductViewState : DetailsViewState
    {
        static public ProductViewState CreateDefault() => new ProductViewState();

        public string ProductID { get; set; }

        public bool IsNew => String.IsNullOrEmpty(ProductID);
    }
}
