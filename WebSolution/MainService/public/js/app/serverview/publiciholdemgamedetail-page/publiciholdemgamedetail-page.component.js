/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('publiciholdemPage').
component('publiciholdemPage', {
    templateUrl: '/js/app/serverview/publiciholdemgamedetail-page/publiciholdemgamedetail-page.template.html',
    controller: ['$routeParams','Auth','Game','$scope','$location','$cacheFactory','$anchorScroll','$window',
        function PubliciholdemPageController($routeParams,Auth,Game,$scope,$location,$cacheFactory,$anchorScroll,$window) {
            $scope.lang=$lang;
            var me = this;

            $scope.isEndGame=false;
            $scope.doneGame = false;
            $scope.isLogin = false;

            me.pstartidx=0;
            me.pendidx=50;
            me.cstartidx=0;
            me.cendidx=50;
            me.errmsg="";
            me.myUserNO=0;
            me.mainType=1;
            $scope.gameID = g_gameId;

            Auth.SetInGame(true);
            $scope.UpdateGameList = function () {
                Game.GetIndianHoldemPlayList(me.pstartidx,me.pendidx,me.mainType,0,$scope.gameID,me.myUserNO,function (isSucced,data) {
                    if(isSucced){
                        me.PlayGameList = data;
                    }
                });

                Game.GetIndianHoldemPlayList(me.pstartidx,me.pendidx,me.mainType,1,$scope.gameID,me.myUserNO,function (isSucced,data) {
                    if(isSucced){
                        me.PlayCompleteList = data;
                        if(data.length>0)
                            $scope.isEndGame = true;
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
            $scope.gamememo1="무적행운";
            //$scope.gamememo2="도전합니다.";
            //$scope.gamememo2="";

            $scope.joineravatar={
                "name":"/img/avatar/default/avartar_10.png"
            }
            //$scope.joinerAvatar="/img/avatar/default/avartar_10.png";

            var ranUser = Math.floor((Math.random() * 10) + 1);
            var ranNum = Math.floor((Math.random() * 100) + 1);
            var randomNameList = ["홍길동","이순신","대왕","철수","현정","미정","지훈","승현","동현","건우"];
            //$scope.joinernick=randomNameList[ranUser] + ranNum;
            //$scope.joinernick="";

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

            $scope.CloseDlg = function (obj) {
                if($scope.dlgType==2){
                    $scope.dataLoading = true;
                    Game.JoinPublicIndianHoldem($scope.gameID,$scope.rhand.name,obj.form.gamememo2.$viewValue,obj.form.joinernick.$viewValue,$scope.joineravatar.name,function(isSucced,data){
                        if(isSucced){
                            $scope.resultShow=true;
                            $scope.resultLeft=data.gameFlag_left1;
                            $scope.resultRight=Number(data.gameFlag_right1);
                            $scope.result_left=data.result_left;
                            $scope.result_right=data.result_right;

                            $scope.doneGame = true;

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

            $scope.gotoAnchor = function(x) {
                var newHash = 'anchor' + x;
                if ($location.hash() !== newHash) {
                    // set the $location.hash to `newHash` and
                    // $anchorScroll will automatically scroll to it
                    $location.hash('anchor' + x);
                } else {
                    // call $anchorScroll() explicitly,
                    // since $location.hash hasn't changed
                    $anchorScroll();
                }
            };

            $scope.$watch(
                function () {
                    return $window.innerWidth;
                },
                function (value) {
                    $scope.windowWidth = value;
                },
                true
            );

            var w = angular.element($window);
            w.bind('resize', function(){
                $scope.$apply();
            });

        }
    ]
});