using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessManager.ProcessInterface;

namespace ProcessManager.Filters
{
    public class VerificationFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext) {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) {

        }
    }
}