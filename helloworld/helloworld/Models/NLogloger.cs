using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace helloworld.Models
{
    public class NLogloger
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();


        public static void WriteLog(MethodBase method, LogLevel level, LogType type, string content )
        {
            string result = string.Empty;
            string methodNmae = GetCurrentMethodInfo(method);

            var log = new LogContent(null)
            {
                Level = level,
                Status = level == LogLevel.Error ? LogStatus.Pending : LogStatus.None,
                Type = type,
                Content = content,
            };
            

            result = CreateLogStr(log);

            logger.Log(level, result);

        }

        private static string GetCurrentMethodInfo(MethodBase method)
        {
            //取得當前方法類別命名空間名稱 取得當前類別名稱 取得當前所使用的方法
            return $"[Method:{method.DeclaringType.Namespace}.{method.DeclaringType.FullName}.{method.Name}]";
        }

        private static string CreateLogStr(object content)
        {

            var props = content.GetType().GetProperties()
                .Where(o => o.GetValue(content) != null);

            string[] signArray = props
                .Select(o => $"[{o.CustomAttributes.Single().ConstructorArguments.Single().Value}]{o.GetValue(content)}").ToArray();

            return string.Join("",signArray);
        }
    }
}