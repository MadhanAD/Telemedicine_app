using DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helper;
using WebApp.Models;
using WebApp.Repositories.DoctorRepositories;

namespace WebApp.Controllers
{
    //[PatientSessionExpire]
    //[Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        TeleMedEntities db = new TeleMedEntities();

        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PatientForApproval()
        {
            try
            {
                long UserId = SessionHandler.UserInfo.Id;
                var doc = db.Patients.Where(x=>x.active==false && x.DoctorId==UserId).ToList();
                
                return View(doc);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult ApprovePatient(FormCollection collection)
        {
            try
            {
                long UserId = SessionHandler.UserInfo.Id;
                long id = Convert.ToInt64(Request.Form["id"].ToString());
                var email = Request.Form["email"].ToString();
                Patient doc = db.Patients.Where(d => d.patientID == id && d.active == false ).FirstOrDefault();
                doc.active = true;
                doc.mb = doc.patientID.ToString();
                doc.md = System.DateTime.Now;
                db.Entry(doc).State = EntityState.Modified;
                db.SaveChangesAsync();


                // edit anita - (30-08-2019)
                Alert alert = new Alert();
                alert = db.Alerts.Where(al => al.alertFor == UserId && al.alertRefID == id).FirstOrDefault();
                if (alert != null)
                {
                    alert.alertFor = UserId;
                    alert.alertText = "Patient Waiting for your Approval";
                    alert.alertRefID = id;
                    alert.cd = System.DateTime.Now;
                    alert.cb = db.Doctors.Where(p => p.doctorID == UserId && p.active == true).Select(pt => pt.userId).FirstOrDefault();
                    alert.active = true;
                    alert.read = true;
                    alert.mb = db.Doctors.Where(p => p.doctorID == UserId && p.active == true).Select(pt => pt.userId).FirstOrDefault();
                    alert.md = System.DateTime.Now;
                    db.Entry(alert).State = EntityState.Modified;
                    db.SaveChangesAsync();
                }

                FavouriteDoctor favdoc = new FavouriteDoctor();
                favdoc = db.FavouriteDoctors.Where(fav => fav.doctorID == UserId && fav.patientID == id && fav.active == false).FirstOrDefault();
                if (favdoc != null)
                {
                    favdoc.active = true;
                    favdoc.doctorID = UserId;
                    favdoc.patientID = id;
                    favdoc.mb = id.ToString();
                    favdoc.md = System.DateTime.Now;
                    db.Entry(favdoc).State = EntityState.Modified;
                    db.SaveChangesAsync();                                    
                }
                else
                {
                    favdoc = new FavouriteDoctor();
                    favdoc.active = true;
                    favdoc.doctorID = UserId;
                    favdoc.patientID = id;
                    favdoc.mb = id.ToString();
                    favdoc.md = System.DateTime.Now;
                    db.FavouriteDoctors.Add(favdoc);
                    db.SaveChangesAsync();
                }


                var sampleEmailBody = @"
                        <h3>Mail From Telemedicine</h3>
                        <p>Your account has been approved by Telemedicine.</p>
                        <p>You can login now.</p>
                        <p>&nbsp;</p>
                        <p><strong>-Best Regards,<br/>Telemedicine</strong></p>
                        ";

                var oSimpleEmail = new EmailHelper1(email, "Telemedicine Membership", sampleEmailBody);
                oSimpleEmail.SendMessage();
                var reloaddoc = db.Patients.Where(x => x.active == false && x.DoctorId == UserId);
                return View("PatientForApproval", reloaddoc);
                
            }
            catch (Exception ex)
            {
                return RedirectToAction("PatientForApproval");
            }
        }



    }
}