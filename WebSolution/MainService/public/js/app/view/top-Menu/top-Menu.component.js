// Register `phoneList` component, along with its associated controller and template
var curmodule = angular.module('topMenu')
curmodule.component('topMenu', {
    templateUrl: '/js/app/view/top-Menu/top-Menu.template.html',
    controller: ['$routeParams','$scope','$location',
        function TopMenuController($routeParams,$scope,$location) {
            $scope.lang=$lang;
            $scope.isActive = function (viewLocation) {
                var active = (viewLocation === $location.path());
                return active;
            };
        }
    ]
});
