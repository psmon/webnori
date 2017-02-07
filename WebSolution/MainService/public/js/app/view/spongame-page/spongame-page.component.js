/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('spongamePage').
component('spongamePage', {
    templateUrl: '/js/app/view/spongame-page/spongame-page.template.html',
    controller: ['$routeParams','Auth','$scope','$location','$sce',
        function SpongamePageController($routeParams,Auth,$scope,$location,$sce) {
            $scope.lang=$lang;
            this.phoneId = $routeParams.phoneId;
            Auth.SetInGame(false);
            var myLocation=$location;

            Auth.GetAdverList(function(isSucced,data){
                if(isSucced){
                    $scope.adverList = data;

                    if(data.length>0){
                        $(window).scroll(function() {
                            if($(window).scrollTop() + $(window).height() > $(document).height() - 10) {
                                if(0<myLocation.path().indexOf('Adver')){
                                    Auth.SetTabBonus(function(isSucced,data){
                                        if(isSucced){
                                            $scope.bonusData=data;
                                        }
                                    });
                                }
                            }
                        });
                    }
                }
            });

            $scope.safeHtml = function (htmlText) {
                return $sce.trustAsHtml(htmlText);
            }

        }
    ]
});