﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace helloworld.Models
{
    public class Info
    {
        public Info(string  name) {
            this.Name = name;
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public void ChangeName(string name)
        {
            this.Name = name;
        }
    }
}