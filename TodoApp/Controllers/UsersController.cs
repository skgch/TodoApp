using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers
{

    [Authorize(Roles = "Administrators")]
    public class UsersController : Controller
    {
        private TodoesContext db = new TodoesContext();
        readonly private CustomMembershipProvider membershipProvider = new CustomMembershipProvider();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            SetRoles(new List<Role>());
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,RoleIds")] User user)
        {
            var roles = db.Roles.Where(role => user.RoleIds.Contains(role.Id)).ToList();

            if (ModelState.IsValid)
            {
                user.Roles = roles;
                user.Password = membershipProvider.GeneratePasswodHash(user.UserName, user.Password);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SetRoles(roles);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            SetRoles(user.Roles);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,RoleIds")] User user)
        {
            var roles = db.Roles.Where(role => user.RoleIds.Contains(role.Id)).ToList();

            if (ModelState.IsValid)
            {
                User dbUser = db.Users.Find(user.Id);
                if (dbUser == null)
                {
                    return HttpNotFound();
                }

                dbUser.UserName = user.UserName;
                if (dbUser.Password != user.Password)
                {
                    dbUser.Password = membershipProvider.GeneratePasswodHash(user.UserName, user.Password);
                }

                dbUser.Roles.Clear();
                foreach (var role in roles)
                {
                    dbUser.Roles.Add(role);
                }

                db.Entry(dbUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SetRoles(roles);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void SetRoles(ICollection<Role> roles)
        {
            int[] roleIds = roles.Select(role => role.Id).ToArray();
            var list = db.Roles.Select(item => new SelectListItem()
            {
                Text = item.RoleName,
                Value = item.Id.ToString(),
                Selected = roleIds.Contains(item.Id)
            }).ToList();

            ViewBag.RoleIds = list;
        }
    }
}
