using System.Collections.Generic;
using System.Web.Mvc;
using Ext.Net.MVC;
using CitizenStore.Models;
using Ext.Net;
using FastReport.Web;
using CitizenStore.Extensions;
using CitizenStore.Repositories;
using System;

namespace CitizenStore.Controllers
{
    public class CitizensController : Controller
    {
        private WebReport webReport = new WebReport();
        private ICitizenRepository repo = new CitizenRepository();

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Search(CitizenViewModel model)
        {
            List<Citizen>  data = repo.GetCitizenList(model);
            return this.Store(data);
        }


        public ActionResult Add(Citizen model)
        {
            if (ModelState.IsValid)
            {
                repo.Create(model);
                return this.Store(model);
            }
            else
                return (ActionResult)this.Content("");
        }

        public ActionResult HandleChanges(StoreDataHandler handler)
        {
            List<Citizen> citizens = handler.ObjectData<Citizen>();
            string errorMessage = null;

            if (handler.Action == StoreAction.Create)
            {
                try
                {
                    foreach (Citizen created in citizens)
                    {
                        repo.Create(created);
                    }
                }catch(Exception e)
                {
                    errorMessage = e.Message;
                }
            }
            else if (handler.Action == StoreAction.Destroy)
            {
                foreach (Citizen deleted in citizens)
                {
                    repo.Delete(deleted.Id);
                    //citizen citizen = await db.citizens.FirstOrDefaultAsync(c => c.id == deleted.id);
                    //db.citizens.Remove(citizen);
                    //await db.SaveChangesAsync();
                }
            }
            else if (handler.Action == StoreAction.Update)
            {
                foreach (Citizen updated in citizens)
                {
                    try
                    {
                        repo.Update(updated);
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.Message;
                    }
                    //citizen citizen = await db.citizens.FirstOrDefaultAsync(c => c.id == updated.id);
                    //CitizenHelper.CitizenUpdate(citizen, updated);
                    //await db.SaveChangesAsync();
                }
            }

            if (errorMessage != null)
            {
                return this.Store(errorMessage);
            }

            return handler.Action != StoreAction.Destroy ? (ActionResult)this.Store(citizens) : (ActionResult)this.Content("");
        }


        public void PrintData(List<Citizen> citizens)
        {
            TempData["citizens"] = citizens;
        }

        public ActionResult Report()
        {
            WebReport webReport = new WebReport
            {
                Width = 780,
                Height = 800 
            };
            var citizens = TempData["citizens"] as List<Citizen>;
            string report_path = GetReportPath();            ;
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

    }
}
