using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jobsity.Repository;
using jobsity.Utilities;
using Newtonsoft.Json;

namespace jobsity.Web.Controllers
{
    public class ChatController : Controller
    {
        private jobsityEntities db = new jobsityEntities();

        [Security()]
        public ActionResult Index()
        {
            ViewBag.BotUrl = ConfigurationManager.AppSettings["botUrl"];
            ViewBag.BotUserId = db.User.Where(r => r.IdRole == 3).Select(r=>r.IdUser).FirstOrDefault();
            return View();
        }

        public JsonResult GetMessages(int lastMessageId=0)
        {
            List<Message> messages= db.Message.Where(r => r.IdMessage > lastMessageId).OrderByDescending(r => r.IdMessage).Take(50)
                .Include(r=>r.User)
                .ToList().OrderBy(r => r.Date).ToList();
            string json=JsonConvert.SerializeObject(messages);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void CreateMessage(Message message)
        {
            message.Date = DateTime.Now;
            db.Message.Add(message);
            db.SaveChanges();
        }
    }
}