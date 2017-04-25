using System;
using System.Collections.Generic;
using System.Linq;
using ProcessManager.Models;
using ProcessBasice.ChangLiang;
using ProcessManager.ProcessInterface;

namespace ProcessManager.DAO
{
    /// <summary>
    /// 抄送数据层
    /// </summary>
    public class ProcessChaoSongDAO : IProcessDao<ProcessChaoSong>
    {

        /// <summary>
        /// chaosong转换为chaosongmodel
        /// </summary>
        /// <param name="chaosong"></param>
        /// <returns></returns>
        private ProcessChaoSong chaoSongToChaoSongModel(Chaosong chaosong)
        {
            ProcessChaoSong chaoSongModel = new ProcessChaoSong();
            chaoSongModel.Pid = (int)chaosong.pid;
            chaoSongModel.Order = (int)chaosong.steps;
            chaoSongModel.Hanlder = chaosong.hanlder;
            chaoSongModel.State =(ChaoSongState)Enum.Parse(typeof(ChaoSongState),chaosong.state);
            return chaoSongModel;
        }

        /// <summary>
        /// chaosongmodel转换为chaosong
        /// </summary>
        /// <param name="chaoSongModel"></param>
        /// <param name="chaosong"></param>
        /// <returns></returns>
        private Chaosong chaoSongModelToChaoSong(ProcessChaoSong chaoSongModel,Chaosong chaosong=null)
        {
            if (chaosong == null)
            {
                chaosong = new Chaosong();
                chaosong.id = Guid.NewGuid();
            }
            chaosong.pid = chaoSongModel.Pid;
            chaosong.steps = chaoSongModel.Order;
            chaosong.hanlder = chaoSongModel.Hanlder;
            chaosong.state = Enum.GetName(typeof(ChaoSongState), chaoSongModel.State);
            return chaosong;
        }

        /// <summary>
        /// 插入抄送表单条数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int insert(ProcessChaoSong model)
        {
            using(ProcessManagerDbEntities db=new ProcessManagerDbEntities())
            {
                Chaosong chaosong = chaoSongModelToChaoSong(model);
                db.Chaosong.Add(chaosong);
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 插入抄送表多条数据
        /// </summary>
        /// <param name="lModel"></param>
        /// <returns></returns>
        public int insertList(List<ProcessChaoSong> lModel)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                lModel.ForEach(model =>
                {
                    Chaosong chaosong = chaoSongModelToChaoSong(model);
                    db.Chaosong.Add(chaosong);
                });
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 根据pid查询抄送数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProcessChaoSong> selectById(int id)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                List<ProcessChaoSong> lChaoSongModel = new List<ProcessChaoSong>();
                var selectString = from s in db.Chaosong
                                   where s.pid == id
                                   select s;
                List<Chaosong> lChaoSong = selectString.ToList();
                lChaoSong.ForEach(chaosong =>
                {
                    ProcessChaoSong chaoSongModel = chaoSongToChaoSongModel(chaosong);
                    lChaoSongModel.Add(chaoSongModel);
                });
                return lChaoSongModel;
            }
        }

        /// <summary>
        /// 更新抄送数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int update(ProcessChaoSong model)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from s in db.Chaosong
                                   where s.pid == model.Pid && s.hanlder == model.Hanlder
                                   select s;
                Chaosong chaosong = selectString.First();
                chaoSongModelToChaoSong(model, chaosong);
                int i = db.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 根据pid和order查询抄送数据
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<ProcessChaoSong> selectByIdOrder(int pid,int order)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                var selectString = from s in db.Chaosong
                                   where s.pid == pid && s.steps == order
                                   select s;
                List<Chaosong> lChaoSong = selectString.ToList();
                List<ProcessChaoSong> lChaoSongModel = new List<ProcessChaoSong>();
                lChaoSong.ForEach(chaosong =>
                {
                    ProcessChaoSong chaoSongModel = chaoSongToChaoSongModel(chaosong);
                    lChaoSongModel.Add(chaoSongModel);
                });
                return lChaoSongModel;
            }
        }
    }
}