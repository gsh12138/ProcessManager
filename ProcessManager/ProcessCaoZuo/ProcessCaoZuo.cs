using System;
using System.Collections.Generic;
using ProcessManager.Models;
using ProcessManager.DAO;
using ProcessBasice.Helper;
using ProcessBasice.ChangLiang;

namespace ProcessManager.ProcessCaoZuo
{
    /// <summary>
    /// 流程类，代表一个流程。
    /// 使用类工厂，通过传入流程号和当前处理人创建
    /// </summary>
    public class Processing
    {
        private int pid=0;
        private string hanlder = null;
        private List<ProcessProcessModel> processModel=null;
        private ProcessPredefineModel predefineModel=null;
        private List<ProcessHuiQian> huiqianModel = null;
        private List<ProcessChaoSong> chaosongModel = null;
        private ProcessProcessDAO processDAO=new ProcessProcessDAO();
        private ProcessPredefineDAO predefineDAO=new ProcessPredefineDAO();
        private ProcessHuiQianDAO huiqianDAO = new ProcessHuiQianDAO();
        private ProcessChaoSongDAO chaosongDAO = new ProcessChaoSongDAO();
        private MoveStep<ProcessProcessModel> move = new MoveStep<ProcessProcessModel>();

        private Processing(int pid,List<ProcessProcessModel> process,ProcessPredefineModel predefine,
            List<ProcessHuiQian> huiqian=null,List<ProcessChaoSong> chaosong=null,string hanlder=null)
        {
            this.pid = pid;
            this.processModel = process;
            this.predefineModel = predefine;
            this.huiqianModel = huiqian;
            this.chaosongModel = chaosong;
            this.hanlder = hanlder;
        }

        private Processing()
        {

        }


        /// <summary>
        /// 流程工厂，创建流程用
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="hanlder"></param>
        /// <returns></returns>
        public static Processing processingFactory(int pid,string hanlder)
        {
            ProcessProcessDAO pDAO = new ProcessProcessDAO();
            ProcessPredefineDAO dDAO = new ProcessPredefineDAO();
            List<ProcessProcessModel> process = pDAO.selectById(pid);
            ProcessPredefineModel predefine = dDAO.selectByIdSinged(pid);
            List<ProcessHuiQian> huiqian = null;
            List<ProcessChaoSong> chaosong = null;
            if (predefine.Handler.Equals("会签"))
            {
                ProcessHuiQianDAO hDAO = new ProcessHuiQianDAO();
                huiqian = hDAO.selectByIdOrder(predefine.Pid, predefine.Order);
            }
            if (predefine.Handler.Equals("抄送"))
            {
                ProcessChaoSongDAO sDAO = new ProcessChaoSongDAO();
                chaosong = sDAO.selectByIdOrder(predefine.Pid, predefine.Order);
            }
            return new Processing(pid, process, predefine,huiqian,chaosong,hanlder);
        }

        private static Processing processingFactory(int pid,List<ProcessProcessModel> process,ProcessPredefineModel predefine)
        {
            return new Processing(pid, process, predefine);
        }

        /// <summary>
        /// 测试实体中processmodel和predefinemodel有值否，如果没有就创建
        /// </summary>
        private void creatModel()
        {
            if (this.processModel == null)
            {
                processModel = processDAO.selectById(this.pid);
            }
            if (this.predefineModel == null)
            {
                predefineModel = predefineDAO.selectByIdSinged(this.pid);
            }
        }

        public List<ProcessProcessModel> lProcess
        {
            get
            {
                //creatModel();
                return this.processModel;
            }
        }

        public ProcessPredefineModel predefine
        {
            get
            {
                //creatModel();
                return this.predefineModel;
            }
        }
        /// <summary>
        /// 创建标准流程静态方法
        /// </summary>
        /// <param name="pid">pid</param>
        /// <param name="bid">bid</param>
        /// <param name="liuchengren">所有流程处理人组成的字符串list</param>
        /// <returns></returns>
        public static Processing creatLiuCheng(int pid,int bid,List<string> liuchengren)
        {
            CreatHelper<ProcessProcessModel, ProcessPredefineModel> creatHelper =
                new CreatHelper<ProcessProcessModel, ProcessPredefineModel>(pid,bid,liuchengren);
            CreatHelper<ProcessProcessModel,ProcessPredefineModel>.CuShiLiuCheng liucheng = creatHelper.createLiuCheng();
            try
            {
                ProcessProcessDAO pDAO = new ProcessProcessDAO();
                ProcessPredefineDAO fDAO = new ProcessPredefineDAO();
                pDAO.insertList(liucheng.processModel);
                fDAO.insert(liucheng.predefineModel);
                return processingFactory(pid, liucheng.processModel, liucheng.predefineModel);
                
            }
            catch (Exception)
            {
                return null;
                
            }
        }

