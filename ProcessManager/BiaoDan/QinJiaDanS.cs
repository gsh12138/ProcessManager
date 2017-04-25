using System;
using System.Linq;
using ProcessManager.Models;
using ProcessManager.Helper;
using ProcessManager.ProcessCaoZuo;
using System.Web.Mvc;

namespace ProcessManager.BiaoDan
{
    //继承于AbsutBiaoDan
    public class QinJiaDanS : AbsutBiaoDan
    {
        //请假单视图MODEL
        private QingJiaDanModel model;
        //请假单数据库MODEL
        private QingjiaDan qing;

        /// <summary>
        /// 通过user构造请假单类
        /// </summary>
        /// <param name="user"></param>
        public QinJiaDanS(GtestUser user) : base(user) {

        }

        /// <summary>
        /// 发起表单时使用的构造函数，只需提供操作人信息
        /// </summary>
        /// <param name="user">操作人</param>
        /// <param name="model">视图MODEL（可选）</param>
        public QinJiaDanS(GtestUser user, QingJiaDanModel model) : this(user) {
            this.model = model;

        }

        /// <summary>
        /// 查询已有表单时使用的构造函数，需提供表单号
        /// </summary>
        /// <param name="user">操作人</param>
        /// <param name="bid">表单号</param>
        public QinJiaDanS(GtestUser user, int bid) : this(user) {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                qing = db.QingjiaDan.Where(m => m.bid == bid).FirstOrDefault();
            }
            this.process = Processing.processingFactory((int)qing.pid, user.userxm);
            this.headHelper = new BiaoDanHeadHelper(user, BIAO_DAN_HAO, basiceModle,
                new QinJiaDanHead(qing), process);
        }

        /// <summary>
        /// 发起表单
        /// </summary>
        override public void faqi() {
            QingjiaDan qing = new QingjiaDan();
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                int bascbid = IdHelper.makeBid(BiaoLeiXing.请假单.ToString(), user.username) * 100;
                qing.bid = IdHelper.makeid(db.QingjiaDan.Where(m => m.bid > bascbid).OrderByDescending(m => m.bid), bascbid);
                qing.id = Guid.NewGuid();
                qing.detil = model.detil;
                qing.finishtime = DateTime.Parse(model.finishtime);

                qing.leixing = Enum.GetName(typeof(QingJiaLeiXing), model.leixing);
                qing.shenqingren = user.userxm;
                qing.startime = DateTime.Parse(model.startime);
                qing.tianshu = ((TimeSpan)(qing.finishtime - qing.startime)).Days;
                qing.tijiaotime = DateTime.Today;
                try {
                    CreatPorcess cp = new CreatPorcess();
                    qing.pid = cp.creatProcessByBaice((int)qing.bid, qing.shenqingren, 10).predefine.Pid;
                    db.QingjiaDan.Add(qing);

                    Pizhu pizhu = new Pizhu();
                    pizhu.id = Guid.NewGuid();
                    pizhu.pid = qing.pid;
                    pizhu.steps = 0;
                    pizhu.hanlder = qing.shenqingren;
                    pizhu.detail = "发起";
                    pizhu.pdate = qing.tijiaotime;
                    db.Pizhu.Add(pizhu);

                    db.SaveChanges();
                } catch (Exception) {

                    throw;
                }
            }
        }

        /// <summary>
        /// 获取表单MODEL
        /// </summary>
        /// <returns>表单视图MODEL</returns>
        override public QingJiaDanModel getModel() {
            QingJiaDanModel mod = new QingJiaDanModel();
            //数据库MODEL不为空时把数据库MODE转换为视图MODEL
            if (this.qing != null) {
                mod.bid = (int)qing.bid;
                mod.detil = qing.detil;
                mod.finishtime = ((DateTime)qing.finishtime).ToString("yyyy-MM-dd");
                mod.leixing = (QingJiaLeiXing)Enum.Parse(typeof(QingJiaLeiXing), qing.leixing);
                mod.shenqingren = qing.shenqingren;
                mod.startime = ((DateTime)qing.startime).ToString("yyyy-MM-dd");
                mod.tianshu = (int)qing.tianshu;
            }
            //数据库MODEL不空时只填充头信息
            mod.head = headHelper.getHead();
            return mod;
        }

        /// <summary>
        /// 设置表单编号
        /// </summary>
        override protected void getBiaoDanHao() {
            this.BIAO_DAN_HAO = 10;
        }

        public bool yanZheng(Controller controller) {
            if (model.leixing == 0) {
                controller.ModelState.AddModelError("leixing", "请假类型未填写");
            }
            if (!controller.ModelState.IsValid) {
                model.head = this.getHead();
                return false;
            }
            return true;

        }

        /// <summary>
        /// 表单头类
        /// </summary>
        private class QinJiaDanHead : IHead
        {
            private QingjiaDan qing;

            public QinJiaDanHead(QingjiaDan qing) {
                this.qing = qing;
            }

            public string getFaQiRen() {
                return qing.shenqingren;
            }

            public DateTime getFaQiTime() {
                return (DateTime)qing.tijiaotime;
            }

            public int getPid() {
                return (int)qing.pid;
            }
        }


        
    }




}