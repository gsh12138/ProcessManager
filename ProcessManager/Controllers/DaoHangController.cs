using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProcessManager.Controllers
{
    public class DaoHangController : Controller
    {
        // GET: DaoHang
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult logout()
        {
            Session.Remove("user");
            HttpCookie cook = new HttpCookie("liuchenglogin");
            cook.Values["user"] = null;
            cook.Values["pass"] = null;
            cook.Expires = System.DateTime.Now.AddMonths(-1);
            Response.SetCookie(cook);
            return RedirectToAction("Index", "Login");
        }
    }
}