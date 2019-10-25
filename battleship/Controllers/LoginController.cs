using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BL.Interfaces;
using BL.Services;


namespace battleship.Controllers
{
    public class LoginController : Controller
    {
        ILoginService _ls;
        public LoginController()
        {
            _ls = new LoginService();
        }
        public void addCookies(string name)
        {
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(6);
            HttpContext.Response.Cookies.Append("Login", name, option);

        }

        public string getCookies()
        {
            if (HttpContext.Request.Cookies.ContainsKey("Login"))
                return HttpContext.Request.Cookies["Login"];
            else
                return null;

        }


        public IActionResult Login()
        {
            string name = getCookies();
            if (name != null)
            {
                //service
              string res =  _ls.Login(name);
              if(res!="") return View(res);

            }
            return View();
        }

        public IActionResult Rooms(string username)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signin(string playername)
        {

            if(playername==""&&playername==null)
            {
                ViewBag.errormsg = "Ошибка, попробуйте еще раз!";
                return View("Login");
            }


            string res = _ls.SignIn(playername);
            if(res=="Rooms")
            {
               addCookies(playername);
               ViewBag.playername = playername;
            }

            ViewBag.errormsg = "Игрок с таким именем уже есть на сервере!";
            return View("Login");

        }
    }
}