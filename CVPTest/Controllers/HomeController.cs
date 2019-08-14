using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVPTest.Common;
using CVPTest.Models;
using Microsoft.AspNetCore.Mvc;

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

        [AjaxOnly]
        [RequireReferrer("/home/index", "/")]
        [HttpPost]
        [ActionName("d")]
        public async Task<IActionResult> d(int id)
        {
            await DeleteJobAsync(id);

            var model = GetJobList(1);
            var viewModel = new IndexViewModel(model);
            return PartialView(U.PartialViews.ListOfJobs, viewModel);
        }

        public async Task DeleteJobAsync(int id)
        {
            var job = jobDatabase.Jobs
                                 .Where(j => j.Id == id)
                                 .FirstOrDefault();
            if (job == null)
                return;

            jobDatabase.Jobs.Remove(job);

            try
            {
                await jobDatabase.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
                //TODO: エラー復帰方法を考える
            }
        }
    }
}
