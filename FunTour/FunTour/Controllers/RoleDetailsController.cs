using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using FunTourDataLayer;
using FunTourDataLayer.AccountManagement;

namespace FunTour.Controllers
{
    public class RoleDetailsController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: RoleDetails
        public ActionResult Index()
        {
            return View(_context.RoleDetails.Where(p => p.IsDeleted == false).ToList());
        }

        // GET: RoleDetails/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleDetails roleDetails = _context.RoleDetails.Find(id);
            if (roleDetails == null)
            {
                return HttpNotFound();
            }
            return View(roleDetails);
        }

        // GET: RoleDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleDetails/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_Role,RoleName,RoleDescription,IsSysAdmin")] RoleDetails roleDetails)
        {
            if (ModelState.IsValid)
            {
                
                _context.RoleDetails.Add(roleDetails);
                _context.SaveChanges();


                var auxrole = new IdentityRole
                {
                    Id = _context.RoleDetails.FirstOrDefault( p=> p.RoleName == roleDetails.RoleName).Id_Role.ToString(),
                    Name = roleDetails.RoleName
                };
                _context.Roles.Add(auxrole);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(roleDetails);
        }

        // GET: RoleDetails/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleDetails roleDetails = _context.RoleDetails.Find(id);
            if (roleDetails == null)
            {
                return HttpNotFound();
            }
            return View(roleDetails);
        }

        // POST: RoleDetails/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_Role,RoleName,RoleDescription,IsSysAdmin")] RoleDetails roleDetails)
        {
            if (ModelState.IsValid)
            {
                var auxrole = new IdentityRole
                {
                    Id = roleDetails.Id_Role.ToString(),
                    Name = roleDetails.RoleName
                };
                _context.Entry(auxrole).State = EntityState.Modified;
                _context.Entry(roleDetails).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roleDetails);
        }

        // GET: RoleDetails/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleDetails roleDetails = _context.RoleDetails.Find(id);
            if (roleDetails == null)
            {
                return HttpNotFound();
            }
            return View(roleDetails);
        }

        // POST: RoleDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            RoleDetails roleDetails = _context.RoleDetails.Find(id);
            roleDetails.IsDeleted = true;
            _context.Entry(roleDetails).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
