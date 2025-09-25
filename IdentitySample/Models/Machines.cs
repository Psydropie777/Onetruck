using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace YourNamespace.Models
    {
        public class MachineBrand
        {
            [Key]
            public int BrandId { get; set; }

            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            // Navigation property: A brand can have many machines
            public virtual ICollection<Machine> Machines { get; set; }
        }

        public class MachineType
        {
            [Key]
            public int MachineTypeId { get; set; }

            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            // Navigation property: A machine type can have many machines
            public virtual ICollection<Machine> Machines { get; set; }
        }

        public class Machine
        {
            [Key]
            public int MachineId { get; set; }

            [Required]
            [MaxLength(100)]
            public string SerialNumber { get; set; }

            [Required]
            public int BrandId { get; set; }

            [ForeignKey("BrandId")]
            public virtual MachineBrand Brand { get; set; }

            [Required]
            public int MachineTypeId { get; set; }

            [ForeignKey("MachineTypeId")]
            public virtual MachineType MachineType { get; set; }

            [MaxLength(200)]
            public string Model { get; set; }

            public int? Year { get; set; }
        }
    }

}