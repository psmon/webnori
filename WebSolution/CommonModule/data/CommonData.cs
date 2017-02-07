using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Data;
namespace CommonModule.data
{
    public enum eGameType:int { HandGame=0,IndianHoldem=1,QHoldem=2,BlackJack=3,Jusawi=4 };

    public class BaseWebResult
    {
        public int statusCode = 200;
        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }

    public class WebException : Exception
    {
        public int statusCode = 500;
        public WebException(string message,int _statusCode)
            : base(message)
        {
            statusCode = _statusCode;
        }

        public override string ToString()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }

    public class WebOK : BaseWebResult
    { }

    public class LoginOK : BaseWebResult
    {
        public string authToken;
        public LoginOK()
        {

        }
        public LoginOK(int _statusCode,string _authToken)            
        {
            statusCode = _statusCode;
            authToken = _authToken;
        }
    }

    public class BonusOK : BaseWebResult
    {
        public int bonusType;
        public int bonusPoint;
        public int bonusCoins;
    }

    public class UserInfo : BaseWebResult
    {
        public string Nick;
        public string PicUrl;
        public string Email;
        public int GamePoint;
        public int CoinCount;
        public int WinCount;
        public int LoseCount;
        public int MemberID;
        public int MonthPoint;

    }

    public class DBLoginInfo : BaseWebResult
    {
        public int MemberID;
        public int SocialType;
        public string SocialID;
        public string Nick;
        public string PicUrl;
        public string Email;
        public int Createtime;

        public DBLoginInfo()
        {

        }
    }

    public class GameResult : BaseWebResult
    {
        public int whoWinner;
        public int gameMoneyAmount;
        public string gameFlag_left1 = "";
        public string gameFlag_right1 = "";
        public string result_left = "";
        public string result_right = "";
    }

    public class AvartarInfo
    {
        public AvartarInfo(int _itemNo,string _imgUrl)
        {
            itemNo = _itemNo;
            imgUrl = _imgUrl;
        }
        public int itemNo;
        public string imgUrl;        
    }


    public class GameInfo : BaseWebResult
    {
        public int gameID;        
        public string gameData;
        public int gameMoneyAmount;
        public int gameType;
        public int comcard1;
        public int comcard2;
        public int cratorcard;
    }

    public class GameHandInfo
    {
        public int HandScore;
        public string HandTxt;
    }
    
    public class HandGameListInfo
    {
        public HandGameListInfo(DataRow data)
        {
            gameID = data.Field<int>("gameID");
            mainType = data.Field<int>("mainType");
            gameType = data.Field<int>("gameType");
            authType = data.Field<int>("authType");
            gameState = data.Field<int>("gameState");
            gameTitle = data.Field<string>("gameTitle");
            memberID_left = data.Field<int>("memberID_left");
            memberNick_left = data.Field<string>("memberNick_left");
            gameMemo_left = data.Field<string>("gameMemo_left");
            gameToken = data.Field<string>("gameToken");
            gameCount = data.Field<int>("gameCount");
            curGameIdx = data.Field<int>("curGameIdx");
            gameMoneyAmount = data.Field<int>("gameMoneyAmount");
            lockPoint = data.Field<int>("lockPoint");
            
            createtime = data.Field<int>("createtime");
            endtime = data.Field<int>("endtime");

            if (curGameIdx != 0)
            {
                subGameID = data.Field<int>("subGameID");                
                memberID_right = data.Field<int>("memberID_right");
                memberNick_right = data.Field<string>("memberNick_right");
                gameMemo_right = data.Field<string>("gameMemo_right");
                winnerID = data.Field<int>("winnerID");
                earnPoint = data.Field<int>("earnPoint");

                if (winnerID == 0)
                    whowiner = "무";
                else if(winnerID == memberID_right)
                    whowiner = "승";
                else
                    whowiner = "패";

                isJoinGame = true;

            }
            else
            {
                subGameID = -1;
                isJoinGame = true;
            }
        }        

