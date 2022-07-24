using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models
{
    public class Emergency
    {
        public string Id { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string PetType { get; set; }
        public string EMail { get; set; }
        public DateTime Datetime { get; set; }
        public bool IsActive { get; set; }

    }
}
