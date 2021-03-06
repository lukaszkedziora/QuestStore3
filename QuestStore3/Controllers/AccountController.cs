﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestStore3.Data;
using QuestStore3.Models;
using System.Collections.Generic;

namespace QuestStore3.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller

    {
        private readonly QuestContext _context;


        public AccountController(QuestContext context)
        {
            _context = context;
        }


        //[HttpPost] //TODO: spr. atrybuty
        //[ValidateAntiForgeryToken]

        [Route("reg")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            if (ModelState.IsValid)
            {
                var temp = GetHash(user.Password);
                user.Password = temp;
                user.RegistrationDate = DateTime.UtcNow;
                user.Status = Status.Inactive;
                _context.Add(user);
                await _context.SaveChangesAsync();
                
            }
            return Ok(user);
        }

        //public IActionResult RegisterUser()
        //{
        //    return View();
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}
        //public IActionResult AccessDenied()
        //{
        //    return View();
        //}

        //public IActionResult Login()
        //{
        //    return RedirectToAction("Login", "Home");
        //}


        [HttpGet]
        public string[] Get()
        {
            var identityString = new string[3];
            identityString[0] = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            identityString[1] = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            identityString[2] = User.FindFirst(claim => claim.Type == System.Security.Claims.ClaimTypes.SerialNumber)?.Value;

            return identityString;
        }


        [HttpPost]
        public async Task<bool> Post(UserLogin user)

        {
            ClaimsIdentity identity = null;
    
            
            if (ModelState.IsValid)
            {
                var accessDataHashed = GetHash(user.Password);
                user.Password = accessDataHashed;
                var pass = await _context.User
                    .FirstOrDefaultAsync(m => m.Email == user.Email);

                if (pass != null && user.Password == pass.Password && pass.Role == Role.Admin && pass.Status == Status.Active)

                {
                    identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, pass.FirstName),
                        new Claim(ClaimTypes.Role, "Admin"),
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return true;
                }

                if (pass != null && user.Password == pass.Password && pass.Role == Role.Mentor && pass.Status == Status.Active)

                {
                    identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, pass.FirstName),
                        new Claim(ClaimTypes.Role, "Mentor"),
                        new Claim(ClaimTypes.SerialNumber, pass.ID.ToString())
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return true;
                }

                if (pass != null && user.Password == pass.Password && pass.Role == Role.Student && pass.Status == Status.Active)
                {
                    identity = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Name, pass.FirstName),
                        new Claim(ClaimTypes.Role, "Student"),
                        new Claim(ClaimTypes.SerialNumber, pass.ID.ToString())
                    }, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return true;
                }
            }
            return false;
        }

        [HttpOptions]
        public bool Logout()
        {
            var login =  HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //TODO: Redirect to Home
            return true;
        }


        public string GetHash(string password)
        {   //TODO: send salt to database
            byte[] salt = new byte[128 / 8];
            //using (var rng = RandomNumberGenerator.Create())
            //{
            //    rng.GetBytes(salt);
            //}
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            return hashed;
        }



        //[AcceptVerbs("GET", "POST")]

        //public IActionResult VerifyUserName(string UserName)

        //{

        //    _accessData.UserName = UserName;
        //    if (_iAccesData.GetAccessData(_accessData) != null)

        //    {

        //        return Json($"Username {_accessData.UserName} is already in use.");

        //    }



        //    return Json(true);



        //}



    }

}