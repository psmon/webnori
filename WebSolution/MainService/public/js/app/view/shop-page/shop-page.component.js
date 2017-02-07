/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('shopPage').
component('shopPage', {
    templateUrl: '/js/app/view/shop-page/shop-page.template.html',
    controller: ['$routeParams','Auth',
        function ShopPageController($routeParams,Auth) {
            $scope.lang=$lang;
            this.phoneId = $routeParams.phoneId;
            Auth.SetInGame(false);

        }
    ]
});