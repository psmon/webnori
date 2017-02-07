/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('mygamePage').
component('mygamePage', {
    templateUrl: '/js/app/view/mygame-page/mygame-page.template.html',
    controller: ['$routeParams','Auth','$location','$scope',
        function MygamePageController($routeParams,Auth,$location,$scope) {
            $scope.lang=$lang;
            this.phoneId = $routeParams.phoneId;
            Auth.SetInGame(false);

            if( $location.path().indexOf('MyPublicGames') > 0){
                $scope.gameList = $lang.myGameList;

            }else{
                $scope.gameList = $lang.gameList;
            }

            $scope.OpenGame = function (url) {
                Auth.SetInGame(true);
                $location.path(url);
            }

        }
    ]
});