using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessManager.Helper;
using ProcessManager.Filters;
using ProcessManager.Models;
using ProcessManager.ProcessCaoZuo;
using ProcessBasice.ChangLiang;
using ProcessManager.ProcessInterface;

namespace ProcessManager.Controllers
{
    public class Test1Controller : Controller
    {
        // GET: Test1
        GtestUser us = null;
        public ActionResult Index()
        {
     
            return View();
        }


        /// <summary>
        /// GET表单
        /// </summary>
        /// <param name="id">
        /// 1:创建表单，layout为 ~/Views/Test/_Biaodan.cshtml，readonly为false
        /// 流程号:GET已有表单
        /// *1：如果表单pid为null，表示表单为保存未提交的表单,layout为 ~/Views/Test/_Biaodan.cshtml，readonly为false
        /// *2:如果表单pid不为null时需判断表单打开人:
        ///     -1:表单打开人为发起人时:判断表单状态：
        ///         审核中时:layout为 ~/Views/Shared/_Layout.cshtml,readonly为true
        ///         审核结束:layout为 ~/Views/Shared/_Layout.cshtml,readonly为true
        ///     -2:表单打开人为审核人时:判断表单状态:
        ///         审核中时:layout为 ~/Views/Test/_Testqiantao.cshtml,readonly为true
        ///         审核结束:layout为 ~/Views/Shared/_Layout.cshtml,readonly为true
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [UserFilter]
        public ActionResult test(string id)
        {
            us = UserHelper.makeUserByidOrName(Session["user"].ToString());
            if (id.Equals("1"))
            {
                ViewBag.lout = "~/Views/Test/_Biaodan.cshtml";
                ViewBag.readol = "false";
                return View();
            }
            else
            {
                using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
                {
                    int bid = int.Parse(id);
                    KeSuDan ke = db.KeSuDan.Where(m => m.bid == bid).FirstOrDefault();
                    if (ke.pid == null)
                    {
                        ViewBag.lout = "~/Views/Test/_Biaodan.cshtml";
                        ViewBag.readol = "false";
                    }
                    else
                    {
                        Processing process = Processing.processingFactory((int)ke.pid, us.userxm);
                        process.lProcess.Sort();
                        if (process.lProcess[0].Handler.Equals(us.userxm))
                        {
                            if (ke.state!=null&&ke.state.Equals("daiguanbi"))
                            {
                                if (process.predefine.State != PredefineState.PROCESSING)
                                {
                                    ViewBag.lout = "_JueDing.cshtml";
                                    ViewBag.readol = "true";
                                } else if (process.predefine.State == PredefineState.PROCESSING)
                                {
                                    ViewBag.lout = "~/Views/Shared/_Layout.cshtml";
                                    ViewBag.readol = "true";
                                }
                            } else if (ke.state != null && ke.state.Equals("guanbi"))
                            {
                                ViewBag.lout = "~/Views/Shared/_Layout.cshtml";
                                ViewBag.readol = "true";
                            }
                            else
                            {
                                ViewBag.lout = "~/Views/Shared/_Layout.cshtml";
                                ViewBag.readol = "true";
                            }
                        }
                        else
                        {
                            if (ke.state != null && ke.state.Equals("daiguanbi"))
                            {
                                if (process.predefine.State == PredefineState.FINISH)
                                {
                                    ViewBag.lout = "~/Views/Shared/_Layout.cshtml";
                                    ViewBag.readol = "true";
                                }
                                else if (process.predefine.State == PredefineState.PROCESSING)
                                {
                                    ViewBag.lout = "~/Views/Test/_Testqiantao.cshtml";
                                    ViewBag.readol = "true";
                                    ViewBag.canshen = "true";
                                    ViewBag.yjpid = bid;
                                    ViewBag.burl = "/Test1";
                                }
                            }
                            else if (ke.state != null && ke.state.Equals("guanbi"))
                            {
                                ViewBag.lout = "~/Views/Shared/_Layout.cshtml";
                                ViewBag.readol = "true";
                            }
                            else
                            {
                                ViewBag.lout = "_XiaYiBu.cshtml";
                                ViewBag.readol = "true";
                            }
                        }
                        
                    }
                    List<KeSuChanPinYWY> ywys = db.KeSuChanPinYWY.Where(m => m.bid == bid).ToList();
                    List<KeSuChanPinZZ> yzz = db.KeSuChanPinZZ.Where(m => m.bid == bid).ToList();
                    KeSuDanModel dan = new KeSuDanModel();
                    dan.bid = int.Parse(id);
                    dan.canpin = new List<CanPinModel>();
                    dan.khbh = ke.khbh;
                    dan.tbrq = ((DateTime)ke.tbrq).ToString("yyyy-MM-dd");
                    if (yzz.Count != 0)
                    {
                        yzz.ForEach(m =>
                        {
                            CanPinModel can = new CanPinModel();
                            can.dingdanid = m.ddh;
                            can.shuliang = m.sl.ToString();
                            can.tuiorhuan = m.thh;
                            can.wenti = m.wt;
                            can.yuanying = m.yy;
                            dan.canpin.Add(can);
                        });
                    }
                    else
                    {
                        ywys.ForEach(m =>
                        {
                            CanPinModel can = new CanPinModel();
                            can.dingdanid = m.ddh;
                            can.shuliang = m.sl.ToString();
                            can.tuiorhuan = m.thh;
                            can.wenti = m.wt;
                            can.yuanying = m.yy;
                            dan.canpin.Add(can);
                        });
                    }
                    return View(dan);
                }
            }

        }


