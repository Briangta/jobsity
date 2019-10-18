using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jobsity.Utilities;

namespace jobsity.Web.Controllers
{
    public class ChatController : Controller
    {
        [Security()]
        public ActionResult Index()
        {

            return View();
        }
    }
}