using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PRSProject.Controllers
{
    public static class ModelStateErrors
    {

        public static IEnumerable<string> GetModelStateErrors(ModelStateDictionary modelState)
        {
            var keys = modelState.Keys.ToList();
            var values = modelState.Values.ToList();
            var errorMessages = new List<string>();
            for (var idx = 0; idx < values.Count; idx++)
            {
                if (values[idx].Errors.Count > 0)
                {
                    var key = keys[idx];
                    var value = values[idx];
                    errorMessages.Add($"{key} has an error with value of [{value.Value.AttemptedValue ?? "null"}]");
                }
            }
            return errorMessages;
        }
    }
}
