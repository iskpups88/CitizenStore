
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CitizenStore.Utils
{
    public static class CitizenHelper
    {

        public static void CitizenUpdate(citizen citizen, citizen updateCitizen)
        {
            citizen.surname = updateCitizen.surname;
            citizen.citizenname = updateCitizen.citizenname;
            citizen.middlename = updateCitizen.middlename;
            citizen.birthdate = updateCitizen.birthdate;


        }
        
   
    }
}