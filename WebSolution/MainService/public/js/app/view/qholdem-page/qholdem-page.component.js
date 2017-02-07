/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('qholdemPage').
component('qholdemPage', {
    templateUrl: '/js/app/view/qholdem-page/qholdem-page.template.html',
    controller: ['$routeParams','Auth','Game','$scope','$location','$cacheFactory',
        function QholdemPageController($routeParams,Auth,Game,$scope,$location,$cacheFactory) {
            $scope.lang=$lang;
            var me = this;
            $scope.lang=$lang;

            if( $location.path().indexOf('MyPublicGameQHoldem') > 0){
                $scope.isShareGame=true;
                me.mainType=1;
                me.myUserNO = Auth.GetMyInfoLocal().memberID;
            }else{
                $scope.isShareGame=false;
                me.mainType=0;
                me.myUserNO=0;
            }

            me.pstartidx=0;
            me.pendidx=50;
            me.cstartidx=0;
            me.cendidx=50;
            me.errmsg="";
            me.gameID = 0;
            $scope.doneGame = false;

            Auth.SetInGame(true);
            $scope.UpdateGameList = function () {
                Game.GetQHoldemPlayList(me.pstartidx,me.pendidx,me.mainType,0,me.gameID,me.myUserNO,function (isSucced,data) {
                    if(isSucced){
                        me.PlayGameList = data;
                    }
                });

                Game.GetQHoldemPlayList(me.pstartidx,me.pendidx,me.mainType,1,me.gameID,me.myUserNO,function (isSucced,data) {
                    if(isSucced){
                        me.PlayCompleteList = data;
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
            $scope.gameCount = 1;
            //$scope.gamememo1="무적행운";
            //$scope.gamememo2="도전합니다.";

            $scope.ClearError = function(){
                $scope.errorvisibility = false;
                $scope.dataLoading = false;
                me.errmsg="";
                $scope.resultShow = false;
            }

            $scope.JoinGame = function(data){
                setTimeout(function(){
                    var scrollTop = $(window).scrollTop();
                    $('#popup-container').animate({'margin-top':scrollTop});
                },100);
                $scope.dlgType = 2;
                $scope.isShowDodal = true;
                $scope.gameID=data.gameID;
                $scope.preGameData= data;
                $scope.ClearError();
            }

            $scope.OnCreateGame = function(){
                $scope.GetUserCards(function () {
                    setTimeout(function(){
                        var scrollTop = $(window).scrollTop();
                        $('#popup-container').animate({'margin-top':scrollTop});
                    },100);
                    $scope.dlgType = 0;
                    $scope.currentGameID = 0;
                    $scope.isShowDodal = true;
                    $scope.ClearError();
                });
            }

            $scope.GetUserCards = function (completed) {
                Game.GetPokerCards(4,function(isSucced,data){
                    if(isSucced){
                        $scope.userCardData = data;
                        var cardArray = data.split(":");
                        $scope.userCardL = {};
                        $scope.userCardR = {};
                        $scope.userCardL[0]=Number(cardArray[0])+1;
                        $scope.userCardL[1]=Number(cardArray[1])+1;
                        $scope.userCardR[0]=Number(cardArray[2])+1;
                        $scope.userCardR[1]=Number(cardArray[3])+1;
                        $scope.resuestUserCard = Number(cardArray[0]) + ":" + Number(cardArray[1]) + "-" + Number(cardArray[2]) + ":" + Number(cardArray[3]);
                        completed();
                    }else{
                        $scope.onError(data);
                    }
                });
            }

            $scope.CreateQHoldem = function (obj) {
                $scope.dataLoading = true;
                Game.CreateQHoldem(me.mainType, $scope.resuestUserCard, obj.gamememo1,obj.gameCount,obj.betMount,Auth.GetAuthToken(),function(isSucced,data){
                    if(isSucced){
                        $scope.UpdateGameList();
                        $scope.isShowDodal = false;
                        $scope.dlgType = -1;
                        $scope.ClearError();
                    }else{
                        $scope.onError(data);
                    }
                });

            }

            $scope.CloseDlg = function (obj) {
                if($scope.dlgType==2){
                    $scope.dataLoading = true;
                    Game.JoinQHoldem($scope.gameID,obj.rhand.name,obj.gamememo2,Auth.GetAuthToken(),function(isSucced,data){
                        if(isSucced){
                            $scope.resultShow=true;
                            //$scope.resultLeft=data.gameFlag_left1;
                            //$scope.resultRight=Number(data.gameFlag_right1);
                            $scope.comCards = $scope.GetComCardInfo(data.gameFlag_left1)
                            $scope.result_left=data.result_left;
                            $scope.result_right=data.result_right;

                            if(data.whoWinner==0)
                                $scope.resultMsg=$lang.commGame.dosame;

                            if(data.whoWinner==1)
                                $scope.resultMsg=$lang.commGame.dolose + data.gameMoneyAmount + $lang.commGame.lostpoint;

                            if(data.whoWinner==2)
                                $scope.resultMsg=$lang.commGame.dowin + data.gameMoneyAmount + $lang.commGame.earnpoint;

                            setTimeout(function(){
                                $scope.UpdateGameList();
                            },500);

                        }else{
                            $scope.onError(data);
                        }
                    });

                }

            }

            $scope.CancelDlg = function () {
                $scope.isShowDodal = false;
                $scope.dlgType = -1;
            }

            $scope.Back = function () {
                $location.path('/GameList');
            }

            $scope.dlgType = -1; //0:none 1:CreateGame 2:JoinGame

            $scope.UpdateGameList();

            $scope.GetActionName= function(action){
                var actionName="Unknown";
                switch (action){
                    case 0:actionName="Select:CardA"; break;
                    case 1:actionName="Select:CardB"; break;
                }
                return actionName;
            }

            $scope.GetUserCardInfo = function (cardStr) {
                var leftCards = cardStr.split("-")[0].split(":");
                var rightCards = cardStr.split("-")[1].split(":");
                var resultValue={
                    L0:Number(leftCards[0])+1,
                    L1:Number(leftCards[1])+1,
                    R0:Number(rightCards[0])+1,
                    R1:Number(rightCards[1])+1
                }
                return resultValue;
            }

            $scope.GetComCardInfo = function (cardStr) {
                var comCards=cardStr.split("-")[0].split(":");
                var resultValue = new Array();
                resultValue[0] = Number(comCards[0])+1;
                resultValue[1] = Number(comCards[1])+1;
                resultValue[2] = Number(comCards[2])+1;
                resultValue[3] = Number(comCards[3])+1;
                resultValue[4] = Number(comCards[4])+1;
                return resultValue;
            }

        }
    ]
});