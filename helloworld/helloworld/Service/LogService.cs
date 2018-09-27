using helloworld.IService;
using helloworld.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace helloworld.Service
{
    public class LogService : ILogService
    {
        private string fileName;
        public LogService() {
            fileName = (LogManager.Configuration.FindTargetByName("logfile") as NLog.Targets.FileTarget).FileName.Render(new LogEventInfo() { TimeStamp = DateTime.Now, LoggerName = "loggerName" });
        }

        public void WriteLog(int count = 1)
        {
            for (int i = 0; i <= count; i++)
            {
                var type = (LogType)new Random().Next(1, 2);

                var levelVal = new Random().Next(1, 3);

                var level = LogLevel.Debug;

                switch (levelVal)
                {
                    case 1:
                        level = LogLevel.Error;
                        break;
                    case 2:
                        level = LogLevel.Info;
                        break;
                }

                var content = string.Empty;

                NLogloger.WriteLog(System.Reflection.MethodBase.GetCurrentMethod(), level, type, content);
            }

        }

        public List<ViewLogContent> ReadLog(string dateStr)
        {
            List<ViewLogContent> result = new List<ViewLogContent>();

            var rawStr = string.Empty;
            fileName.Replace(DateTime.Now.ToString("yyyy-MM-dd"), dateStr);

            using (StreamReader sr = new StreamReader(fileName))
            {
                int i = 0;
                JObject obj = new JObject();
                while ((rawStr = sr.ReadLine()) != null)
                {
                    obj = new JObject();

                    var sp = rawStr.Split('[');
                    obj.Add(new JProperty("CreatedTime", rawStr.Substring(0, 19)));
                    obj.Add(new JProperty("Raw", i.ToString()));
                    foreach (var item in sp)
                    {
                        var index = item.IndexOf("]");
                        if (index < 1) continue;
                        var front = item.Substring(0, index);
                        var back = item.Substring(index + 1, item.Length - index - 1);
                        obj.Add(new JProperty(front, back));
                    }

                    string jsonString = JsonConvert.SerializeObject(obj);
                    result.Add(JsonConvert.DeserializeObject<ViewLogContent>(jsonString));
                    i++;
                }
                sr.Close();
            }

            return result;

        }
    }
}