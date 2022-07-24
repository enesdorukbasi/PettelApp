using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models
{
    public class Product
    {
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }

        public string Id { get; set; }
        public string ProductName { get; set; }
        public string Explanation { get; set; }
        public string Tags { get; set; }
        public decimal Price { get; set; }
        public int Number { get; set; }
        public string PetType { get; set; }
        public string MainCategoryName { get; set; }

        public string EMail { get; set; }
    }
}
