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
          
            
            string player_id = CookiesGetSet.getCookies("Player_Id",HttpContext);
            if (player_id != null)
            {
                //service
              string res =  _ls.Login(player_id);
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

            uint player_id;
            string res = _ls.SignIn(playername, out player_id);
            if(res=="Rooms")
            {
                CookiesGetSet.addCookies("Login", playername, HttpContext, Parameters.KeepLoginCokies);
                CookiesGetSet.addCookies("Player_Id", player_id.ToString(), HttpContext, Parameters.KeepLoginCokies);
                // ViewBag.playername = playername;
                //   return RedirectToAction("Rooms", "Rooms");
                _logger.LogInformation("Player_Id: " + player_id + ", Player '" + playername + "'" + " log in");
                return Redirect("~/Rooms/Rooms");
                
               // return Redirect("Rooms");
            }

            ViewBag.errormsg = "Игрок с таким именем уже есть на сервере!";
            return View("Login");

        }

        [HttpPost]
        public ActionResult Signout()
        {
            string playername = CookiesGetSet.getCookies("Login", HttpContext);
            string player_id = CookiesGetSet.getCookies("Player_Id", HttpContext);
            _ls.SignOut(player_id);
            CookiesGetSet.deleteCookies("Player_Id",HttpContext);
            CookiesGetSet.deleteCookies("Login", HttpContext);
            _logger.LogInformation("Player_Id: " + player_id + ", Player '" + playername + "'" + " log out");
            return Redirect("Login");
        }

        public ActionResult ErrorNewTab()
        {
            string player_id = CookiesGetSet.getCookies("Player_Id", HttpContext);
            _logger.LogInformation("Player_Id: " + player_id+", Try to open a new tab");
            return View("errorNewTab");
        }


    }
}