using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace internetiot.Models
{
    public class ForbiddenViewResult : ViewResult
    {
        //public string ViewName { get; set; }
        //public int StatusCode { get; set; }
        public ForbiddenViewResult(string viewName, string message = "Forbidden")
        {
            this.ViewName = viewName;
            this.StatusCode = (int)HttpStatusCode.Forbidden;
            this.ViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new ModelStateDictionary());
            ViewData["Message"] = message;
        }
    }
}
