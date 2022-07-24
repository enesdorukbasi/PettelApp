using Pettel.Models.PetProperties.PetBreedForTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pettel.Repositories.PetPropertyRepositories
{
    public class PetBreedReposiroty
    {
        public List<string> GetAll()
        {
            List<string> catBreed = Enum.GetNames(typeof(CatBreed)).ToList();
            List<string> dogBreed = Enum.GetNames(typeof(DogBreed)).ToList();
            List<string> birdBreed = Enum.GetNames(typeof(BirdBreed)).ToList();

            var allBreeds = (catBreed.Concat(dogBreed).Concat(birdBreed))
                .ToList();

            return allBreeds;
        }

        public List<string> GetSelectedTypeToBreed(string PetType)
        {
            List<string> selectedBreedList = new List<string>();
            if (PetType == "Köpek")
            {
                selectedBreedList = Enum.GetNames(typeof(DogBreed)).ToList();
            }
            else if (PetType == "Kedi")
            {
                selectedBreedList = Enum.GetNames(typeof(CatBreed)).ToList();
            }
            else if (PetType == "Kuş")
            {
                selectedBreedList = Enum.GetNames(typeof(BirdBreed)).ToList();
            }
            return selectedBreedList;
        }
    }
}
