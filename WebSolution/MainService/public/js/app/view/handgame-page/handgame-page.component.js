/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('handgamePage').
component('handgamePage', {
    templateUrl: '/js/app/view/handgame-page/handgame-page.template.html',
    controller: ['$routeParams','Auth','Game','$scope','$location','$cacheFactory',
        function HandgamePageController($routeParams,Auth,Game,$scope,$location,$cacheFactory) {
            $scope.lang=$lang;
            var me = this;
            me.pstartidx=0;
            me.pendidx=50;
            me.cstartidx=0;
            me.cendidx=50;
            me.errmsg="";
            me.userInfo = Auth.userInfoCache;

            if( $location.path().indexOf('MyPublicGameHand') > 0){
                $scope.isShareGame=true;
                me.mainType=1;
                me.myUserNO = Auth.GetMyInfoLocal().memberID;
            }else{
                $scope.isShareGame=false;
                me.mainType=0;
                me.myUserNO=0;
            }

            me.gameID = 0;


            Auth.SetInGame(true);

            $scope.UpdateMedalInfo = function(UserNo,Nick){
                if( angular.isUndefined(Auth.userInfoCache[Nick]) ){
                    Auth.userInfoCache[Nick]=null;
                    Auth.GetUserInfo(UserNo,function (isSucced,data) {
                        if(isSucced){
                            Auth.userInfoCache[data.Nick] = data;
                        }
                    });
                }
            }

            $scope.UpdateGameList = function () {
                Game.GetHandPlayList(me.pstartidx,me.pendidx,me.mainType,me.gameID,me.myUserNO,function (isSucced,data) {
                    if(isSucced){
                        me.PlayGameList = data;
                        for( var gameEleIdx in data){
                            var gameEle = data[gameEleIdx];
                            var hostID = gameEle.header.memberID_left;
                            var hostNick = gameEle.header.memberNick_left;
                            $scope.UpdateMedalInfo(hostID,hostNick);
                            if($scope.isShareGame==false) {
                                for (var gameInfoIdx in gameEle.list) {
                                    var gameInfo = gameEle.list[gameInfoIdx];
                                    $scope.UpdateMedalInfo(gameInfo.memberID_right, gameInfo.memberNick_right);
                                }
                            }
                        }
                    }
                });

                Game.GetHandPlayCompletedList(me.cstartidx,me.cendidx,me.mainType,me.gameID,me.myUserNO,function (isSucced,data) {
                    if(isSucced){
                        me.PlayCompleteList = data;
                        for( var gameEleIdx in data){
                            var gameEle = data[gameEleIdx];
                            var hostID = gameEle.header.memberID_left;
                            var hostNick = gameEle.header.memberNick_left;
                            $scope.UpdateMedalInfo(hostID,hostNick);
                            if($scope.isShareGame==false) {
                                for(var gameInfoIdx in gameEle.list){
                                    var gameInfo = gameEle.list[gameInfoIdx];
                                    $scope.UpdateMedalInfo(gameInfo.memberID_right,gameInfo.memberNick_right);
                                }
                            }
                        }
                    }
                });
            }

            $scope.errorvisibility=false;
            $scope.dataLoading = false;

            $scope.resultShow = false;

            $scope.onError = function (msg) {
                me.errmsg = msg;
                $scope.errorvisibility = true;
                $scope.dataLoading = false;
            }

            //Default Value
            $scope.lhand={
                "name":"0"
            }

            $scope.rhand={
                "name":"0"
            }

            $scope.betMount = 10;
            $scope.gameCount = 3;
            //$scope.gamememo1="안내면술래";
            //$scope.gamememo2="도전합니다.";


            $scope.ClearError = function(){
                $scope.errorvisibility = false;
                $scope.dataLoading = false;
                me.errmsg="";
                $scope.resultShow = false;
            }

            $scope.JoinGame = function(gameID){
                setTimeout(function(){
                    var scrollTop = $(window).scrollTop();
                    $('#popup-container').animate({'margin-top':scrollTop});
                },100);

                $scope.dlgType = 2;
                $scope.isShowDodal = true;
                $scope.gameID=gameID;

                $scope.ClearError();
            }

            $scope.CreateGame = function(){
                setTimeout(function(){
                    var scrollTop = $(window).scrollTop();
                    $('#popup-container').animate({'margin-top':scrollTop});
                },100);
                $scope.dlgType = 1;
                $scope.isShowDodal = true;
                $scope.ClearError();
            }

            $scope.CloseDlg = function (obj) {
                if($scope.dlgType==1){
                    $scope.dataLoading = true;
                    //Game.CreateGame(obj.gamememo1,obj.gameCount,obj.betMount,obj.lhand.name,Auth.GetAuthToken(),function(isSucced,data){
                    Game.CreateHandGame(me.mainType,obj.gamememo1,obj.gameCount,obj.betMount,obj.lhand.name,Auth.GetAuthToken(),function(isSucced,data){
                        if(isSucced){
                            $scope.UpdateGameList();
                            $scope.isShowDodal = false;
                            $scope.dlgType = 0;
                            $scope.ClearError();
                        }else{
                            $scope.onError(data);
                        }
                    });
                }

                if($scope.dlgType==2){
                    $scope.dataLoading = true;
                    Game.JoinGame($scope.gameID,obj.rhand.name,obj.gamememo2,Auth.GetAuthToken(),function(isSucced,data){
                        if(isSucced){

                            $scope.resultShow=true;
                            $scope.resultLeft=data.gameFlag_left1;
                            $scope.resultRight=data.gameFlag_right1;

                            if(data.whoWinner==0){
                                $scope.resultMsg=$lang.commGame.dosame;
                            }

                            if(data.whoWinner==1) {
                                $scope.resultMsg=$lang.commGame.dolose + data.gameMoneyAmount + $lang.commGame.lostpoint;
                            }

                            if(data.whoWinner==2){
                                $scope.resultMsg=$lang.commGame.dowin + data.gameMoneyAmount + $lang.commGame.earnpoint;
                            }

                            setTimeout(function(){
                                $scope.UpdateGameList();
                                //$scope.isShowDodal = false;
                                //$scope.dlgType = 0;
                                //$scope.ClearError();
                            },500);

                        }else{
                            $scope.onError(data);
                        }
                    });

                }

            }

            $scope.CancelDlg = function () {
                $scope.isShowDodal = false;
                $scope.dlgType = 0;
            }

            $scope.Back = function () {
                $location.path('/GameList');
            }

            $scope.dlgType = 0; //0:none 1:CreateGame 2:JoinGame

            $scope.UpdateGameList();

            $scope.leadingZeros = function(n, digits) {
                var zero = '';
                n = n.toString();

                if (n.length < digits) {
                    for (var i = 0; i < digits - n.length; i++)
                        zero += '0';
                }
                return zero + n;
            }

        }
    ]
});