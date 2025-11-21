using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassStruture
{
    public class ProviderContact
    {
        [Required]
        public string JobTitle { get; set; }

        public void AddProviderContactPerson(string jobTitle)
        {
            JobTitle = JobTitle;
        }

        public void UpdateProviderContactPerson(string jobTitle)
        {
            JobTitle = jobTitle;
        }        
    }
}
