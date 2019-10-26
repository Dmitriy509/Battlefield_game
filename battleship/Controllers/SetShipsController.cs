using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace battleship.Controllers
{
    public class SetShipsController : Controller
    {
         //   ILoginService _ls;
        public SetShipsController()
        {

          //  _ls = new LoginService();
        }

        public IActionResult Index()
        {




            return View();
        }
    }
}