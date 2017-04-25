using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProcessManager.Filters;
using ProcessManager.Models;
using ProcessManager.Helper;
using ProcessManager.BiaoDan;

namespace ProcessManager.Controllers
{
    [UserFilter]
    public class YiChangChuLiController : Controller
    {

        // GET: YiChangChuLi
        public ActionResult Index() {
            using (TJZHEntities db = new TJZHEntities()) {
                List<Gtestbiaodan> yichanglist = db.Gtestbiaodan.Where(
                    m => m.zrbm == "晶圆" && m.bdzt != 3).ToList();
                YiChangKanBanModel yichang = new YiChangKanBanModel() {
                    yiChangList = yichanglist
                };
                return View(yichang);
            }
        }
        [HttpGet]
        public ActionResult add() {
            YiChangChuLiS yichang = new YiChangChuLiS(this);
            YiChangModel model = yichang.getYiChang();
            return View(model);
        }

        [HttpPost]
        public ActionResult add(YiChangModel model) {
            YiChangChuLiS yichang = new YiChangChuLiS(this, model);
            yichang.postYiChang();
            return View("_success");
        }

        [HttpGet]
        public ActionResult detail(string id) {
            YiChangCuLiModel detail = new YiChangCuLiModel();
            using (TJZHEntities db = new TJZHEntities()) {
                List<YiChangCuLi> guocheng = db.YiChangCuLi.Where(m => m.yichangid == id).
                            OrderByDescending(m => m.culitime).ToList();
                if (guocheng == null) {
                    detail.chuliguocheng = new List<CuLiGuoCheng>();
                } else {
                    detail.chuliguocheng = guochengtoModelList(guocheng);
                }
                Gtestbiaodan yichang = db.Gtestbiaodan.Where(m => m.id == id).FirstOrDefault();
                detail.yichang = yichangToModel(yichang);
            }
            return View(detail);
        }

        [HttpPost]
        public ActionResult cuLiYiChang(CuLiGuoCheng model) {
            GtestUser us = ViewData["Puser"] as GtestUser;
            using (TJZHEntities db = new TJZHEntities()) {
                YiChangCuLi culi = new YiChangCuLi() {
                    culiren = us.userxm,
                    culitime = DateTime.Now,
                    flag = "已处理",
                    id = Guid.NewGuid(),
                    leixing = CuLiLeiXing.处理异常.ToString(),
                    yichangid = model.id,
                    yyfx = model.yyfx,
                    zgjg = model.zgjg
                };
                Gtestbiaodan yichang = db.Gtestbiaodan.Where(m => m.id == model.id).FirstOrDefault();
                if (yichang != null) {
                    yichang.bdzt = (int)YiChangState.已处理;
                }
                db.YiChangCuLi.Add(culi);
                db.SaveChanges();
            }
            return View("_success");
        }

        [HttpPost]
        public ActionResult fanKuiYiChang(CuLiGuoCheng model) {
            GtestUser us = ViewData["Puser"] as GtestUser;
            using (TJZHEntities db = new TJZHEntities()) {
                YiChangCuLi culi = new YiChangCuLi() {
                    culiren = us.userxm,
                    culitime = DateTime.Now,
                    flag = "已反馈",
                    id = Guid.NewGuid(),
                    leixing = CuLiLeiXing.反馈异常.ToString(),
                    yichangid = model.id,
                    xgqr = model.xgqr
                };
                Gtestbiaodan yichang = db.Gtestbiaodan.Where(m => m.id == model.id).FirstOrDefault();
                if (yichang != null) {
                    yichang.bdzt = (int)YiChangState.未解决;
                }
                db.YiChangCuLi.Add(culi);
                db.SaveChanges();
            }
            return View("_success");

        }


        private CuLiGuoCheng guochengToModel(YiChangCuLi culi) {
            if (culi == null) {
                return new CuLiGuoCheng();
            }
            CuLiGuoCheng guocheng = new CuLiGuoCheng() {
                id = culi.yichangid,
                culiren = culi.culiren,
                culitime = ((DateTime)culi.culitime).ToString("yyyy-MM-dd"),
                flag = culi.flag,
                leixing = culi.leixing,
                xgqr = culi.xgqr,
                yyfx = culi.yyfx,
                zgjg = culi.zgjg
            };
            return guocheng;
        }

        private List<CuLiGuoCheng> guochengtoModelList(List<YiChangCuLi> culiList) {
            List<CuLiGuoCheng> lculi = new List<CuLiGuoCheng>();
            if (culiList == null) {
                return new List<CuLiGuoCheng>();
            }
            int i = 0;
            culiList.ForEach(m => {
                CuLiGuoCheng guocheng = guochengToModel(m);
                guocheng.jishu = i;
                lculi.Add(guocheng);
                i += 1;
            });
            return lculi;
        }

        private YiChang yichangToModel(Gtestbiaodan yichang) {
            if (yichang == null) {
                return new YiChang();
            }
            YiChang yi = new YiChang() {
                id = yichang.id,
                cxrq = ((DateTime)(yichang.cxrq)).ToString("yyyy-MM-dd"),
                tbbm = yichang.tbbm,
                tbr = UserHelper.makeUserByidOrName(yichang.tbrgh).userxm,
                tbrq = ((DateTime)(yichang.tbrq)).ToString("yyyy-MM-dd"),
                ycfl = yichang.ycfl,
                ycms = yichang.ycms,
                zt = yichang.zt,
                state = (YiChangState)yichang.bdzt
            };
            return yi;
        }
    }
}