using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models
{
    public class Basket : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public int Number { get; set; }
        public string Image { get; set; }
        public string ProductName { get; set; }
        public double TotalPrice { get; set; }
        public string ProductId { get; set; }
    }
}
