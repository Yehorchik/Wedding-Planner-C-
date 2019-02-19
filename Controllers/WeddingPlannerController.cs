using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class WeddingPlannerController : Controller
    {
        private WeddingPlannerContext dbContext;
        public WeddingPlannerController(WeddingPlannerContext context)
        {
            dbContext = context;
        }


        [HttpGet("")]
        public IActionResult Index()
        {
            return View("index");
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View("login");
        }
        [HttpGet("newWedding")]
        public IActionResult NewWEdding()
        { 
            int? userId = HttpContext.Session.GetInt32("userId");
            ViewBag.Id = userId;
            return View("newwedding");
        }

        [HttpGet("info/{wedId}")]
        public IActionResult WeddingInfo(int wedId)
        {
           Wedding One = dbContext.weddings
                            .Include(p => p.Guest)
                            .ThenInclude(u => u.User)
                            .FirstOrDefault(p => p.WeddingId == wedId);
           ViewBag.Wedding = One;
           return View("wedinfo");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            var id = HttpContext.Session.GetInt32("userId");
            if(id == null)
            {
               ModelState.AddModelError("Email", "Please Log In");
               return View("login");
            }
            List<Wedding> AllWeddings = dbContext.weddings.Include(p => p.Guest).ThenInclude(u => u.User).ToList();
            ViewBag.Weddings = AllWeddings;
            ViewBag.MainId=id;
            var all = dbContext.guests.ToList();
            ViewBag.AllGuests = all;
            return View("dashboard");
        }


        [HttpPost("register")]
        public IActionResult SignUp(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                string email = user.Email;
                HttpContext.Session.SetInt32("userId" ,  user.UserId);
                return Redirect("dashboard");
            }
            else
            {
                return View("index");
            }

        }

        [HttpPost("confirm")]
        public IActionResult Confirm(LoginUser userSubmission)
        {    
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.users.FirstOrDefault(u => u.Email == userSubmission.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email");
                    return View("login");
                }             
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Password" , "Invalid Password");
                    return View("login");
                }
                HttpContext.Session.SetInt32("userId" ,  userInDb.UserId);
                return Redirect("dashboard ");
            }
            else
            {
                return View("login");
            }
        }
        
        [HttpPost("createWedding")]
        public IActionResult CreateWedding(Wedding wedding)
        {
            if(ModelState.IsValid)
            {
                dbContext.Add(wedding);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("newwedding");
            }
        }
        [HttpGet("deletewed/{wedId}")]
        public IActionResult DeleteWedding(int wedId)
        {
            Wedding One = dbContext.weddings.FirstOrDefault(p => p.WeddingId == wedId);
            dbContext.Remove(One);  
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpPost("addguest")]
        public IActionResult AddGuest(Guest guest)
        {
            dbContext.Add(guest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost("removeguest")]
        public IActionResult RemoveGuest(Guest guest)
        {
            dbContext.Remove(guest);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

    }
}
