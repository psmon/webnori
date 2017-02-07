/**
 * Created by psmon_qapgr0w on 2016-11-22.
 */
angular.
module('core').
filter('checkmark', function() {
    return function(input) {
        return input ? '\u2713' : '\u2718';
    };
});