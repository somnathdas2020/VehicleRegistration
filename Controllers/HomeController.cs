using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleRegistration.Abstract;
using VehicleRegistration.Utility;

namespace VehicleRegistration.Controllers
{
    public class HomeController : Controller
    {
        IVehicleRegistration vReg = null;
        public HomeController()
        {
            vReg = new VehicleRegistration.Concrete.VehicleRegistration();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("All-Vehicle-Registrations")]
        public ActionResult VehicleRegistrationDetail()
        {
            return View(vReg.GetAllVehicleRegistration());
        }

        [Route("Payment/{regid}")]
        [HttpGet]
        public ActionResult PaymentDetail(string regid)
        {
            int _regid = Convert.ToInt32(GlobalUtility.DecryptString(regid));
            return View(vReg.GetVehicleRegistrationDetail(_regid));
        }
        
        [Route("Pay/{regid}")]
        [HttpPost]
        public JsonResult PayRegistrationamount(string regid, string payamount)
        {
            int _regid = Convert.ToInt32(GlobalUtility.DecryptString(regid));
            decimal _amount = decimal.Parse(payamount);
            
            return Json(vReg.PayRegistrationAmount(_regid, _amount));
        }
    }
}