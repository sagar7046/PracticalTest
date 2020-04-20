using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Test.Models;
using Test.Helpers;
using System.Security.Cryptography;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Test.Data;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostingEnvironment hosting;
        private readonly Context dbcontext;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment _hosting, Context context)
        {
            _logger = logger;
            hosting = _hosting;
            dbcontext = context;
        }

        [AuthUser]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SaveDetail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveDetail(UserViewModel user)
        {
            if (ModelState.IsValid)
            {

                var newuser = new UserDetail();
                newuser.MobileNumber = user.MobileNumber;
                newuser.Username = user.Username;
                newuser.Password = user.Password;

                var profile = Path.GetFileName(user.ProfileImage.FileName);
                newuser.ProfileImage = Path.GetFileNameWithoutExtension(profile) + "_" + Guid.NewGuid().ToString().Substring(0, 6) + Path.GetExtension(profile);
                user.ProfileImage.CopyToAsync(new FileStream(Path.Combine(Path.Combine(hosting.WebRootPath, "uploads/img"), newuser.ProfileImage), FileMode.Create));

                var resume = Path.GetFileName(user.Resume.FileName);
                newuser.Resume = Path.GetFileNameWithoutExtension(resume) + "_" + Guid.NewGuid().ToString().Substring(0, 6) + Path.GetExtension(resume);
                user.Resume.CopyToAsync(new FileStream(Path.Combine(Path.Combine(hosting.WebRootPath, "uploads/doc"), newuser.Resume), FileMode.Create));

                dbcontext.User.Add(newuser);
                dbcontext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Operation()
        {
            var users= dbcontext.User.ToList();
            return View(users);
        }

        public IActionResult View(int id)
        {
            var model=dbcontext.User.FirstOrDefault(a=>a.Id==id);
            return PartialView("ViewUser",model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if(id!=null)
            {
                var model=dbcontext.User.FirstOrDefault(m=>m.Id==id);
                var user=new UserViewModel();
                user.Username=model.Username;
                user.MobileNumber=model.MobileNumber;
                user.Password=model.Password; 
                
                @ViewBag.Photo=model.ProfileImage;
                @ViewBag.Resume=model.Resume;               

                return View(user);
            }            
            else{
                return RedirectToAction("Operation");
            }            
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel model,int id)
        {            
            var user=dbcontext.User.FirstOrDefault(s=>s.Id==id);
            user.Username=model.Username;
            user.MobileNumber=model.MobileNumber;
            user.Password=model.Password;

            if(model.ProfileImage!=null)
            {
                var profile = Path.GetFileName(model.ProfileImage.FileName);
                user.ProfileImage = Path.GetFileNameWithoutExtension(profile) + "_" + Guid.NewGuid().ToString().Substring(0, 6) + Path.GetExtension(profile);
                model.ProfileImage.CopyToAsync(new FileStream(Path.Combine(Path.Combine(hosting.WebRootPath, "uploads/img"), user.ProfileImage), FileMode.Create));
            }            

            if(model.Resume!=null)
            {
                var resume = Path.GetFileName(model.Resume.FileName);
                user.Resume = Path.GetFileNameWithoutExtension(resume) + "_" + Guid.NewGuid().ToString().Substring(0, 6) + Path.GetExtension(resume);
                model.Resume.CopyToAsync(new FileStream(Path.Combine(Path.Combine(hosting.WebRootPath, "uploads/doc"), user.Resume), FileMode.Create));
            }

            dbcontext.User.Update(user);
            dbcontext.SaveChanges();
            return RedirectToAction("Operation");         
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserViewModel model)
        {
            var user=dbcontext.User.FirstOrDefault(m=>m.Username==model.Username && m.Password==model.Password);
            if(user!=null)
            {
                TempData["user"]=user.Username;
                HttpContext.Session.SetString("user",user.Username);
                return RedirectToAction("Index");
            }
            else{
                TempData["Message"]="User not found";
                return RedirectToAction("Login");
            }            
        }

        public IActionResult Logout()
        {            
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user=dbcontext.User.Find(id);
            dbcontext.User.Remove(user);
            await dbcontext.SaveChangesAsync();            
            return RedirectToAction("Operation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
