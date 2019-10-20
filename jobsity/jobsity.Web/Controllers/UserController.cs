using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jobsity.Repository;
using jobsity.Utilities;

namespace jobsity.Web.Controllers
{
    public class UserController : Controller
    {
        private jobsityEntities db = new jobsityEntities();

        // GET: User
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.Role);
            return View(user.ToList());
        }

        public ActionResult Login(bool exit = false)
        {
            if (exit)
                Security.SignOut();
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(User user)
        {
            Security security = new Security();
            string pass = Security.Md5Enc(user.Password);
            var us = db.User.Where(r => r.Email == user.Email && r.Password == pass).FirstOrDefault();
            if (us == null)
            {
                ModelState.AddModelError("Password", "Incorrect User/Password");
                return View();
            }

            security.Login(us);

            return RedirectToAction("Index", "Chat");
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.IdRole = new SelectList(db.Role, "IdRole", "Name");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUser,FirstName,LastName,Email,Password,IdRole")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Password = Security.Md5Enc(user.Password);
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRole = new SelectList(db.Role, "IdRole", "Name", user.IdRole);
            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            user.Password = "";
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRole = new SelectList(db.Role, "IdRole", "Name", user.IdRole);
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUser,FirstName,LastName,Email,Password,IdRole")] User user)
        {
            ModelState.Remove("Password");
            string passwod = db.User.Where(r => r.IdUser == user.IdUser).Select(r => r.Password).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(user.Password))
                    user.Password = passwod;
                else
                    user.Password = Security.Md5Enc(user.Password);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRole = new SelectList(db.Role, "IdRole", "Name", user.IdRole);
            return View(user);
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
