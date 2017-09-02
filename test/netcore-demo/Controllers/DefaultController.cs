using System;
using Microsoft.AspNetCore.Mvc;

namespace netcore_demo.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
