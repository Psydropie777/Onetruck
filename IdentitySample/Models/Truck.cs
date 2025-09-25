using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class Truck
    {
        [Key]
        public int truckId { get; set; }
        public string RegistrationNo { get; set; }
        public int size { get; set; }
        [Display(Name = "Brand")]
        public int truckBrandId { get; set; }
        [Display(Name = "Model")]
        public int truckModelId { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public bool twoColour { get; set; }
        [Display(Name = "Primary Colour")]
        public string colour { get; set; }

        [DisplayName("Second Colour")]
        public string colour2 { get; set; }
        partial class TruckDetail { }
        ApplicationDbContext db = new ApplicationDbContext();

        public string getTruckBrand()
        {
            TruckBrand mk = db.TruckBrands.ToList().Find(x => x.truckBrandId == truckBrandId);
            return mk.Name;
        }
        public string getTruckModel()
        {
            TruckModel mk = db.TruckModels.ToList().Find(x => x.truckModelId == truckModelId);
            return mk.Name;
        }

    }

    public class TruckModel
    {
        [Key]
        public int truckModelId { get; set; }
        public string Name { get; set; }
        public int truckBrandId { get; set; }

        // Navigation property
        public TruckBrand TruckBrand { get; set; }
    }
    public class TruckBrand
    {
        [Key]
        public int truckBrandId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string ImageType { get; set; }
        public List<TruckModel> TruckModels { get; set; }
    }


}