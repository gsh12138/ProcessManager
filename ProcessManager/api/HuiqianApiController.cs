using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessManager.DAO;
using ProcessManager.Models;

namespace ProcessManager.api
{
    public class HuiqianApiController : Controller
    {
        // GET: HuiqianApi
        public JsonResult Index(int pid,int order)
        {
            ProcessHuiQianDAO hdao = new ProcessHuiQianDAO();
            List<ProcessHuiQian> lhuiqian = hdao.selectByIdOrder(pid, order);
            string test = Json(lhuiqian).ToString();

            return Json(lhuiqian,JsonRequestBehavior.AllowGet);
        }
    }
}