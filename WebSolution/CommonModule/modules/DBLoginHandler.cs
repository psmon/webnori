using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntLibContrib.Data.MySql;

using System.Net.Http;
using System.Net.Http.Headers;

using System.Data;
using CommonModule.data;

namespace CommonModule
{
    
    public class DBLoginHandler : DBMgr
    {
        public DBLoginHandler(string _conStr)
        {
            SetConnectionString(_conStr);
        }

        public int sp_createuser(int socialType,string socialID,string nick,string picurl,string email,string pw)
        {
            int resultValue = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("socialType",DbType.Int32, socialType));
            dbParams.Add(new DBREQ_PARAM("socialID", DbType.String, socialID));
            dbParams.Add(new DBREQ_PARAM("nick", DbType.String, nick));
            dbParams.Add(new DBREQ_PARAM("picurl", DbType.String, picurl));
            dbParams.Add(new DBREQ_PARAM("email", DbType.String, email));
            dbParams.Add(new DBREQ_PARAM("pw", DbType.String, pw));            
            DataTable result = RunSP("sp_createuser", CreateDB(), dbParams);
            resultValue = result.Rows[0].Field<int>("Result");
            return resultValue;            
        }

        public int sp_deleteuser(int socialType, string socialID)
        {
            int resultValue = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("socialType", DbType.Int32, socialType));
            dbParams.Add(new DBREQ_PARAM("socialID", DbType.String, socialID));
            DataTable result = RunSP("sp_deleteuser", CreateDB(), dbParams);
            resultValue = result.Rows[0].Field<int>("Result");
            return resultValue;
        }

        public LoginOK sp_loginuser(int _socialType, string _socialID,string _pw)
        {
            LoginOK resultValue = new LoginOK();

            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("_socialType", DbType.Int32, _socialType));
            dbParams.Add(new DBREQ_PARAM("_socialID", DbType.String, _socialID));
            dbParams.Add(new DBREQ_PARAM("_pw", DbType.String, _pw));
            DataTable result = RunSP("sp_loginuser", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
                resultValue.authToken = result.Rows[0].Field<string>("AuthToken");
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }

        public LoginOK sp_loginSocial(int socialType, string socialID, string nick, string picurl, string email)
        {
            LoginOK resultValue = new LoginOK();

            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("_socialType", DbType.Int32, socialType));
            dbParams.Add(new DBREQ_PARAM("_socialID", DbType.String, socialID));
            dbParams.Add(new DBREQ_PARAM("_nick", DbType.String, nick));
            dbParams.Add(new DBREQ_PARAM("_picurl", DbType.String, picurl));
            dbParams.Add(new DBREQ_PARAM("_email", DbType.String, email));

            
            DataTable result = RunSP("sp_loginSocial", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
                resultValue.authToken = result.Rows[0].Field<string>("AuthToken");
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }

        public UserInfo sp_userinfo(string _authtoken)
        {
            UserInfo resultValue = new UserInfo();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("_authtoken", DbType.String, _authtoken));
            DataTable result = RunSP("sp_userinfo", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;                
                resultValue.Nick = result.Rows[0].Field<string>("Nick");
                resultValue.PicUrl = result.Rows[0].Field<string>("PicUrl");
                resultValue.GamePoint = result.Rows[0].Field<int>("GamePoint");
                resultValue.CoinCount = result.Rows[0].Field<int>("CoinCount");
            }
            else
            {
                resultValue.statusCode = spError;
            }

            return resultValue;            
        }

        public DBLoginInfo sp_validtoken(string _authtoken,int isUpdateLoginTime)
        {
            DBLoginInfo resultValue = null;
            int spError = -1; 
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();            
            dbParams.Add(new DBREQ_PARAM("_authtoken", DbType.String, _authtoken));
            dbParams.Add(new DBREQ_PARAM("isUpdateLoginTime", DbType.Int32, isUpdateLoginTime));
            DataTable result = RunSP("sp_validtoken", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue = new DBLoginInfo();
                resultValue.MemberID = result.Rows[0].Field<int>("MemberID");
                resultValue.SocialType = result.Rows[0].Field<int>("SocialType");
                resultValue.SocialID = result.Rows[0].Field<string>("SocialID");
                resultValue.Nick = result.Rows[0].Field<string>("Nick");
                resultValue.PicUrl = result.Rows[0].Field<string>("PicUrl");
                resultValue.Email = result.Rows[0].Field<string>("Email");
                resultValue.Createtime = result.Rows[0].Field<int>("Createtime");
            }
            return resultValue;
        }



    }
}
