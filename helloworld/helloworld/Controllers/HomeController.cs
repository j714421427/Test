using helloworld.IService;
using helloworld.Models;
using helloworld.Service;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace helloworld.Controllers
{
    public class HomeController : Controller
    {
        private ILogService logService;
        public HomeController() {
            this.logService = new LogService();
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

        public ActionResult Adult()
        {
            ViewBag.Message = "Your test page.";
            Info result = new Info("new");
            Console.WriteLine(result.Name);
            result.ChangeName("cool");
            Console.WriteLine(result.Name);
            return View(result);
        }

        public ActionResult Game()
        {
            string domainName = System.Web.HttpContext.Current.Request.Url.Host;

            Info result = new Info("Game");
            result.Url = domainName;

            return View(result);
        }
        

        public ActionResult Log()
        {
            ViewBag.Message = "Your log page.";

            return View("Log", logService.ReadLog(""));
        }

        [HttpPost]
        public ActionResult Query(string date)
        {

            return View("Log", logService.ReadLog(date));
        }

        public ActionResult JieLinEdit()
        {
            return View();
        }
    }
}