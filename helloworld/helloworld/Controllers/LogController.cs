using helloworld.IService;
using helloworld.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace helloworld.Controllers
{
    public class LogController : Controller
    {
        private ILogService logService;
        public LogController()
        {
            this.logService = new LogService();
        }

        public string WriteLog() {
            var result = string.Empty;

            return result;
        }

    }
}