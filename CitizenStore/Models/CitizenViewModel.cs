using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CitizenStore.Models
{
    public class CitizenViewModel 
    {

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Middlename { get; set; }

        public DateTime? BeginBirthDate { get; set; }

        public DateTime? EndBirthDate { get; set; }
    }
}