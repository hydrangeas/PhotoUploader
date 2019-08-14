using System.Collections.Generic;
using System.Linq;
using CVPTest.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CVPTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly JobDatabase jobDatabase;

        public HomeController(JobDatabase _jobDatabase)
        {
            jobDatabase = _jobDatabase;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pageConfigure = new PageConfigure()
            {
                IsHome = true
            };
            ViewData["PageConfigure"] = pageConfigure;

            var model = GetJobList(1);
            var viewModel = new IndexViewModel(model);
            return View(viewModel);
        }

        public List<Job> GetJobList(int page)
        {
            var pageSize = 20;
            var pageNumber = page - 1;

            var jobs = jobDatabase.Jobs
                                  .OrderByDescending(j => j.Modified)
                                  .Skip(pageSize * pageNumber)
                                  .Take(pageSize)
                                  .ToList();
            return jobs;
        }
    }
}
