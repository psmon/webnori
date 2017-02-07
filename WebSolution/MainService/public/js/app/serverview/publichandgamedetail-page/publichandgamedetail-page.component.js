/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('publichandgamedetailPage').
component('publichandgamedetailPage', {
    templateUrl: '/js/app/serverview/publichandgamedetail-page/publichandgamedetail-page.template.html',
    controller: ['$routeParams','Auth','Game','$scope','$location','$cacheFactory','$window',
        function PublichandgamedetailPageController($routeParams,Auth,Game,$scope,$location,$cacheFactory,$window) {
            $scope.lang=$lang;
            var me = this;
            me.pstartidx=0;
            me.pendidx=50;
            me.cstartidx=0;
            me.cendidx=50;
            me.errmsg="";

            $scope.isEndGame=false;
            $scope.doneGame = false;
            $scope.isLogin = false;

            $scope.gameID = g_gameId;

            $scope.UpdateGameList = function () {
                Game.GetHandPlayList(me.pstartidx,me.pendidx,1,$scope.gameID,0,function (isSucced,data) {
                    if(isSucced){
                        me.PlayGameList = data;
                    }
                });

                Game.GetHandPlayCompletedList(me.cstartidx,me.cendidx,1,$scope.gameID,0,function (isSucced,data) {
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
            //$scope.gamememo1="안내면술래";
            //$scope.gamememo2="도전합니다.";
            //$scope.playNick="홍길동";

            var ranUser = Math.floor((Math.random() * 10) + 1);
            var ranNum = Math.floor((Math.random() * 100) + 1);
            var randomNameList = ["홍길동","이순신","대왕","철수","현정","미정","지훈","승현","동현","건우"];
            //$scope.playNick=randomNameList[ranUser] + ranNum;

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
                    Game.CreateHandGame(1,obj.gamememo1,obj.gameCount,obj.betMount,obj.lhand.name,Auth.GetAuthToken(),function(isSucced,data){
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
                    Game.JoinPublicHandGame($scope.gameID,obj.rhand.name,obj.gamememo2,obj.playNick,function(isSucced,data){
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

                            $scope.doneGame = true;

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