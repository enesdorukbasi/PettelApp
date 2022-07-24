using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountType { get; set; }
        public bool IsActive { get; set; }
    }
}
