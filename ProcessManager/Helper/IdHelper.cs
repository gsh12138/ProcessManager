using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProcessManager.ProcessInterface;
using System.Linq.Expressions;

namespace ProcessManager.Helper
{
    public class IdHelper
    {
        public static int makeBid(string leixing,string gh)
        {
            int bid = 0;
            string lei = ((int)Enum.Parse(typeof(BiaoLeiXing), leixing)).ToString();
            string y = (int.Parse(DateTime.Today.Year.ToString().Substring(2)) + int.Parse(gh.Substring(2, 1))).ToString();
            string m = (DateTime.Today.Month + int.Parse(gh.Substring(3, 1))).ToString("D2");
            string d = (DateTime.Today.Day + int.Parse(gh.Substring(4, 1))).ToString("D2");
            bid = int.Parse(lei + y + m + d);
            return bid;
        }

        public static int makePid(System.Data.Entity.DbSet<Process> process,int bid)
        {
            string b = bid.ToString().Substring(2);
            int id= int.Parse(b);
            if (process.Where(m => m.pid > id * 100).ToList().Count != 0)
            {
                id = (int)process.Where(m => m.pid > id * 100).OrderByDescending(m => m.pid).First().pid + 1;
            }
            else
            {
                id = id * 100 + 1;
            }

            return id;
        }


        public static int makeid<T>(IOrderedQueryable<T> db,int bacbid) where T:class
        {
            int id = 0;
            /*var selecb = from b in db
                         where b.bid > ib * 100
                         select b;*/
            if (db.ToList().Count != 0)
            {
                T biaodan = db.FirstOrDefault();
                id =int.Parse(((T)biaodan).GetType().GetProperty("bid").GetValue(biaodan,null).ToString()) + 1;
            }
            else
            {
                id = bacbid + 1;
            }
            return id;
        }

        
    }
}