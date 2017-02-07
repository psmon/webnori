using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainService.test;

using System.IO;
using System.Web;
using System.Data;
using CommonModule;
using CommonModule.data;
using System.Configuration;

#pragma warning disable 1998

namespace MainService.controllers
{
    public class InfomationController : BaseController
    {
        protected DBInfomationHandler dbhandler;
        public InfomationController()
        {
            parentSpace = "/api/info";
            dbhandler = new DBInfomationHandler(ConfigurationManager.AppSettings.Get("ConectionDBString"));

            Get["Event/{id}", true] = async (parameters, ct) =>
            {
                FBSahareModel model = new FBSahareModel();
                model.appid = "588833947973720";
                model.pageID = parameters.id;
                model.pageUrl = "http://game.webnori.com/Event/" + parameters.id;
                model.pageImg = "http://game.webnori.com/img/event/" + parameters.id + "/fbshare.jpg";
                model.popupurl = string.Format("https://www.facebook.com/sharer/sharer.php?u={0}&amp;src=sdkpreparse", model.pageUrl );

                string pageName = string.Format("views/event/page_{0}", parameters.id);
                return View[pageName, model];
            };


            Get["/PublicDetail/{id}", true] = async (parameters, ct) => {
                FBSahareModel model = new FBSahareModel();                
                model.pageID = int.Parse(parameters.id);
                model.appid = "588833947973720";
                model.langcode = "ko_KR";
                model.langfile = string.Format("/js/lang/ko_KR.js");

                List<HandGameListInfo> gameList1 = new List<HandGameListInfo>();
                Task t = Task.Run(() => {
                    gameList1 = dbhandler.GetHandGameList(0, 5, 1, model.pageID, 0);
                });
                await t;
                
                List<HandGameListInfoCompleted> gameList2 = new List<HandGameListInfoCompleted>();

                Task t2 = Task.Run(() => {
                    gameList2 = dbhandler.GetHandGameListCompleted(0, 5, 1, model.pageID, 0);
                });
                await t2;
                
                if (gameList1.Count > 0)
                {
                    model.pageTitle = gameList1[0].memberNick_left + " 의 가위바위보게임";
                    model.pageContent = gameList1[0].gameMemo_left;
                }
                if (gameList2.Count > 0)
                {
                    model.pageTitle = gameList2[0].memberNick_left + " 의 가위바위보게임";
                    model.pageContent = gameList2[0].gameMemo_left;
                }
                
                model.pageUrl = "http://game.webnori.com/PublicDetail/" + parameters.id;
                model.pageImg = "http://game.webnori.com/img/share_handimg.png";                
                model.popupurl = string.Format("https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fgame.webnori.com%2FPublicDetail%2F{0}&amp;src=sdkpreparse", model.pageID);
                                
                return View["views/HandGameShareDetail.html", model];
                //return View["public/index.html", model];
            };

            Get["/PublicIholdemDetail/{id}", true] = async (parameters, ct) => {
                FBSahareModel model = new FBSahareModel();
                model.pageID = int.Parse(parameters.id);
                model.appid = "588833947973720";
                model.langcode = "ko_KR";
                model.langfile = string.Format("/js/lang/ko_KR.js");

                List<IndianHoldemListInfo> gameList1 = new List<IndianHoldemListInfo>();
                Task t = Task.Run(() => {
                    gameList1 = dbhandler.GetIndianHoldemGameList(0,5,1,0,model.pageID,0);
                });
                await t;

                List<IndianHoldemListInfo> gameList2 = new List<IndianHoldemListInfo>();

                Task t2 = Task.Run(() => {
                    gameList2 = dbhandler.GetIndianHoldemGameList(0, 5, 1, 1, model.pageID, 0);
                });
                await t2;

                if (gameList1.Count > 0)
                {
                    model.pageTitle = gameList1[0].creatorNick + " 의 인디언홀덤 게임";
                    model.pageContent = gameList1[0].gameTitle;
                }
                if (gameList2.Count > 0)
                {
                    model.pageTitle = gameList2[0].creatorNick + " 의 인디언홀덤 게임";
                    model.pageContent = gameList2[0].gameTitle;
                }

                model.pageUrl = "http://game.webnori.com/PublicIholdemDetail/" + parameters.id;
                model.pageImg = "http://game.webnori.com/img/share_iholdem.png";
                model.popupurl = string.Format("https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fgame.webnori.com%2FPublicIholdemDetail%2F{0}&amp;src=sdkpreparse", model.pageID);

                return View["views/IHoldemGameShareDetail.html", model];
                //return View["public/index.html", model];
            };

            Get["/PublicDetail/{id}/{langID}", true] = async (parameters, ct) => {
                FBSahareModel model = new FBSahareModel();
                model.pageID = int.Parse(parameters.id);
                model.appid = "588833947973720";                

                model.langcode = parameters.langID;
                model.langfile = string.Format("/js/lang/{0}.js", parameters.langID);

                List<HandGameListInfo> gameList1 = new List<HandGameListInfo>();
                Task t = Task.Run(() => {
                    gameList1 = dbhandler.GetHandGameList(0, 5, 1, model.pageID, 0);
                });
                await t;

                List<HandGameListInfoCompleted> gameList2 = new List<HandGameListInfoCompleted>();

                Task t2 = Task.Run(() => {
                    gameList2 = dbhandler.GetHandGameListCompleted(0, 5, 1, model.pageID, 0);
                });
                await t2;

                string titleEnd = "";
                switch (model.langcode.ToLower())
                {
                    case "en_en":
                        titleEnd = "'s Rock Paper Scissors Game";
                        break;
                    case "ko_kr":
                        titleEnd = "의 가위바위보게임 게임";
                        break;
                }

                if (gameList1.Count > 0)
                {
                    model.pageTitle = gameList1[0].memberNick_left + titleEnd;
                    model.pageContent = gameList1[0].gameMemo_left;
                }
                if (gameList2.Count > 0)
                {
                    model.pageTitle = gameList2[0].memberNick_left + titleEnd;
                    model.pageContent = gameList2[0].gameMemo_left;
                }

                model.pageUrl = "http://game.webnori.com/PublicDetail/" + parameters.id + "/" + parameters.langID;
                model.pageImg = "http://game.webnori.com/img/share_handimg.png";
                model.popupurl = string.Format("https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fgame.webnori.com%2FPublicDetail%2F{0}%2F{1}&amp;src=sdkpreparse", model.pageID, parameters.langID);

                return View["views/HandGameShareDetail.html", model];
                //return View["public/index.html", model];
            };

            Get["/PublicIholdemDetail/{id}/{langID}", true] = async (parameters, ct) => {
                FBSahareModel model = new FBSahareModel();
                model.pageID = int.Parse(parameters.id);
                model.appid = "588833947973720";
                model.langcode = parameters.langID;
                model.langfile = string.Format("/js/lang/{0}.js", parameters.langID);

                List<IndianHoldemListInfo> gameList1 = new List<IndianHoldemListInfo>();
                Task t = Task.Run(() => {
                    gameList1 = dbhandler.GetIndianHoldemGameList(0, 5, 1, 0, model.pageID, 0);
                });
                await t;

                List<IndianHoldemListInfo> gameList2 = new List<IndianHoldemListInfo>();

                Task t2 = Task.Run(() => {
                    gameList2 = dbhandler.GetIndianHoldemGameList(0, 5, 1, 1, model.pageID, 0);
                });
                await t2;

                string titleEnd = "";
                switch (model.langcode.ToLower())
                {
                    case "en_en":
                        titleEnd = "'s IndianHodem Game";
                        break;
                    case "ko_kr":
                        titleEnd = "의 인디언홀덤 게임";
                        break;
                }

                if (gameList1.Count > 0)
                {
                    model.pageTitle = gameList1[0].creatorNick + titleEnd;
                    model.pageContent = gameList1[0].gameTitle;
                }
                if (gameList2.Count > 0)
                {
                    model.pageTitle = gameList2[0].creatorNick + titleEnd;
                    model.pageContent = gameList2[0].gameTitle;
                }

                model.pageUrl = "http://game.webnori.com/PublicIholdemDetail/" + parameters.id + "/" + parameters.langID;
                model.pageImg = "http://game.webnori.com/img/share_iholdem.png";
                model.popupurl = string.Format("https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fgame.webnori.com%2FPublicIholdemDetail%2F{0}%2F{1}&amp;src=sdkpreparse", model.pageID, parameters.langID);

                return View["views/IHoldemGameShareDetail.html", model];
                //return View["public/index.html", model];
            };

            Get["/PublicQHoldemDetail/{id}/{langID}", true] = async (parameters, ct) => {
                FBSahareModel model = new FBSahareModel();
                model.pageID = int.Parse(parameters.id);
                model.appid = "588833947973720";
                model.langcode = parameters.langID;
                model.langfile = string.Format("/js/lang/{0}.js", parameters.langID);

                List<QHoldemListInfo> gameList1 = new List<QHoldemListInfo>();
                Task t = Task.Run(() => {
                    gameList1 = dbhandler.GetQHoldemGameList(0, 5, 1, 0, model.pageID, 0);                    
                });
                await t;

                List<QHoldemListInfo> gameList2 = new List<QHoldemListInfo>();

                Task t2 = Task.Run(() => {
                    gameList2 = dbhandler.GetQHoldemGameList(0, 5, 1, 1, model.pageID, 0);
                });
                await t2;

                string titleEnd = "";
                switch (model.langcode.ToLower())
                {
                    case "en_en":
                        titleEnd = "'s Hodelm hand Game";
                        break;
                    case "ko_kr":
                        titleEnd = "의 홀덤 핸드게임";
                        break;
                }

                if (gameList1.Count > 0)
                {
                    model.pageTitle = gameList1[0].creatorNick + titleEnd;
                    model.pageContent = gameList1[0].gameTitle;
                }
                if (gameList2.Count > 0)
                {
                    model.pageTitle = gameList2[0].creatorNick + titleEnd;
                    model.pageContent = gameList2[0].gameTitle;
                }

                model.pageUrl = "http://game.webnori.com/PublicQHoldemDetail/" + parameters.id + "/" + parameters.langID;
                model.pageImg = "http://game.webnori.com/img/share_qholdem.png";
                model.popupurl = string.Format("https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fgame.webnori.com%2FPublicQHoldemDetail%2F{0}%2F{1}&amp;src=sdkpreparse", model.pageID, parameters.langID);

                return View["views/QHoldemGameShareDetail.html", model];
                //return View["public/index.html", model];
            };

            Post[parentSpace + "/me", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                string authToken = (string)payload["authtoken"];
                UserInfo info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_userinfo(authToken);
                });
                await t;
                string argMonth = string.Format("{0}-{1:D2}-01", DateTime.Now.Year, DateTime.Now.Month);
                int earnPoint = 0;
                Task t2 = Task.Run(() => {
                    earnPoint = dbhandler.sp_earnpoint("", info.MemberID, argMonth);
                });
                
