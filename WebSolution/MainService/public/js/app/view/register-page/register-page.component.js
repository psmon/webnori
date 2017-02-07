// Register `phoneList` component, along with its associated controller and template
angular.
module('registerPage').
component('registerPage', {
    templateUrl: '/js/app/view/register-page/register-page.template.html',
    controller: ['$scope','$routeParams','$location','$rootScope','Auth',
        function RegisterPageController($scope,$routeParams,$location,$rootScope,Auth) {
            $scope.lang=$lang;
            var me = this;
            me.errmsg="";
            $scope.visibility = false;
            $scope.dataLoading = false;

            $scope.onError = function (msg) {
                me.errmsg = msg;
                $scope.visibility = true;
                $scope.dataLoading = false;
            }

            $scope.onRegister = function(){
                $scope.dataLoading = true;

                if($scope.password!=$scope.password2){
                    $scope.onError($lang.failedrepw);
                    return;
                }

                var validInfo = Auth.ValidChk($scope.email,"email");
                if(!validInfo.IsValid)
                    return $scope.onError(validInfo.Reason);

                var validInfo = Auth.ValidChk($scope.password,"password");
                if(!validInfo.IsValid)
                    return $scope.onError(validInfo.Reason);

                var reqData = {
                    socialType:0,
                    socialID:$scope.email,
                    nick:encodeURIComponent($scope.nickname),
                    picurl:"empty",
                    email:$scope.email,
                    pw:$scope.password
                }

                Auth.CreateUser(reqData,function(isRegisted,msg){
                    if(isRegisted){
                        $scope.dataLoading = false;
                        $location.path('/Login');
                        Auth.infoMessage=$lang.doneregister;
                    }else{
                        $scope.onError(msg);
                    }
                })
            }

        }
    ]
});/**
 * Created by psmon_qapgr0w on 2016-11-24.
 */
