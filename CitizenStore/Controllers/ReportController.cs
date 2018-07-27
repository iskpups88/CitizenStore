using FastReport.Web;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CitizenStore.Controllers
{
    public class ReportController : Controller
    {
        private CitizenruDbEntities db = new CitizenruDbEntities();
       
        public ActionResult Index()
        {
            WebReport webReport = new WebReport
            {
                Width = 780,// Unit.Percentage(100);
                Height = 800 // Unit.Percentage(100); 
            };
       
            var citizens = TempData["list"] as List<citizen>;
            //List<citizen> citizens = db.citizens.Where(c => c.surname == "a").ToList();
            webReport.Report.RegisterData(citizens, "AppData");
            webReport.ReportFile = this.Server.MapPath("~/App_Data/report.frx");
            ViewBag.WebReport = webReport;
            return View();
        }
    }
}