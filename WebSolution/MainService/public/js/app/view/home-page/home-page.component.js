/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('homePage').
component('homePage', {
    templateUrl: '/js/app/view/home-page/home-page.template.html',
    controller: ['$routeParams','Auth','$scope','Game','$window',
        function HomePageController($routeParams,Auth,$scope,Game,$window) {
            $scope.lang=$lang;
            this.phoneId = $routeParams.phoneId;
            Auth.SetInGame(false);
            $scope.windowWidth = $window.innerWidth;

            Game.GetEarnPointRank(0,function(isSucced,data1){
                if(isSucced=true){
                    $scope.handRank = data1;
                }
            });

            Game.GetEarnPointRank(1,function(isSucced,data2){
                if(isSucced=true){
                    $scope.iholRank = data2;
                }
            });

            Auth.SetDayBonus(function(isSucced,data){
                if(isSucced){
                    $scope.bonusData=data;
                }
            });
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