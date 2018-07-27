using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Ext.Net.MVC;
using CitizenStore.Models;
using Ext.Net;
using CitizenStore.Utils;
using FastReport.Web;
using FastReport.Data;
using CitizenStore.Extensions;

namespace CitizenStore.Controllers
{
    public class CitizensController : Controller
    {
        private WebReport webReport = new WebReport();
        private CitizenruDbEntities db = new CitizenruDbEntities();

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Search(CitizenViewModel model)
        {
            List<citizen> data = new List<citizen>();
            if (ModelState.IsValid)
            {
                //data = await db.citizens.Where(c => c.citizenname.ToUpper() == model.Name.ToUpper()
                //                                    && c.surname.ToUpper() == model.Surname.ToUpper()
                //                                    && c.middlename.ToUpper() == model.Middlename.ToUpper()
                //                                    && ((model.BeginBirthDate <= c.birthdate) && (c.birthdate <= model.EndBirthDate))
                //                                     ).ToListAsync();

                data = CitizenHelper.CitizenQueryBuilder(model);
                return this.Store(data);
            }
            //else
            //{
            //    List<string> errorList = new List<string>();
            //    List<ModelErrorCollection> errors = ModelState.Select(x => x.Value.Errors)
            //               .Where(y => y.Count > 0)
            //               .ToList();
            //    foreach (var error in errors)
            //    {
            //        errorList.Add(error[0].ErrorMessage);
            //    }
            //    CitizenHelper.CitizenQuery(model);
            //    //ViewBag.ErrorMessage = $"{errorsMessage}";

            //}
            return View("Index");

        }


        public async Task<ActionResult> Add(citizen model)
        {
            if (ModelState.IsValid)
            {
                db.citizens.Add(model);
                await db.SaveChangesAsync();
                return this.Store(model);
            }
            else
                return (ActionResult)this.Content("");
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

  
        public void PrintData(List<citizen> citizens)
        {
            TempData["list"] = citizens;
            //return RedirectToAction("Report");
        }

        public ActionResult Report()
        {
            WebReport webReport = new WebReport
            {
                Width = 780,// Unit.Percentage(100);
                Height = 800 // Unit.Percentage(100); 
            };
            var citizens = TempData["list"] as List<citizen>;
            //List<citizen> citizens = db.citizens.Where(c => c.surname == "a").ToList();
            //webReport.Report.RegisterData(citizens, "AppData");

            //webReport.Report.SetParameterValue("Surname", "a");
            //webReport.ReportFile = this.Server.MapPath("~/App_Data/InheritedReport.frx");
            string report_path = GetReportPath();
            System.Data.DataSet CitizenDataSet = citizens.ToDataSet();
            webReport.Report.RegisterData(CitizenDataSet, "CitizenDataSet");
            webReport.Report.Load(report_path + "InheritedReport.frx");

            ViewBag.WebReport = webReport;
            return View();
        }


        private string GetReportPath()
        {
            return this.Server.MapPath("~/App_Data/");
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
