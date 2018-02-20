using System;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class SubCategoryModel : ModelBase
    {
        public SubCategoryModel()
        {
        }
        public SubCategoryModel(SubCategory source)
        {
            CategoryID = source.CategoryID;
            SubCategoryID = source.SubCategoryID;
            Name = source.Name;
            Description = source.Description;
        }

        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
