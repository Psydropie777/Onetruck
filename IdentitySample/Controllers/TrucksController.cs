using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;

namespace IdentitySample.Controllers
{
    public class TrucksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Trucks
        public ActionResult AvailabeTrucks()
        {
            return View(db.Trucks.ToList());
        }
        // GET: Trucks
        public ActionResult Index()
        {
            return View(db.Trucks.ToList());
        }

        // GET: Trucks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        public ActionResult Create()
        {
            ViewBag.MakeList = new SelectList(db.TruckBrands.ToList(), "truckBrandId", "Name");
            ViewBag.ModelList = new SelectList(Enumerable.Empty<SelectListItem>()); // empty at first
            return View();
        }

        public JsonResult GetModelList(int truckBrandId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var listModel = db.TruckModels.Where(x => x.truckBrandId == truckBrandId).ToList();
            
            return Json(listModel, JsonRequestBehavior.AllowGet);
        }
        // POST: Trucks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "truckId,RegistrationNo,truckBrandId,truckModelId,Image,ImageType,size,twoColour,colour,colour2")] Truck truck, HttpPostedFileBase file)
        {
            {
                if (file != null && file.ContentLength > 0)
                {
                    truck.ImageType = file.ContentType; // Use content type (e.g. "image/jpeg")
                    truck.Image = ConvertToBytes(file);
                }
                if (ModelState.IsValid)
                {
                    db.Trucks.Add(truck);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(truck);
            }
        }

        //--------------------------------Convert file to binary------------------------------------------
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            using (BinaryReader reader = new BinaryReader(file.InputStream))
            {
                return reader.ReadBytes((int)file.ContentLength);
            }
        }

        //--------------------------------Display File-----------------------------------------------
        public ActionResult RenderImage(int id)
        {
            try
            {
                var item = db.Trucks.FirstOrDefault(x => x.truckId == id);
                if (item?.Image == null || string.IsNullOrEmpty(item.ImageType))
                {
                    // Could log warning here
                    return new HttpNotFoundResult("Image not found.");
                }

                MemoryStream ms = new MemoryStream(item.Image);
                return new FileStreamResult(ms, item.ImageType);
            }
            catch (Exception ex)
            {
                // Log the exception (use a logger or write to a file/event log)
                System.Diagnostics.Debug.WriteLine("Error in RenderImage: " + ex.Message);

                // Optional: show a default image or error image
                string defaultImagePath = Server.MapPath("~/Content/images/tipper-truck vector.jpg");
                return File(defaultImagePath, "image/png");
            }
        }

        // ----------------------------GET: Trucks/Edit/5----------------------------------------------
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // POST: Trucks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "truckId,RegistrationNo,size,twoColour,colour,colour2")] Truck truck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(truck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(truck);
        }

        // GET: Trucks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Truck truck = db.Trucks.Find(id);
            if (truck == null)
            {
                return HttpNotFound();
            }
            return View(truck);
        }

        // POST: Trucks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Truck truck = db.Trucks.Find(id);
            db.Trucks.Remove(truck);
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
