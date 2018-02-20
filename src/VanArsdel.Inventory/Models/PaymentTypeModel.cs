using System;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class PaymentTypeModel : ModelBase
    {
        public PaymentTypeModel()
        {
        }
        public PaymentTypeModel(PaymentType source)
        {
            PaymentTypeID = source.PaymentTypeID;
            Name = source.Name;
        }

        public int PaymentTypeID { get; set; }
        public string Name { get; set; }
    }
}
