using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProcessManager.Helper
{
    public class MyReadOnlyHelper
    {
        public static object hasReadonly(string classstring, bool has)
        {

            if (has)
            {
                return new { @class = classstring, @readonly = "readonly" };
            }
            else
            {
                return new { @class = classstring };
            }
        }

        public static object hasDisable(string classstring, bool has)
        {
            if (has)
            {
                return new { @class = classstring, disabled = "disabled" };
            }
            else
            {
                return new { @class = classstring };
            }
        }

        public static object test(string obj, bool has)
        {
            List<string> o = obj.Split(',').ToList();
            dynamic jz = new System.Dynamic.ExpandoObject();
            o.ForEach(s =>
            {
                ((IDictionary<string, object>)jz).Add(s.Split('=')[0], s.Split('=')[1]);
            });

            return jz;
        }

        public static object datePickerHTMLAttr(string classtring,string value,string id,string name,bool disabled) {
            if (disabled) {
                return new {
                    @class = classtring,
                    @readonly = "readonly",
                    value = value,
                    id = id,
                    name = name,
                    disabled
                };
            } else {
                return new {
                    @class = classtring,
                    @readonly = "readonly",
                    value = value,
                    id = id,
                    name = name,
                };
            }
        }

    }

    public class UserHelper
    {
        public static GtestUser makeUserByidOrName(string id)
        {
            using(TJZHEntities db = new TJZHEntities())
            {
                List<GtestUser> us = null;
                if (id.StartsWith("TS"))
                {
                    us = db.GtestUser.Where(m => m.username.Equals(id)).ToList();
                }
                else
                {
                    us = db.GtestUser.Where(m => m.userxm.Equals(id)).ToList();
                }
                if (us.Count == 0)
                {
                    return null;
                }
                else
                {
                    return us.First();
                }
            }
        }

        
    }

    
}