                await t2;

                info.MonthPoint = earnPoint;
                if (info == null)
                {
                    return new WebException("Login failed", 401);
                }                
                return info;
            };

            Post[parentSpace + "/avatar/getlist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                string authToken = (string)payload["authtoken"];
                List<AvartarInfo> avarTartList = new List<AvartarInfo>();

                UserInfo info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_userinfo(authToken);
                });
                await t;
                if (info == null)
                {
                    return new WebException("check auth failed", 401);
                }

                for(int idx = 1; idx < 71; idx++)
                {
                    string imgUrl = string.Format("/img/avatar/default/avartar_{0:00}.png", idx);
                    int itemNo = idx;
                    avarTartList.Add(new AvartarInfo(itemNo, imgUrl));
                }                
                return avarTartList;
            };

            Post[parentSpace + "/avatar/set", true] = async (parameters, ct) =>
            {
                WebOK result = new WebOK();
                IDictionary<string, object> payload = GetPayLoad();
                string authToken = (string)payload["authtoken"];
                int itemNo = (int)payload["itemNo"];
                string imgUrl = string.Format("/img/avatar/default/avartar_{0:00}.png", itemNo);

                List<AvartarInfo> avarTartList = new List<AvartarInfo>();

                UserInfo info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_userinfo(authToken);
                });
                await t;
                if (info == null)
                {
                    return new WebException("check auth failed", 401);
                }
                

                Task taskUpdateAvatar = Task.Run(() => {
                    string sqltext = string.Format("update tblmember set picurl='{0}' where memberID={1};", imgUrl, info.MemberID);
                    dbhandler.RunSQL(sqltext,false);
                });

                await taskUpdateAvatar;

                return result;
            };

            Post[parentSpace + "/user", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int userNo = (int)payload["userNo"];
                UserInfo info = null;
                Task t = Task.Run(() => {
                    info = dbhandler.sp_userinfo("NAN", userNo);
                });
                await t;

                info.GamePoint = 0;
                info.CoinCount = 0;
                if (info == null)
                {
                    return new WebException("Login failed", 401);
                }
                return info.ToString();
            };

            Post[parentSpace + "/game/playlist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int startidx = (int)payload["startidx"];
                int endidx = (int)payload["endidx"];

                List<HandGameListInfo> gameList2 = new List<HandGameListInfo>();
                Task t = Task.Run(() => {
                    gameList2 = dbhandler.GetHandGameList(startidx, endidx);
                });
                await t;

                List<HandGamePlayList> gameList = new List<HandGamePlayList>();                
                List<HandGameListInfo> detailList = new List<HandGameListInfo>();
                int curGameID = 0;
                HandGamePlayList addData = new HandGamePlayList();                
                foreach (HandGameListInfo dbdata in gameList2)
                {
                    //New Header
                    if (curGameID != dbdata.gameID)
                    {
                        addData = new HandGamePlayList();
                        detailList = new List<HandGameListInfo>();
                        addData.header = dbdata;                        
                        addData.list = detailList;
                        curGameID = dbdata.gameID;
                        gameList.Add(addData);
                    }

                    if( curGameID == dbdata.gameID)
                    {
                        if(dbdata.subGameID>-1)
                            addData.list.Add(dbdata);
                    }
                }
                     
                return gameList;
            };

            Post[parentSpace + "/game/completelist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int startidx = (int)payload["startidx"];
                int endidx = (int)payload["endidx"];

                //List<GameListInfoCompleted> gameList2 = dbhandler.GetGameListCompleted(startidx, endidx);
                List<HandGameListInfoCompleted> gameList2 = new List<HandGameListInfoCompleted>();
                Task t = Task.Run(() => {
                    gameList2 = dbhandler.GetHandGameListCompleted(startidx, endidx);
                });
                await t;

                List<HandGamePlayCompletedList> gameList = new List<HandGamePlayCompletedList>();
                List<HandGameListInfoCompleted> detailList = new List<HandGameListInfoCompleted>();
                int curGameID = 0;
                HandGamePlayCompletedList addData = new HandGamePlayCompletedList();
                foreach (HandGameListInfoCompleted dbdata in gameList2)
                {
                    //New Header
                    if (curGameID != dbdata.gameID)
                    {
                        addData = new HandGamePlayCompletedList();
                        detailList = new List<HandGameListInfoCompleted>();
                        addData.header = dbdata;
                        addData.list = detailList;
                        curGameID = dbdata.gameID;
                        gameList.Add(addData);
                    }

                    if (curGameID == dbdata.gameID)
                    {
                        if (dbdata.subGameID > -1)
                            addData.list.Add(dbdata);
                    }
                }
                return gameList;

            };

            Post[parentSpace + "/game/handplaylist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int startidx = (int)payload["startidx"];
                int endidx = (int)payload["endidx"];
                int mainType = (int)payload["mainType"];
                int gameID = (int)payload["gameID"];
                int creatorID = (int)payload["creatorID"];

                List<HandGameListInfo> gameList2 = new List<HandGameListInfo>();
                Task t = Task.Run(() => {
                    gameList2 = dbhandler.GetHandGameList(startidx, endidx, mainType, gameID, creatorID);
                });
                await t;

                List<HandGamePlayList> gameList = new List<HandGamePlayList>();
                List<HandGameListInfo> detailList = new List<HandGameListInfo>();
                int curGameID = 0;
                HandGamePlayList addData = new HandGamePlayList();
                foreach (HandGameListInfo dbdata in gameList2)
                {
                    //New Header
                    if (curGameID != dbdata.gameID)
                    {
                        addData = new HandGamePlayList();
                        detailList = new List<HandGameListInfo>();
                        addData.header = dbdata;
                        addData.list = detailList;
                        curGameID = dbdata.gameID;
                        gameList.Add(addData);
                    }

                    if (curGameID == dbdata.gameID)
                    {
                        if (dbdata.subGameID > -1)
                            addData.list.Add(dbdata);
                    }
                }

                return gameList;
            };

            Post[parentSpace + "/game/handcompletelist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int startidx = (int)payload["startidx"];
                int endidx = (int)payload["endidx"];
                int mainType = (int)payload["mainType"];
                int gameID = (int)payload["gameID"];
                int creatorID = (int)payload["creatorID"];

                //List<GameListInfoCompleted> gameList2 = dbhandler.GetGameListCompleted(startidx, endidx);
                List<HandGameListInfoCompleted> gameList2 = new List<HandGameListInfoCompleted>();
                Task t = Task.Run(() => {
                    gameList2 = dbhandler.GetHandGameListCompleted(startidx, endidx, mainType, gameID, creatorID);
                });
                await t;


                List<HandGamePlayCompletedList> gameList = new List<HandGamePlayCompletedList>();
                List<HandGameListInfoCompleted> detailList = new List<HandGameListInfoCompleted>();
                int curGameID = 0;
                HandGamePlayCompletedList addData = new HandGamePlayCompletedList();
                foreach (HandGameListInfoCompleted dbdata in gameList2)
                {
                    //New Header
                    if (curGameID != dbdata.gameID)
                    {
                        addData = new HandGamePlayCompletedList();
                        detailList = new List<HandGameListInfoCompleted>();
                        addData.header = dbdata;
                        addData.list = detailList;
                        curGameID = dbdata.gameID;
                        gameList.Add(addData);
                    }

                    if (curGameID == dbdata.gameID)
                    {
                        if (dbdata.subGameID > -1)
                            addData.list.Add(dbdata);
                    }
                }
                return gameList;

            };
            //GetQHoldemGameList
            Post[parentSpace + "/game/indianholdemlist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int startidx = (int)payload["startidx"];
                int endidx = (int)payload["endidx"];
                int mainType = (int)payload["mainType"];
                int gameState = (int)payload["gameState"];
                int gameID = (int)payload["gameID"];
                int creatorID = (int)payload["creatorID"];

                List<IndianHoldemListInfo> gameList2 = new List<IndianHoldemListInfo>();
                Task t = Task.Run(() => {
                    gameList2 = dbhandler.GetIndianHoldemGameList(startidx, endidx, mainType, gameState, gameID, creatorID);
                });
                await t;

                List<IndianHoldemListInfoList> gameList = new List<IndianHoldemListInfoList>();
                List<IndianHoldemListInfo> detailList = new List<IndianHoldemListInfo>();
                int curGameID = 0;
                IndianHoldemListInfoList addData = new IndianHoldemListInfoList();
                foreach (IndianHoldemListInfo dbdata in gameList2)
                {
                    //New Header
                    if (curGameID != dbdata.gameID)
                    {
                        addData = new IndianHoldemListInfoList();
                        detailList = new List<IndianHoldemListInfo>();
                        addData.header = dbdata;
                        addData.list = detailList;
                        curGameID = dbdata.gameID;
                        gameList.Add(addData);
                    }

                    if (curGameID == dbdata.gameID)
                    {
                        if (dbdata.subGameID > -1)
                            addData.list.Add(dbdata);
                    }
                }

                return gameList;
            };

            Post[parentSpace + "/game/QHoldemlist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                int startidx = (int)payload["startidx"];
                int endidx = (int)payload["endidx"];
                int mainType = (int)payload["mainType"];
                int gameState = (int)payload["gameState"];
                int gameID = (int)payload["gameID"];
                int creatorID = (int)payload["creatorID"];

                List<QHoldemListInfo> gameList2 = new List<QHoldemListInfo>();
                Task t = Task.Run(() => {
                    gameList2 = dbhandler.GetQHoldemGameList(startidx, endidx, mainType, gameState, gameID, creatorID);
                });
                await t;

                List<QHoldemListInfoList> gameList = new List<QHoldemListInfoList>();
                List<QHoldemListInfo> detailList = new List<QHoldemListInfo>();
                int curGameID = 0;
                QHoldemListInfoList addData = new QHoldemListInfoList();
                foreach (QHoldemListInfo dbdata in gameList2)
                {
                    //New Header
                    if (curGameID != dbdata.gameID)
                    {
                        addData = new QHoldemListInfoList();
                        detailList = new List<QHoldemListInfo>();
                        addData.header = dbdata;
                        addData.list = detailList;
                        curGameID = dbdata.gameID;
                        gameList.Add(addData);
                    }

                    if (curGameID == dbdata.gameID)
                    {
                        if (dbdata.subGameID > -1)
                            addData.list.Add(dbdata);
                    }
                }
                return gameList;
            };

            Post[parentSpace + "/earnpoint/rank", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                string monthData = string.Format("{0}-{1:D2}-01", DateTime.Now.Year, DateTime.Now.Month);
                int gameType = (int)payload["gameType"];

                RankListInfoList resultValue = new RankListInfoList();
                resultValue.currentMonth = string.Format("{0}-{1}", DateTime.Now.Year, DateTime.Now.Month);
                Task t = Task.Run(() => {
                    resultValue.list = dbhandler.sp_earnpointList(monthData, gameType);
                });
                await t;
                        
                return resultValue;
            };

            Post[parentSpace + "/adver/getlist", true] = async (parameters, ct) =>
            {
                IDictionary<string, object> payload = GetPayLoad();
                string authToken = (string)payload["authtoken"];
                List<AdverInfo> adverList = new List<AdverInfo>();                
                Task t = Task.Run(() => {
                    adverList = dbhandler.GetAdverList();
                });
                await t;                                
                return adverList;
            };

        }
        
    }
}
