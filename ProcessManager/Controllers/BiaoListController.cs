using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProcessManager.Models;
using ProcessManager.Helper;
using ProcessManager.Filters;

namespace ProcessManager.Controllers
{
    public class BiaoListController : Controller
    {
        // GET: BiaoList
        [UserFilter]
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                string su = Session["user"].ToString();
                GtestUser us = UserHelper.makeUserByidOrName(su);
                List<BiaoList> blist = null;
                ViewBag.user = us.userxm;
                ViewBag.userid = us.userid;
                ViewBag.userbm = us.bumen;
                using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
                {
                    blist = db.BiaoList.ToList();
                }
                List<BiaoListModel> list = new List<BiaoListModel>();
                blist.ForEach(m =>
                {
                    BiaoListModel model = new BiaoListModel();
                    model.name = m.name;
                    model.detil = m.detil;
                    model.pid = (int)m.pid;
                    model.url = m.url;
                    list.Add(model);
                });
                ListBiaoListModel listmodel = new ListBiaoListModel();
                listmodel.list = list;
                
                return View(listmodel);
            }
            return View("../Login/Index");
        }
    }
}