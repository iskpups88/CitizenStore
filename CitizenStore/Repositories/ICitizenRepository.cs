using CitizenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenStore.Repositories
{
    interface ICitizenRepository : IDisposable
    {
        List<Citizen> GetCitizenList(CitizenViewModel model);

        Citizen GetCitizen(int id);

        void Create(Citizen item);

        void Update(Citizen item);

        void Delete(int id);
    }
}
