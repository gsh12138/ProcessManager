using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessManager.Helper;

namespace ProcessManager.Filters
{
    public class UserFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
            string user = filterContext.Controller.ControllerContext.HttpContext.Session["user"].ToString();
            GtestUser us = UserHelper.makeUserByidOrName(user);
            filterContext.Controller.ViewBag.user = us.userxm;
            filterContext.Controller.ViewBag.testuser = us;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
            if (filterContext.Controller.ControllerContext.HttpContext.Session["user"] == null)
            {
                
                filterContext.Result = new RedirectResult("/Login/Index");
                return;
            }
            GtestUser us = UserHelper.makeUserByidOrName(
                filterContext.Controller.ControllerContext.HttpContext.
                Session["user"].ToString());
            filterContext.Controller.ViewData.Add("Puser", us);
        }
    }
}