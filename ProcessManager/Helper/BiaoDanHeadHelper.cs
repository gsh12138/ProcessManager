using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.Models;
using ProcessManager.BiaoDan;
using ProcessManager.ProcessCaoZuo;
using ProcessBasice.ChangLiang;
using ProcessManager.DAO;

namespace ProcessManager.Helper
{
    public class BiaoDanHeadHelper
    {
        //当前用户
        private GtestUser user;
        //头信息
        private IHead headmessage;
        //流程信息
        private Processing process;
        //表单编号
        private int biaodanhao;
        //编号对应基础表单信息
        private List<ProcessBasiceModel> basiceModle;


        //无具体表单号时使用的构造函数
        public BiaoDanHeadHelper(GtestUser user,int biaodanhao, List<ProcessBasiceModel> basiceModel) {
            this.user = user;
            this.biaodanhao = biaodanhao;
            this.basiceModle = basiceModel;
        }
        //有具体表单号时使用的构造函数
        public BiaoDanHeadHelper(GtestUser user, int biaodanhao,
            List<ProcessBasiceModel> basiceModel,
            IHead headmessage,
            Processing process) : this(user,biaodanhao,basiceModel) {
            this.headmessage = headmessage;
            this.process = process;
            
        }

        /// <summary>
        /// 获取表单头MODEL
        /// </summary>
        /// <returns>表单头MODEL</returns>
        public BiaoDanHeadModel getHead() {
            if (headmessage == null) {
                return getHeadDefine();
            }
            BiaoDanHeadModel head = new BiaoDanHeadModel();
            BiaoList jishao = getJieShao();
            head.name = jishao.name;
            head.canyuren = getCanYuRen();
            head.jieshao = jishao.detil;
            head.faqiren = headmessage.getFaQiRen();
            head.creatdate = headmessage.getFaQiTime().ToString("yyyy-MM-dd");
            if (head.faqiren.Equals(user.userxm)) {
                head.isShenHE = BiaoDanState.chakan;
            } else {
                head.isShenHE = BiaoDanState.shenhe;
            }
            head.jindu = getJinDu();
            head.lastshenhedate = getLastDate();
            head.lastshenheren = getLastRen();
            head.shijianxian = getShiJianXian();
            switch (process.predefine.State) {
                case PredefineState.FINISH:
                    head.state = "已完成";
                    break;
                case PredefineState.BACK:

                    break;
                case PredefineState.RETURN:
                    head.state = "被打回";
                    break;
                case PredefineState.PROCESSING:
                    head.state = "审核中";
                    break;
                case PredefineState.JIAQIAN:
                    break;
                case PredefineState.XIANQIAN:
                    break;
                default:
                    break;
            }
            return head;
        }
        /// <summary>
        /// 获取基本表单头信息
        /// </summary>
        /// <returns>基本表单头信息</returns>
        public BiaoDanHeadModel getHeadDefine() {
            BiaoDanHeadModel head = new BiaoDanHeadModel();
            //默认头信息----------------------------------------------------------
            head.name = getJieShao().name;
            head.faqiren = user.userxm;
            head.canyuren = getCanYuRen();
            head.jieshao = getJieShao().detil;
            head.creatdate = DateTime.Today.ToString("yyyy-MM-dd");
            head.isShenHE = BiaoDanState.faqi;
            head.jindu = "0";
            head.lastshenhedate = null;
            head.lastshenheren = null;
            head.state = "发起";
            head.shijianxian = new ListShiJianXian(new List<ShiJianXianModel>());
            //-------------------------------------------------------------------
            return head;
        }

        /// <summary>
        /// 获取审核时间线信息
        /// 只在数据库表单不为空时使用
        /// </summary>
        /// <returns>审核时间线</returns>
        private ListShiJianXian getShiJianXian() {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                ListShiJianXian shijianxians = new ListShiJianXian(new List<ShiJianXianModel>());
                int pid = headmessage.getPid();
                List<Pizhu> pizhus = db.Pizhu.Where(m => m.pid == pid).
                    OrderByDescending(m => m.pdate).ToList();
                pizhus.ForEach(m => {
                    ShiJianXianModel shijianxian = new ShiJianXianModel();
                    shijianxian.pizhu = m.detail;
                    shijianxian.caozuoren = m.hanlder;
                    shijianxian.caozuotime = ((DateTime)m.pdate).ToString("yyyy-MM-dd");
                    Process pro = db.Process.Where(p => p.pid == m.pid && p.steps == m.steps).FirstOrDefault();
                    if (pro != null) {
                        if (pro.steps == 0) {
                            shijianxian.state = ProcessState.BEGIN;
                        } else {
                            shijianxian.state = (ProcessState)Enum.Parse(typeof(ProcessState), pro.state);
                        }
                    }
                    shijianxians.shijianxians.Add(shijianxian);
                });
                return shijianxians;

            }
        }

