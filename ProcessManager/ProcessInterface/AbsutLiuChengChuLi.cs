using ProcessManager.ProcessCaoZuo;
using ProcessManager.Models;
using ProcessManager.DAO;
using System.Collections.Generic;
using System;

namespace ProcessManager.ProcessInterface
{
    public  class AbsutLiuChengChuLi : ILiuChengChuLi
    {
        public delegate void CuLiEvent();

        protected GtestUser us = null;
        protected Processing pro = null;
        protected IDictionary<string,object> dic = null;
        private ProcessPiZhuDAO pdao = new ProcessPiZhuDAO();
        private ProcessPredefineDAO fdao = new ProcessPredefineDAO();
        private ChuLiFangShi state;
        public event CuLiEvent beforeculi;
        public event CuLiEvent afterculi;

       public AbsutLiuChengChuLi(GtestUser us,Processing pro,ChuLiFangShi state,IDictionary<string, object> dic)
        {
            this.us = us;
            this.pro = pro;
            this.dic = dic;
            this.state = state;
        }

        

        public Processing cuLi()
        {
            beforeculi?.Invoke();
            ProcessPiZhu pizhu = new ProcessPiZhu();
            ProcessPredefineModel predefine = pro.predefine;
            pizhu.Pid = predefine.Pid;
            pizhu.Order = predefine.Order;
            pizhu.Hanlder = us.userxm;
            pizhu.pizhutime = DateTime.Now;
            if (dic.Keys.Contains("yijian"))
            {
                pizhu.Detail = this.dic["yijian"].ToString();
            }
            switch (state)
            {
                case ChuLiFangShi.tongyi:
                    beforeTongYi();
                    pro.toNextStep();
                    pdao.insertupdate(pizhu);
                    afterTongYi();
                    break;
                case ChuLiFangShi.tuihui:
                    biforeTuiHui();
                    pro.backStep();
                    pdao.insertupdate(pizhu);
                    afterTuiHui();
                    break;
                case ChuLiFangShi.xiayibu:
                    beforeXiaYiBu();
                    pro.toNextStep();
                    afterXiaYiBu();
                    break;
                case ChuLiFangShi.jiaqian:
                    beforeJiaQian();
                    pro.jiaQian(dic["jiaqianren"].ToString());
                    pdao.insertupdate(pizhu);
                    afterJiaQian();
                    break;
                case ChuLiFangShi.dahui:
                    beforeDaHui();
                    pro.returnStep();
                    pdao.insertupdate(pizhu);
                    afterDaHui();
                    break;
                case ChuLiFangShi.chaosong:
                    beforeChaoSong();
                    afterChaoSong();
                    break;
                default:
                    break;
            }
            afterculi?.Invoke();
            return this.pro;
        }






        public virtual void afterChaoSong() { }
        public virtual void afterDaHui() { }
        public virtual void afterJiaQian() { }
        public virtual void afterTongYi() { }
        public virtual void afterTuiHui() { }
        public virtual void afterXiaYiBu() { }
        public virtual void beforeChaoSong() { }
        public virtual void beforeDaHui() { }
        public virtual void beforeJiaQian() { }
        public virtual void beforeTongYi() { }
        public virtual void beforeXiaYiBu() { }
        public virtual void biforeTuiHui() { }
    }



    public enum ChuLiFangShi
    {
        tongyi=1,
        tuihui=2,
        xiayibu=3,
        jiaqian=4,
        dahui=5,
        chaosong=6
    }
}