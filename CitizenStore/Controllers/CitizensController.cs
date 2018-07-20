using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ext.Net.MVC;
using CitizenStore.Models;
using Ext.Net;
using CitizenStore.Utils;

namespace CitizenStore.Controllers
{
    public class CitizensController : Controller
    {
        private CitizenruDbEntities db = new CitizenruDbEntities();

        // GET: Citizens
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Search(CitizenViewModel model)
        {
            List<citizen> data = new List<citizen>();

            if (ModelState.IsValid)
            {

                data = await db.citizens.Where(c => c.citizenname == model.Name && c.surname == model.Surname
                                                           && c.middlename == model.Middlename
                                                           && ((model.BeginBirthDate <= c.birthdate) && (c.birthdate <= model.EndBirthDate))
                                                            ).ToListAsync();

                return this.Store(data);
            }
            return View("Index");

        }


        public void Add(citizen model)
        {
            if (ModelState.IsValid)
            {
                db.citizens.Add(model);
                db.SaveChanges();
            }
        }

        public async Task<ActionResult> HandleChanges(StoreDataHandler handler)
        {
            List<citizen> citizens = handler.ObjectData<citizen>();
            //string errorMessage = null;
            if (handler.Action == StoreAction.Destroy)
            {
                foreach (citizen deleted in citizens)
                {
                    citizen citizen = await db.citizens.FirstOrDefaultAsync(c => c.id == deleted.id);
                    db.citizens.Remove(citizen);
                    await db.SaveChangesAsync();
                }
            }
            else if (handler.Action == StoreAction.Update)
            {
                foreach (citizen updated in citizens)
                {
                    citizen citizen = await db.citizens.FirstOrDefaultAsync(c => c.id == updated.id);
                    CitizenHelper.CitizenUpdate(citizen, updated);
                    await db.SaveChangesAsync();
                }
            }

            //if (errorMessage != null)
            //{
            //    return this.Store(errorMessage);
            //}

            return handler.Action != StoreAction.Destroy ? (ActionResult)this.Store(citizens) : (ActionResult)this.Content("");
        }

        public void PrintData(List<citizen> grindData)
        {
            //List<Citizen> citizens = handler.ObjectData<Citizen>();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
