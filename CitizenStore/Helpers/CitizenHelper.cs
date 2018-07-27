
using CitizenStore.Models;
using IBM.Data.DB2;
using System;
using System.Collections.Generic;

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

        public static List<citizen> CitizenQueryBuilder(CitizenViewModel model)
        {
            List<citizen> data = new List<citizen>();
            string selectCommnand = "select * from citizen where ";
            DB2DataReader rd;
            string connectionString = "Database=citizenrudb;Server=localhost:9090;User ID=informix;Password=root;";
            int andCounter = 0;
            using (DB2Connection conn = new DB2Connection(connectionString))
            {
                conn.Open();
                
                if (model.Name != null)
                {
                    selectCommnand += $"UPPER(citizenname) = '{model.Name}' and ";
                    andCounter++;
                }
                if (model.Surname != null)
                {
                    selectCommnand += $"surname = '{model.Surname}' and ";
                    andCounter++;
                }
                if (model.Middlename != null)
                {
                    selectCommnand += $"middlename = '{model.Middlename}' and ";
                    andCounter++;
                }
                if (model.BeginBirthDate != null)
                {
                    selectCommnand += $"birthdate >= '{model.BeginBirthDate?.ToString("dd.MM.yyyy")}' and ";
                    andCounter++;
                }
                if (model.EndBirthDate != null)
                {
                    selectCommnand += $"birthdate <= '{model.EndBirthDate?.ToString("dd.MM.yyyy")}'    ";
                    andCounter++;
                }

                if (andCounter != 5)
                {
                    selectCommnand = selectCommnand.Substring(0, selectCommnand.Length - 4);
                    andCounter++;
                }
                using (DB2Command cmd = conn.CreateCommand())
                {
                    cmd.CommandText = selectCommnand;
                    rd = cmd.ExecuteReader();
                    rd.Read();
                    do
                    {
                        if (rd.HasRows)
                        {
                            citizen citizen = new citizen
                            {
                                id = int.Parse(rd[0].ToString()),
                                surname = rd[1].ToString(),
                                citizenname = rd[2].ToString(),
                                middlename = rd[3].ToString(),
                                birthdate = DateTime.Parse(rd[4].ToString())
                            };
                            data.Add(citizen);
                        }
                    } while (rd.Read());
                }
            }
            return data;
        }
    }
}