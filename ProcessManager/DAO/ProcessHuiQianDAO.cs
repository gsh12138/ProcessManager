using System;
using System.Collections.Generic;
using System.Linq;
using ProcessManager.Models;
using ProcessBasice.ChangLiang;
using ProcessManager.ProcessInterface;

namespace ProcessManager.DAO
{
    /// <summary>
    /// 会签数据层
    /// </summary>
    public class ProcessHuiQianDAO:IProcessDao<ProcessHuiQian>
    {
        /// <summary>
        /// huiqian转换为huiqianmodel
        /// </summary>
        /// <param name="huiQian"></param>
        /// <returns></returns>
        private ProcessHuiQian huiQianToHuiQianModel(Huiqian huiQian)
        {
            ProcessHuiQian huiQianModel = new ProcessHuiQian();
            huiQianModel.Pid = (int)huiQian.pid;
            huiQianModel.Order = (int)huiQian.steps;
            huiQianModel.Hanlder = huiQian.hanlder;
            huiQianModel.State = (HuiQianState)Enum.Parse(typeof(HuiQianState),huiQian.state);
            return huiQianModel;
        }

        /// <summary>
        /// huiqianmodel转换为huiqian
        /// </summary>
        /// <param name="huiQianModel"></param>
        /// <param name="huiqian"></param>
        /// <returns></returns>
        private Huiqian huiQianModelToHuiQian(ProcessHuiQian huiQianModel,Huiqian huiqian=null)
        {
            if (huiqian == null)
            {
                huiqian = new Huiqian();
                huiqian.id = Guid.NewGuid();
            }
            huiqian.pid = huiQianModel.Pid;
            huiqian.steps = huiQianModel.Order;
            huiqian.hanlder = huiQianModel.Hanlder;
            huiqian.state = Enum.GetName(typeof(HuiQianState), huiQianModel.State);
            return huiqian;
        }

        /// <summary>
        /// 插入多条会签数据
        /// </summary>
        /// <param name="lHuiQianModel"></param>
        /// <returns></returns>
        public int insertList(List<ProcessHuiQian> lHuiQianModel)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                lHuiQianModel.ForEach(huiqianmodel =>
                {
                    Huiqian huiqian = huiQianModelToHuiQian(huiqianmodel);
                    db.Huiqian.Add(huiqian);
                });
                int i= db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 插入单条会签数据
        /// </summary>
        /// <param name="huiQianModel"></param>
        /// <returns></returns>
        public int insert(ProcessHuiQian huiQianModel)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                Huiqian huiqian = huiQianModelToHuiQian(huiQianModel);
                db.Huiqian.Add(huiqian);
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 根据PID和ORDER查找会签数据
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<ProcessHuiQian> selectByIdOrder(int pid,int order)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from h in db.Huiqian where h.pid == pid && h.steps == order select h;
                List<Huiqian> lHuiQian = selectString.ToList();
                List<ProcessHuiQian> lHuiQianModel = new List<ProcessHuiQian>();
                lHuiQian.ForEach(huiqian =>
                {
                    ProcessHuiQian huiQianModel = huiQianToHuiQianModel(huiqian);
                    lHuiQianModel.Add(huiQianModel);
                });
                return lHuiQianModel;
            }
        }

        

        /// <summary>
        /// 修改会签数据
        /// </summary>
        /// <param name="huiQianModel"></param>
        /// <returns></returns>
        public int update(ProcessHuiQian huiQianModel)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from h in db.Huiqian
                                   where h.pid == huiQianModel.Pid && h.hanlder == huiQianModel.Hanlder
                                   select h;

                Huiqian huiQian = selectString.First();
                huiQianModelToHuiQian(huiQianModel, huiQian);
                int i= db.SaveChanges();
                return i;
            }
        }

        public List<ProcessHuiQian> selectById(int id)
        {
            return null;
        }
    }
}