/**
 * Created by psmon_qapgr0w on 2016-12-04.
 */
angular.
module('core.game').
factory('Game', ['$resource','$http',
    function($resource,$http) {
        var GameService = {};
        //member

        //func
        GameService.GetPlayList=GetPlayList;
        GameService.GetPlayCompletedList=GetPlayCompletedList;
        GameService.GetHandPlayList=GetHandPlayList;
        GameService.GetHandPlayCompletedList=GetHandPlayCompletedList;
        GameService.GetIndianHoldemPlayList=GetIndianHoldemPlayList;
        GameService.GetQHoldemPlayList=GetQHoldemPlayList;
        GameService.CreateGame=CreateGame;
        GameService.JoinGame=JoinGame;
        GameService.CreateHandGame=CreateHandGame;
        GameService.JoinPublicHandGame=JoinPublicHandGame;
        GameService.CreateIndianHoldem=CreateIndianHoldem;
        GameService.EdtIndianHoldem=EdtIndianHoldem;
        GameService.JoinIndianHoldem=JoinIndianHoldem;
        GameService.JoinPublicIndianHoldem=JoinPublicIndianHoldem;
        GameService.CreateQHoldem=CreateQHoldem;
        GameService.JoinQHoldem=JoinQHoldem;
        GameService.GetPokerCards=GetPokerCards;
        GameService.JoinPublicQHoldem=JoinPublicQHoldem;
        GameService.GetEarnPointRank=GetEarnPointRank;

        return GameService;

        function GetEarnPointRank(gameType,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/earnpoint/rank',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameType':gameType}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"get list call failed");
            });
        }

        function GetPlayList(startidx,endidx,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/game/playlist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'startidx':startidx,'endidx':endidx}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"get list call failed");
            });
        }

        function GetPlayCompletedList(startidx,endidx,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/game/completelist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'startidx':startidx,'endidx':endidx}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"get list call failed");
            });
        }

        function GetHandPlayList(startidx,endidx,mainType,gameID,creatorID,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/game/handplaylist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{
                    'startidx':startidx,'endidx':endidx,
                    'mainType':mainType,'gameID':gameID,'creatorID':creatorID
                }
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"get list call failed");
            });
        }

        function GetHandPlayCompletedList(startidx,endidx,mainType,gameID,creatorID,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/game/handcompletelist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{
                    'startidx':startidx,'endidx':endidx,
                    'mainType':mainType,'gameID':gameID,'creatorID':creatorID
                }
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"get list call failed");
            });
        }

        function GetIndianHoldemPlayList(startidx,endidx,mainType,gameState,gameID,creatorID,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/game/indianholdemlist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'startidx':startidx,'endidx':endidx,'gameState':gameState,'mainType':mainType,
                'gameID':gameID,'creatorID':creatorID}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"get list call failed");
            });
        }

        function GetQHoldemPlayList(startidx,endidx,mainType,gameState,gameID,creatorID,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/game/QHoldemlist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'startidx':startidx,'endidx':endidx,'gameState':gameState,'mainType':mainType,
                    'gameID':gameID,'creatorID':creatorID}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"get list call failed");
            });
        }

        //가위바위보
        function CreateGame(gameMemo_left,gameCount,gameMoneyAmount,gameFlag_left1,creatorAuthToken,callback) {
            var mainType = 0;
            var gameType = 0;
            var gameTitle = "";
            var appendtime = 60 * 24 * 7;

            gameCount=Number(gameCount);
            gameMoneyAmount=Number(gameMoneyAmount);

            var resuestData = {
                method: 'POST',
                url: '/api/game/creategame',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'mainType':mainType,'gameType':gameType,'gameTitle':gameTitle,'gameMemo_left':gameMemo_left,
                'gameCount':gameCount,'gameMoneyAmount':gameMoneyAmount,'gameFlag_left1':gameFlag_left1,'appendtime':appendtime,
                'creatorAuthToken':creatorAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.autherror;
                    
                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.creategamefull;

                    if(data.statusCode==10)
                        showErrorMsg = $lang.api.creategamelimit;

                    if(data.statusCode==11)
                        showErrorMsg = $lang.api.betpointlimit;
                    
                    
                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function JoinGame(gameID,gameFlag_right1,gameMemo_right,playerAuthToken,callback) {

            gameID=Number(gameID);

            var resuestData = {
                method: 'POST',
                url: '/api/game/rungame',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameID':gameID,'gameFlag_right1':gameFlag_right1,
                    'gameMemo_right':gameMemo_right,'playerAuthToken':playerAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.notexistgame;

                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-4)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-5)
                        showErrorMsg = $lang.api.denymyname;

                    if(data.statusCode==-6)
                        showErrorMsg = $lang.api.denyinmyname;

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function CreateHandGame(mainType,gameMemo_left,gameCount,gameMoneyAmount,gameFlag_left1,creatorAuthToken,callback) {
            var gameType = 0;
            var gameTitle = "";
            var appendtime = 60 * 24 * 7;

            gameCount=Number(gameCount);
            gameMoneyAmount=Number(gameMoneyAmount);

            var resuestData = {
                method: 'POST',
                url: '/api/game/creategame',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'mainType':mainType,'gameType':gameType,'gameTitle':gameTitle,'gameMemo_left':gameMemo_left,
                    'gameCount':gameCount,'gameMoneyAmount':gameMoneyAmount,'gameFlag_left1':gameFlag_left1,'appendtime':appendtime,
                    'creatorAuthToken':creatorAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.autherror;

                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.creategamefull;

                    if(data.statusCode==10)
                        showErrorMsg = $lang.api.creategamefull;

                    if(data.statusCode==11)
                        showErrorMsg = $lang.api.betpointlimit;

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function JoinPublicHandGame(gameID,gameFlag_right1,gameMemo_right,memberNick_right,callback) {

            gameID=Number(gameID);

            var resuestData = {
                method: 'POST',
                url: '/api/game/runpublichandgame',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameID':gameID,'gameFlag_right1':gameFlag_right1,
                    'gameMemo_right':gameMemo_right,'memberNick_right':memberNick_right}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.notexistgame;

                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-4)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-5)
                        showErrorMsg = $lang.denymyname;

                    if(data.statusCode==-6)
                        showErrorMsg = $lang.denyinmyname;

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        //인디언홀덤
        function CreateIndianHoldem(mainType,gameTitle,gameCount,gameMoneyAmount,creatorAuthToken,callback) {
            gameCount=Number(gameCount);
            gameMoneyAmount=Number(gameMoneyAmount);
            var resuestData = {
                method: 'POST',
                url: '/api/game/createIndianHoldem',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'mainType':mainType,'gameTitle':gameTitle, 'gameCount':gameCount,'gameMoneyAmount':gameMoneyAmount,
                    'creatorAuthToken':creatorAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.autherror;

                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.creategamefull;

                    if(data.statusCode==-4)
                        showErrorMsg = $lang.api.coinlack;

                    if(data.statusCode==10)
                        showErrorMsg = $lang.api.creategamefull;

                    if(data.statusCode==11)
                        showErrorMsg = $lang.api.betpointlimit;


                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function EdtIndianHoldem(gameID,gameAction,creatorAuthToken,callback) {
            var gameTitle = "";
            gameID=Number(gameID);
            gameAction=Number(gameAction);
            var resuestData = {
                method: 'POST',
                url: '/api/game/editIndianHoldem',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameAction':gameAction, 'gameID':gameID,
                    'creatorAuthToken':creatorAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.autherror;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.notexistgame;
                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function JoinIndianHoldem(gameID,gameAction,joinerMemo,joinerAuthToken,callback) {
            gameID=Number(gameID);
            gameAction=Number(gameAction);
            var resuestData = {
                method: 'POST',
                url: '/api/game/runIndianHoldem',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameID':gameID,'gameAction':gameAction,
                    'joinerMemo':joinerMemo,'joinerAuthToken':joinerAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = "게임이없거나 인증에러.";

                    if(data.statusCode==-2)
                        showErrorMsg = "게임이 이미 종료되었습니다.";

                    if(data.statusCode==-3)
                        showErrorMsg = "참여금액이 부족합니다.";

                    if(data.statusCode==-4)
                        showErrorMsg = "게임이 이미 종료되었습니다.";

                    if(data.statusCode==-5)
                        showErrorMsg = "자신의 게임에는 참여할수 없습니다.";

                    if(data.statusCode==-6)
                        showErrorMsg = "이미 참여한 게임입니다.";

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function JoinPublicIndianHoldem(gameID,gameAction,joinerMemo,joinerNick,joinerAvatar,callback) {
            gameID=Number(gameID);
            gameAction=Number(gameAction);
            var resuestData = {
                method: 'POST',
                url: '/api/game/runPublicIndianHoldem',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameID':gameID,'gameAction':gameAction,
                    'joinerMemo':joinerMemo,'joinerNick':joinerNick,'joinerAvatar':joinerAvatar}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.notexistgame;

                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-4)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-5)
                        showErrorMsg = $lang.api.denymyname;

                    if(data.statusCode==-6)
                        showErrorMsg = $lang.api.denyinmyname;

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        //Qholdem
        function CreateQHoldem(mainType,gameData,gameTitle,gameCount,gameMoneyAmount,creatorAuthToken,callback) {
            gameCount=Number(gameCount);
            gameMoneyAmount=Number(gameMoneyAmount);
            var resuestData = {
                method: 'POST',
                url: '/api/game/createQHoldem',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'mainType':mainType,'gameTitle':gameTitle, 'gameCount':gameCount,'gameMoneyAmount':gameMoneyAmount,'gameData':gameData,
                    'creatorAuthToken':creatorAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.autherror;

                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.creategamefull;

                    if(data.statusCode==-4)
                        showErrorMsg = $lang.api.coinlack;

                    if(data.statusCode==10)
                        showErrorMsg = $lang.api.creategamefull;

                    if(data.statusCode==11)
                        showErrorMsg = $lang.api.betpointlimit;


                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function JoinQHoldem(gameID,joinerGameData,joinerMemo,joinerAuthToken,callback) {
            gameID=Number(gameID);
            joinerGameData=Number(joinerGameData);
            var resuestData = {
                method: 'POST',
                url: '/api/game/runQHoldem',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameID':gameID,'joinerGameData':joinerGameData,
                    'joinerMemo':joinerMemo,'joinerAuthToken':joinerAuthToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = "게임이없거나 인증에러.";

                    if(data.statusCode==-2)
                        showErrorMsg = "게임이 이미 종료되었습니다.";

                    if(data.statusCode==-3)
                        showErrorMsg = "참여금액이 부족합니다.";

                    if(data.statusCode==-4)
                        showErrorMsg = "게임이 이미 종료되었습니다.";

                    if(data.statusCode==-5)
                        showErrorMsg = "자신의 게임에는 참여할수 없습니다.";

                    if(data.statusCode==-6)
                        showErrorMsg = "이미 참여한 게임입니다.";

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function JoinPublicQHoldem(gameID,joinerGameData,joinerMemo,joinerNick,joinerAvatar,callback) {
            gameID=Number(gameID);
            joinerGameData=Number(joinerGameData);
            var resuestData = {
                method: 'POST',
                url: '/api/game/runPublicQHoldem',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'gameID':gameID,'joinerGameData':joinerGameData,
                    'joinerMemo':joinerMemo,'joinerNick':joinerNick,'joinerAvatar':joinerAvatar}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;

                    if(data.statusCode==-1)
                        showErrorMsg = $lang.api.notexistgame;

                    if(data.statusCode==-2)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-3)
                        showErrorMsg = $lang.api.pointlack;

                    if(data.statusCode==-4)
                        showErrorMsg = $lang.api.aleadycomplete;

                    if(data.statusCode==-5)
                        showErrorMsg = $lang.denymyname;

                    if(data.statusCode==-6)
                        showErrorMsg = $lang.denyinmyname;

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function GetPokerCards(cardCnt,callback) {
            cardCnt=Number(cardCnt);
            var resuestData = {
                method: 'POST',
                url: '/api/game/getPokerCards',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'cardCnt':cardCnt}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                callback(true,data);
            }, function(response){
                callback(false,"api call failed");
            });

        }


    }
]);