        /// <summary>
        /// 提交表单
        /// 如果为提交按钮就创建流程
        /// 如果不为就只保存表单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult test(KeSuDanModel model)
        {
            us = UserHelper.makeUserByidOrName(Session["user"].ToString());
            List<CanPinModel> lcp = makecanPinModel(HttpContext);
            KeSuDan dan = new KeSuDan();
            List<KeSuChanPinYWY> ksywy = new List<KeSuChanPinYWY>();
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                if (model.bid != 0)
                {
                    dan = db.KeSuDan.Where(m => m.bid == model.bid).First();
                    db.KeSuChanPinYWY.Where(m => m.bid == model.bid).ToList().ForEach(m=> {
                        db.KeSuChanPinYWY.Remove(m);
                    });
                    db.SaveChanges();
                }
                else
                {
                    int basbid = IdHelper.makeBid(BiaoLeiXing.客诉单.ToString(), us.username);
                    dan.bid = IdHelper.makeid(db.KeSuDan.Where(m=>m.bid>basbid).OrderByDescending(m=>m.bid),basbid);
                    dan.id = Guid.NewGuid();
                }
                dan.khbh = model.khbh;
                dan.tbr = us.username;
                dan.tbrq = DateTime.Parse(model.tbrq);
                
                lcp.ForEach(m =>
                {
                    if (m.dingdanid != "" && m.shuliang != "")
                    {
                        KeSuChanPinYWY ymy = new KeSuChanPinYWY();
                        ymy.bid = dan.bid;
                        ymy.ddh = m.dingdanid;
                        ymy.id = Guid.NewGuid();
                        ymy.sl = int.Parse(m.shuliang);
                        ymy.thh = m.tuiorhuan;
                        ymy.wt = m.wenti;
                        ymy.yy = m.yuanying;
                        db.KeSuChanPinYWY.Add(ymy);
                    }
                });
                if (model.fangshi.Equals("tijiao"))
                {
                    CreatPorcess cp = new CreatPorcess();
                    dan.pid= cp.creatProcessByBaice((int)dan.bid, us.userxm, 12).predefine.Pid;
                    MakeViewStateHelper.changeBaoCunState((int)dan.bid);
                }
                else
                {
                    MakeViewStateHelper.baoCunHelper((int)dan.bid, us.username);
                }
                if (model.bid == 0)
                {
                    db.KeSuDan.Add(dan);
                }
                
                db.SaveChanges();
            }


            return RedirectToAction("Index", "BiaoList");
        }

