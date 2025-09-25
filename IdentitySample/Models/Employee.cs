
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{
    public class Employee
    {
        [Key]
        public string EmpId { get; set; }
        public string userId { get; set; }
        [Display(Name = "Title")]
        public string title { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Identity Number")]
        public string IDno { get; set; }
        [Display(Name = "Date of Birth")]
        public string DoB { get; set; }
        [Display(Name = "Gender")]
        public string gender { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }

        //Contacts details
      
        [DisplayName("Contact Number")]
        public string phoneNo { get; set; }
        [Display(Name = "Employeee Number")]
        public string employeeNumber { get; set; }
        
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public bool isActive { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();

     
    }
    public class Clerk : Employee
    {

    }
    public class Driver : Employee
    {
        public string licenceNumber { get; set; }
        [Required]
        [Display(Name = "Valid From")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime validFrom { get; set; }
        [Required]
        [Display(Name = "Valid To:")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime validTo { get; set; }
        public int lcId { get; set; }
        public LicenceCode LicenceCodes { get; set; }
        public string firstIssue { get; set; }
    }
    public class LicenceCode
    {
        [Key]
        public int lcId { get; set; }
        public string lcName { get; set; }
    }
}