using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using IdentitySample.Models;
using Owin;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Data.Entity.Migrations;
using System.Globalization;

namespace IdentitySample
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and role manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(
            //    clientId: "",
            //    clientSecret: "");
            CreateRolesAndUsers();
        }
        public void CreateRolesAndUsers()
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            string pwd = "Password@01";


            //creating Admin role
            if (!roleManager.RoleExists("Admin"))
            {
                //--------------------LicenceCode---------------------------------------------------------
                db.LicenceCodes.AddOrUpdate(
                p => p.lcId,
                new LicenceCode { lcId = 1, lcName = "A" },
                new LicenceCode { lcId = 2, lcName = "A1" },
                new LicenceCode { lcId = 3, lcName = "B" },
                new LicenceCode { lcId = 4, lcName = "C" },
                new LicenceCode { lcId = 5, lcName = "C1" },
                new LicenceCode { lcId = 6, lcName = "EB" },
                new LicenceCode { lcId = 7, lcName = "EC" },
                new LicenceCode { lcId = 8, lcName = "EC1" }
                );
                //--------------------TruckBrand---------------------------------------------------------
                db.TruckBrands.AddOrUpdate(
                p => p.truckBrandId,
                new TruckBrand { truckBrandId = 1, Name = "Mercedes" },
                new TruckBrand { truckBrandId = 2, Name = "Isuzu" },
                new TruckBrand { truckBrandId = 3, Name = "FAW" },
                new TruckBrand { truckBrandId = 4, Name = "MAN" },
                new TruckBrand { truckBrandId = 5, Name = "Powerstar" }
                );
                //--------------------Truck Model---------------------------------------------------------
                db.TruckModels.AddOrUpdate(
                p => p.truckModelId,
                new TruckModel { truckModelId = 1, truckBrandId = 1, Name = "Axor 1829" },
                new TruckModel { truckModelId = 2, truckBrandId = 1, Name = "Axor 1833" },
                new TruckModel { truckModelId = 3, truckBrandId = 1, Name = "Axor 1840" }, 
                new TruckModel { truckModelId = 4, truckBrandId = 1, Name = "Axor 3344 K" },
                new TruckModel { truckModelId = 5, truckBrandId = 1, Name = "Axor 3335" },
                new TruckModel { truckModelId = 6, truckBrandId = 1, Name = "Actros 1835" },
                new TruckModel { truckModelId = 7, truckBrandId = 1, Name = "Actros 3341 AK" },
                new TruckModel { truckModelId = 8, truckBrandId = 1, Name = "Actros 4146 K" },
                new TruckModel { truckModelId = 9, truckBrandId = 1, Name = "Actros 2655 V8" },
                new TruckModel { truckModelId = 10, truckBrandId = 2, Name = "FXZ 26-360" },
                new TruckModel { truckModelId = 11, truckBrandId = 2, Name = "FXZ 28-360" },
                new TruckModel { truckModelId = 12, truckBrandId = 2, Name = "FYH 33-360" },
                new TruckModel { truckModelId = 13, truckBrandId = 2, Name = "GXR 35-360" },
                new TruckModel { truckModelId = 14, truckBrandId = 2, Name = "GXZ 45-360" },
                new TruckModel { truckModelId = 15, truckBrandId = 3, Name = "28.280FD" },
                new TruckModel { truckModelId = 16, truckBrandId = 3, Name = "35.340FD" },
                new TruckModel { truckModelId = 17, truckBrandId = 3, Name = "J5N 28.290FD" },
                new TruckModel { truckModelId = 18, truckBrandId = 3, Name = "J5N 33.340FD" },
                new TruckModel { truckModelId = 19, truckBrandId = 3, Name = "J5N 35.340FD" },
                new TruckModel { truckModelId = 20, truckBrandId = 4, Name = "TGS 33.360" },
                new TruckModel { truckModelId = 21, truckBrandId = 4, Name = "TGS 41.440" },
                new TruckModel { truckModelId = 21, truckBrandId = 4, Name = "TGS 33.480" },
                new TruckModel { truckModelId = 22, truckBrandId = 4, Name = "GS 33.480" },
                new TruckModel { truckModelId = 23, truckBrandId = 5, Name = "VX16-27" },
                new TruckModel { truckModelId = 24, truckBrandId = 5, Name = "VX-2527" },
                new TruckModel { truckModelId = 25, truckBrandId = 5, Name = "VX-2628" },
                new TruckModel { truckModelId = 26, truckBrandId = 5, Name = "VX-4035B" }
                );

                //--------------------Truck---------------------------------------------------------
                db.Trucks.AddOrUpdate(
                p => p.truckId,
                new Truck { truckId = 1, truckModelId = 5, truckBrandId = 1, RegistrationNo = "KR 39 CH - ZN",
                    Image = new byte[]{0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01,0x00, 0x25, 0x80, 0x25, 0x80, 0x00, 0x00, 0xFF, 0xDB, 0x00, 0x43}, 
                    ImageType = "image/jpeg", size = 10,twoColour = true, colour = "White", colour2 = "white" }
                );
                //create super admin role
                var role = new IdentityRole("Admin");
                roleManager.Create(role);

                //create default user
                var user = new ApplicationUser();
                user.UserName = "admin@truck1.ac.za";
                user.Email = "admin@truck1.ac.za";

                user.title = "Mr";

                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                    userManager.AddToRole(user.Id, "Admin");
            }
            if (!roleManager.RoleExists("Driver"))
            {
                var Trole = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                Trole.Name = "Driver";
                roleManager.Create(Trole);

                //create default Staff user
                var Tuser = new ApplicationUser();
                Tuser.title = "Mr";
                Tuser.UserName = "2025100@truck1.ac.za";
                Tuser.Email = "2025100@truck1.ac.za";
                Tuser.FirstName = "Samkelo";
                Tuser.LastName = "Gwala";
                Tuser.Address = "12 Zuma rd, Durban West, 3610";
                Tuser.gender = "Male";
                Tuser.IDno = "9405056608087";
                Tuser.PhoneNumber = "0789635214";
                Tuser.DoB = "05-05-1994";
                var dnewuser = userManager.Create(Tuser, pwd);
                if (dnewuser.Succeeded)
                {
                    userManager.AddToRole(Tuser.Id, "Driver");
                }
                Driver s = new Driver();
                Random r = new Random();
                s.EmpId = Guid.NewGuid().ToString();
                s.userId = Tuser.Id;
                s.title = "Mr";
                s.FirstName = Tuser.FirstName;
                s.LastName = Tuser.LastName;
                s.Address = "12 Zuma rd, Durban West, 3610";
                s.IDno = Tuser.IDno;
                s.gender = "Male";
                s.phoneNo = "0789635214";
                s.DoB = "05-5-1994";
                s.Email = "2025100@truck1.ac.za";
                s.employeeNumber = "2025100";
                s.lcId = 8;
                s.licenceNumber = "8739000070HX";
                s.validFrom = DateTime.Parse("2021-03-21");
                s.validTo = DateTime.Parse("2025-03-21");

        s.firstIssue = "24-03-2015";
                db.Drivers.Add(s);
                db.SaveChanges();

            }
        }
    }
}