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
    public class UnableToDeleteViewResult : ViewResult
    {
        //public string ViewName { get; set; }
        //public int StatusCode { get; set; }
        public UnableToDeleteViewResult(string viewName, string message)
        {
            this.ViewName = viewName;
            this.StatusCode = (int)HttpStatusCode.InternalServerError;
            this.ViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new ModelStateDictionary());
            ViewData["Message"] = message;
        }
    }
}