        /// <summary>
        /// 获取最后一次审核时间
        /// </summary>
        /// <returns>字符串类型(yyyy-MM-dd)时间</returns>
        private string getLastDate() {
            //如果是被打回的表单返回最后一次审核的日期
            if (process.predefine.State == PredefineState.RETURN) {
                using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                    Pizhu piz = db.Pizhu.Where(m => m.pid == process.predefine.Pid).
                        OrderByDescending(m => m.pdate).First();
                    return ((DateTime)piz.pdate).ToString("yyyy-MM-dd");
                }
            }
            //其他所有状态的表单根据当前ORDER返回上一个order的日期
            ProcessPiZhuDAO pizhu = new ProcessPiZhuDAO();
            ProcessPiZhu pi = pizhu.selectByIdOrder(process.predefine.Pid, process.predefine.Order - 100);
            if (pi != null) {
                return pi.pizhutime.ToString("yyyy-MM-dd");
            } else {
                return null;
            }

        }

        /// <summary>
        /// 获取最后一个审核人
        /// </summary>
        /// <returns>审核人姓名</returns>
        private string getLastRen() {
            if (process.predefine.State == PredefineState.RETURN) {
                using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                    Pizhu piz = db.Pizhu.Where(m => m.pid == process.predefine.Pid).
                        OrderByDescending(m => m.pdate).First();
                    return piz.hanlder;
                }
            }
            if (process.predefine.Lasthandler == "FINISH") {
                return process.predefine.Handler;
            }
            return process.predefine.Lasthandler;
        }

        /// <summary>
        /// 获取审核进度
        /// </summary>
        /// <returns>字符串类型数字</returns>
        private string getJinDu() {
            int zstep = process.lProcess.Max().Order;

            if (process.predefine.State == PredefineState.FINISH) {
                return "100";
            }
            //如果是被打回状态，用最后一次审核order作为除数
            if (process.predefine.State == PredefineState.RETURN) {
                using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                    Pizhu piz = db.Pizhu.Where(m => m.pid == process.predefine.Pid).
                        OrderByDescending(m => m.pdate).First();
                    return (Convert.ToInt32((Convert.ToDouble(piz.steps) / Convert.ToDouble(zstep)) * 100)).ToString();
                }
            }
            //如果不是打回状态，用当前order作为除数
            
            int tt = Convert.ToInt32((Convert.ToDouble(process.predefine.Order-100) / Convert.ToDouble(zstep)) * 100);
            return tt.ToString();
        }
        

        /// <summary>
        /// 获取参与人信息
        /// </summary>
        /// <returns>所有参与人</returns>
        private List<CanYuRen> getCanYuRen() {
            List<CanYuRen> lfuzeren = findFuZeRen();
            return lfuzeren;
        }

        /// <summary>
        /// 找出所有参与人
        /// </summary>
        /// <returns>参与人</returns>
        private List<CanYuRen> findFuZeRen() {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                List<CanYuRen> lcanyu = new List<CanYuRen>();
                basiceModle.ForEach(b => {
                    //b.Handler.Split('&')[0]为部门
                    //b.Handler.Split('&')[1]为职位
                    string bumen = b.Handler.Split('&')[0];
                    string zhiwei = b.Handler.Split('&')[1];
                    FuZeRen fuze = db.FuZeRen.Where(m => m.bumen.Equals(bumen) && m.zhiwei.Equals(zhiwei)).
                                    FirstOrDefault();
                    if (fuze != null) {
                        lcanyu.Add(new CanYuRen(fuze.xingming, b.Lasthandler));
                    }
                });
                return lcanyu;
            }
        }

        /// <summary>
        /// 获取表单介绍
        /// </summary>
        /// <returns>表单介绍</returns>
        private BiaoList getJieShao() {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities()) {
                BiaoList biao = db.BiaoList.Where(m =>
                m.pid == biaodanhao).FirstOrDefault();
                return biao;
            }
        }
    }
}