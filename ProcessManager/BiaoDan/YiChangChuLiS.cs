using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.Filters;
using ProcessManager.Models;
using ProcessManager.Helper;
using System.Web.Mvc;

namespace ProcessManager.BiaoDan
{
    public class YiChangChuLiS
    {
        private Controller controller;
        private GtestUser us;
        private YiChangModel model;

        public YiChangChuLiS(Controller controller) {
            this.controller = controller;
            this.us = controller.ViewData["Puser"] as GtestUser;
        }
        public YiChangChuLiS(Controller controller,YiChangModel model):
            this(controller) {
            this.model = model;
        }

        public YiChangModel getYiChang() {
            using (TJZHEntities db = new TJZHEntities()) {
                List<YiChangFenLei> yichangfenlei = db.YiChangFenLei.ToList();
                List<YiChangFuZeRen> yichangfuzeren = db.YiChangFuZeRen.ToList();
                controller.ViewBag.fenlei = yichangfenlei;
                controller.ViewBag.fuzeren = yichangfuzeren;
                YiChangModel model = new YiChangModel() {
                    state = BiaoDanState.faqi
                };
                return model;
            }
        }

        public void postYiChang() {
            if (model != null) {
                using (TJZHEntities db = new TJZHEntities()) {
                    string id = getMaxId();
                    Gtestbiaodan biaodan = new Gtestbiaodan() {
                        id = id,
                        tbrq = DateTime.Today,
                        tbbm = us.bumen,
                        tbrgh = us.username,
                        cxrq = DateTime.Parse(model.cxrq),
                        ycgx = model.ycgx,
                        zrbm = model.zrbm,
                        btzr = db.YiChangFuZeRen.Where(m => m.fzrxm == model.zrbm).
                                    FirstOrDefault().fzrgh,
                        wlmc = model.wlmc,
                        wlxh = model.wlxh,
                        ph = model.ph,
                        sb = model.sb,
                        sbbh = model.sbbh,
                        zt = model.zt,
                        ycfl = model.ycfl,
                        ycms = model.ycms,
                        bdzt = (int)YiChangState.未处理
                    };
                    db.Gtestbiaodan.Add(biaodan);
                    db.SaveChanges();
                }
            }
        }

        private string getMaxId() {
            using(TJZHEntities db = new TJZHEntities()) {
                DateTime todayS = DateTime.Today;
                string id = null;
                Gtestbiaodan maxId = db.Gtestbiaodan.Where(m => m.tbrq >= todayS).
                    OrderByDescending(m => m.id).FirstOrDefault();
                if (maxId != null) {
                    id = ((long.Parse(maxId.id)) + 1).ToString();
                } else {
                    id = todayS.ToString("yyyyMMdd") + "0001";
                }
                return id;
            }
        }
    }
}