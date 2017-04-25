using System.Collections.Generic;
using ProcessBasice.Model;
using ProcessBasice.ChangLiang;
using System;

namespace ProcessBasice.Helper
{
    /// <summary>
    /// 创建流程工具类
    /// </summary>
    /// <typeparam name="S">详细流程;ProcessModel的子类</typeparam>
    /// <typeparam name="D">当前流程;PredefineModel的子类</typeparam>
    public class CreatHelper<S,D> where S:ProcessModel where D:PredefineModel
    {
        private List<S> processModel;
        private int pid;
        private int bid;
        private IList<string> liuchengren;

        /// <summary>
        /// 初始流程内部类
        /// </summary>
        public class CuShiLiuCheng
        {
            private List<S> _processModel;
            private D _predefineModel;

            public List<S> processModel
            {
                get
                {
                    return _processModel;
                }

                set
                {
                    _processModel = value;
                }
            }

            public D predefineModel
            {
                get
                {
                    return _predefineModel;
                }

                set
                {
                    _predefineModel = value;
                }
            }
        }

        private CreatHelper()
        {
        }

        public CreatHelper(int pid,int bid,IList<string> liuchengren)
        {
            this.pid = pid;
            this.bid = bid;
            this.liuchengren = liuchengren;
        }
            

        /// <summary>
        /// 创建详细流程
        /// </summary>
        /// <returns>详细流程List</returns>
        public List<S> createProcess()
        {
            
            List<S> lprocess = new List<S>();
            int order = 0;
            foreach (string renyuan in liuchengren)
            {
                Type t = typeof(S);
                S process = (S)t.Assembly.CreateInstance(t.FullName);
                process.Pid = pid;
                process.Bid = bid;
                process.Order = order;
                
                if (process.Order == 0)
                {
                    process.Handler = renyuan;
                    process.Nexthandler = liuchengren[liuchengren.IndexOf(renyuan)+1];
                    process.Lasthandler = ProcessState.BEGIN.ToString();
                    process.State = ProcessState.FINISH;
                    process.sdate = DateTime.Today;
                    
                }
                else if (liuchengren.IndexOf(renyuan)==liuchengren.Count-1)
                {
                    process.Handler = renyuan;
                    process.Nexthandler = PredefineState.FINISH.ToString();
                    process.Lasthandler = ProcessState.FINISH.ToString();
                    process.State = ProcessState.DEFINE;
                }
                else
                {
                    process.Handler = renyuan;
                    process.Nexthandler = liuchengren[liuchengren.IndexOf(renyuan) + 1];
                    process.Lasthandler = liuchengren[liuchengren.IndexOf(renyuan) - 1];
                    process.State = ProcessState.DEFINE;
                }
                lprocess.Add(process);
                order += 100;
                
            }
            processModel = lprocess;
            return processModel;
        }

        /// <summary>
        /// 根据详细流程创建初始化当前流程
        /// </summary>
        /// <param name="lprocess">详细流程</param>
        /// <param name="order">从第几步开始创建，默认为0</param>
        /// <returns></returns>
        public  D createPredefine(IList<S> lprocess,int order=0)
        {
            List<S> listProcess = (List<S>)lprocess;
            listProcess.Sort();
            D predefine = (D)typeof(D).Assembly.CreateInstance(typeof(D).FullName);
            predefine.Bid = listProcess[order].Bid;
            predefine.Pid = listProcess[order].Pid;
            predefine.Order = listProcess[order].Order;
            predefine.Handler = listProcess[order].Handler;
            predefine.Nexthandler = listProcess[order].Nexthandler;
            predefine.Lasthandler = listProcess[order].Lasthandler;
            predefine.State = PredefineState.PROCESSING;
            return predefine;
        }

        /// <summary>
        /// 创建当前流程
        /// </summary>
        /// <param name="order">从第几步开始创建，必须在创建过详细流程之后调用</param>
        /// <returns></returns>
        public D createPredefine(int order = 0)
        {
            if (this.processModel == null)
            {
                return null;
            }
            return this.createPredefine(this.processModel, order);
        }

        /// <summary>
        /// 创建标准流程
        /// </summary>
        /// <returns>标准流程字典，KEY：初始当前流程；VALUE:详细流程列表</returns>
        public CuShiLiuCheng createLiuCheng()
        {
            CuShiLiuCheng liuCheng = new CuShiLiuCheng();
            List<S> lProcess = this.createProcess();
            D predefine = this.createPredefine(lProcess, 1);
            liuCheng.processModel = lProcess;
            liuCheng.predefineModel = predefine;
            return liuCheng;
        }

    }

    public class HuiqianHelper<T> where T:HuiQian
    {
        public static List<T> creatHuiqian(int pid,int order,List<string> huiqianren)
        {
            List<T> huiqian = new List<T>();
            huiqianren.ForEach(h =>
            {
                T hui = (T)typeof(T).Assembly.CreateInstance(typeof(T).FullName);
                hui.Pid = pid;
                hui.Order = order;
                hui.Hanlder = h;
                hui.State = HuiQianState.DEFINE;
                huiqian.Add(hui);
            });
            return huiqian;
        }

        public static bool isHuiqianFinish(List<T> lhuiqian)
        {
            foreach(T huiqian in lhuiqian)
            {
                if (huiqian.State == HuiQianState.DEFINE)
                {
                    return false;
                }
            }

            return true;
        }

    }

    public class ChaoSongHelper<T> where T:ChaoSong
    {
        public static List<T> creatChaosong(int pid,int order,List<string> chaosongren)
        {
            List<T> lchaosong = new List<T>();
            chaosongren.ForEach(chaosong =>
            {
                T c = (T)typeof(T).Assembly.CreateInstance(typeof(T).FullName);
                c.Pid = pid;
                c.Order = order;
                c.Hanlder = chaosong;
                c.State = ChaoSongState.DEFINE;
                lchaosong.Add(c);
            });

            return lchaosong;
        }
    }
}
