using Portal.Models;
using SampleMVC.Data.Entities;
using SampleMVC.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class HomeController : Controller
    {
        public const int PageSize = 1000;
        private IMyRepository _repository { get; set; }
        public HomeController(IMyRepository repository)
        {
            _repository = repository;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult People()
        {
            PeopleModel people = new PeopleModel();
            people.People = _repository.GetPeople().Take(10000).Select(p => new PersonModel() { Id = p.Id, FirstName = p.FirstName, LastName = p.LastName, MiddleName = p.MiddleName }).ToList();
            return View(people);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> People (PeopleModel model)
        {
            var fileInfo = new FileInformation();
            _repository.SavePeople(model.Upload.InputStream, model.Upload.FileName, out fileInfo);

            model.People = _repository.GetPeople().Take(10000).Select(p => new PersonModel() { Id = p.Id, FirstName = p.FirstName, LastName = p.LastName, MiddleName = p.MiddleName }).ToList();
            return View(model);
        }

        [HttpPost]
        public string PercentDone(string fileName)
        {
            var splitname = fileName.Split('\\').Last();
            var fileInfo = _repository.GetFileInfo(splitname);
            return fileInfo?.PercentSaved?.ToString();
        }
    }
}