using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.Models;
using ProcessManager.ProcessCaoZuo;
using ProcessBasice.ChangLiang;

namespace ProcessManager.Helper
{
    public class MakeViewStateHelper
    {
        public static ViewStateModel makeViewState(Processing process, GtestUser us)
        {
            ViewStateModel vsm = new ViewStateModel();
            process.lProcess.Sort();
            if (process.lProcess[0].Handler.Equals(us.userxm))
            {
                vsm.layout = "~/Views/Shared/_Layout.cshtml";
                vsm.read = "true";
            }
            else
            {
                if (process.predefine.State == PredefineState.PROCESSING)
                {
                    vsm.layout = "~/Views/Test/_Testqiantao.cshtml";
                    vsm.read = "true";
                }
                else
                {
                    vsm.layout = "~/Views/Shared/_Layout.cshtml";
                    vsm.read = "true";
                }
            }
            return vsm;
        }


        public static void baoCunHelper(int bid, string username)
        {
            using (ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                BaoCun bc = new BaoCun();
                if (db.BaoCun.Where(m => m.bid == bid).ToList().Count != 0)
                {
                    bc = db.BaoCun.Where(m => m.bid == bid).FirstOrDefault();
                }
                else
                {
                    bc.id = Guid.NewGuid();
                }
                bc.bid = bid;
                bc.bcr = username;
                bc.bcrq = DateTime.Today;
                bc.state = "weichuli";
                if (db.BaoCun.Where(m => m.bid == bid).ToList().Count == 0)
                {
                    db.BaoCun.Add(bc);
                }
                db.SaveChanges();
            }
        }

        public static void changeBaoCunState(int bid)
        {
            using(ProcessManagerDbEntities db = new ProcessManagerDbEntities())
            {
                BaoCun bc = db.BaoCun.Where(m => m.bid == bid).FirstOrDefault();
                if (bc != null)
                {
                    bc.state = "yichuli";
                }
                db.SaveChanges();
            }
        }
    }
}