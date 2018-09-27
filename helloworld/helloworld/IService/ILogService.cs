using helloworld.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace helloworld.IService
{
    public interface ILogService
    {
        void WriteLog(int count = 1);

        List<ViewLogContent> ReadLog(string dateStr);
    }
}