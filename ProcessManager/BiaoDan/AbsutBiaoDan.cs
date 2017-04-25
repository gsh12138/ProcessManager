using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.Models;
using ProcessManager.ProcessCaoZuo;
using ProcessManager.Helper;
using ProcessManager.DAO;
using ProcessManager.ProcessInterface;

namespace ProcessManager.BiaoDan
{
    public abstract class AbsutBiaoDan:IBiaoDan
    {
        //操作人
        protected GtestUser user;
        //基础表步骤信息
        protected List<ProcessBasiceModel> basiceModle;
        //请假单流程信息
        protected Processing process;
        //生成表头
        protected BiaoDanHeadHelper headHelper;
        //表单编号
        protected int BIAO_DAN_HAO;

        
        public AbsutBiaoDan(GtestUser user) {
            getBiaoDanHao();
            this.basiceModle = getBasiceModel();
            basiceModle.Sort();
            this.user = user;
            this.headHelper = new BiaoDanHeadHelper(user, BIAO_DAN_HAO,basiceModle);
        }

        //获取表单MODEL接口
        abstract public QingJiaDanModel getModel();
        //发起表单接口
        abstract public void faqi();
        //获取表单号接口
        abstract protected void getBiaoDanHao();

        
        /// <summary>
        /// 同意操作
        /// </summary>
        /// <param name="yijian"></param>
        virtual public void tongyi(string yijian) {
            getCuLi(yijian,ChuLiFangShi.tongyi).cuLi();
        }

        /// <summary>
        /// 打回操作
        /// </summary>
        /// <param name="yijian"></param>
        virtual public void dahui(string yijian) {
            getCuLi(yijian, ChuLiFangShi.dahui).cuLi();
        }

        public BiaoDanHeadModel getHead() {
            return this.headHelper.getHead();
        }

        //获取基础表MODEL
        protected List<ProcessBasiceModel> getBasiceModel() {
            return new ProcessBasiceDAO().selectById(BIAO_DAN_HAO);
        }
        //获取处理流程类
        protected AbsutLiuChengChuLi getCuLi(string yijian,ChuLiFangShi state) {
            IDictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(new KeyValuePair<string, object>("yijian", yijian));
            AbsutLiuChengChuLi culi = new AbsutLiuChengChuLi(user, process,state,dic);
            return culi;
        }


    }
}