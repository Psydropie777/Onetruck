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
    public class TruckModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TruckModels
        public ActionResult Index()
        {
            var truckModels = db.TruckModels.Include(t => t.TruckBrand);
            return View(truckModels.ToList());
        }

        // GET: TruckModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckModel truckModel = db.TruckModels.Find(id);
            if (truckModel == null)
            {
                return HttpNotFound();
            }
            return View(truckModel);
        }

        // GET: TruckModels/Create
        public ActionResult Create()
        {
            ViewBag.truckBrandId = new SelectList(db.TruckBrands, "truckBrandId", "Name");
            return View();
        }

        // POST: TruckModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "truckModelId,Name,truckBrandId")] TruckModel truckModel)
        {
            if (ModelState.IsValid)
            {
                db.TruckModels.Add(truckModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.truckBrandId = new SelectList(db.TruckBrands, "truckBrandId", "Name", truckModel.truckBrandId);
            return View(truckModel);
        }

        // GET: TruckModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckModel truckModel = db.TruckModels.Find(id);
            if (truckModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.truckBrandId = new SelectList(db.TruckBrands, "truckBrandId", "Name", truckModel.truckBrandId);
            return View(truckModel);
        }

        // POST: TruckModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "truckModelId,Name,truckBrandId")] TruckModel truckModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truckModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.truckBrandId = new SelectList(db.TruckBrands, "truckBrandId", "Name", truckModel.truckBrandId);
            return View(truckModel);
        }

        // GET: TruckModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TruckModel truckModel = db.TruckModels.Find(id);
            if (truckModel == null)
            {
                return HttpNotFound();
            }
            return View(truckModel);
        }

        // POST: TruckModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TruckModel truckModel = db.TruckModels.Find(id);
            db.TruckModels.Remove(truckModel);
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
