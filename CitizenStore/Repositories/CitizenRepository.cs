using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CitizenStore.Models;
using CitizenStore.Utils;
using IBM.Data.DB2;

namespace CitizenStore.Repositories
{
    public class CitizenRepository : ICitizenRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DB2_ConnectionString"].ConnectionString;
        private DB2DataReader rd;
        private DB2Connection conn;

        public List<Citizen> GetCitizenList(CitizenViewModel model)
        {
            List<Citizen> citizenList = new List<Citizen>();

            using (conn = new DB2Connection(connectionString))
            {
                conn.Open();
                using (DB2Command cmd = conn.CreateCommand())
                {
                    cmd.CommandText = CitizenHelper.CitizenSearchQueryBuilder(model);
                    rd = cmd.ExecuteReader();
                    rd.Read();
                    do
                    {
                        if (rd.HasRows)
                        {
                            Citizen citizen = new Citizen
                            {
                                Id = int.Parse(rd[0].ToString()),
                                Surname = rd[1].ToString().Trim(' '),
                                CitizenName = rd[2].ToString().Trim(' '),
                                Middlename = rd[3].ToString().Trim(' '),
                                BirthDate = DateTime.Parse(rd[4].ToString())
                            };
                            citizenList.Add(citizen);
                        }
                    } while (rd.Read());
                }
            }

            return citizenList;
        }

        public void Create(Citizen citizen)
        {
            if (CheckExistCitizen(citizen) == false)
            {
                using (conn = new DB2Connection(connectionString))
                {

                    conn.Open();
                    using (DB2Command cmd = conn.CreateCommand())
                    {

                        cmd.CommandText = $"INSERT INTO citizen VALUES (0,'{citizen.Surname}','{citizen.CitizenName}','{citizen.Middlename}','{citizen.BirthDate.ToShortDateString()}')";
                        cmd.ExecuteNonQuery();
                    }
                }
            }else
                throw new Exception("Пользователь с такими данными уже существует");
        }
        public void Update(Citizen citizen)
        {
            if (CheckExistCitizen(citizen) == false)
            {
                using (conn = new DB2Connection(connectionString))
                {
                    conn.Open();
                    using (DB2Command cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"UPDATE citizen SET surname = '{citizen.Surname}', citizenname = '{citizen.CitizenName}', middlename = '{citizen.Middlename}', " +
                                          $"birthdate = '{citizen.BirthDate.ToShortDateString()}' WHERE id = {citizen.Id}";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
                throw new Exception("Пользователь с такими данными уже существует");
        }

        public void Delete(int id)
        {
            using (conn = new DB2Connection(connectionString))
            {
                conn.Open();
                using (DB2Command cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"DELETE FROM citizen WHERE id = {id}";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private bool CheckExistCitizen(Citizen citizen)
        {
            using (conn = new DB2Connection(connectionString))
            {
                conn.Open();
                using (DB2Command cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM citizen WHERE surname = '{citizen.Surname}' AND citizenname = '{citizen.CitizenName}' " +
                                    $"and middlename = '{citizen.Middlename}' AND birthdate = '{citizen.BirthDate.ToShortDateString()}'";
                    rd = cmd.ExecuteReader();
                    rd.Read();
                    return rd.HasRows;
                }
            }
        }
        public Citizen GetCitizen(int id)
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}