        /// <summary>
        /// 创建带会签步骤的流程
        /// </summary>
        /// <param name="pid">流程号</param>
        /// <param name="bid">表单号</param>
        /// <param name="liuchengren">所有流程处理人组成的字符串list</param>
        /// <param name="lHuiqian">
        /// 所有会签人组成的字符串list
        /// 每个会签步骤按步骤先后顺序传入会签list
        /// </param>
        /// <returns></returns>
        public static Processing creatLiuChengHuiqian(int pid,int bid,List<string> liuchengren,List<List<string>> lHuiqian)
        {
            int i = 0;
            ProcessHuiQianDAO hDAO = new ProcessHuiQianDAO();
            ProcessProcessDAO pDAO = new ProcessProcessDAO();
            ProcessPredefineDAO fDAO = new ProcessPredefineDAO();
            List<List<ProcessHuiQian>> lhuiqian = new List<List<ProcessHuiQian>>();
            CreatHelper<ProcessProcessModel, ProcessPredefineModel> creatHelper =
                new CreatHelper<ProcessProcessModel, ProcessPredefineModel>(pid, bid, liuchengren);
            CreatHelper<ProcessProcessModel, ProcessPredefineModel>.CuShiLiuCheng liucheng = creatHelper.createLiuCheng();
            liucheng.processModel.ForEach(process =>
            {
                if (process.Handler.Equals("会签"))
                {
                    List<ProcessHuiQian> huiqian = HuiqianHelper<ProcessHuiQian>.creatHuiqian(pid, process.Order, lHuiqian[i]);
                    lhuiqian.Add(huiqian);
                    i += 1;
                }
            });
            try
            {
                pDAO.insertList(liucheng.processModel);
                fDAO.insert(liucheng.predefineModel);
                lhuiqian.ForEach(h =>
                {
                    hDAO.insertList(h);
                });
                return processingFactory(pid, liucheng.processModel, liucheng.predefineModel);
            }
            catch (Exception)
            {
                return null;
            }


        }

