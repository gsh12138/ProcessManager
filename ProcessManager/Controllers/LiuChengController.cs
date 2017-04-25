using System.Web.Mvc;
using ProcessManager.Models;
using ProcessManager.ProcessCaoZuo;
using ProcessManager.DAO;
using ProcessManager.Helper;
using ProcessManager.Filters;
using System.Linq;
using System.Web.Routing;

namespace ProcessManager.Controllers
{
    public class LiuChengController : Controller
    {
        ProcessPiZhuDAO pdao = new ProcessPiZhuDAO();
        ProcessPredefineDAO fdao = new ProcessPredefineDAO();
        // GET: LiuCheng
        [UserFilter]
        [HttpPost]
        public ActionResult Index()
        {
            using(ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                int pid = int.Parse(HttpContext.Request["pid"]);
                int bpid = int.Parse(HttpContext.Request["pid"].Substring(0, 2));
                string url = db.Basice.Where(m => m.pid==pid).FirstOrDefault().nexthanlder;
                Test1Controller test = new Test1Controller();
                return View();
                
                
            }
            
        }

        

        
    }
}