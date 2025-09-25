using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }
        //
        // GET: /Users/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);
            return View(user);
        }

        //
        // GET: /Users/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            RegisterViewModel model = new RegisterViewModel();
            ViewBag.Password = "Password@01";
            //model.TitleCollection = db.Titles.ToList();
            //Get the list of Roles
            ViewBag.licenceCode = new SelectList(db.LicenceCodes, "lcId", "lcName");
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }
        //
        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterDriver userViewModel, params string[] selectedRoles)
        {
            IDValidationAttribute idAttr = new IDValidationAttribute();
            userViewModel.Password = "Password@01";
            string gender = "";
            string Dob = "";
            int age = 0;
            idAttr.getIDnumberDetails(userViewModel.IDno, out gender, out age, out Dob);
            if (userViewModel.title == "none")
            {
                TempData["AlertMessage"] = "Please select title";
                return View(userViewModel);
            }
            if (userViewModel.phoneNo.Length != 10)
            {
                TempData["AlertMessage"] = "Phone number must be 10 digits";
                return View(userViewModel);
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    title = userViewModel.title,
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    IDno = userViewModel.IDno,
                    Address = userViewModel.Address,
                    PhoneNumber = userViewModel.phoneNo,
                    gender = gender,
                    DoB = Dob
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);
                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    Driver st = new Driver();
                    Random r = new Random();
                    st.EmpId = Guid.NewGuid().ToString();
                    st.title = userViewModel.title;
                    st.Email = userViewModel.Email;
                    st.FirstName = userViewModel.FirstName.Substring(0, 1).ToUpper() + userViewModel.FirstName.Substring(1, (userViewModel.FirstName.Length - 1));
                    st.LastName = userViewModel.LastName.Substring(0, 1).ToUpper() + userViewModel.LastName.Substring(1, (userViewModel.LastName.Length - 1));
                    st.Address = userViewModel.Address;
                    st.IDno = userViewModel.IDno;
                    st.phoneNo = userViewModel.phoneNo;
                    st.employeeNumber = "2025" + r.Next(100, 999);
                    st.gender = gender;
                    st.DoB = Dob;
                    st.licenceNumber = userViewModel.licenceNumber;
                    st.validFrom = userViewModel.validFrom;
                    st.validTo = userViewModel.validTo;
                    st.lcId = userViewModel.lcId;
                    db.Drivers.Add(st);
                    db.SaveChanges();
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.LicenceCode = new SelectList(db.LicenceCodes, "lcId", "lcName", userViewModel.lcId);

            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }


        //
        // GET: /Users/Edit/1
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.Email;
                user.Email = editUser.Email;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
