using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using CommonModule.data;

namespace CommonModule
{
    public class DBInfomationHandler : DBMgr
    {
        public DBInfomationHandler(string _conStr)
        {
            SetConnectionString(_conStr);
        }

        public UserInfo sp_userinfo(string _authtoken,int userNO=0)
        {
            UserInfo resultValue = new UserInfo();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("_authtoken", DbType.String, _authtoken));
            dbParams.Add(new DBREQ_PARAM("userNO", DbType.Int32, userNO));
            DataTable result = RunSP("sp_userinfo", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
                resultValue.Nick = result.Rows[0].Field<string>("Nick");
                resultValue.PicUrl = result.Rows[0].Field<string>("PicUrl");
                resultValue.GamePoint = result.Rows[0].Field<int>("GamePoint");
                resultValue.CoinCount = result.Rows[0].Field<int>("CoinCount");
                resultValue.WinCount = result.Rows[0].Field<int>("winCount");
                resultValue.LoseCount = result.Rows[0].Field<int>("loseCount");
                resultValue.MemberID = result.Rows[0].Field<int>("MemberID");                
            }
            else
            {
                resultValue.statusCode = spError;
            }

            return resultValue;
        }

        public int sp_earnpoint(string _authtoken, int userNO, string monthData)
        {
            int EarnPointTotal = 0;                        
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("_authtoken", DbType.String, _authtoken));
            dbParams.Add(new DBREQ_PARAM("userNO", DbType.Int32, userNO));
            dbParams.Add(new DBREQ_PARAM("monthData", DbType.String, monthData));
            DataTable result = RunSP("sp_earnpoint", CreateDB(), dbParams);
            int spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                EarnPointTotal = result.Rows[0].Field<int>("EarnPointTotal");
            }            
            return EarnPointTotal;
        }

        public List<RankListInfo> sp_earnpointList(string monthData , int gameType)
        {
            List<RankListInfo> resultValue = new List<RankListInfo>();
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("monthData", DbType.String, monthData));
            dbParams.Add(new DBREQ_PARAM("gameType", DbType.Int32, gameType));            
            DataTable result = RunSP("sp_earnpointList", CreateDB(), dbParams);
            if (result != null)
            {
                foreach (DataRow gameInfoRow in result.Rows)
                {
                    RankListInfo addInfo = new RankListInfo(gameInfoRow);
                    resultValue.Add(addInfo);
                }
            }

            return resultValue;
        }

        public List<HandGameListInfo>   GetHandGameList(  int startIdx , int endIdx, int mainType=0,int gameID=0 , int creatorID=0)
        {
            List<HandGameListInfo> resultValue = new List<HandGameListInfo>();
            string sqlText = string.Format("select * from tblHandGameList left join tblHandGameDetail on tblHandGameDetail.gameID = tblHandGameList.gameID ");            
            string addCondition = "";

            
            addCondition = addCondition + string.Format(" and tblHandGameList.mainType={0}", mainType);

            if (gameID > 0)
                addCondition = addCondition + string.Format(" and tblHandGameList.gameID={0}", gameID);

            if(creatorID > 0)
                addCondition = addCondition + string.Format(" and tblHandGameList.memberID_left={0}", creatorID);

            string selCondition = string.Format("where tblHandGameList.gameState=0 {2} ORDER  BY tblHandGameList.gameID DESC ,tblHandGameDetail.subGameID LIMIT {0}, {1};", startIdx, endIdx, addCondition);

            string sqlLast = sqlText + selCondition;

            DataTable result = RunSQL(sqlLast);        
            if(result!=null)
            {
                foreach (DataRow gameInfoRow in result.Rows)
                {
                    HandGameListInfo addInfo = new HandGameListInfo(gameInfoRow);
                    resultValue.Add(addInfo);
                }

            }

            
            return resultValue;
        }

        public List<HandGameListInfoCompleted> GetHandGameListCompleted(int startIdx, int endIdx, int mainType = 0, int gameID = 0, int creatorID = 0)
        {
            List<HandGameListInfoCompleted> resultValue = new List<HandGameListInfoCompleted>();
            string sqlText = string.Format("select * from tblHandGameList left join tblHandGameDetail on tblHandGameDetail.gameID = tblHandGameList.gameID ");            
            string addCondition = "";

            
            addCondition = addCondition + string.Format(" and tblHandGameList.mainType={0}", mainType);

            if (gameID > 0)
                addCondition = addCondition + string.Format(" and tblHandGameList.gameID={0}", gameID);

            if (creatorID > 0)
                addCondition = addCondition + string.Format(" and tblHandGameList.memberID_left={0}", creatorID);

            string selCondition = string.Format("where tblHandGameList.gameState=1 {2} ORDER  BY tblHandGameList.gameID DESC ,tblHandGameDetail.subGameID LIMIT {0}, {1};", startIdx, endIdx, addCondition);
            string sqlLast = sqlText + selCondition;
            DataTable result = RunSQL(sqlLast);

            if (result != null)
            {
                foreach (DataRow gameInfoRow in result.Rows)
                {
                    HandGameListInfoCompleted addInfo = new HandGameListInfoCompleted(gameInfoRow);
                    resultValue.Add(addInfo);
                }

            }

            return resultValue;
        }

        public List<IndianHoldemListInfo> GetIndianHoldemGameList(int startIdx, int endIdx,int mainType,int gameState, int gameID = 0, int creatorID = 0)
        {
            List<IndianHoldemListInfo> resultValue = new List<IndianHoldemListInfo>();
            string sqlText = string.Format("select * from tblindianholdemlist left join tblindianholdemdetail on tblindianholdemlist.gameID = tblindianholdemdetail.gameID ");

            string addCondition = "";            
            addCondition = addCondition + string.Format(" and tblindianholdemlist.mainType={0}", mainType);
            if (gameID > 0)
                addCondition = addCondition + string.Format(" and tblindianholdemlist.gameID={0}", gameID);
            if (creatorID > 0)
                addCondition = addCondition + string.Format(" and tblindianholdemlist.creatorID={0}", creatorID);


            string selCondition = string.Format("where tblindianholdemlist.gameState={2} {3}  ORDER  BY tblindianholdemlist.gameID DESC ,tblindianholdemdetail.subGameID LIMIT {0}, {1};", startIdx, endIdx, gameState, addCondition);
            string sqlLast = sqlText + selCondition;

            DataTable result = RunSQL(sqlLast);
            if (result != null)
            {
                foreach (DataRow gameInfoRow in result.Rows)
                {
                    IndianHoldemListInfo addInfo = new IndianHoldemListInfo(gameInfoRow);
                    resultValue.Add(addInfo);
                }
            }            
            return resultValue;
        }

        public List<QHoldemListInfo> GetQHoldemGameList(int startIdx, int endIdx, int mainType, int gameState, int gameID = 0, int creatorID = 0)
        {
            List<QHoldemListInfo> resultValue = new List<QHoldemListInfo>();
            string sqlText = string.Format("select * from tblQHoldemlist left join tblQHoldemdetail on tblQHoldemlist.gameID = tblQHoldemdetail.gameID ");

            string addCondition = "";
            addCondition = addCondition + string.Format(" and tblQHoldemlist.mainType={0}", mainType);
            if (gameID > 0)
                addCondition = addCondition + string.Format(" and tblQHoldemlist.gameID={0}", gameID);
            if (creatorID > 0)
                addCondition = addCondition + string.Format(" and tblQHoldemlist.creatorID={0}", creatorID);

            string selCondition = string.Format("where tblQHoldemlist.gameState={2} {3}  ORDER  BY tblQHoldemlist.gameID DESC ,tblQHoldemdetail.subGameID LIMIT {0}, {1};", startIdx, endIdx, gameState, addCondition);
            string sqlLast = sqlText + selCondition;

            DataTable result = RunSQL(sqlLast);
            if (result != null)
            {
                foreach (DataRow gameInfoRow in result.Rows)
                {
                    QHoldemListInfo addInfo = new QHoldemListInfo(gameInfoRow);
                    resultValue.Add(addInfo);
                }
            }
            return resultValue;
        }

        //QHoldemListInfo

        public List<AdverInfo> GetAdverList()
        {
            List<AdverInfo> resultValue = new List<AdverInfo>();
            string sqlText = string.Format("select * from tbladverlist ");
            string selCondition = ";";
            string sqlLast = sqlText + selCondition;
            DataTable result = RunSQL(sqlLast);
            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    AdverInfo addInfo = new AdverInfo(row);
                    resultValue.Add(addInfo);
                }                
            }
            return resultValue;
        }

    }
}
