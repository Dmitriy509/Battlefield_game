using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace battleship.Controllers
{
    public class CookiesGetSet
    {

        public static void addCookies(string name, HttpContext context)
        {
           
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(6);
            context.Response.Cookies.Append("Login", name, option);

        }

        public static string getCookies(HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey("Login"))
                return context.Request.Cookies["Login"];
            else
                return null;

        }

    }
}
