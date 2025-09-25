using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;

namespace IdentitySample.Controllers
{
    public class TruckBrandsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TruckBrands
        public ActionResult Index()
        {
            return View(db.TruckBrands.ToList());
        }

        // GET: TruckBrands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckBrand truckBrand = db.TruckBrands.Find(id);
            if (truckBrand == null)
            {
                return HttpNotFound();
            }
            return View(truckBrand);
        }

        // GET: TruckBrands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TruckBrands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "truckBrandId,Name,Image,ImageType")] TruckBrand truckBrand)
        {
            if (ModelState.IsValid)
            {
                db.TruckBrands.Add(truckBrand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(truckBrand);
        }

        // GET: TruckBrands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckBrand truckBrand = db.TruckBrands.Find(id);
            if (truckBrand == null)
            {
                return HttpNotFound();
            }
            return View(truckBrand);
        }

        // POST: TruckBrands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "truckBrandId,Name,Image,ImageType")] TruckBrand truckBrand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truckBrand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(truckBrand);
        }

        // GET: TruckBrands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckBrand truckBrand = db.TruckBrands.Find(id);
            if (truckBrand == null)
            {
                return HttpNotFound();
            }
            return View(truckBrand);
        }

        // POST: TruckBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TruckBrand truckBrand = db.TruckBrands.Find(id);
            db.TruckBrands.Remove(truckBrand);
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
    }
}
