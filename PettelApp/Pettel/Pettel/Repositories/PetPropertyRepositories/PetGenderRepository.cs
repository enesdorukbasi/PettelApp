using Pettel.Models.PetProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pettel.Repositories.PetPropertyRepositories
{
    public class PetGenderRepository
    {
        public List<string> GetAll()
        {
            List<string> petGenders = Enum.GetNames(typeof(PetGender)).ToList();
            return petGenders;
        }
    }
}
