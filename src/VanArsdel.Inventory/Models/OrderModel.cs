using System;

using VanArsdel.Data;

namespace VanArsdel.Inventory.Models
{
    public class OrderModel : ModelBase
    {
        public long OrderID { get; set; }
        public long CustomerID { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

        private int _status;
        public int Status
        {
            get => _status;
            set { if (Set(ref _status, value)) UpdateStatusDependencies(); }
        }

        public int PaymentType { get; set; }
        public string TrackingNumber { get; set; }

        public int? ShipVia { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipCountryCode { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipPhone { get; set; }

        public CustomerModel Customer { get; set; }

        public bool IsNew => OrderID <= 0;
        public bool CanEditCustomer => IsNew && CustomerID <= 0;
        public bool CanEditPaymentType => Status > 1;
        public bool CanEditShipping => Status > 2;

        public string StatusDesc => DataHelper.GetOrderStatus(Status);

        private void UpdateStatusDependencies()
        {
            NotifyPropertyChanged(nameof(CanEditPaymentType));
            NotifyPropertyChanged(nameof(CanEditShipping));
        }

        public override void Merge(ModelBase source)
        {
            if (source is OrderModel model)
            {
                Merge(model);
            }
        }

        public void Merge(OrderModel source)
        {
            if (source != null)
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
                Customer = source.Customer;
            }
        }

        public override string ToString()
        {
            return OrderID.ToString();
        }
    }
}
