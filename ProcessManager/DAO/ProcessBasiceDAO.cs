using System;
using System.Collections.Generic;
using System.Linq;
using ProcessManager.Models;
using ProcessManager.ProcessInterface;

namespace ProcessManager.DAO
{
    /// <summary>
    /// 基础表数据层
    /// </summary>
    public class ProcessBasiceDAO : IProcessDao<ProcessBasiceModel>
    {
        /// <summary>
        /// basice转换为basicemodel
        /// </summary>
        /// <param name="basice"></param>
        /// <returns></returns>
        private ProcessBasiceModel basiceToModel(Basice basice)
        {
            ProcessBasiceModel model = new ProcessBasiceModel();
            model.Pid = (int)basice.pid;
            model.Bid = (int)basice.bid;
            model.Handler = basice.hanlder;
            model.Nexthandler = basice.nexthanlder;
            model.Lasthandler = basice.lasthanlder;
            model.Order = (int)basice.steps;
            model.State =int.Parse(basice.state);
            return model;
        }

        /// <summary>
        /// basicemodel转换为basice
        /// </summary>
        /// <param name="model"></param>
        /// <param name="basice"></param>
        /// <returns></returns>
        private Basice modelToBasice(ProcessBasiceModel model,Basice basice=null)
        {
            if (basice == null)
            {
                basice = new Basice();
                basice.id = Guid.NewGuid();
            }
            basice.pid = model.Pid;
            basice.bid = model.Bid;
            basice.hanlder = model.Handler;
            basice.nexthanlder = model.Nexthandler;
            basice.lasthanlder = model.Lasthandler;
            basice.state = model.State.ToString();
            basice.steps = model.Order;
            return basice;
        }
        /// <summary>
        /// 插入基础表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int insert(ProcessBasiceModel model)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                Basice basice = modelToBasice(model);
                db.Basice.Add(basice);
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 插入多条数据到基础表
        /// </summary>
        /// <param name="lModel"></param>
        /// <returns></returns>
        public int insertList(List<ProcessBasiceModel> lModel)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                lModel.ForEach(model =>
                {
                    Basice basice = modelToBasice(model);
                    db.Basice.Add(basice);
                });
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 根据id查询基础表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProcessBasiceModel> selectById(int id)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from b in db.Basice
                                   where b.pid == id
                                   select b;
                List<Basice> lBasice = selectString.ToList();
                List<ProcessBasiceModel> lModel = new List<ProcessBasiceModel>();
                lBasice.ForEach(basice =>
                {
                    ProcessBasiceModel model = basiceToModel(basice);
                    lModel.Add(model);
                });
                return lModel;
            }
        }

        /// <summary>
        /// 修改基础表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int update(ProcessBasiceModel model)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from b in db.Basice
                                   where b.pid == model.Pid && b.steps == model.Order
                                   select b;
                Basice basice = selectString.First();
                modelToBasice(model, basice);
                int i = db.SaveChanges();
                return i;
            }
        }
    }
}