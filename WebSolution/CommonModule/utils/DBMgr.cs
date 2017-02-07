using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntLibContrib.Data.MySql;

using System.Data;
using System.Data.Common;

namespace CommonModule
{
    public class DBREQ_PARAM
    {
        public DBREQ_PARAM(string _ParameterName, System.Data.DbType _DBType, object _value)
        {
            ParameterName = _ParameterName;
            DBType = _DBType;
            Value = _value;
        }
        public string ParameterName;
        public System.Data.DbType DBType;
        public object Value;
    }

    public class DBMgr
    {
        private string connectionString = "";
        private int dbTimeout = 600;

        public void SetConnectionString(string conStr)
        {
            connectionString = conStr;
        }        

        protected MySqlDatabase CreateDB()
        {
            //Database db = DatabaseFactory.CreateDatabase("MGPRM");  DatabaseFactory Simple but, JavaApplication can't Access app.config, can't Apply Setting at AutoBuild Time
            return new MySqlDatabase(connectionString);
        }

        string ToString_Params(List<DBREQ_PARAM> dbParams)
        {
            StringBuilder resultBuilder = new StringBuilder();
            foreach (DBREQ_PARAM param in dbParams)
            {
                resultBuilder.Append(string.Format("{0}:{1} ", param.ParameterName, param.Value.ToString()));
            }
            return resultBuilder.ToString();
        }

        public DataTable RunSQL(string sqltext,bool needResult = true)
        {
            DataTable result = null;
            DataSet dataSet = null;

            MySqlDatabase db = CreateDB();

            using (DbCommand cmd = db.GetSqlStringCommand(sqltext))
            {
                cmd.CommandTimeout = dbTimeout;
                dataSet = db.ExecuteDataSet(cmd);
                //IDataReader dr = db.ExecuteReader(sqltext);
                if (dataSet == null)
                {
                    
                    //LogUtil.Instance.writeLog(LOGLEVELTYPE.FATAL, "SYSTEM", "LSSOCK", string.Format("DBError SPName:{0} Param:{1}", SPName, arguments), "DBModule::RunSP");
                    //throw new PokerAPIException("undefined", API_ERRTYPE.INTERNAL_ERROR, "sp error");
                    throw new Exception("DBSPError:dataset null");
                }
                else
                {
                    if (!needResult)
                    {
                        result = null;
                    }
                    else
                    {
                        if (dataSet.Tables[0].Rows.Count == 0)
                        {
                            result = null;
                        }
                        else
                        {
                            result = dataSet.Tables[0];
                        }
                    }                    
                }
            }            

            return result;

        }

        protected DataTable RunSP(string SPName, MySqlDatabase db, List<DBREQ_PARAM> dbParams, bool needResult = true)
        {
            DataTable result = null;
            DataSet dataSet = null;

            using (DbCommand cmd = db.GetStoredProcCommand(SPName))
            {
                foreach (DBREQ_PARAM param in dbParams)
                {
                    db.AddInParameter(cmd, param.ParameterName, param.DBType, param.Value);
                }
                string arguments = ToString_Params(dbParams);

                //LogUtil.Instance.writeLog(LOGLEVELTYPE.DEBUG, "SYSTEM", "LSSOCK", string.Format("Try SP SPName:{0} Param:{1}", SPName, arguments), "DBModule::RunSP");
                cmd.CommandTimeout = dbTimeout;

                try
                {
                    dataSet = db.ExecuteDataSet(cmd);
                }
                catch (System.Exception ex)
                {
                    //LogUtil.Instance.writeLog(LOGLEVELTYPE.FATAL, "SYSTEM", "LSSOCK", string.Format("DBError SPName:{0} Param:{1} Error:{2}", SPName, arguments, ex.ToString()), "DBModule::RunSP");                    
                    //throw new PokerAPIException("undefined", API_ERRTYPE.INTERNAL_ERROR, "sp error");
                    throw new Exception("DBSPError:"+ ex.Message);
                }

                if (dataSet == null)
                {

                    //LogUtil.Instance.writeLog(LOGLEVELTYPE.FATAL, "SYSTEM", "LSSOCK", string.Format("DBError SPName:{0} Param:{1}", SPName, arguments), "DBModule::RunSP");
                    //throw new PokerAPIException("undefined", API_ERRTYPE.INTERNAL_ERROR, "sp error");
                    throw new Exception("DBSPError:dataset null");
                }
                else
                {
                    if (!needResult)
                    {
                        result = null;
                    }
                    else
                    {
                        if (dataSet.Tables[0].Rows.Count == 0)
                        {
                            result = null;
                        }
                        else
                        {
                            result = dataSet.Tables[0];
                        }
                    }
                }
            }
            return result;
        }
    }
}
