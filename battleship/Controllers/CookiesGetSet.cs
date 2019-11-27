using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace battleship.Controllers
{
    public class CookiesGetSet
    {

        public static void addCookies(string key, string value, HttpContext context, int expiretime)
        {
           
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(expiretime);
            context.Response.Cookies.Append(key, value, option);

        }

        public static string getCookies(string key, HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey(key))
                return context.Request.Cookies[key];
            else
                return null;

           
        }
        public static bool deleteCookies(string key, HttpContext context)
        {
            if (context.Request.Cookies.ContainsKey(key))
            {
                context.Response.Cookies.Delete(key);
                return true;
            }
            return false;
        }
    }
}
