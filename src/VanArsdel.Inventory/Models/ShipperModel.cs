using System;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class ShipperModel : ModelBase
    {
        public ShipperModel()
        {
        }
        public ShipperModel(Shipper source)
        {
            ShipperID = source.ShipperID;
            Name = source.Name;
            Phone = source.Phone;
        }

        public int ShipperID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
