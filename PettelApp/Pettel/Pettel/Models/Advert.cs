using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models
{
    public class Advert
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string PetName { get; set; }
        public int PetAge { get; set; }
        public string PetType { get; set; }
        public string PetBreed { get; set; }
        public string PetSize { get; set; }
        public string PetGender { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string UserEmail { get; set; }
        public string Datetime { get; set; }
        public bool IsActive { get; set; }
    }
}
