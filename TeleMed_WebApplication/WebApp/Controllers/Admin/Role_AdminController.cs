﻿using DataAccess;
using Identity.Membership;
using Identity.Membership.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helper;

namespace TeleMed.Controllers
{
    [AdminSessionExpire]
    [Authorize(Roles = "Admin")]
    public class Role_AdminController : Controller
    {
        TeleMedEntities db = new TeleMedEntities();
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

        public ActionResult Create()
        {
            if (Session["LogedUserID"] != null)
            {

                try
                {
                    var role = db.AspNetRoles.ToList();
                    return View(role);

                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = "Error occurred while loading data.";
                    return View();
                }
            }
            else
            {

                return RedirectToAction("AdminLogin", "Account");
            }


        }

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> Create(FormCollection collection)
        {
            if (Session["LogedUserID"] != null)
            {
                var rolename = "";
                var desc = "";
                var roleid = "";
                ViewBag.successMessage = "";
                ViewBag.errorMessage = "";
                try
                {
                    var action = Request.Form["action"].ToString();
                    if (action == "create")
                    {
                        rolename = Request.Form["rolename"].ToString();
                        desc = Request.Form["desc"].ToString();
                        
                        var role= new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));
                        if (!(role.RoleExists(rolename)))
                        {
                            var rm = new RoleManager<ApplicationRole>(

                                                        new RoleStore<ApplicationRole>(new ApplicationDbContext()));

                            var idResult = rm.Create(new ApplicationRole(rolename));
                            if(idResult.Succeeded)
                            {
                                AspNetRole thisRole = db.AspNetRoles.Where(r => r.Name.Equals(rolename, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                                thisRole.Description = desc;
                                db.Entry(thisRole).State= EntityState.Modified;
                                db.SaveChanges();
                            }
                            
                            ViewBag.successMessage = "Record has been saved successfully";
                            ViewBag.errorMessage = "";
                        }
                        else
                        {
                            ViewBag.errorMessage = "Role already exists.";
                            ViewBag.successMessage = "";
                        }
                            

                    }
                    if (action == "edit")
                    {
                        //roleid = Request.Form["id"].ToString();
                        rolename = Request.Form["rolename"].ToString();
                        desc = Request.Form["desc"].ToString();
                        AspNetRole thisRole = db.AspNetRoles.Where(r => r.Name.Equals(rolename, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        thisRole.Description = desc;
                        thisRole.Name = rolename;
                        db.Entry(thisRole).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.successMessage = "Record has been saved successfully";
                        ViewBag.errorMessage = "";
                        var _existingroleList = db.AspNetRoles.ToList();
                        return View(_existingroleList);
                    }
                    if (action == "delete")
                    {
                        roleid = Request.Form["id"].ToString();
                        AspNetRole thisRole = db.AspNetRoles.Where(r => r.Id.Equals(roleid, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                        db.AspNetRoles.Remove(thisRole);
                        db.SaveChanges();
                        ViewBag.successMessage = "Record has been deleted successfully";
                        ViewBag.errorMessage = "";
                    }
                    var __existingroleList = db.SP_SelectRole();
                    return View(__existingroleList);

                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage = "Error occurred while processing your request.";
                    var _existingroleList = db.SP_SelectRole();
                    return View(_existingroleList);
                }
            }
            else
            {
                return RedirectToAction("AdminLogin", "Account");
            }
        }

    }
}

