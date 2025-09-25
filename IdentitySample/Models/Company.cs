using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentitySample.Models
{

    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }


        // Navigation Properties
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual Account Account { get; set; }
    }

    public class Account
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }

        // Foreign Key
        public int CompanyId { get; set; }

        // Navigation
        public virtual Company Company { get; set; }
        public virtual Administrator Administrator { get; set; }
    }

    public class Administrator
    {
        public int AdministratorId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        // Foreign Key
        public int AccountId { get; set; }

        // Navigation
        public virtual Account Account { get; set; }
    }

    public class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Navigation
        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
    }


}