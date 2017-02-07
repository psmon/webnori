/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('headerPage').
component('headerPage', {
    templateUrl: '/js/app/view/header-page/header-page.template.html',
    controller: ['$routeParams','Auth','$scope','$location',
        function HeaderPageController($routeParams,Auth,$scope,$location) {
            $scope.lang=$lang;
            var me = this;
            $scope.showProfile = false;

            Auth.parentScope = $scope;

            $scope.isUpdateMedal = function(left,right){
                if(left==null)
                    return true;

                if(left.coinCount!=right.coinCount)
                    return true;

                if(left.gamePoint!=right.gamePoint)
                    return true;

                if(left.loseCount!=right.loseCount)
                    return true;

                if(left.winCount!=right.winCount)
                    return true;

                if(left.picUrl!=right.picUrl)
                    return true;

                return false;
            }

            $scope.userInfo = Auth.userInfo;

            $scope.isShowPage = !Auth.IsInGame();

            $scope.isLogin = Auth.IsLogin();


            $scope.onLogout = function(){
                Auth.SetLogin(false,null);
                $location.path('/Login');
            }

            $scope.onProfile = function(){
                $scope.showProfile = true;
            }

            $scope.OnCloseProfile = function () {
                $scope.showProfile = false;
                Auth.GetMyInfo(function(succed,data){
                    if(succed){
                        $scope.userInfo = data;
                    }
                });
            }

            $scope.completedLoad = function () {
                $scope.isShowPage = !Auth.IsInGame();
                Auth.GetMyInfo(function(succed,data){
                    if(succed){
                        if($scope.isUpdateMedal(Auth.GetMyInfoLocal(),data)){
                            $scope.userInfo = data;
                        }
                    }
                });

            }

            $scope.$watch('$locationChangeSuccess', function(){
                setTimeout(
                    function(){
                        $scope.completedLoad();
                    },
                    300
                );
            });
        }
    ]
});