/**
 * Created by psmon_qapgr0w on 2016-11-22.
 */
angular.
module('myapp').
config(['$locationProvider', '$routeProvider',
    function config($locationProvider, $routeProvider) {
        $locationProvider.hashPrefix('!');

        $routeProvider.
        when('/main', {
            template: 'main<appHeader></appHeader>'+
                '<appFooter></appFooter>'
        }).
        when('/docu', {
            template: 'main<appHeader></appHeader>'+
            '<appFooter></appFooter>'
        }).
        otherwise('/main/1');
    }
]);
/**
 * Created by psmon_qapgr0w on 2016-11-22.
 */
