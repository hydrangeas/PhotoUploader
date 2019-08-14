using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVPTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVPTest.Controllers
{
    public class UploadController : Controller
    {
        // GET: /Upload/
        public IActionResult Index()
        {
            var pageConfigure = new PageConfigure()
            {
                IsUpload = true
            };
            ViewData["PageConfigure"] = pageConfigure;

            return View();
        }
    }
}
