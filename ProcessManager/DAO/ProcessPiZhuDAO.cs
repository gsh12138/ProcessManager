using System;
using System.Collections.Generic;
using System.Linq;
using ProcessManager.Models;
using ProcessManager.ProcessInterface;

namespace ProcessManager.DAO
{
    /// <summary>
    /// 批注数据层
    /// </summary>
    public class ProcessPiZhuDAO : IProcessDao<ProcessPiZhu>
    {

        /// <summary>
        /// pizhu转换为pizhumodel
        /// </summary>
        /// <param name="pizhu"></param>
        /// <returns></returns>
        private ProcessPiZhu pizhuToPizhuModel(Pizhu pizhu)
        {
            ProcessPiZhu model = new ProcessPiZhu();
            model.Pid = (int)pizhu.pid;
            model.Order = (int)pizhu.steps;
            model.Hanlder = pizhu.hanlder;
            model.Detail = pizhu.detail;
            model.pizhutime = (DateTime)pizhu.pdate;
            return model;
        }

        /// <summary>
        /// pizhumodel转换为pizhu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pizhu"></param>
        /// <returns></returns>
        private Pizhu piZhuModelToPiZhu(ProcessPiZhu model,Pizhu pizhu=null)
        {
            if (pizhu == null)
            {
                pizhu = new Pizhu();
                pizhu.id = Guid.NewGuid();
            }
            pizhu.pid = model.Pid;
            pizhu.steps = model.Order;
            pizhu.hanlder = model.Hanlder;
            pizhu.detail = model.Detail;
            pizhu.pdate = model.pizhutime;
            return pizhu;
        }
        /// <summary>
        /// 插入单条数据到批注表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int insert(ProcessPiZhu model)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                Pizhu pizhu = piZhuModelToPiZhu(model);
                db.Pizhu.Add(pizhu);
                int i = db.SaveChanges();
                return i;
            }
        }

        public int insertupdate(ProcessPiZhu model)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                int i = 0;
                Pizhu pizhu = piZhuModelToPiZhu(model);
                List<Pizhu> pi = db.Pizhu.Where(m => m.pid == pizhu.pid && m.steps == pizhu.steps && m.hanlder == pizhu.hanlder).ToList();
                if (pi.Count != 0)
                {
                    pi.First().detail = pizhu.detail;
                    i = db.SaveChanges();
                    return i;
                }
                db.Pizhu.Add(pizhu);
                i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 插入多条数据到批注表
        /// </summary>
        /// <param name="lModel"></param>
        /// <returns></returns>
        public int insertList(List<ProcessPiZhu> lModel)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                lModel.ForEach(model =>
                {
                    Pizhu pizhu = piZhuModelToPiZhu(model);
                    db.Pizhu.Add(pizhu);
                });
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 根据id查询批注表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProcessPiZhu> selectById(int id)
        {
            return null;
        }

        /// <summary>
        /// 修改单条批注表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int update(ProcessPiZhu model)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from z in db.Pizhu
                                   where z.pid == model.Pid && z.steps == model.Order
                                   select z;
                Pizhu pizhu = selectString.First();
                piZhuModelToPiZhu(model, pizhu);
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 根据pid和order查询单条批注表数据
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public ProcessPiZhu selectByIdOrder(int pid,int order)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from z in db.Pizhu
                                   where z.pid == pid && z.steps == order
                                   select z;
                
                if (selectString.ToList().Count == 0)
                {
                    return null;
                }
                Pizhu pizhu = selectString.First();
                ProcessPiZhu model = pizhuToPizhuModel(pizhu);
                return model;
            }
        }
    }
}