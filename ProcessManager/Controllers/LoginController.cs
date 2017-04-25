using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessManager.Models;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ProcessManager.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public async Task<ActionResult> Index()
        {
            
            if (Request.Cookies["liuchenglogin"] == null)
            {
                return View();
            }
            ProcessUserModel user = new ProcessUserModel();
            user.user = Request.Cookies["liuchenglogin"].Values["user"];
            user.password = Request.Cookies["liuchenglogin"].Values["pass"];
            user.RememberMe = true;
            return await this.Index(user);


        }

        [HttpPost]
        public async Task<ActionResult> Index(ProcessUserModel model)
        {
            List<ModelErr> mer = new List<ModelErr>();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ProcessUserModel user = null;
            using(TJZHEntities db=new TJZHEntities())
            {
                var selectstring = from u in db.GtestUser
                                   where u.username == model.user
                                   select u;
                var aus = await selectstring.ToListAsync();
                if (aus.Count == 0)
                {
                    ModelErr m = new ModelErr();
                    m.ziduan = "user";
                    m.xinxi = "无此用户";
                    mer.Add(m);
                    ViewBag.modelerr = mer;
                    ModelState.AddModelError("", "无此用户");
                    return View(model);
                }
                ProcessUserModel us = new ProcessUserModel();
                us.user = aus.First().username;
                us.password = aus.First().password;
                us.xingming = aus.First().userxm;
                user = us;
            }
            
            if (!user.user.Equals(model.user))
            {
                ModelErr m = new ModelErr();
                m.ziduan = "user";
                m.xinxi = "无效的用户名";
                mer.Add(m);
                ViewBag.modelerr = mer;
                ModelState.AddModelError("", "无效的用户名");
                return View(model);
            }
            if (!user.password.Equals(model.password))
            {
                ModelErr m = new ModelErr();
                m.ziduan = "pass";
                m.xinxi = "密码错误";
                mer.Add(m);
                ViewBag.modelerr = mer;
                ModelState.AddModelError("", "密码错误");
                return View(model);
            }

            if (model.RememberMe)
            {
                HttpCookie cook = new HttpCookie("liuchenglogin");
                cook.Values["user"] = model.user;
                cook.Values["pass"] = model.password;
                Response.AppendCookie(cook);
            }
            Session.Timeout = 10;
            Session["user"] = model.user;
            return RedirectToAction("Index", "BiaoList");
        }
    }

    public class ModelErr
    {
        public string ziduan { get; set; }
        public string xinxi { get; set; }
    }
}