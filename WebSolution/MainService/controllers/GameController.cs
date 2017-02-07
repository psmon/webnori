using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonModule;
using CommonModule.data;
using System.Configuration;
using MainService.core;

#pragma warning disable 1998

namespace MainService.controllers
{
    public class GameController : BaseController
    {
        protected DBGameHandler dbhandler;

        public GameController()
        {
            parentSpace = "/api/game";
            dbhandler = new DBGameHandler(ConfigurationManager.AppSettings.Get("ConectionDBString"));

            Post[parentSpace + "/tabbonus", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();                
                string playerAuthToken = (string)payload["playerAuthToken"];

                BonusOK info = null;

                Task t = Task.Run(() => {
                    info = dbhandler.sp_tabbonus(playerAuthToken);
                });
                await t;

                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }
                
                return info;
            };

            Post[parentSpace + "/daybonus", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                string playerAuthToken = (string)payload["playerAuthToken"];

                BonusOK info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_daybonus(playerAuthToken);
                });
                await t;

                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                return info;
            };

            //가위바위보게임
            Post[parentSpace + "/creategame",true] = async (parameters,ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();                
                int mainType = (int)payload["mainType"];
                int gameType = (int)payload["gameType"];
                string gameTitle = (string)payload["gameTitle"];
                string gameMemo_left = (string)payload["gameMemo_left"];
                int gameCount = (int)payload["gameCount"];
                int gameMoneyAmount = (int)payload["gameMoneyAmount"];
                string gameFlag_left1 = (string)payload["gameFlag_left1"];
                int appendtime = (int)payload["appendtime"];
                string creatorAuthToken = (string)payload["creatorAuthToken"];

                if (mainType == 0)
                {
                    if (gameCount > 5 || gameCount < 1)
                        return new WebException("gameCount Limit", 10);

                    if (gameMoneyAmount > 1000 || gameMoneyAmount < 1)
                        return new WebException("gameMoneyAmount Limit", 11);
                }
                else
                {
                    if (gameCount > 20 || gameCount < 1)
                        return new WebException("gameCount Limit", 20);

                    if (gameMoneyAmount > 1000000000 || gameMoneyAmount < 1)
                        return new WebException("gameMoneyAmount Limit", 11);
                }

                WebOK info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_creategame(mainType, gameType, gameTitle, gameMemo_left, gameCount,
                    gameMoneyAmount, gameFlag_left1, appendtime, creatorAuthToken);
                });
                await t;

                if (info == null)
                {
                    return new WebException("creategame failed", 401);
                }
                return info.ToString();
            };

            Post[parentSpace + "/rungame", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameID = (int)payload["gameID"];
                string gameFlag_right1 = (string)payload["gameFlag_right1"];
                string playerAuthToken = (string)payload["playerAuthToken"];
                string gameMemo_right = (string)payload["gameMemo_right"];
                GameInfo gameInfo = dbhandler.getGameInfo(0,gameID);

                string result1="", result2="";
                int whoWinner = GameReferee.GetWinner(gameInfo.gameType,gameInfo.gameData, gameFlag_right1,ref result1,ref result2);
                WebOK info = null;

                string userIp = Request.UserHostAddress;

                Task t = Task.Run(() => {
                    info = dbhandler.sp_rungame(gameID, gameFlag_right1, playerAuthToken, gameMemo_right, whoWinner, userIp);
                });
                await t;

                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if(info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                GameResult gameResult = new GameResult();
                gameResult.whoWinner = whoWinner;
                gameResult.gameFlag_left1 = gameInfo.gameData;
                gameResult.gameFlag_right1 = gameFlag_right1;
                gameResult.gameMoneyAmount = gameInfo.gameMoneyAmount;

                return gameResult;
            };

            Post[parentSpace + "/runpublichandgame", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameID = (int)payload["gameID"];
                string gameFlag_right1 = (string)payload["gameFlag_right1"];
                string memberNick_right = (string)payload["memberNick_right"];
                string gameMemo_right = (string)payload["gameMemo_right"];
                GameInfo gameInfo = null;
                Task t = Task.Run(() => {
                    gameInfo = dbhandler.getGameInfo(0, gameID);
                });
                await t;
                if (gameInfo == null)
                {
                    return new WebException("rungame failed", 401);
                }

                string result1 = "", result2 = "";
                int whoWinner = GameReferee.GetWinner(gameInfo.gameType, gameInfo.gameData, gameFlag_right1, ref result1, ref result2);
                WebOK info = null;
                Task t2 = Task.Run(() => {
                    info = dbhandler.sp_runPublicHandGame(gameID, gameFlag_right1, memberNick_right, gameMemo_right, whoWinner);
                });
                await t2;

                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                GameResult gameResult = new GameResult();
                gameResult.whoWinner = whoWinner;
                gameResult.gameFlag_left1 = gameInfo.gameData;
                gameResult.gameFlag_right1 = gameFlag_right1;
                gameResult.gameMoneyAmount = gameInfo.gameMoneyAmount;

                return gameResult;
            };

            //인디언홀덤
            Post[parentSpace + "/createIndianHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int mainType = (int)payload["mainType"];
                string gameTitle = (string)payload["gameTitle"];
                //string gameData = (string)payload["gameData"];
                int gameCount = (int)payload["gameCount"];
                int gameMoneyAmount = (int)payload["gameMoneyAmount"];
                string creatorAuthToken = (string)payload["creatorAuthToken"];

                //Get Card
                string gameData = GameReferee.GetIndianGameCard(null);

                if (mainType == 0)
                {
                    if (gameCount > 5 || gameCount < 1)
                        return new WebException("gameCount Limit", 10);

                    if (gameMoneyAmount > 1000 || gameMoneyAmount < 1)
                        return new WebException("gameMoneyAmount Limit", 11);
                }
                else
                {
                    if (gameCount > 20 || gameCount < 1)
                        return new WebException("gameCount Limit", 10);

                    if (gameMoneyAmount > 1000000 || gameMoneyAmount < 1)
                        return new WebException("gameMoneyAmount Limit", 11);
                }
                
                GameInfo info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_CreateIndianHoldem(mainType,gameTitle, gameData, gameCount, gameMoneyAmount, creatorAuthToken);
                });
                await t;
                if (info == null)
                {
                    return new WebException("creategame failed", 401);
                }
                
                return info;
            };

            Post[parentSpace + "/editIndianHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameAction = (int)payload["gameAction"];    // 0:half 1:call 2:double
                string creatorAuthToken = (string)payload["creatorAuthToken"];
                int gameID = (int)payload["gameID"];

                WebOK info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_EdtIndianHoldem(gameID, gameAction, creatorAuthToken);
                });
                await t;
                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }
                return info;
            };

            Post[parentSpace + "/runIndianHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameID = (int)payload["gameID"];
                //string joinerGameData = (string)payload["joinerGameData"];
                string joinerMemo = (string)payload["joinerMemo"];
                string joinerAuthToken = (string)payload["joinerAuthToken"];
                int gameAction = (int)payload["gameAction"];

                GameInfo gameInfo = dbhandler.getGameInfo(1,gameID);

                string joinerGameData = GameReferee.GetIndianGameCard(gameInfo.gameData);

                string joinerHandStr = "";
                string cratorHandStr = "";

                int whoWinner = GameReferee.GetWinner(gameInfo.gameType, gameInfo.gameData, joinerGameData,ref cratorHandStr, ref joinerHandStr);
                //Super Fold
                if (gameAction == 2)
                {
                    if (whoWinner == 1)
                        whoWinner = 2;
                    else if (whoWinner == 2)
                        whoWinner = 1;
                }

                string userIp = Request.UserHostAddress;

                WebOK info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_RunIndianHoldem(gameID, joinerGameData, joinerMemo, joinerAuthToken, whoWinner, gameAction, cratorHandStr, joinerHandStr, userIp);
                });
                await t;
                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                GameResult gameResult = new GameResult();
                gameResult.whoWinner = whoWinner;
                gameResult.gameFlag_left1 = gameInfo.gameData;
                gameResult.gameFlag_right1 = joinerGameData;

                int fixGameMoneyAmount = gameInfo.gameMoneyAmount;
                if (gameAction == 0)
                {
                    fixGameMoneyAmount = fixGameMoneyAmount / 2;
                }
                gameResult.gameMoneyAmount = fixGameMoneyAmount;
                gameResult.result_left = cratorHandStr;
                gameResult.result_right = joinerHandStr;

                return gameResult;
            };

            //Todo:Remove(네이밍 잘못함)
            Post[parentSpace + "/runIndpublicianHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameID = (int)payload["gameID"];
                //string joinerGameData = (string)payload["joinerGameData"];
                string joinerMemo = (string)payload["joinerMemo"];                
                int gameAction = (int)payload["gameAction"];
                string joinerNick = (string)payload["joinerNick"];
                string joinerAvatar = (string)payload["joinerAvatar"];

                GameInfo gameInfo = dbhandler.getGameInfo(1, gameID);

                string joinerGameData = GameReferee.GetIndianGameCard(gameInfo.gameData);

                string joinerHandStr = "";
                string cratorHandStr = "";

                int whoWinner = GameReferee.GetWinner(gameInfo.gameType, gameInfo.gameData, joinerGameData, ref cratorHandStr, ref joinerHandStr);
                //Super Fold
                if (gameAction == 2)
                {
                    if (whoWinner == 1)
                        whoWinner = 2;
                    else if (whoWinner == 2)
                        whoWinner = 1;
                }

                WebOK info = null;
                Task t = Task.Run(() => {
                    //sp_RunPublicIndianHoldem
                    info = dbhandler.sp_RunPublicIndianHoldem(gameID, joinerGameData, joinerMemo, whoWinner, gameAction, cratorHandStr, joinerHandStr, joinerNick, joinerAvatar);
                });
                await t;
                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                GameResult gameResult = new GameResult();
                gameResult.whoWinner = whoWinner;
                gameResult.gameFlag_left1 = gameInfo.gameData;
                gameResult.gameFlag_right1 = joinerGameData;

                int fixGameMoneyAmount = gameInfo.gameMoneyAmount;
                if (gameAction == 0)
                {
                    fixGameMoneyAmount = fixGameMoneyAmount / 2;
                }
                gameResult.gameMoneyAmount = fixGameMoneyAmount;
                gameResult.result_left = cratorHandStr;
                gameResult.result_right = joinerHandStr;

                return gameResult;
            };

            Post[parentSpace + "/runPublicIndianHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameID = (int)payload["gameID"];
                //string joinerGameData = (string)payload["joinerGameData"];
                string joinerMemo = (string)payload["joinerMemo"];
                int gameAction = (int)payload["gameAction"];
                string joinerNick = (string)payload["joinerNick"];
                string joinerAvatar = (string)payload["joinerAvatar"];

                GameInfo gameInfo = dbhandler.getGameInfo(1, gameID);

                string joinerGameData = GameReferee.GetIndianGameCard(gameInfo.gameData);

                string joinerHandStr = "";
                string cratorHandStr = "";

                int whoWinner = GameReferee.GetWinner(gameInfo.gameType, gameInfo.gameData, joinerGameData, ref cratorHandStr, ref joinerHandStr);
                //Super Fold
                if (gameAction == 2)
                {
                    if (whoWinner == 1)
                        whoWinner = 2;
                    else if (whoWinner == 2)
                        whoWinner = 1;
                }

                WebOK info = null;
                Task t = Task.Run(() => {
                    //sp_RunPublicIndianHoldem
                    info = dbhandler.sp_RunPublicIndianHoldem(gameID, joinerGameData, joinerMemo, whoWinner, gameAction, cratorHandStr, joinerHandStr, joinerNick, joinerAvatar);
                });
                await t;
                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                GameResult gameResult = new GameResult();
                gameResult.whoWinner = whoWinner;
                gameResult.gameFlag_left1 = gameInfo.gameData;
                gameResult.gameFlag_right1 = joinerGameData;

                int fixGameMoneyAmount = gameInfo.gameMoneyAmount;
                if (gameAction == 0)
                {
                    fixGameMoneyAmount = fixGameMoneyAmount / 2;
                }
                gameResult.gameMoneyAmount = fixGameMoneyAmount;
                gameResult.result_left = cratorHandStr;
                gameResult.result_right = joinerHandStr;

                return gameResult;
            };

            //QHoldem
            Post[parentSpace + "/getPokerCards", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int cardCnt = (int)payload["cardCnt"];
                string result = GameReferee.GetPokerCard(cardCnt, null);
                return result;
            };

            Post[parentSpace + "/createQHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int mainType = (int)payload["mainType"];
                string gameTitle = (string)payload["gameTitle"];
                string gameData = (string)payload["gameData"];
                int gameCount = (int)payload["gameCount"];
                int gameMoneyAmount = (int)payload["gameMoneyAmount"];
                string creatorAuthToken = (string)payload["creatorAuthToken"];
                //Get Card
                //string gameData = GameReferee.GetIndianGameCard(null);
                if (mainType == 0)
                {
                    if (gameCount > 5 || gameCount < 1)
                        return new WebException("gameCount Limit", 10);

                    if (gameMoneyAmount > 1000 || gameMoneyAmount < 1)
                        return new WebException("gameMoneyAmount Limit", 11);
                }
                else
                {
                    if (gameCount > 20 || gameCount < 1)
                        return new WebException("gameCount Limit", 10);

                    if (gameMoneyAmount > 1000000 || gameMoneyAmount < 1)
                        return new WebException("gameMoneyAmount Limit", 11);
                }

                GameInfo info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_CreateQHoldem(mainType, gameTitle, gameData, gameCount, gameMoneyAmount, creatorAuthToken);
                });
                await t;
                if (info == null)
                {
                    return new WebException("creategame failed", 401);
                }

                return info;
            };

            Post[parentSpace + "/runQHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameID = (int)payload["gameID"];
                int joinerGameData = (int)payload["joinerGameData"];
                string joinerMemo = (string)payload["joinerMemo"];
                string joinerAuthToken = (string)payload["joinerAuthToken"];
                

                GameInfo gameInfo = dbhandler.getGameInfo((int)eGameType.QHoldem, gameID);
                int delPos = gameInfo.gameData.IndexOf('-');
                string commCardData = GameReferee.GetPokerCard(5, gameInfo.gameData.Remove(delPos,1) );

                string[] userCardInfo = gameInfo.gameData.Split('-');   //"1:2-3:4"

                string pulishCards = "";
                string myCards = "";

                pulishCards = commCardData + ":" + userCardInfo[0] + "-" + userCardInfo[0];
                myCards = commCardData + ":" + userCardInfo[1] + "-" + userCardInfo[1];
                
                string cratorHandStr = "";
                string joinerHandStr = "";

                int whoWinner = GameReferee.GetWinner(gameInfo.gameType, pulishCards, myCards, ref cratorHandStr, ref joinerHandStr);

                if (joinerGameData == 0)
                {
                    if (whoWinner == 1)
                        whoWinner = 2;
                    else if (whoWinner == 2)
                        whoWinner = 1;
                }                

                WebOK info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_RunQHoldem(gameID, joinerGameData, commCardData, joinerMemo, joinerAuthToken, whoWinner, cratorHandStr, joinerHandStr);
                });
                await t;
                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                GameResult gameResult = new GameResult();
                gameResult.whoWinner = whoWinner;
                gameResult.gameFlag_left1 = pulishCards;
                gameResult.gameFlag_right1 = myCards;
                                
                gameResult.gameMoneyAmount = gameInfo.gameMoneyAmount;
                gameResult.result_left = cratorHandStr;
                gameResult.result_right = joinerHandStr;

                return gameResult;
            };
            
            Post[parentSpace + "/runPublicQHoldem", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int gameID = (int)payload["gameID"];
                int joinerGameData = (int)payload["joinerGameData"];
                string joinerMemo = (string)payload["joinerMemo"];
                string joinerNick = (string)payload["joinerNick"];
                string joinerAvatar = (string)payload["joinerAvatar"];


                GameInfo gameInfo = dbhandler.getGameInfo((int)eGameType.QHoldem, gameID);
                int delPos = gameInfo.gameData.IndexOf('-');
                string commCardData = GameReferee.GetPokerCard(5, gameInfo.gameData.Remove(delPos, 1));

                string[] userCardInfo = gameInfo.gameData.Split('-');   //"1:2-3:4"

                string pulishCards = "";
                string myCards = "";

                pulishCards = commCardData + ":" + userCardInfo[0] + "-" + userCardInfo[0];
                myCards = commCardData + ":" + userCardInfo[1] + "-" + userCardInfo[1];

                string cratorHandStr = "";
                string joinerHandStr = "";

                int whoWinner = GameReferee.GetWinner(gameInfo.gameType, pulishCards, myCards, ref cratorHandStr, ref joinerHandStr);

                if (joinerGameData == 0)
                {
                    if (whoWinner == 1)
                        whoWinner = 2;
                    else if (whoWinner == 2)
                        whoWinner = 1;
                }

                WebOK info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_RunPublicQHoldem(gameID, joinerGameData, commCardData, joinerMemo, whoWinner, cratorHandStr, joinerHandStr, joinerNick, joinerAvatar);
                });
                await t;
                if (info == null)
                {
                    return new WebException("rungame failed", 401);
                }

                if (info.statusCode != 200)
                {
                    return new WebException("rungame failed", info.statusCode);
                }

                GameResult gameResult = new GameResult();
                gameResult.whoWinner = whoWinner;
                gameResult.gameFlag_left1 = pulishCards;
                gameResult.gameFlag_right1 = myCards;

                gameResult.gameMoneyAmount = gameInfo.gameMoneyAmount;
                gameResult.result_left = cratorHandStr;
                gameResult.result_right = joinerHandStr;

                return gameResult;
            };


        }
    }
}
