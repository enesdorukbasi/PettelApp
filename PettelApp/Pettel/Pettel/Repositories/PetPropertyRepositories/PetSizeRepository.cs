using Pettel.Models.PetProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pettel.Repositories.PetPropertyRepositories
{
    public class PetSizeRepository
    {
        public List<string> GetAll()
        {
            List<string> petSizes = Enum.GetNames(typeof(PetSize)).ToList();
            return petSizes;
        }
    }
}