        /// <summary>
        /// 处理表单流程
        /// xiayibu:下一步
        /// tongyi:同意
        /// guanbi:关闭
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [UserFilter]
        public ActionResult cuLi(ProcessChuLiModel model)
        {
            int bid = 0;
            string fangshi = null;

            bid = model.bid;
            fangshi = model.fangshi;

            us = UserHelper.makeUserByidOrName(Session["user"].ToString());
            
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                List<CanPinModel> lcp = new List<CanPinModel>();
                if (HttpContext.Request["dingdanhao"] != null)
                {
                    lcp = makecanPinModel(HttpContext);
                }
                List<KeSuChanPinZZ> kzz = new List<KeSuChanPinZZ>();
                IDictionary<string, object> dic = new Dictionary<string, object>();
                int pid = (int)db.KeSuDan.Where(m => m.bid == bid).FirstOrDefault().pid;
                Processing pro = Processing.processingFactory(pid, us.userxm);
                //KeSuDanChuLi chuli = new KeSuDanChuLi(us, pro, dic);
                if (fangshi.Equals("xiayibu"))
                {
                    dic.Add(new KeyValuePair<string, object>("lcp",lcp));
                }
                if (fangshi.Equals("tongyi")||fangshi.Equals("dahui"))
                {
                    dic.Add(new KeyValuePair<string, object>("yijian", model.yijian));
                }
                if (fangshi.Equals("guanbi"))
                {
                    db.KeSuDan.Where(m => m.bid == bid).FirstOrDefault().state = "guanbi";
                    db.SaveChanges();
                }
                if (fangshi.Equals("faqijueyi"))
                {
                    db.KeSuChanPinZZ.Where(m => m.bid == bid).ToList().ForEach(m =>
                    {
                        db.KeSuChanPinZZ.Remove(m);
                    });
                    db.SaveChanges();
                    lcp.ForEach(m =>
                    {
                        if (m.dingdanid != "" && m.shuliang != "")
                        {
                            KeSuChanPinZZ Kzz = new KeSuChanPinZZ();
                            Kzz.bid = bid;
                            Kzz.ddh = m.dingdanid;
                            Kzz.id = Guid.NewGuid();
                            Kzz.sl = int.Parse(m.shuliang);
                            Kzz.thh = m.tuiorhuan;
                            Kzz.wt = m.wenti;
                            Kzz.yy = m.yuanying;
                            db.KeSuChanPinZZ.Add(Kzz);
                        }
                    });
                    CreatPorcess creatprocess = new CreatPorcess();
                    KeSuDan kesu = db.KeSuDan.Where(m => m.bid == bid).FirstOrDefault();
                    kesu.pid = creatprocess.creatProcessByBaice((int)kesu.bid, us.userxm, 13).predefine.Pid;
                    db.SaveChanges();
                    
                }
                if (Enum.IsDefined(typeof(ChuLiFangShi), fangshi))
                {
                    //chuli.cuLi((ChuLiFangShi)Enum.Parse(typeof(ChuLiFangShi), fangshi));
                }
                
            }

            return RedirectToAction("Index", "BiaoList");
        }

        private List<CanPinModel> makecanPinModel(HttpContextBase httpcontext)
        {
            List<string> ldingdanhao = HttpContext.Request["dingdanhao"].Split(',').ToList();
            List<string> lshuliang = HttpContext.Request["shuliang"].Split(',').ToList();
            List<string> lyuanying = HttpContext.Request["yuanying"].Split(',').ToList();
            List<string> ltuiohuan = HttpContext.Request["tuiorhuan"].Split(',').ToList();
            List<string> lwenti = HttpContext.Request["wenti"].Split(',').ToList();
            List<CanPinModel> lcanpin = new List<CanPinModel>();
            for (int i = 0; i < ldingdanhao.Count; i++)
            {
                CanPinModel can = new CanPinModel();
                can.dingdanid = ldingdanhao[i];
                can.shuliang = lshuliang[i];
                can.yuanying = lyuanying[i];
                can.tuiorhuan = ltuiohuan[i];
                can.wenti = lwenti[i];
                lcanpin.Add(can);
            }
            return lcanpin;
        }


        /*private class KeSuDanChuLi : AbsutLiuChengChuLi
        {
            private ProcessManagerDbEntities db = new ProcessManagerDbEntities();
           public KeSuDanChuLi(GtestUser us, Processing pro,IDictionary<string,object> dic) :base(us,pro,dic)
            {
                
            }







            
            public override void afterXiaYiBu()
            {
                if (pro.predefine.State == PredefineState.FINISH)
                {
                    db.KeSuDan.Where(m => m.bid == pro.predefine.Bid).FirstOrDefault().state = "daiguanbi";
                }
                db.SaveChanges();
            }

            

            public override void beforeXiaYiBu()
            {
                List<CanPinModel> lcp = dic["lcp"] as List<CanPinModel>;
                List<KeSuChanPinZZ> kzz = new List<KeSuChanPinZZ>();
                int pid = pro.predefine.Pid;
                int bid = pro.predefine.Bid;
                if (db.KeSuChanPinZZ.Where(m => m.bid == bid).ToList().Count != 0)
                {
                    db.KeSuChanPinZZ.Where(m => m.bid == bid).ToList().ForEach(m =>
                    {
                        db.KeSuChanPinZZ.Remove(m);
                    });
                    db.SaveChanges();
                }
                lcp.ForEach(m =>
                {
                    if (m.dingdanid != "" && m.shuliang != "")
                    {
                        KeSuChanPinZZ Kzz = new KeSuChanPinZZ();
                        Kzz.bid = bid;
                        Kzz.ddh = m.dingdanid;
                        Kzz.id = Guid.NewGuid();
                        Kzz.sl = int.Parse(m.shuliang);
                        Kzz.thh = m.tuiorhuan;
                        Kzz.wt = m.wenti;
                        Kzz.yy = m.yuanying;
                        db.KeSuChanPinZZ.Add(Kzz);
                    }
                });
            }


            
        }*/
    }
}