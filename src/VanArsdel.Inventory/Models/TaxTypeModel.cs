using System;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class TaxTypeModel : ModelBase
    {
        public TaxTypeModel()
        {
        }
        public TaxTypeModel(TaxType source)
        {
            TaxTypeID = source.TaxTypeID;
            Name = source.Name;
            Rate = source.Rate;
        }

        public int TaxTypeID { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
    }
}
