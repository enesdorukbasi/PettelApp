using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models
{
    public class Vetetirener
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string EMail { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string FullAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
