using System;
using System.Collections.Generic;
using System.Linq;
using ProcessManager.Models;
using ProcessBasice.ChangLiang;
using ProcessManager.ProcessInterface;
namespace ProcessManager.DAO
{
    /// <summary>
    /// 当前表数据层
    /// </summary>
    public class ProcessPredefineDAO:IProcessDao<ProcessPredefineModel>
    {
        /// <summary>
        /// predefine转换成predefineModel
        /// </summary>
        /// <param name="predefine"></param>
        /// <returns></returns>
        private ProcessPredefineModel predefineToPredefineModel(Predefine predefine)
        {
            ProcessPredefineModel predefineModel = new ProcessPredefineModel();
            predefineModel.Pid = (int)predefine.pid;
            predefineModel.Bid = (int)predefine.bid;
            predefineModel.Order = (int)predefine.steps;
            predefineModel.Handler = predefine.hanlder;
            predefineModel.Nexthandler = predefine.nexthanlder;
            predefineModel.Lasthandler = predefine.lasthanlder;
            predefineModel.State = (PredefineState)Enum.Parse(typeof(PredefineState),predefine.state);
            return predefineModel;
        }

        /// <summary>
        /// predefineModel转换成predefine
        /// </summary>
        /// <param name="predefineModel"></param>
        /// <param name="predefine"></param>
        /// <returns></returns>
        private Predefine predefineModelToPredefeine(ProcessPredefineModel predefineModel,Predefine predefine=null)
        {
            if (predefine == null)
            {
                predefine = new Predefine();
                predefine.id = Guid.NewGuid();
            }
            predefine.pid = predefineModel.Pid;
            predefine.bid = predefineModel.Bid;
            predefine.steps = predefineModel.Order;
            predefine.hanlder = predefineModel.Handler;
            predefine.nexthanlder = predefineModel.Nexthandler;
            predefine.lasthanlder = predefineModel.Lasthandler;
            predefine.state = Enum.GetName(typeof(PredefineState),predefineModel.State);
            return predefine;
        }

        /// <summary>
        /// 插入当前情况表
        /// </summary>
        /// <param name="predefineModel"></param>
        /// <returns></returns>
        public int insert(ProcessPredefineModel predefineModel)
        {
            using (ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                Predefine predefine = predefineModelToPredefeine(predefineModel);
                db.Predefine.Add(predefine);
                int i= db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 根据pid查询当前情况表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ProcessPredefineModel selectByIdSinged(int pid)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                var selectString = from p in db.Predefine where p.pid == pid select p;
                Predefine predefine = selectString.First();
                ProcessPredefineModel predefineModel = predefineToPredefineModel(predefine);
                return predefineModel;
            }
        }

        /// <summary>
        /// 根据查询条件查询当前情况表
        /// </summary>
        /// <param name="selectString"></param>
        /// <returns></returns>
        public List<ProcessPredefineModel> selectPredefine(IQueryable<Predefine> selectString)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                List<Predefine> lPredefine = selectString.ToList();
                List<ProcessPredefineModel> lPredefineModel = new List<ProcessPredefineModel>();
                lPredefine.ForEach(predefein =>
                {
                    ProcessPredefineModel predefineModel = predefineToPredefineModel(predefein);
                    lPredefineModel.Add(predefineModel);
                });
                return lPredefineModel;
            }
        }

        /// <summary>
        /// 修改当前情况表
        /// </summary>
        /// <param name="predefineModel"></param>
        /// <returns></returns>
        public int update(ProcessPredefineModel predefineModel)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                var selectString = from p in db.Predefine where p.pid == predefineModel.Pid select p;
                Predefine predefine = selectString.First();
                predefineModelToPredefeine(predefineModel, predefine);
                int i = db.SaveChanges();
                return i;
            }
        }

        public List<ProcessPredefineModel> selectById(int id)
        {
            return null;
        }

        public int insertList(List<ProcessPredefineModel> lModel)
        {
            return 0;
        }
    }
}