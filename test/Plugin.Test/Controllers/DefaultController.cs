using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Plugin.Test.Models;

namespace Plugin.Test.Controllers
{
    [Area("admin")]
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            var model = new Person { FullName = "Quaider Zhang", Age = 30 };
            return View(model);
        }
    }
}