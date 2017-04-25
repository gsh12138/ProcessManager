using System;
using System.Linq;
using System.Web.Mvc;
using ProcessManager.Models;
using ProcessManager.Helper;
using ProcessManager.ProcessCaoZuo;
using ProcessManager.Filters;
using ProcessManager.ProcessInterface;
using System.Collections.Generic;

namespace ProcessManager.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        /// <summary>
        /// 显示流程页面
        /// </summary>
        /// <param name="id">
        /// wfq:我发起的流程
        /// xsh:需审核的流程
        /// </param>
        /// <returns></returns>
        [UserFilter]
        public ActionResult Index(string id)
        {
            if (Session["user"] != null)
            {
                string su = Session["user"].ToString();
                MakeModelsHelper make = new MakeModelsHelper();
                GtestUser us = null;
                us = UserHelper.makeUserByidOrName(su);
                if (id.Equals("wbc"))
                {
                    BaoCunBiaoDan lbc = new BaoCunBiaoDan();
                    using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
                    {
                        List<BaoCun> lb = db.BaoCun.Where(m => m.bcr.Equals(us.username) &&
                        m.state.Equals("weichuli")).ToList();
                        List<BaoCunBiaoDan> lbd = new List<BaoCunBiaoDan>();
                        lb.ForEach(m => {
                            BaoCunBiaoDan bc = new BaoCunBiaoDan();
                            bc.bid = (int)m.bid;
                            int bbid = int.Parse(m.bid.ToString().Substring(0, 2));
                            bc.leixing = Enum.GetName(typeof(BiaoLeiXing), bbid);
                            bc.url = db.Basice.Where(c => c.pid == bbid).FirstOrDefault().nexthanlder;
                            lbd.Add(bc);
                        });
                        lbc.lbcb = lbd;
                    }
                    return View("BaoCunBiaoDan",lbc);
                }
                ProcessListInfo linfo = make.makeListInfo(us,id);
                return View(linfo);
            }

            return View("../Login/Index");
            
        }
        /// <summary>
        /// GET请假单
        /// </summary>
        /// <param name="id">
        /// 1:发起请假单
        /// 单号:查看请假单
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [UserFilter]
        public ActionResult Testdao(int id)
        {

            if (id==1)
            {
                ViewBag.lout = "~/Views/Test/_Biaodan.cshtml";   //发起单据时的母板
                ViewBag.readol = "false";           //可否修改表单项 false:可以修改 true:不可以修改
                return View();
            }
            else {
                /*ViewBag.lout = "_Testqiantao.cshtml";   //正常审核单据时的母板
                ViewBag.readol = "true";*/
                using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
                {
                    //查询请假单
                    
                    QingjiaDan qing = db.QingjiaDan.Where(m=>m.bid==id).FirstOrDefault();
                    GtestUser us = UserHelper.makeUserByidOrName(Session["user"].ToString());
                    //----------------------------------------------------
                    //数据库MODEL转化为页面MODEL
                    if (qing != null)
                    {
                        
                        if (qing.pid == null)
                        {
                            ViewBag.lout = "~/Views/Test/_Biaodan.cshtml";
                            ViewBag.readol = "false";
                        }
                        else
                        {
                            int pid = (int)qing.pid;
                            Processing process = Processing.processingFactory(pid, us.userxm);
                            ViewStateModel vsm = MakeViewStateHelper.makeViewState(process, us);
                            ViewBag.lout = vsm.layout;
                            ViewBag.readol = vsm.read;
                            if (vsm.layout.Equals("~/Views/Test/_Testqiantao.cshtml"))
                            {
                                ViewBag.yjpid = qing.bid;
                                ViewBag.canshen = "true";
                                ViewBag.burl = "/Test";
                            }
                        }
                        QingJiaDanModel model = new QingJiaDanModel();
                        model.bid = (int)qing.bid;
                        model.detil = qing.detil;
                        model.finishtime = ((DateTime)qing.finishtime).ToString("d");
                        model.startime = ((DateTime)qing.startime).ToString("d");
                        model.leixing = (QingJiaLeiXing)Enum.Parse(typeof(QingJiaLeiXing), qing.leixing);
                        model.tianshu = (int)qing.tianshu;
                        return View(model);
                    }                    
                    return View();
                }
            }
            
        }

        /// <summary>
        /// POST请假单
        /// 请假单基础号10
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFilter]
        public ActionResult Testdao(QingJiaDanModel model)
        {
            QingjiaDan qing = new QingjiaDan();
            GtestUser us = UserHelper.makeUserByidOrName(Session["user"].ToString());
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                //页面MODLE转化为数据库MODEL
                bool hasbid = false;
                if (model.bid != 0)
                {
                    hasbid = true;
                }
                if (hasbid)
                {
                    qing = db.QingjiaDan.Where(m => m.bid == model.bid).FirstOrDefault();
                }
                else
                {
                    int bascbid = IdHelper.makeBid(BiaoLeiXing.请假单.ToString(), us.username) * 100;
                    qing.bid = IdHelper.makeid(db.QingjiaDan.Where(m => m.bid > bascbid).OrderByDescending(m => m.bid), bascbid);
                    qing.id = Guid.NewGuid();
                }
                qing.detil = model.detil;
                qing.finishtime =DateTime.Parse(model.finishtime);
                
                qing.leixing = Enum.GetName(typeof(QingJiaLeiXing), model.leixing);
                qing.shenqingren = us.userxm;
                qing.startime =DateTime.Parse(model.startime);
                qing.tianshu = ((TimeSpan)(qing.finishtime - qing.startime)).Days;
                qing.tijiaotime = DateTime.Today;
                if (model.fangshi.Equals("tijiao"))
                {
                    CreatPorcess cp = new CreatPorcess();
                    qing.pid = cp.creatProcessByBaice((int)qing.bid, qing.shenqingren, 10).predefine.Pid;
                    MakeViewStateHelper.changeBaoCunState((int)qing.bid);
                }
                else
                {
                    MakeViewStateHelper.baoCunHelper((int)qing.bid, us.username);
                }
                if (!hasbid)
                {
                    db.QingjiaDan.Add(qing);
                }
                db.SaveChanges();
            }
            
            
            return RedirectToAction("Index", "BiaoList");
        }

        [HttpPost]
        [UserFilter]
        public ActionResult cuLi(ProcessChuLiModel model)
        {
            GtestUser us = UserHelper.makeUserByidOrName(Session["user"].ToString());
            using(ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                QingjiaDan qing = db.QingjiaDan.Where(m => m.bid == model.bid).FirstOrDefault();
                int pid = (int)qing.pid;
                Processing pro = Processing.processingFactory(pid, us.userxm);
                IDictionary<string, object> dic = new Dictionary<string, object>();
                //QingJiaDanChuLi cl = new QingJiaDanChuLi(us, pro, dic);
                if (model.fangshi.Equals("tongyi") || model.fangshi.Equals("dahui"))
                {
                    dic.Add(new KeyValuePair<string, object>("yijian", model.yijian));
                }
                if (Enum.IsDefined(typeof(ChuLiFangShi), model.fangshi))
                {
                    //cl.cuLi((ChuLiFangShi)Enum.Parse(typeof(ChuLiFangShi), model.fangshi));
                }
            }
            return RedirectToAction("Index", "BiaoList");
        }

        /*private class QingJiaDanChuLi : AbsutLiuChengChuLi
        {
            public QingJiaDanChuLi(GtestUser us, Processing pro, IDictionary<string, object> dic) : base(us, pro, dic)
            {
            }

            
        }*/
    }

   

   
}