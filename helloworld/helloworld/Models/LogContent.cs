using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace helloworld.Models
{
    public class LogContent
    {
        public LogContent(Guid? guid) {
            this.Id = guid ?? Guid.NewGuid();
        }

        [JsonProperty("Id")]
        public Guid Id { get; set; }

        [JsonProperty("Level")]
        public LogLevel Level { get; set; }

        [JsonProperty("Type")]
        public LogType Type { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("Status")]
        public LogStatus Status { get; set; }
    
        //public DateTime CreatedTime { get; set; }
    }
}