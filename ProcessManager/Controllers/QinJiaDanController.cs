using System.Web.Mvc;
using ProcessManager.Filters;
using ProcessManager.Helper;
using ProcessManager.BiaoDan;
using ProcessManager.Models;

namespace ProcessManager.Controllers
{
    [UserFilter]
    public class QinJiaDanController : Controller
    {
        private GtestUser us;
        // GET: QinJiaDan
        
        [HttpGet]
        public ActionResult Index(int id=0)
        { 
            //us = UserHelper.makeUserByidOrName(Session["user"].ToString());
            us= ViewData["Puser"] as GtestUser;
            if (id != 0) {
                QinJiaDanS idan = new QinJiaDanS(us, id);
                QingJiaDanModel imodel = idan.getModel();
                return View(imodel);
            }
            QinJiaDanS dan = new QinJiaDanS(us);
            QingJiaDanModel model = dan.getModel();

            return View(model);
        }
        

        [HttpPost]
        [VerificationFilter]
        public ActionResult Index(QingJiaDanModel model) {
            //us = UserHelper.makeUserByidOrName(Session["user"].ToString());
            us = ViewData["Puser"] as GtestUser;
            QinJiaDanS dan = new QinJiaDanS(us, model);
            if (!dan.yanZheng(this)) {
                return View(model);
            }
            dan.faqi();
            return View("_success");
        }

        

        


    }
}