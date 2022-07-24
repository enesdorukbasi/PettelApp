using System;
using System.Collections.Generic;
using System.Text;

namespace Pettel.Models.VeterinaryClinicProperties
{
    public class ClinicalTreatmentInformation
    {
        public string Id { get; set; }
        public string ClinicalTreatmentType { get; set; }
        public string Content { get; set; }
        public decimal Price { get; set; }
        public string EMail { get; set; }
        public string VetetirenerId { get; set; }
    }
}
