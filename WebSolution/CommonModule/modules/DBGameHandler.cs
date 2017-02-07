using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CommonModule.data;

namespace CommonModule
{
    public class DBGameHandler : DBMgr
    {
        public DBGameHandler(string _conStr)
        {
            SetConnectionString(_conStr);
        }

        //가위바위보게임
        public WebOK sp_creategame(int mainType,int gameType,string gameTitle,string gameMemo_left,int gameCount
            ,int gameMoneyAmount,string gameFlag_left1,int appendtime,string creatorAuthToken)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;

            //Default Value for Reserved..
            int authType = 0;
            int gameState = 0;
            string gameToken = "NAN";
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("mainType", DbType.Int32, mainType));
            dbParams.Add(new DBREQ_PARAM("gameType", DbType.Int32, gameType));
            dbParams.Add(new DBREQ_PARAM("authType", DbType.Int32, authType));
            dbParams.Add(new DBREQ_PARAM("gameState", DbType.Int32, gameState));
            dbParams.Add(new DBREQ_PARAM("gameTitle", DbType.String, gameTitle));
            dbParams.Add(new DBREQ_PARAM("gameMemo_left", DbType.String, gameMemo_left));
            dbParams.Add(new DBREQ_PARAM("gameToken", DbType.String, gameToken));
            dbParams.Add(new DBREQ_PARAM("gameCount", DbType.Int32, gameCount));
            dbParams.Add(new DBREQ_PARAM("gameMoneyAmount", DbType.Int32, gameMoneyAmount));
            dbParams.Add(new DBREQ_PARAM("gameFlag_left1", DbType.String, gameFlag_left1));
            dbParams.Add(new DBREQ_PARAM("addendtime", DbType.Int32, appendtime));
            dbParams.Add(new DBREQ_PARAM("creatorAuthToken", DbType.String, creatorAuthToken));
            DataTable result = RunSP("sp_creategame", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }            
            return resultValue;            
        }

        public WebOK sp_rungame(int gameID,string gameFlag_right1,string playerAuthToken
            ,string gameMemo_right,int whoWinner,string userIP)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("gameID", DbType.Int32, gameID));
            dbParams.Add(new DBREQ_PARAM("gameFlag_right1", DbType.String, gameFlag_right1));
            dbParams.Add(new DBREQ_PARAM("playerAuthToken", DbType.String, playerAuthToken));
            dbParams.Add(new DBREQ_PARAM("gameMemo_right", DbType.String, gameMemo_right));
            dbParams.Add(new DBREQ_PARAM("whoWinner", DbType.Int32, whoWinner));
            dbParams.Add(new DBREQ_PARAM("userIP", DbType.String, userIP));
            DataTable result = RunSP("sp_rungame", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }

        public WebOK sp_runPublicHandGame(int gameID, string gameFlag_right1, string memberNick_right
            , string gameMemo_right, int whoWinner)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("gameID", DbType.Int32, gameID));
            dbParams.Add(new DBREQ_PARAM("gameFlag_right1", DbType.String, gameFlag_right1));
            dbParams.Add(new DBREQ_PARAM("memberNick_right", DbType.String, memberNick_right));
            dbParams.Add(new DBREQ_PARAM("gameMemo_right", DbType.String, gameMemo_right));
            dbParams.Add(new DBREQ_PARAM("whoWinner", DbType.Int32, whoWinner));
            DataTable result = RunSP("sp_runPublicHandGame", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }

        //인디언홀덤
        public GameInfo sp_CreateIndianHoldem(int mainType,string gameTitle,string gameData,int gameCount,int gameMoneyAmount,string creatorAuthToken)
        {
            GameInfo resultValue = new GameInfo();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("mainType", DbType.Int32, mainType));
            dbParams.Add(new DBREQ_PARAM("gameTitle", DbType.String, gameTitle));
            dbParams.Add(new DBREQ_PARAM("gameData", DbType.String, gameData));
            dbParams.Add(new DBREQ_PARAM("gameCount", DbType.Int32, gameCount));
            dbParams.Add(new DBREQ_PARAM("gameMoneyAmount", DbType.Int32, gameMoneyAmount));
            dbParams.Add(new DBREQ_PARAM("creatorAuthToken", DbType.String, creatorAuthToken));
            DataTable result = RunSP("sp_CreateIndianHoldem", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.gameID = result.Rows[0].Field<int>("_gameID");
                resultValue.statusCode = 200;
                resultValue.gameData = gameData;
                resultValue.gameMoneyAmount = gameMoneyAmount;
                resultValue.gameType = (int)eGameType.IndianHoldem;

                string[] gameDataArr = gameData.Split(':');
                resultValue.comcard1 = int.Parse(gameDataArr[0]);
                resultValue.comcard2 = int.Parse(gameDataArr[1]);
                resultValue.cratorcard = int.Parse(gameDataArr[2]);
            }
            else
            {
                resultValue.statusCode = spError;                
            }
            return resultValue;

        }

        public WebOK sp_EdtIndianHoldem(int gameID,int gameAction,string creatorAuthToken)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();            
            dbParams.Add(new DBREQ_PARAM("gameID", DbType.Int32, gameID));
            dbParams.Add(new DBREQ_PARAM("gameAction", DbType.Int32, gameAction));
            dbParams.Add(new DBREQ_PARAM("creatorAuthToken", DbType.String, creatorAuthToken));
            DataTable result = RunSP("sp_EdtIndianHoldem", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }

        public WebOK sp_RunIndianHoldem(int gameID,string joinerGameData,string joinerMemo,string joinerAuthToken,int whoWinner,int gameAction,
                string creatorHandStr,string joinerHandStr,string userIP)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("gameID", DbType.Int32, gameID));
            dbParams.Add(new DBREQ_PARAM("joinerGameData", DbType.String, joinerGameData));
            dbParams.Add(new DBREQ_PARAM("joinerMemo", DbType.String, joinerMemo));
            dbParams.Add(new DBREQ_PARAM("joinerAuthToken", DbType.String, joinerAuthToken));
            dbParams.Add(new DBREQ_PARAM("whoWinner", DbType.Int32, whoWinner));
            dbParams.Add(new DBREQ_PARAM("gameAction", DbType.Int32, gameAction));
            dbParams.Add(new DBREQ_PARAM("creatorHandStr", DbType.String, creatorHandStr));
            dbParams.Add(new DBREQ_PARAM("joinerHandStr", DbType.String, joinerHandStr));
            dbParams.Add(new DBREQ_PARAM("userIP", DbType.String, userIP));


            DataTable result = RunSP("sp_RunIndianHoldem", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;

        }

        public WebOK sp_RunPublicIndianHoldem(int gameID, string joinerGameData, string joinerMemo, int whoWinner, int gameAction, string creatorHandStr, string joinerHandStr,
                string joinerNick, string joinerAvatar)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("gameID", DbType.Int32, gameID));
            dbParams.Add(new DBREQ_PARAM("joinerGameData", DbType.String, joinerGameData));
            dbParams.Add(new DBREQ_PARAM("joinerMemo", DbType.String, joinerMemo));            
            dbParams.Add(new DBREQ_PARAM("whoWinner", DbType.Int32, whoWinner));
            dbParams.Add(new DBREQ_PARAM("gameAction", DbType.Int32, gameAction));
            dbParams.Add(new DBREQ_PARAM("creatorHandStr", DbType.String, creatorHandStr));
            dbParams.Add(new DBREQ_PARAM("joinerHandStr", DbType.String, joinerHandStr));
            dbParams.Add(new DBREQ_PARAM("joinerNick", DbType.String, joinerNick));
            dbParams.Add(new DBREQ_PARAM("joinerAvatar", DbType.String, joinerAvatar));


            DataTable result = RunSP("sp_RunPublicIndianHoldem", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;

        }

        //Q홀덤
        public GameInfo sp_CreateQHoldem(int mainType, string gameTitle, string gameData, int gameCount, int gameMoneyAmount, string creatorAuthToken)
        {
            GameInfo resultValue = new GameInfo();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("mainType", DbType.Int32, mainType));
            dbParams.Add(new DBREQ_PARAM("gameTitle", DbType.String, gameTitle));
            dbParams.Add(new DBREQ_PARAM("gameData", DbType.String, gameData));
            dbParams.Add(new DBREQ_PARAM("gameCount", DbType.Int32, gameCount));
            dbParams.Add(new DBREQ_PARAM("gameMoneyAmount", DbType.Int32, gameMoneyAmount));
            dbParams.Add(new DBREQ_PARAM("creatorAuthToken", DbType.String, creatorAuthToken));
            DataTable result = RunSP("sp_CreateQHoldem", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.gameID = result.Rows[0].Field<int>("_gameID");
                resultValue.statusCode = 200;
                resultValue.gameData = gameData;
                resultValue.gameMoneyAmount = gameMoneyAmount;
                resultValue.gameType = (int)eGameType.QHoldem;                
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;

        }
        
        public WebOK sp_RunQHoldem(int gameID, int joinerGameData,string commCardData, string joinerMemo, string joinerAuthToken, int whoWinner,
                string creatorHandStr, string joinerHandStr)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("gameID", DbType.Int32, gameID));
            dbParams.Add(new DBREQ_PARAM("joinerGameData", DbType.Int32, joinerGameData));
            dbParams.Add(new DBREQ_PARAM("commCardData", DbType.String, commCardData));
            dbParams.Add(new DBREQ_PARAM("joinerMemo", DbType.String, joinerMemo));
            dbParams.Add(new DBREQ_PARAM("joinerAuthToken", DbType.String, joinerAuthToken));
            dbParams.Add(new DBREQ_PARAM("whoWinner", DbType.Int32, whoWinner));            
            dbParams.Add(new DBREQ_PARAM("creatorHandStr", DbType.String, creatorHandStr));
            dbParams.Add(new DBREQ_PARAM("joinerHandStr", DbType.String, joinerHandStr));

            DataTable result = RunSP("sp_RunQHoldem", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }

        public WebOK sp_RunPublicQHoldem(int gameID, int joinerGameData,string commCardData, string joinerMemo, int whoWinner , string creatorHandStr, string joinerHandStr,
                string joinerNick, string joinerAvatar)
        {
            WebOK resultValue = new WebOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("gameID", DbType.Int32, gameID));
            dbParams.Add(new DBREQ_PARAM("joinerGameData", DbType.Int32, joinerGameData));
            dbParams.Add(new DBREQ_PARAM("joinerMemo", DbType.String, joinerMemo));
            dbParams.Add(new DBREQ_PARAM("commCardData", DbType.String, commCardData));
            dbParams.Add(new DBREQ_PARAM("whoWinner", DbType.Int32, whoWinner));            
            dbParams.Add(new DBREQ_PARAM("creatorHandStr", DbType.String, creatorHandStr));
            dbParams.Add(new DBREQ_PARAM("joinerHandStr", DbType.String, joinerHandStr));
            dbParams.Add(new DBREQ_PARAM("joinerNick", DbType.String, joinerNick));
            dbParams.Add(new DBREQ_PARAM("joinerAvatar", DbType.String, joinerAvatar));


            DataTable result = RunSP("sp_RunPublicQHoldem", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;

        }


        //기타정보
        public GameInfo getGameInfo(int gameType,int gameID)
        {
            GameInfo resultValue = null;
            switch (gameType)
            {
                case (int)eGameType.HandGame:
                    {
                        string sqltext = string.Format("select * from tblHandGameList where gameID={0};", gameID);
                        DataTable result = RunSQL(sqltext);
                        if (result != null)
                        {
                            resultValue = new GameInfo();
                            resultValue.gameType = gameType;
                            resultValue.gameID = (int)result.Rows[0].Field<int>("gameID");                            
                            resultValue.gameData = (string)result.Rows[0].Field<string>("gameFlag_left1");
                            resultValue.gameMoneyAmount = (int)result.Rows[0].Field<int>("gameMoneyAmount");
                        }

                    }
                    break;
                case (int)eGameType.IndianHoldem:
                    {
                        string sqltext = string.Format("select * from tblindianholdemlist where gameID={0};", gameID);
                        DataTable result = RunSQL(sqltext);
                        if (result != null)
                        {
                            resultValue = new GameInfo();
                            resultValue.gameType = gameType;
                            resultValue.gameID = (int)result.Rows[0].Field<int>("gameID");                            
                            resultValue.gameData = (string)result.Rows[0].Field<string>("gameData");
                            resultValue.gameMoneyAmount = (int)result.Rows[0].Field<int>("gameMoneyAmount");
                        }
                    }
                    break;
                case (int)eGameType.QHoldem:
                    {
                        string sqltext = string.Format("select * from tblQHoldemlist where gameID={0};", gameID);
                        DataTable result = RunSQL(sqltext);
                        if (result != null)
                        {
                            resultValue = new GameInfo();
                            resultValue.gameType = gameType;
                            resultValue.gameID = (int)result.Rows[0].Field<int>("gameID");
                            resultValue.gameData = (string)result.Rows[0].Field<string>("gameData");
                            resultValue.gameMoneyAmount = (int)result.Rows[0].Field<int>("gameMoneyAmount");
                        }
                    }
                    break;
            }
            return resultValue;
        }

        public BonusOK sp_tabbonus(string playerAuthToken)
        {
            BonusOK resultValue = new BonusOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();            
            dbParams.Add(new DBREQ_PARAM("playerAuthToken", DbType.String, playerAuthToken));            
            DataTable result = RunSP("sp_tabbonus", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
                resultValue.bonusType = 1;
                resultValue.bonusPoint = result.Rows[0].Field<int>("Bonus");
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }

        public BonusOK sp_daybonus(string playerAuthToken)
        {
            BonusOK resultValue = new BonusOK();
            int spError = -1;
            List<DBREQ_PARAM> dbParams = new List<DBREQ_PARAM>();
            dbParams.Add(new DBREQ_PARAM("playerAuthToken", DbType.String, playerAuthToken));
            DataTable result = RunSP("sp_daybonus", CreateDB(), dbParams);
            spError = result.Rows[0].Field<int>("Result");
            if (spError == 0)
            {
                resultValue.statusCode = 200;
                resultValue.bonusType = 1;
                resultValue.bonusPoint = result.Rows[0].Field<int>("BonusPoint");
                resultValue.bonusCoins = result.Rows[0].Field<int>("BonusCoin");
            }
            else
            {
                resultValue.statusCode = spError;
            }
            return resultValue;
        }
        
    }
}
