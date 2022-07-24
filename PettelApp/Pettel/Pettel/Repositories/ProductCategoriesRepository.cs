using Pettel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Pettel.Repositories
{
    public class ProductCategoriesRepository
    {
        public List<string> GetAllMainCategories()
        {
            List<string> mainCategories = Enum.GetNames(typeof(ProductCategories)).ToList();

            return mainCategories;
        }
    }
}