        public int gameID;
        public int mainType;
        public int gameType;
        public int authType;
        public int gameState;
        public string gameTitle;
        public int memberID_left;
        public string memberNick_left;
        public string gameMemo_left;
        public string gameToken;
        public int gameCount;
        public int curGameIdx;
        public int gameMoneyAmount;
        public int lockPoint;
        
        public int createtime;
        public int endtime;
        //gameID1 
        public int subGameID;        
        public int memberID_right;
        public string memberNick_right;
        public string gameMemo_right;
        public int winnerID;
        public int earnPoint;

        public string whowiner;

        public bool isJoinGame = false;
    }

    public class HandGameListInfoCompleted : HandGameListInfo
    {
        public HandGameListInfoCompleted(DataRow data) : base(data)
        {
            isJoinGame = false;
            gameFlag_left1 = data.Field<string>("gameFlag_left1");
            gameFlag_right1 = data.Field<string>("gameFlag_right1");

            leftImg = "/img/game0_" + gameFlag_left1 + ".png";
            rightImg = "/img/game0_" + gameFlag_right1 + ".png";

        }
        public string gameFlag_left1;        
        public string gameFlag_right1;

        public string leftImg;
        public string rightImg;

    }

    public class HandGamePlayList
    {
        public HandGameListInfo header;
        public List<HandGameListInfo> list;
    }

    public class HandGamePlayCompletedList
    {
        public HandGameListInfoCompleted header;
        public List<HandGameListInfoCompleted> list;
    }

    public class IndianHoldemListInfo
    {
        public IndianHoldemListInfo(DataRow data)
        {
            gameID = data.Field<int>("gameID");
            gameState = data.Field<int>("gameState");
            gameTitle = data.Field<string>("gameTitle");
            creatorID = data.Field<int>("creatorID");
            creatorNick = data.Field<string>("creatorNick");
            gameData = data.Field<string>("gameData");

            string[] gameDataArr = gameData.Split(':');
            comcard1 = int.Parse(gameDataArr[0]);
            comcard2 = int.Parse(gameDataArr[1]);
            //joinercard = int.Parse(gameDataArr[2]);
            cratorcard = int.Parse(gameDataArr[2]);

            gameCount = data.Field<int>("gameCount");
            curGameIdx = data.Field<int>("curGameIdx");
            gameMoneyAmount = data.Field<int>("gameMoneyAmount");
            creatorAvatar = data.Field<string>("creatorAvatar");
                        
            if (curGameIdx > 0)
            {
                subGameID = data.Field<int>("subGameID");
                joinerID = data.Field<int>("joinerID");
                joinerNick = data.Field<string>("joinerNick");
                joinerAvatar = data.Field<string>("joinerAvatar");
                joinerMemo = data.Field<string>("joinerMemo");
                joinerGameData = data.Field<string>("joinerGameData");
                //cratorcard = int.Parse(joinerGameData);
                joinercard = int.Parse(joinerGameData);
                earnPoint = data.Field<int>("earnPoint");
                winnerID = data.Field<int>("winnerID");
                action = data.Field<int>("action");

                creatorHandStr = data.Field<string>("creatorHandStr");
                joinerHandStr = data.Field<string>("joinerHandStr");
            }
            else
            {
                subGameID = -1;
            }

        }

        public int gameID;
        public int gameState;
        public string gameTitle;
        public int creatorID;
        public string creatorNick;
        public string gameData;
        public int gameCount;
        public int curGameIdx;
        public int gameMoneyAmount;
        public string creatorAvatar;
        public int subGameID;
        public int joinerID;
        public string joinerNick;
        public string joinerAvatar;
        public string joinerMemo;
        public string joinerGameData;
        public int earnPoint;
        public int winnerID;

        public int comcard1;
        public int comcard2;
        public int joinercard;
        public int cratorcard;
        public int action;

