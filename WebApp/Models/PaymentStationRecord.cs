using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class PayStationViewModel
    {
        public List<PaymentStationRecord> PayStations { get; set; }
    }
    public class PaymentStationRecord
    {
        public string PayStationId { get; set; }
        public string PayStationName { get; set; }
        public string Direction { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string County { get; set; }
        public string PhoneNumber { get; set; }
        public decimal FeeAmount { get; set; }
        public string SecondaryLanguage { get; set; }
        public string TypeOfBusiness { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Comment { get; set; }
        public bool ActiveFlag { get; set; }
        public bool PublicPayStation { get; set; }
        public double DistanceFromMapLocation { get; set; }
        public string Vendor { get; set; }
        public bool AcceptsCash { get; set; }
        public bool AcceptsCreditCard { get; set; }
        public bool AcceptsDebit { get; set; }
        public bool AcceptsMoneyOrder { get; set; }
        public bool AcceptsCheck { get; set; }
    }
}
