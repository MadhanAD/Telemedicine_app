﻿using System.Globalization;
using WebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Helper;
using Newtonsoft.Json;
using Identity.Membership;
using Identity.Membership.Models;
using DataAccess;
using WebApp.Repositories.DoctorRepositories;
using WebApp.Repositories.PatientRepositories;
using System.Text;
using WebApp.Repositories.AdminRepository;
using System.Data;
using WebApp.DatabaseMethods;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Data.Entity;
using DataAccess.CustomModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        string error = "";
        private TeleMedEntities db = new TeleMedEntities();
        DBMethods ObjDBmethods = new DBMethods();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        [AllowAnonymous]
        public ActionResult SKDefault(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }
        [AllowAnonymous]
        public JsonResult SessionExpiry()
        {

            var flag = 0;
            if (SessionHandler.IsExpired)
            {
                flag = 1;
            }
            else
            {
                flag = 0;
            }
            return Json(new { result = flag });
        }
        // change anita
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (returnUrl.Contains("SeeDoctor"))
            {
                return RedirectToAction("PatientLogin", "Account");
            }
            if (returnUrl.Contains("Doctor"))
            {
                return RedirectToAction("DoctorLogin", "Account");
            }
            //else if (!returnUrl.Contains("Doctor") && !returnUrl.Contains("Admin")) // comment anita(Tele-Login) 04-01-2020
            //{
            //    return RedirectToAction("PatientLogin", "Account");
            //}
            else if (!returnUrl.Contains("Doctor") && !returnUrl.Contains("Admin") && !returnUrl.Contains("Patient"))
            {
                return RedirectToAction("TeleLogin", "Account");
                //return RedirectToAction("PatientLogin", "Account");
            }
            else if (returnUrl.Contains("Admin"))
            {
                return RedirectToAction("AdminLogin", "Account");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: /Account/PatientLogin_Old
        [AllowAnonymous]
        public ActionResult PatientLogin_Old(string returnUrl)
        {
            var list = new List<SelectListItem>
                   {
                new SelectListItem{ Text="PayPal", Value = "1" },
                new SelectListItem{ Text="BluePay", Value = "2" },
                new SelectListItem{ Text="Amazon Pay", Value = "3" },
                 new SelectListItem{ Text="GoURL", Value = "4"}
            };

            var states = db.States.ToList();


            ViewBag.payment = list;
            var res = db.Doctors.Where(x => x.zip != null).GroupBy(g=>g.zip).Select(x=>x.FirstOrDefault()).ToList();
            ViewBag.ReturnUrl = returnUrl;
            //ViewBag.drpdnlist = res.Select(x => new SelectListItem { Text = x.firstName + x.lastName, Value = x.doctorID.ToString() }).ToList();
            ViewBag.Location = res;
            ViewBag.error = TempData["error"];
           // ViewBag.doctors = res;
            return View();
        }


        [AllowAnonymous]
        public ActionResult TeleLogin()
        {
            return View();
        }
        // add anita
        // GET: /Account/PatientLogin
        [AllowAnonymous]
        public ActionResult PatientLogin(string returnUrl)
        {
            var list = new List<SelectListItem>
                   {
                new SelectListItem{ Text="PayPal", Value = "1" },
                new SelectListItem{ Text="BluePay", Value = "2" },
                new SelectListItem{ Text="Amazon Pay", Value = "3" },
                 new SelectListItem{ Text="GoURL", Value = "4"}
            };

            var states = db.States.ToList();


            ViewBag.payment = list;
            var res = db.Doctors.Where(x => x.zip != null).GroupBy(g => g.zip).Select(x => x.FirstOrDefault()).ToList();
            ViewBag.ReturnUrl = returnUrl;
            //ViewBag.drpdnlist = res.Select(x => new SelectListItem { Text = x.firstName + x.lastName, Value = x.doctorID.ToString() }).ToList();
            ViewBag.Location = res;
            ViewBag.error = TempData["error"];
            // ViewBag.doctors = res;
            return View();
        }

        // GET: /Account/GetFillDoc
        [AllowAnonymous]
        public ActionResult GetFillDoc(string zip)
        {
            var Doctors = db.Doctors.Where(c => c.zip == zip).Select(s => new { s.firstName, s.doctorID }).ToList();
            var result = Json(Doctors, JsonRequestBehavior.AllowGet);
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetDoctorsFill(string zip)
        {
            var Doctors = db.Doctors.Where(c => c.zip == zip).Select(s => new { s.firstName, s.doctorID }).ToList();
            var result = Json(Doctors, JsonRequestBehavior.AllowGet);
            return result;
        }
        public ActionResult PatientSignup()
        {
            return View();
        }

        // change anita
        // GET: /Account/DoctorLogin_Old
        [AllowAnonymous]
        public ActionResult DoctorLogin_Old(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: /Account/AdminLogin_Old
        [AllowAnonymous]
        public ActionResult AdminLogin_Old(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // add anita
        // GET: /Account/DoctorLogin
        [AllowAnonymous]
        public ActionResult DoctorLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        // GET: /Account/AdminLoginNew
        [AllowAnonymous]
        public ActionResult AdminLogin(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginRegisterViewModel model, string returnUrl)
        {
            //var IsPatient = (bool)ViewBag.IsPatient;


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var strContent = JsonConvert.SerializeObject(model);
            //var response = ApiConsumerHelper.PostData("api/Account/Login", strContent);
            //var resultTest = JsonConvert.DeserializeObject<SignInStatus>(response);

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, model.LoginViewModel.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    {

                        //var userId = HttpContext.User.Identity.GetUserId();
                        string userId = UserManager.FindByName(model.LoginViewModel.Email)?.Id;
                        SessionHandler.UserName = model.LoginViewModel.Email;
                        SessionHandler.Password = model.LoginViewModel.Password;
                        SessionHandler.UserId = userId;

                        var roles = UserManager.GetRoles(userId);
                        if (roles.Contains("Doctor"))
                        {
                            var objRepo = new DoctorRepository();
                            var doctor = objRepo.GetByUserId(userId);
                            var userModel = new UserInfoModel();
                            userModel.Id = doctor.doctorID;
                            userModel.Email = doctor.email;
                            userModel.FirstName = doctor.firstName;
                            userModel.LastName = doctor.lastName;
                            userModel.userId = doctor.userId;
                            userModel.title = doctor.title;
                            userModel.timeZoneOffset = doctor.timeZoneoffset;
                            userModel.role = doctor.role;
                            userModel.iOSToken = doctor.iOSToken;
                            userModel.AndroidToken = doctor.AndroidToken;
                            SessionHandler.UserInfo = userModel;

                            if (doctor.picture != null && doctor.picture.Count() > 0)
                            {
                                SessionHandler.ProfilePhoto = Encoding.ASCII.GetString(doctor.picture);
                            }

                            if (doctor.active == null || (bool)doctor.active)
                                return RedirectToAction("DoctorTimings", "Doctor");
                        }
                        else if (roles.Contains("Patient"))
                        {
                            var objRepo = new PatientRepository();
                            var patient = objRepo.GetByUserId(userId);
                            var userModel = new UserInfoModel();
                            userModel.Id = patient.patientID;
                            userModel.Email = patient.email;
                            userModel.FirstName = patient.firstName;
                            userModel.LastName = patient.lastName;
                            userModel.userId = patient.userId;
                            userModel.title = patient.title;
                            userModel.timeZoneOffset = patient.timeZoneoffset;
                            userModel.role = patient.role;
                            userModel.iOSToken = patient.iOSToken;
                            userModel.AndroidToken = patient.AndroidToken;
                            SessionHandler.UserInfo = userModel;

                            if (patient.active == null || (bool)patient.active)
                                return RedirectToAction("Index", "Patient");
                        }
                        else if (roles.Contains("Admin"))
                        {
                            var user = await UserManager.FindAsync(model.LoginViewModel.Email, model.LoginViewModel.Password);
                            Session["LogedUserID"] = model.LoginViewModel.Email;
                            Session["LogedUserFullname"] = user.FirstName + " " + user.LastName;
                            return RedirectToAction("Default", "Admin");
                        }
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // POST: /Account/PatientLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PatientLogin(LoginRegisterViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, model.LoginViewModel.RememberMe, shouldLockout: false);
            var list = new List<SelectListItem>
                             {
                                        new SelectListItem{ Text="PayPal", Value = "1" },
                                        new SelectListItem{ Text="BluePay", Value = "2" },
                                        new SelectListItem{ Text="Amazon Pay", Value = "3" },
                                        new SelectListItem{ Text="GoURL", Value = "4"}
                            };
            var objRepo = new PatientRepository();
            ViewBag.payment = list;
            var states = db.States.ToList();
            //ViewBag.Location = states.Select(x => new SelectListItem { Text = x.stateName, Value = x.stateID.ToString() }).ToList();

            var res = db.Doctors.Where(x => x.zip != null).ToList();
            //List<Doctor> res = objRepo.GetDoctorByPatent();
            ViewBag.drpdnlist = res.Select(x => new SelectListItem { Text = x.firstName + x.lastName, Value = x.doctorID.ToString() }).ToList();
            ViewBag.Location = res.Select(x => new SelectListItem { Text = x.zip, Value = x.zip }).ToList();
            //ViewBag.drpdnlist =  new List<Doctor> { new SelectListItem { Text = res.firstName + res.lastName, Value = res.doctorID.ToString() } };
            //ViewBag.Location =  new SelectListItem { Text = res.zip, Value = res.zip };

            switch (result)
            {
                case SignInStatus.Success:
                    {
                        //var userId = HttpContext.User.Identity.GetUserId();

                        string userId = UserManager.FindByName(model.LoginViewModel.Email)?.Id;
                        SessionHandler.UserName = model.LoginViewModel.Email;
                        SessionHandler.Password = model.LoginViewModel.Password;
                        SessionHandler.UserId = userId;

                        //var objRepo = new PatientRepository();
                        var patient = objRepo.GetByUserId(userId);
                        if (patient == null)
                        {
                            ModelState.AddModelError("", "Invalid Username or Password.");
                            ViewBag.ModelError = "Invalid Username or Password.";
                            return View(model);
                        }
                        var userModel = new UserInfoModel();
                        userModel.Id = patient.patientID;
                        userModel.Email = patient.email;
                        userModel.FirstName = patient.firstName;
                        userModel.LastName = patient.lastName;
                        userModel.userId = patient.userId;
                        userModel.title = patient.title;
                        userModel.timeZone = patient.timeZone;
                        userModel.timeZoneOffset = patient.timeZoneoffset;
                        userModel.role = patient.role;
                        userModel.iOSToken = patient.iOSToken;
                        userModel.AndroidToken = patient.AndroidToken;
                        SessionHandler.UserInfo = userModel;
                        SessionHandler.ProfilePhoto = patient.ProfilePhotoBase64;
                        //SessionHandler.ProfilePhoto = patient.patientID.ToString() + ".png"; //Encoding.ASCII.GetString(patient.picture);

                        if (patient.active == null || (bool)patient.active)
                            return RedirectToAction("Index", "Appointment");
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid Username or Password.");
                    ViewBag.ModelError = "Invalid Username or Password.";
                    return View(model);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AdminLogin(LoginRegisterViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, model.LoginViewModel.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    {

                        //var userId = HttpContext.User.Identity.GetUserId();
                        string userId = UserManager.FindByName(model.LoginViewModel.Email)?.Id;
                        SessionHandler.UserName = model.LoginViewModel.Email;
                        SessionHandler.Password = model.LoginViewModel.Password;
                        SessionHandler.UserId = userId;

                        var objRepo = new AdminRepository();
                        var admin = objRepo.GetByUserId(userId);
                        if (admin == null)
                        {
                            ModelState.AddModelError("", "Invalid login attempt.");
                            ViewBag.ModelError = "Invalid login attempt.";
                            return View(model);
                        }
                        var userModel = new UserInfoModel();
                        userModel.Id = admin.adminID;
                        userModel.Email = admin.email;
                        userModel.FirstName = admin.firstName;
                        userModel.LastName = admin.lastName;
                        SessionHandler.UserInfo = userModel;
                        if (admin.active == null || (bool)admin.active)
                            Session["LogedUserID"] = model.LoginViewModel.Email;
                        Session["LogedUserFullname"] = userModel.FirstName + " " + userModel.LastName;
                        return RedirectToAction("Default", "Admin");

                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    ViewBag.ModelError = "Invalid login attempt.";
                    return View(model);
            }
        }


        // POST: /Account/DoctorLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoctorLogin(LoginRegisterViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await SignInManager.PasswordSignInAsync(model.LoginViewModel.Email, model.LoginViewModel.Password, model.LoginViewModel.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    {
                        //var userId = HttpContext.User.Identity.GetUserId();
                        string userId = UserManager.FindByName(model.LoginViewModel.Email)?.Id;
                        SessionHandler.UserName = model.LoginViewModel.Email;
                        SessionHandler.Password = model.LoginViewModel.Password;
                        SessionHandler.UserId = userId;
                        var objRepo = new DoctorRepository();
                        var doctor = objRepo.GetByUserId(userId);
                        if (doctor == null)
                        {
                            ModelState.AddModelError("", "Invalid login attempt.");
                            ViewBag.ModelError = "Invalid Username or Password.";
                            return View(model);
                        }
                        if (doctor.status == null || !((bool)doctor.status))
                        {
                            ModelState.AddModelError("", "Account review is in progress. You can login after approval.");
                            ViewBag.ModelError = "Account review is in progress. You can login after approval.";
                            return View(model);
                        }
                        string timezoneoffval = string.Empty;
                        var userModel = new UserInfoModel();
                        userModel.Id = doctor.doctorID;
                        userModel.Email = doctor.email;
                        userModel.FirstName = doctor.firstName;
                        userModel.LastName = doctor.lastName;
                        userModel.userId = doctor.userId;
                        userModel.title = doctor.title;
                        userModel.timeZone = doctor.timeZone;
                        DataSet ds = new DataSet();
                        ds = ObjDBmethods.GetTimezoneOffsetval(doctor.timeZone);
                        //string _offsetval = Convert.ToString(ds.Tables[0].Rows[0]["NewOffSet"]);
                        //userModel.timeZone = Convert.ToString(ObjDBmethods.GetTimezoneOffsetval(doctor.timeZone,out timezoneoffval));
                        userModel.timeZone = "(UTC) Casablanca";
                        string _offsetval = "0";
                        userModel.timeZoneOffset = _offsetval;
                        userModel.role = doctor.role;
                        userModel.iOSToken = doctor.iOSToken;
                        userModel.AndroidToken = doctor.AndroidToken;
                        SessionHandler.UserInfo = userModel;
                        SessionHandler.ProfilePhoto = doctor.ProfilePhotoBase64;


                        if (doctor.active == null || (bool)doctor.active)
                            //return RedirectToAction("DoctorTimings", "Doctor");
                            return RedirectToAction("Index", "DoctorAppointment");

                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid Username or Password.");
                    ViewBag.ModelError = "Invalid Username or Password.";
                    return View(model);
            }
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var doctors = db.Doctors;
            ViewBag.IsPatient = true;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(LoginRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.RegisterViewModel.Email,
                    Email = model.RegisterViewModel.Email,
                    FirstName = model.RegisterViewModel.FirstName,
                    LastName = model.RegisterViewModel.LastName,
                };

                // Add the Address properties:



                var result = await UserManager.CreateAsync(user, model.RegisterViewModel.Password);
                dynamic addedResult;
                if (result.Succeeded)
                {
                    SessionHandler.UserName = model.RegisterViewModel.Email;
                    SessionHandler.Password = model.RegisterViewModel.Password;
                    SessionHandler.UserId = user.Id;

                    if (model.IsPatient)
                    {
                        PatientRepository objRepo = new PatientRepository();
                        Patient obj = new Patient
                        {
                            userId = user.Id,
                            lastName = user.LastName,
                            firstName = user.FirstName,
                            email = user.Email
                        };
                        addedResult = objRepo.Add(obj);
                    }
                    else
                    {
                        DoctorRepository objRepo = new DoctorRepository();
                        Doctor obj = new Doctor
                        {
                            userId = user.Id,
                            lastName = user.LastName,
                            firstName = user.FirstName,
                            email = user.Email
                        };
                        addedResult = objRepo.Add(obj);
                    }
                    if (addedResult != null)
                    {
                        ViewBag.SuccessMessage = "Your Account has been created, please login";
                        return View("Login");
                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form

            //return View("Login", model);
            return View("Login", model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signup(LoginRegisterViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.RegisterViewModel.Email,
                    Email = model.RegisterViewModel.Email,
                    FirstName = model.RegisterViewModel.FirstName,
                    LastName = model.RegisterViewModel.LastName,
                };

                // Add the Address properties:

                var list = new List<SelectListItem>
                                   {
                                        new SelectListItem{ Text="PayPal", Value = "1" },
                                        new SelectListItem{ Text="BluePay", Value = "2" },
                                        new SelectListItem{ Text="Amazon Pay", Value = "3" },
                                         new SelectListItem{ Text="GoURL", Value = "4"}
                            };

                ViewBag.payment = list;
                var states = db.States.ToList();
                //ViewBag.Location = states.Select(x => new SelectListItem { Text = x.stateName, Value = x.stateID.ToString() }).ToList();
                //var res = db.Doctors.ToList();
                var res = db.Doctors.Where(x => x.zip != null).GroupBy(g => g.zip).Select(x => x.FirstOrDefault()).ToList();
                ViewBag.drpdnlist = res.Select(x => new SelectListItem { Text = x.firstName + x.lastName, Value = x.doctorID.ToString() }).ToList();
                ViewBag.Location = res;
                //ViewBag.Location = res.Select(x => new SelectListItem { Text = x.zip, Value = x.zip }).ToList();


                var result = await UserManager.CreateAsync(user, model.RegisterViewModel.Password);
                dynamic addedResult;
                if (result.Succeeded)
                {
                    SessionHandler.UserName = model.RegisterViewModel.Email;
                    SessionHandler.Password = model.RegisterViewModel.Password;
                    SessionHandler.UserId = user.Id;
                    
                    PatientRepository objRepo = new PatientRepository();
                    Patient obj = new Patient
                    {
                        userId = user.Id,
                        lastName = user.LastName,
                        firstName = user.FirstName,
                        email = user.Email,
                        DoctorId = model.RegisterViewModel.DoctorId,
                        //LocationId =Convert.ToInt32(model.RegisterViewModel.LocationId),
                        LocationName = model.RegisterViewModel.LocationName,
                        PaymentId = model.RegisterViewModel.PaymentId,
                        ReminderId = 1,                        
                    };
                    //addedResult = objRepo.Add(obj);

                    // edit anita
                    Patient patientResult = objRepo.Add(obj);

                    if (patientResult != null)
                    {
                        //ViewBag.SuccessMessage = "Your Account has been created, please login";

                        var sampleEmailBody = @"
                        <h3>Mail From Telemedicine</h3>
                        <p>Your account has been approved by Telemedicine.</p>
                        <p>You can login now.</p>
                        <p>&nbsp;</p>
                        <p><strong>-Best Regards,<br/>Telemedicine</strong></p>
                        ";

                        //var oSimpleEmail = new EmailHelper(obj.email, "Telemedicine Membership", sampleEmailBody);
                        //oSimpleEmail.SendMessage();
                        
                        // edit anita
                        try
                        {
                            FavouriteDoctorModel favdoc = new FavouriteDoctorModel();
                            favdoc.docID = patientResult.DoctorId;
                            favdoc.patID = patientResult.patientID;
                            SeeDoctorController seedoc = new SeeDoctorController();
                            seedoc.AddFavourite(favdoc);

                            // edit anita - 05/08/2019 - 30-08-2019

                            Alert alert = new Alert();
                            alert.alertFor = patientResult.DoctorId;
                            //alert.ReferenceId = Convert.ToInt32(patientResult.patientID);
                            alert.alertRefID = patientResult.patientID;
                            alert.alertText = "Patient Waiting for your Approval";
                            alert.cd = System.DateTime.Now;
                            //alert.md = System.DateTime.Now;
                            //alert.mb = user.Id;
                            alert.cb = user.Id;
                            alert.active = true;
                            db.Alerts.Add(alert);
                            await db.SaveChangesAsync();

                        }
                        catch(Exception ex)
                        {
                            throw ex;
                        }

                        ViewBag.SuccessMessage = "Your Account has been created, You can login after approval of your account.";
                        return View("PatientLogin", model);
                    }
                }
                AddErrors(result);
                foreach (var item in result.Errors)
                {
                    error += item;
                    break;
                }

            }

            // If we got this far, something failed, redisplay form
            //return View("PatientLogin", model);
            TempData["error"] = error;
            ViewBag.ModelError += "\n" + error;
            return Redirect(Url.Action("PatientLogin", "Account") + "#signup");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterDoctor(LoginRegisterViewModel model)
        {
            //if (ModelState.IsValid)
            //{
            var user = new ApplicationUser
            {
                UserName = model.RegisterViewModel.Email,
                Email = model.RegisterViewModel.Email,
                FirstName = model.RegisterViewModel.FirstName,
                LastName = model.RegisterViewModel.LastName,
            };

            // Add the Address properties:



            var result = await UserManager.CreateAsync(user, model.RegisterViewModel.Password);
            dynamic addedResult;
            if (result.Succeeded)
            {
                SessionHandler.UserName = model.RegisterViewModel.Email;
                SessionHandler.Password = model.RegisterViewModel.Password;
                SessionHandler.UserId = user.Id;


                DoctorRepository objRepo = new DoctorRepository();
                Doctor obj = new Doctor
                {
                    userId = user.Id,
                    lastName = user.LastName,
                    firstName = user.FirstName,
                    email = user.Email,
                    status = false
                };
                addedResult = objRepo.Add(obj);

                if (addedResult != null)
                {
                    ViewBag.success = "Your Account has been created, You can login after approval of your account.";
                    //Send Simple Email

                    var sampleEmailBody = @"
                        <h3>Mail From Telemedicine</h3>
                        <p>Your account has been created with Telemedicine successfully.</p>
                        <p>You can login after approval of your account.</p>
                        <p>&nbsp;</p>
                        <p><strong>-Best Regards,<br/>Telemedicine</strong></p>
                        ";

                    var oSimpleEmail = new EmailHelper1(obj.email, "Telemedicine Membership", sampleEmailBody);
                    oSimpleEmail.SendMessage();
                    return View("DoctorLogin", model);
                }
            }
            AddErrors(result);
            //}

            // If we got this far, something failed, redisplay form

            //return View("Login", model);
            return View("DoctorLogin", model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                    user.EmailConfirmed = true;
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    ViewBag.ErrorMessage = "Your Account does't exist, please try again.";
                    return View("ForgotPassword");
                }
                var code = "";
                code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                ForgotPasswordCodeModel.Token = code;

                var callbackUrl = Url.Action("Questions", "Account", new { email = model.Email, code = code }, protocol: Request.Url.Scheme);

                EmailHelper1 oHelper = new EmailHelper1(user.Email, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                oHelper.SendMessage();

                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                return View("ForgotPasswordConfirmationLink");
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Questions(string email, string code)
        {
            var user = await UserManager.FindByNameAsync(email);
            ForgotPasswordCodeModel.Token = code;


            var objModel = new SecretQuestionModel();
            objModel.Email = user.UserName;
            var roles = UserManager.GetRoles(user.Id);
            Random rnd = new Random();
            int caseSwitch = rnd.Next(1, 4);
            if (roles.Contains("Patient"))
            {
                PatientRepository objRepo = new PatientRepository();
                var resultAdd = objRepo.GetByUserId(user.Id);
                switch (caseSwitch)
                {
                    case 1:
                        objModel.SecretQuestion = resultAdd.secretQuestion1;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer1;
                        break;
                    case 2:
                        objModel.SecretQuestion = resultAdd.secretQuestion2;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer2;
                        break;
                    default:

                        objModel.SecretQuestion = resultAdd.secretQuestion3;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer3;
                        break;
                }
                if (objModel.SecretQuestion == null)
                {
                    if (resultAdd.secretQuestion1 != null)
                    {
                        objModel.SecretQuestion = resultAdd.secretQuestion1;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer1;
                    }
                    if (resultAdd.secretQuestion2 != null)
                    {
                        objModel.SecretQuestion = resultAdd.secretQuestion2;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer2;
                    }
                    if (resultAdd.secretQuestion3 != null)
                    {
                        objModel.SecretQuestion = resultAdd.secretQuestion3;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer3;
                    }
                }
            }
            else if (roles.Contains("Doctor"))
            {
                DoctorRepository objRepo = new DoctorRepository();
                var resultAdd = objRepo.GetByUserId(user.Id);
                switch (caseSwitch)
                {
                    case 1:
                        objModel.SecretQuestion = resultAdd.secretQuestion1;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer1;
                        break;
                    case 2:
                        objModel.SecretQuestion = resultAdd.secretQuestion2;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer2;
                        break;
                    default:
                        objModel.SecretQuestion = resultAdd.secretQuestion3;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer3;
                        break;
                }
                if (objModel.SecretQuestion == null)
                {
                    if (resultAdd.secretQuestion1 != null)
                    {
                        objModel.SecretQuestion = resultAdd.secretQuestion1;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer1;
                    }
                    if (resultAdd.secretQuestion2 != null)
                    {
                        objModel.SecretQuestion = resultAdd.secretQuestion2;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer2;
                    }
                    if (resultAdd.secretQuestion3 != null)
                    {
                        objModel.SecretQuestion = resultAdd.secretQuestion3;
                        objModel.SecretAnswerHidden = resultAdd.secretAnswer3;
                    }
                }
            }

            if (objModel.SecretQuestion == null)
            {

                ViewBag.ErrorMessage = "Sorry! User does not setup his recovery secret questions.";

            }
            return View("ForgotPasswordConfirmation", objModel);
        }


        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmationLink()
        {
            return View();
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPasswordConfirmation(SecretQuestionModel model)
        {
            var token = ForgotPasswordCodeModel.Token;

            if (model.SecretAnswerHidden.Trim().ToLower() == model.SecretAnswer.Trim().ToLower())
            {
                var newPassword = "New@Pa9_" + System.Web.Security.Membership.GeneratePassword(10, 4);
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                //newPassword = "Admin@123";//comment when on live

                var result = await UserManager.ResetPasswordAsync(user.Id, token, newPassword);
                if (result.Succeeded)
                {
                    //send email there...
                    EmailHelper1 oHelper = new EmailHelper1(user.Email, "Your password has been reset successfully.", "Your new temporary password is " + newPassword + " Please change your password after login.");
                    oHelper.SendMessage();

                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                AddErrors(result);
            }
            else
            {
                ViewBag.ErrorMessage = "Your Answer does't match, please try again.";
            }
            //ModelState.AddModelError("", "Please enter valid answer!");

            return View("ForgotPasswordConfirmation", model);
        }



        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //




        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {

            //if (SessionHandler.IsExpired)
            //{
            //    var url = Request.Url.OriginalString;
            //    if(url.Contains("SeeDoctor"))
            //    {
            //        actionName = "PatientLogin";
            //        AuthenticationManager.SignOut();
            //        //return RedirectToAction(actionName, "Account");
            //    }
            //    if (url.Contains("Doctor"))
            //    {
            //        actionName = "DoctorLogin";
            //        AuthenticationManager.SignOut();
            //        //return RedirectToAction(actionName, "Account");
            //    }
            //}
            //else
            //{
            var roles = UserManager.GetRoles(SessionHandler.UserId);
            var actionName = "";
            if (roles.Contains("Doctor"))
                actionName = "DoctorLogin";
            else if (roles.Contains("Patient"))
                actionName = "PatientLogin";
            else if (roles.Contains("Admin")) // add anita
                actionName = "AdminLogin";
            AuthenticationManager.SignOut();

            return RedirectToAction(actionName, "Account");

        }



        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            ViewBag.ModelError = "";
            foreach (var error in result.Errors)
            {

                ModelState.AddModelError("", error);
                ViewBag.ModelError += error;
                break;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            // edit anita
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("patientlogin", "account");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
