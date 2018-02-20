using System;
using System.Threading.Tasks;

using VanArsdel.Inventory.Data;

namespace VanArsdel.Inventory.Models
{
    public class OrderModel : ModelBase
    {
        public OrderModel()
        {
        }
        public OrderModel(Order source)
        {
            OrderID = source.OrderID;
            CustomerID = source.CustomerID;
            OrderDate = source.OrderDate;
            ShippedDate = source.ShippedDate;
            DeliveredDate = source.DeliveredDate;
            Status = source.Status;
            PaymentType = source.PaymentType;
            TrackingNumber = source.TrackingNumber;
            ShipVia = source.ShipVia;
            ShipAddress = source.ShipAddress;
            ShipCity = source.ShipCity;
            ShipRegion = source.ShipRegion;
            ShipCountryCode = source.ShipCountryCode;
            ShipPostalCode = source.ShipPostalCode;
            ShipPhone = source.ShipPhone;

            Customer = new CustomerModel(source.Customer);
        }

        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public int Status { get; set; }
        public int PaymentType { get; set; }
        public string TrackingNumber { get; set; }
        public int? ShipVia { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipCountryCode { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipPhone { get; set; }

        public CustomerModel Customer { get; private set; }

        public async Task LoadAsync()
        {
            await Customer.LoadAsync();
        }
    }
}
