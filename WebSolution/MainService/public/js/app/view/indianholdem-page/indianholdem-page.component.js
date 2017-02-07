/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('indianholdemPage').
component('indianholdemPage', {
    templateUrl: '/js/app/view/indianholdem-page/indianholdem-page.template.html',
    controller: ['$routeParams','Auth','Game','$scope','$location','$cacheFactory',
        function IndianholdemPageController($routeParams,Auth,Game,$scope,$location,$cacheFactory) {
            $scope.lang=$lang;
            var me = this;
            $scope.lang=$lang;

            if( $location.path().indexOf('MyPublicGameIHoldem') > 0){
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
                Game.GetIndianHoldemPlayList(me.pstartidx,me.pendidx,me.mainType,0,me.gameID,me.myUserNO,function (isSucced,data) {
                    if(isSucced){
                        me.PlayGameList = data;
                    }
                });

                Game.GetIndianHoldemPlayList(me.pstartidx,me.pendidx,me.mainType,1,me.gameID,me.myUserNO,function (isSucced,data) {
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
                setTimeout(function(){
                    var scrollTop = $(window).scrollTop();
                    $('#popup-container').animate({'margin-top':scrollTop});
                },100);
                $scope.dlgType = 0;
                $scope.currentGameID = 0;
                $scope.isShowDodal = true;
                $scope.ClearError();
            }

            $scope.CreateIndianHoldem = function (obj) {
                $scope.dataLoading = true;
                Game.CreateIndianHoldem(me.mainType, obj.gamememo1,obj.gameCount,obj.betMount,Auth.GetAuthToken(),function(isSucced,data){
                    if(isSucced){
                        //$scope.UpdateGameList();
                        //$scope.isShowDodal = false;
                        $scope.preGameData = data;
                        $scope.currentGameID = data.gameID;
                        $scope.dlgType = 1;
                        $scope.ClearError();
                    }else{
                        $scope.onError(data);
                    }
                });

            }

            $scope.CloseDlg = function (obj) {
                if($scope.dlgType==1){
                    $scope.dataLoading = true;
                    Game.EdtIndianHoldem($scope.currentGameID,obj.lhand.name,Auth.GetAuthToken(),function(isSucced,data){
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

                if($scope.dlgType==2){
                    $scope.dataLoading = true;
                    Game.JoinIndianHoldem($scope.gameID,obj.rhand.name,obj.gamememo2,Auth.GetAuthToken(),function(isSucced,data){
                        if(isSucced){
                            $scope.resultShow=true;
                            $scope.resultLeft=data.gameFlag_left1;
                            $scope.resultRight=Number(data.gameFlag_right1);
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
                                //$scope.isShowDodal = false;
                                //$scope.dlgType = -1;
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
                    case 0:actionName="Half"; break;
                    case 1:actionName="Call"; break;
                    case 2:actionName="Fold"; break;
                }
                return actionName;
            }

        }
    ]
});