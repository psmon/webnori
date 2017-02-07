using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainService.test
{
    public class t_UserInfo
    {
        public string Name;
    }

    public class TestModel
    {
        public TestModel()
        {
            t_UserInfo te1 = new t_UserInfo();
            te1.Name = "te1";
            Users.Add(te1);

            t_UserInfo te2 = new t_UserInfo();
            te2.Name = "te2";
            Users.Add(te2);

        }
        public string title = "WebGame";
        public string name = "psmon";

        public List<t_UserInfo> Users = new List<t_UserInfo>();
    }



}