        public string creatorHandStr;
        public string joinerHandStr;


        //public bool isJoinGame = false;

    }    

    public class IndianHoldemListInfoList
    {
        public IndianHoldemListInfo header;
        public List<IndianHoldemListInfo> list;
    }

    public class QHoldemListInfo
    {
        public QHoldemListInfo(DataRow data)
        {
            gameID = data.Field<int>("gameID");
            gameState = data.Field<int>("gameState");
            gameTitle = data.Field<string>("gameTitle");
            creatorID = data.Field<int>("creatorID");
            creatorNick = data.Field<string>("creatorNick");
            gameData = data.Field<string>("gameData");

            string[] gameDataArr = gameData.Split('-');

            gameCount = data.Field<int>("gameCount");
            curGameIdx = data.Field<int>("curGameIdx");
            gameMoneyAmount = data.Field<int>("gameMoneyAmount");
            creatorAvatar = data.Field<string>("creatorAvatar");

            if (curGameIdx > 0)
            {
                subGameID = data.Field<int>("subGameID");
                joinerID = data.Field<int>("joinerID");
                joinerNick = data.Field<string>("joinerNick");
                joinerAvatar = data.Field<string>("joinerAvatar");
                joinerMemo = data.Field<string>("joinerMemo");
                joinerGameData = data.Field<int>("joinerGameData");
                commCardData = data.Field<string>("commCardData");

                if (joinerGameData == 0)
                {
                    joinercard = gameDataArr[0];
                    cratorcard = gameDataArr[1];
                }
                else
                {
                    joinercard = gameDataArr[1];
                    cratorcard = gameDataArr[0];
                }                

                earnPoint = data.Field<int>("earnPoint");
                winnerID = data.Field<int>("winnerID");                

                creatorHandStr = data.Field<string>("creatorHandStr");
                joinerHandStr = data.Field<string>("joinerHandStr");
            }
            else
            {
                subGameID = -1;
            }

        }

        public int gameID;
        public int gameState;
        public string gameTitle;
        public int creatorID;
        public string creatorNick;
        public string gameData;
        public int gameCount;
        public int curGameIdx;
        public int gameMoneyAmount;
        public string creatorAvatar;
        public int subGameID;
        public int joinerID;
        public string joinerNick;
        public string joinerAvatar;
        public string joinerMemo;
        public int joinerGameData;
        public int earnPoint;
        public int winnerID;

        public string commCardData;
        public string joinercard;
        public string cratorcard;

        public string creatorHandStr;
        public string joinerHandStr;
        //public bool isJoinGame = false;
    }

    public class QHoldemListInfoList
    {
        public QHoldemListInfo header;
        public List<QHoldemListInfo> list;
    }


    public class RankListInfo
    {
        public RankListInfo(DataRow data)
        {
            earnPointTotal = Convert.ToInt32(data["earnPointTotal"]);
            nick = data.Field<string>("nick");
            picurl = data.Field<string>("picurl");
        }

        public int earnPointTotal;
        public string nick;
        public string picurl;
    }

    public class RankListInfoList
    {
        public string currentMonth;
        public List<RankListInfo> list = new List<RankListInfo>();
    }

    public class FBSahareModel
    {
        public FBSahareModel()
        {
        }

        public int pageID;
        public string pageTitle;
        public string pageContent;
        public string pageImg;
        public string pageUrl;
        public string appid;
        public string popupurl;
        public string langfile;
        public string langcode;
    }

    public class AdverInfo
    {
        public AdverInfo(DataRow data)
        {
            adverID = data.Field<int>("adverID");
            title = data.Field<string>("title");
            mainimageurl = data.Field<string>("mainimageurl");
            content = data.Field<string>("content");
        }
        public int adverID;
        public string title;
        public string mainimageurl;
        public string content;
    }

    public class PageInfo
    {
        public string langcode = "";
        public string langfile = "";

    }


}
