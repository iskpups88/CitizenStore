using CitizenStore.Models;

namespace CitizenStore.Utils
{
    public static class CitizenHelper
    {       
        public static string CitizenSearchQueryBuilder(CitizenViewModel model)
        {
            string selectCommnand = "select * from citizen where ";
            int andCounter = 0;

            if (model.Name != null)
            {
                if (model.Name.Contains("*"))
                    selectCommnand += $"citizenname LIKE '{model.Name.Replace("*", "%")}' and ";
                else
                    selectCommnand += $"citizenname = '{model.Name}' and ";
                andCounter++;
            }
            if (model.Surname != null)
            {
                if (model.Surname.Contains("*"))
                    selectCommnand += $"surname LIKE '{model.Surname.Replace("*", "%")}' and ";
                else
                    selectCommnand += $"surname = '{model.Surname}' and ";
                andCounter++;
            }
            if (model.Middlename != null)
            {
                if (model.Middlename.Contains("*"))
                    selectCommnand += $"middlename LIKE '{model.Middlename.Replace("*", "%")}' and ";
                else
                    selectCommnand += $"middlename = '{model.Middlename}' and ";
                andCounter++;
            }
            if (model.BeginBirthDate != null)
            {
                selectCommnand += $"birthdate >= '{model.BeginBirthDate?.ToShortDateString()}' and ";
                andCounter++;
            }
            if (model.EndBirthDate != null)
            {
                selectCommnand += $"birthdate <= '{model.EndBirthDate?.ToShortDateString()}'    ";
                andCounter++;
            }

            if (andCounter != 5)
            {
                selectCommnand = selectCommnand.Substring(0, selectCommnand.Length - 4);
            }

            return selectCommnand;
        }
      
    }
}