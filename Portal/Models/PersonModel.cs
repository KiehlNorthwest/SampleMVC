using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class PeopleModel
    {
        public HttpPostedFileBase Upload { get; set; }
        public List<PersonModel> People { get; set; }
    }
    public class PersonModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }

        [MaxLength(255)]
        public string MiddleName { get; set; }
    }
}