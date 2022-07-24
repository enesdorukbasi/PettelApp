using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models
{
    public class Address : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string AddressTitle { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
