using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.DAO;
using ProcessManager.Models;
using ProcessManager.Helper;

namespace ProcessManager.ProcessCaoZuo
{
    public class CreatPorcess
    {
        private ProcessBasiceDAO bdao = new ProcessBasiceDAO();
        private ProcessProcessDAO pdao = new ProcessProcessDAO();
        private ProcessHuiQianDAO hdao = new ProcessHuiQianDAO();
        private ProcessChaoSongDAO sdao = new ProcessChaoSongDAO();
        private ProcessPredefineDAO fdao = new ProcessPredefineDAO();
        

        public Processing creatProcessByBaice(int bid,string shenqingren,int bdh)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                List<ProcessBasiceModel> lbmodel = bdao.selectById(bdh);
                lbmodel.Sort();
                List<string> renyuan = new List<string>();
                List<List<string>> lhuiqian = new List<List<string>>();
                List<List<string>> lchaosong = new List<List<string>>();
                List<string> huiqian = new List<string>();
                List<string> chaosong = new List<string>();
                renyuan.Add(shenqingren);
                lbmodel.ForEach(r =>
                {
                    if(r.Lasthandler!=null&&r.Lasthandler.Equals(ZhongLei.HUIQIAN.ToString()))
                    {
                        if ((lbmodel.IndexOf(r) == lbmodel.Count - 1 || lbmodel[lbmodel.IndexOf(r) + 1].Order - r.Order > 1))
                        {
                            string b = r.Handler.Split('&')[0];
                            string z = r.Handler.Split('&')[1];
                            FuZeRen fz = db.FuZeRen.Where(m => m.bumen.Equals(b) && m.zhiwei.Equals(z)).First();
                            huiqian.Add(fz.xingming);
                            lhuiqian.Add(huiqian);
                            renyuan.Add("会签");
                            huiqian = new List<string>();
                        }
                        else
                        {
                            string b = r.Handler.Split('&')[0];
                            string z = r.Handler.Split('&')[1];
                            FuZeRen fz = db.FuZeRen.Where(m => m.bumen.Equals(b) && m.zhiwei.Equals(z)).First();
                            huiqian.Add(fz.xingming);
                        }
                    }
                    else if (r.Lasthandler != null && r.Lasthandler.Equals(ZhongLei.CHAOSONG.ToString()))
                    {
                        if ((lbmodel.IndexOf(r) == lbmodel.Count - 1 || lbmodel[lbmodel.IndexOf(r) + 1].Order - r.Order > 1))
                        {
                            string b = r.Handler.Split('&')[0];
                            string z = r.Handler.Split('&')[1];
                            FuZeRen fz = db.FuZeRen.Where(m => m.bumen.Equals(b) && m.zhiwei.Equals(z)).First();
                            chaosong.Add(fz.xingming);
                            lchaosong.Add(chaosong);
                            renyuan.Add("抄送");
                            chaosong = new List<string>();
                        }
                        else
                        {
                            string b = r.Handler.Split('&')[0];
                            string z = r.Handler.Split('&')[1];
                            FuZeRen fz = db.FuZeRen.Where(m => m.bumen.Equals(b) && m.zhiwei.Equals(z)).First();
                            chaosong.Add(fz.xingming);
                        }
                    }
                    else
                    {
                        string bm = r.Handler.Split('&')[0];
                        string zw = r.Handler.Split('&')[1];
                        FuZeRen ren = db.FuZeRen.Where(m => m.bumen.Equals(bm) && m.zhiwei.Equals(zw)).First();
                        renyuan.Add(ren.xingming);
                    }
                });
                int pid = IdHelper.makePid(db.Process, bid);
                if (lhuiqian.Count != 0 && lchaosong.Count != 0)
                {
                    return Processing.creatLiuChengHuiqianChaosong(pid, bid, renyuan, lhuiqian, lchaosong);
                }
                if (lhuiqian.Count != 0)
                {
                    return Processing.creatLiuChengHuiqian(pid, bid, renyuan, lhuiqian);
                }
                if (lchaosong.Count != 0)
                {
                    return Processing.creatLiuChengChaoSong(pid, bid, renyuan, lchaosong);
                }
                else
                {
                    return Processing.creatLiuCheng(pid, bid, renyuan);
                }

            }
        }
    }


    public enum ZhongLei
    {
        HUIQIAN=1,
        CHAOSONG=2
    }
}