        /// <summary>
        /// 创建带抄送步骤的流程
        /// </summary>
        /// <param name="pid">流程号</param>
        /// <param name="bid">表单号</param>
        /// <param name="liuchengren">所有流程处理人组成的字符串list</param>
        /// <param name="lChaosong">
        /// 所有抄送人组成的字符串list
        /// 每个抄送步骤按步骤先后顺序传入会签list
        /// </param>
        /// <returns></returns>
        public static Processing creatLiuChengChaoSong(int pid,int bid,List<string> liuchengren,List<List<string>> lChaosong)
        {
            int i = 0;
            ProcessChaoSongDAO sDAO = new ProcessChaoSongDAO();
            ProcessProcessDAO pDAO = new ProcessProcessDAO();
            ProcessPredefineDAO fDAO = new ProcessPredefineDAO();
            List<List<ProcessChaoSong>> lchaosong = new List<List<ProcessChaoSong>>();
            CreatHelper<ProcessProcessModel, ProcessPredefineModel> creatHelper =
                new CreatHelper<ProcessProcessModel, ProcessPredefineModel>(pid, bid, liuchengren);
            CreatHelper<ProcessProcessModel, ProcessPredefineModel>.CuShiLiuCheng liucheng = creatHelper.createLiuCheng();
            liucheng.processModel.ForEach(process =>
            {
                if (process.Handler.Equals("抄送"))
                {
                    List<ProcessChaoSong> huiqian = ChaoSongHelper<ProcessChaoSong>.creatChaosong(pid, process.Order, lChaosong[i]);
                    lchaosong.Add(huiqian);
                    i += 1;
                }
            });
            try
            {
                pDAO.insertList(liucheng.processModel);
                fDAO.insert(liucheng.predefineModel);
                lchaosong.ForEach(h =>
                {
                    sDAO.insertList(h);
                });
                return processingFactory(pid, liucheng.processModel, liucheng.predefineModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建带会签和抄送步骤的流程
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="bid"></param>
        /// <param name="liuchengren"></param>
        /// <param name="lHuiqian">会签人</param>
        /// <param name="lChaosong">抄送人</param>
        /// <returns></returns>
        public static Processing creatLiuChengHuiqianChaosong(int pid,int bid,List<string> liuchengren,List<List<string>> lHuiqian,List<List<string>> lChaosong)
        {
            int h = 0;
            int c = 0;
            ProcessChaoSongDAO sDAO = new ProcessChaoSongDAO();
            ProcessProcessDAO pDAO = new ProcessProcessDAO();
            ProcessPredefineDAO fDAO = new ProcessPredefineDAO();
            ProcessHuiQianDAO hDAO = new ProcessHuiQianDAO();
            List<List<ProcessChaoSong>> lchaosong = new List<List<ProcessChaoSong>>();
            List<List<ProcessHuiQian>> lhuiqian = new List<List<ProcessHuiQian>>();
            CreatHelper<ProcessProcessModel, ProcessPredefineModel> creathelp =
                new CreatHelper<ProcessProcessModel, ProcessPredefineModel>
                (pid, bid, liuchengren);
            CreatHelper<ProcessProcessModel, ProcessPredefineModel>.CuShiLiuCheng liucheng = 
                creathelp.createLiuCheng();
            liucheng.processModel.ForEach(p =>
            {
                if (p.Handler.Equals("会签"))
                {
                    List<ProcessHuiQian> huiqian = HuiqianHelper<ProcessHuiQian>.creatHuiqian(pid, p.Order, lHuiqian[h]);
                    lhuiqian.Add(huiqian);
                    h += 1;
                }
                if (p.Handler.Equals("抄送"))
                {
                    List<ProcessChaoSong> chaosong = ChaoSongHelper<ProcessChaoSong>.creatChaosong(pid, p.Order, lChaosong[c]);
                    lchaosong.Add(chaosong);
                    c += 1;
                }
            });

            try
            {
                pDAO.insertList(liucheng.processModel);
                fDAO.insert(liucheng.predefineModel);
                lchaosong.ForEach(chao => {
                    sDAO.insertList(chao);
                });
                lhuiqian.ForEach(hui =>
                {
                    hDAO.insertList(hui);
                });
                return new Processing(pid, liucheng.processModel, liucheng.predefineModel);
            }
            catch (Exception)
            {
                return null;
            }
            

        }

        /// <summary>
        /// 运行到下一步
        /// 当下一步为会签时：
        /// 1、修改当前流程处理人的会签状态为完成。
        /// 2、测试会签是否所有人都通过：
        ///     是：运行到下一步
        ///     否：不修改当前流程表
        /// 当下一步为抄送时:跳过抄送步骤，继续运行到下一步
        /// </summary>
        /// <returns></returns>
        public Processing toNextStep()
        {
            creatModel();
            if (predefineModel.Handler.Equals("会签"))
            {
                foreach(ProcessHuiQian h in huiqianModel)
                {
                    if (h.Hanlder.Equals(this.hanlder))
                    {
                        h.State = HuiQianState.FINISH;
                        huiqianDAO.update(h);
                        break;
                    }
                }
                if (!HuiqianHelper<ProcessHuiQian>.isHuiqianFinish(this.huiqianModel))
                {
                    return this;
                }
            }
            move.downStep(this.predefineModel, this.processModel);
            if (this.predefineModel.Handler.Equals("抄送"))
            {
                move.downStep(this.predefineModel, this.processModel);
            }
            try
            {
                processDAO.updatelist(processModel);
                predefineDAO.update(predefineModel);
                if (this.predefineModel.Handler == "会签")
                {
                    this.huiqianModel = huiqianDAO.selectByIdOrder(predefineModel.Pid, predefineModel.Order);
                }
                return this;
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 打回流程(回到上一步)
        /// </summary>
        /// <returns></returns>
        public Processing backStep()
        {
            creatModel();
            move.upStep(this.predefineModel, this.processModel);
            if (this.predefineModel.Handler.Equals("抄送"))
            {
                move.upStep(this.predefineModel, this.processModel);
            }
            try
            {
                processDAO.updatelist(processModel);
                predefineDAO.update(predefineModel);
                return this;
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 退回流程(回到发起人同时关闭流程)
        /// </summary>
        /// <returns></returns>
        public Processing returnStep()
        {
            creatModel();
            if (this.predefineModel.Handler.Equals("会签"))
            {
                foreach (ProcessHuiQian h in huiqianModel)
                {
                    if (h.Hanlder.Equals(this.hanlder))
                    {
                        h.State = HuiQianState.NO;
                        huiqianDAO.update(h);
                        break;
                    }
                }
            }
            move.returnStep(this.predefineModel, this.processModel);
            try
            {
                processDAO.updatelist(processModel);
                predefineDAO.update(predefineModel);
                return this;
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 加入代签人
        /// </summary>
        /// <param name="jiaqianren"></param>
        /// <returns></returns>
        public Processing jiaQian(string jiaqianren)
        {
            ProcessProcessModel jiaprocess = new ProcessProcessModel();
            jiaprocess.Pid = this.predefineModel.Pid;
            jiaprocess.Bid = this.predefineModel.Bid;
            jiaprocess.Order = this.predefineModel.Order+1;
            jiaprocess.Handler = jiaqianren;
            jiaprocess.Nexthandler = this.predefineModel.Nexthandler;
            jiaprocess.Lasthandler = this.predefineModel.Handler;
            jiaprocess.State = ProcessState.JIAQIAN;

            move.insertStep(this.predefineModel, this.processModel, jiaprocess);

            try
            {
                this.processDAO.insert(jiaprocess);
                this.processDAO.updatelist(processModel);
                this.predefineDAO.update(predefineModel);
                return this;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
    }
}