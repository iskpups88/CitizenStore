//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CitizenStore
{
    using Ext.Net.MVC;
    using System;
    using System.Collections.Generic;
    [Model(Name = "Citizen")]
    [JsonWriter(Encode = true, RootProperty = "data")]
    public partial class citizen
    {
        [ModelField(IDProperty = true)]
        [Field(Ignore = true)]
        public int id { get; set; }
        public string surname { get; set; }
        public string citizenname { get; set; }
        public string middlename { get; set; }
        public System.DateTime birthdate { get; set; }
    }
}