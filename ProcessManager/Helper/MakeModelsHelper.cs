using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.DAO;
using ProcessManager.Models;
using ProcessBasice.ChangLiang;

namespace ProcessManager.Helper
{
    /// <summary>
    /// 生成流程页面MODEL
    /// </summary>
    public class MakeModelsHelper
    {
        private ProcessChaoSongDAO cdao = new ProcessChaoSongDAO();
        private ProcessHuiQianDAO hdao = new ProcessHuiQianDAO();
        private ProcessPiZhuDAO pdao = new ProcessPiZhuDAO();
        private ProcessPredefineDAO fdao = new ProcessPredefineDAO();
        private ProcessProcessDAO sdao = new ProcessProcessDAO();

        /// <summary>
        /// 生成页面MODEL
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="zt"></param>
        /// <returns></returns>
        public ProcessListInfo makeListInfo(GtestUser handler,string zt)
        {
            ProcessListInfo linfo = new ProcessListInfo();
            List<Models.ProcessInfo> info = new List<Models.ProcessInfo>();
            List<ProcessPredefineModel> lpredefine = null;  //流程MODEL
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                
                //我发起的流程的MODEL
                if (zt.Equals("wfq"))
                {
                    //查询我发起的流程 hanlder=当前登陆人步骤号为0
                    var select = from f in db.Process
                                 where f.hanlder == handler.userxm && f.steps == 0
                                 orderby f.pid
                                 select f;
                    List<int> lpid = new List<int>();
                    select.ToList().ForEach(m =>
                    {
                        lpid.Add((int)m.pid);
                    });
                    lpredefine = fdao.selectPredefine(db.Predefine.Where(m => lpid.Contains((int)m.pid)));
                }
                //需要我审核的流程
                else if (zt.Equals("xsh"))
                {
                    //查询需要我审核的流程 hanlder=当前登陆人 state不为FINISH
                    var select = from f in db.Predefine
                                 where f.hanlder == handler.userxm&&f.state== "PROCESSING"
                                 orderby f.pid
                                 select f;
                    lpredefine = fdao.selectPredefine(select);
                    //查询会签流程中需要我审核的流程 hanlder=当前登陆人 state为DEFINE
                    List<Huiqian> huiqian = db.Huiqian
                                         .Where(m => m.hanlder.Equals(handler.userxm) && m.state.Equals("DEFINE")).ToList();
                    //根据查询出的会签步骤查询当前流程
                    huiqian.ForEach(h =>
                    {
                        //pid为会签pid 步骤为会签步骤
                        var se = from n in db.Predefine
                                 where n.pid == (int)h.pid && n.steps == h.steps
                                 orderby n.pid
                                 select n;
                        if (fdao.selectPredefine(se).Count != 0)
                        {
                            ProcessPredefineModel pre = fdao.selectPredefine(se).First();
                            lpredefine.Add(pre);
                        }
                    });
                }
            }
            //使用查询出的当前流程 构造单个页面Model
            lpredefine.ForEach(fine =>
            {
                int mpid = isduoliucheng(fine);
                if (mpid==0||mpid==fine.Pid)
                {
                    Models.ProcessInfo nf = new Models.ProcessInfo();
                    List<ProcessProcessModel> pro = sdao.selectById(fine.Pid);
                    using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
                    {
                        int lx = int.Parse(fine.Bid.ToString().Substring(0, 2));
                        string url = db.BiaoList.Where(m => m.pid == lx).FirstOrDefault().url;
                        nf.url = url;
                    }
                    switch (fine.State)
                    {
                        case PredefineState.FINISH:
                            nf.processstate = "已结束";
                            nf.predefinestate = "完成";
                            break;
                        case PredefineState.BACK:
                            nf.processstate = "审核中";
                            nf.predefinestate = "被打回";
                            break;
                        case PredefineState.RETURN:
                            nf.processstate = "已结束";
                            nf.predefinestate = "被退回";
                            break;
                        case PredefineState.PROCESSING:
                            nf.processstate = "审核中";
                            nf.predefinestate = "审核中";
                            break;
                        case PredefineState.JIAQIAN:
                            nf.processstate = "审核中";
                            nf.predefinestate = "加签";
                            break;
                        case PredefineState.XIANQIAN:
                            break;
                        default:
                            break;
                    }

                    nf.leixing = Enum.GetName(typeof(BiaoLeiXing), int.Parse(fine.Bid.ToString().Substring(0, 2)));
                    pro.Sort();
                    /*pro.ForEach(p =>
                    {
                        ProcessPiZhu zhu = pdao.selectByIdOrder(p.Pid, p.Order);
                        if (zhu != null)
                        {
                            p.pizhu = pdao.selectByIdOrder(p.Pid, p.Order).Detail;
                        }
                        else
                        {
                            p.pizhu = "未审核";
                        }
                    });*/
                    nf.predefine = fine;

                    nf.lprocess = pro;
                    info.Add(nf);
                }
            });
            
            info.Sort();
            linfo.linfo = info;
            linfo.userbm = handler.bumen;
            linfo.userid = handler.username;
            linfo.userxm = handler.userxm;
            return linfo;
        }

        private int isduoliucheng(ProcessPredefineModel lpf)
        {
            using(ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                List<Predefine> lp = db.Predefine.Where(m => m.bid == lpf.Bid).ToList();
                if (lp.Count != 1)
                {
                    int mpid = 0;
                    foreach(Predefine p in lp)
                    {
                        if ((p.pid - mpid) > 0)
                        {
                            mpid = (int)p.pid;
                        }
                    }
                    return mpid;
                }
                return 0;
            }
        }
    }

    enum BiaoLeiXing
    {
        请假单=10,
        异常处理单=11,
        客诉单=12
    }
}