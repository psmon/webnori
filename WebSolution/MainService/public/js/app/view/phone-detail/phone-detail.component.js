/**
 * Created by psmon_qapgr0w on 2016-11-22.
 */
angular.
module('phoneDetail').
component('phoneDetail', {
    template: 'TBD: Detail view for <span>{{$ctrl.phoneId}}</span>',
    controller: ['$routeParams',
        function PhoneDetailController($routeParams) {
            this.phoneId = $routeParams.phoneId;
        }
    ]
});