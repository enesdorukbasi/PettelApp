using Pettel.Models.PetProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pettel.Repositories.PetPropertyRepositories
{
    public class PetTypeRepository
    {
        public List<string> GetAll()
        {
            List<string> petTypes = Enum.GetNames(typeof(PetType)).ToList();
            //List<PetType> petTypes = Enum.GetValues(typeof(PetType)).Cast<PetType>().ToList();

            return petTypes;
        }
    }
}
