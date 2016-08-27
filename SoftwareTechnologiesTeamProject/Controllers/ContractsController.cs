using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftwareTechnologiesTeamProject.Controllers
{
    public class ContactsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Here you can find how to contact us.";

            return View();
        }
    }
}