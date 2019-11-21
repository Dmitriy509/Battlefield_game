using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using BL.Interfaces;
using BL.Services;
using DL.Enums;

namespace battleship.Controllers
{
    public class LoginController : Controller
    {
        ILoginService _ls;
        private readonly ILogger _logger;
        public LoginController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("MyApp");
            _ls = new LoginService(_logger);
        }

        public IActionResult Login()
        {
          
            
            string name = CookiesGetSet.getCookies(HttpContext);
            if (name != null)
            {
                //service
              string res =  _ls.Login(name);
              if(res!="") return Redirect(res);

            }
            return View();
        }


        [HttpPost]
        public ActionResult Signin(string playername)
        {
            if (playername==""||playername==null)
            {
                ViewBag.errormsg = "Введите имя";
                return View("Login");
            }
            if(playername.Length>10)
            {
                ViewBag.errormsg = "Имя должно быть не длиннее 10 символов";
                return View("Login");
            }

            string res = _ls.SignIn(playername);
            if(res=="Rooms")
            {
                CookiesGetSet.addCookies(playername, HttpContext, Parameters.KeepLoginCokies);
                ViewBag.playername = playername;
               //   return RedirectToAction("Rooms", "Rooms");
                return Redirect("~/Rooms/Rooms");
                
               // return Redirect("Rooms");
            }

            ViewBag.errormsg = "Игрок с таким именем уже есть на сервере!";
            return View("Login");

        }

        [HttpPost]
        public ActionResult Signout()
        {
            _ls.SignOut(CookiesGetSet.getCookies(HttpContext));
            CookiesGetSet.deleteCookies(HttpContext);       
            return Redirect("Login");
        }

        public ActionResult ErrorNewTab()
        {
            return View("errorNewTab");
        }


    }
}