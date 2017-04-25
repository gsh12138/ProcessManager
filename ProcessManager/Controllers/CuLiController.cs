using System;
using System.Linq;
using System.Web.Mvc;
using ProcessManager.Models;
using ProcessManager.ProcessInterface;
using ProcessManager.Helper;
using ProcessManager.Filters;

namespace ProcessManager.Controllers
{
    [UserFilter]
    public class CuLiController : Controller
    {
        // GET: CuLi
        public ActionResult Index() {

            return View();
        }


        [HttpPost]
        public ActionResult tongyi(ProcessPiZhu model) {
            IBiaoDan biaodan = getBiaoDan(model);
            biaodan.tongyi(model.Detail);
            return View("_success");

        }
        [HttpPost]
        public ActionResult dahui(ProcessPiZhu model) {
            IBiaoDan biaodan = getBiaoDan(model);
            biaodan.dahui(model.Detail);
            return View("_success");
        }

        /// <summary>
        /// 通过查询表单表获取需要处理的表单名再通过反射机制获取表单实例
        /// </summary>
        /// <param name="model"></param>
        /// <returns>表单实例</returns>
        private IBiaoDan getBiaoDan(ProcessPiZhu model) {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                int djh = int.Parse(model.Pid.ToString().Substring(0, 2));
                string url = db.BiaoList.Where(m => m.pid == djh).FirstOrDefault().url;
                GtestUser us = UserHelper.makeUserByidOrName(Session["user"].ToString());
                Type type = Type.GetType("ProcessManager.BiaoDan." + url + "S");
                object[] args = new object[] { us, model.Pid };
                IBiaoDan biaodan = (IBiaoDan)(type.Assembly.CreateInstance(
                                        type.FullName, 
                                        false,
                                        System.Reflection.BindingFlags.Default, null, args,
                                        null, null));
                return biaodan;
            }
        }
    }
}