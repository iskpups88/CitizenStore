using Ext.Net.MVC;
using System;
using System.ComponentModel.DataAnnotations;

namespace CitizenStore.Models
{
    [Model(Name = "Citizen")]
    [JsonWriter(Encode = true, RootProperty = "data")]
    public class Citizen
    {
        [ModelField(IDProperty = true)]
        [Field(Ignore = true)]
        public int Id { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string CitizenName { get; set; }
        [Required]
        public string Middlename { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
    }
}