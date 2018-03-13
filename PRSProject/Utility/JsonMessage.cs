using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace PRSProject.Utility
{
    public class JsonMessage
    {
        public string Result { get; set; }
        public string Message { get; set; }

        public JsonMessage(string result, string message)
        {
            this.Result = result;
            this.Message = message;
        }
    }      
}
