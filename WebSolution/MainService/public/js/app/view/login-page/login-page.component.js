// Register `phoneList` component, along with its associated controller and template
angular.
module('loginPage').
component('loginPage', {
    templateUrl: '/js/app/view/login-page/login-page.template.html',
    controller: ['$scope','$routeParams','$location','$rootScope','Auth','$route','$window',
        function LoginPageController($scope,$routeParams,$location,$rootScope,Auth,$route,$window) {
            $scope.lang=$lang;
            $scope.fbloginurl = "https://www.facebook.com/dialog/oauth?client_id=588833947973720&redirect_uri=http://game.webnori.com/"+$lang.langcode+"#!/Login&scope=email,public_profile";


            var me = this;
            me.errmsg="";
            $scope.visibility = false;
            $scope.dataLoading = false;

            var tmpToken = localStorage.getItem('tmpToken');
            if(tmpToken==null || tmpToken=="null")
                $scope.progressLogin = false;
            else
                $scope.progressLogin = true;


            $scope.showLogin = !$scope.progressLogin;

            $scope.infoMessage = Auth.infoMessage;

            $scope.windowWidth = $window.innerWidth;

            $scope.onAutoLogin = function () {
                var _authToken = localStorage.getItem('tmpToken');
                if(_authToken==undefined || _authToken=="null" || _authToken==null){
                    return;
                }
                var isLogin = Auth.IsLogin();
                if(_authToken!=null && isLogin==false){
                    Auth.AutoLogin(function (isSucced,data) {
                        if(isSucced){
                            $location.path('/Home');
                        }else{
                            $scope.progressLogin = false;
                            $scope.showLogin = !$scope.progressLogin;
                        }
                    })
                }
            }

            $scope.onDelayLoad = function () {
                FB.getLoginStatus(function(response) {
                    if (response.status === 'connected') {
                        Auth.SocialAutoLogin(1,response.authResponse.accessToken,function(isSuccedLogin,data2){
                            if(isSuccedLogin){
                                $scope.dataLoading = false;
                                //$location.path('/Home');
                                //$location.url("http://game.webnori.com/#!/Login");
                                window.location = "http://game.webnori.com/" + $lang.langcode + "#!/Login";
                            }else{
                                $scope.onError(data2);
                            }
                        });
                    }else{
                        $scope.onError("Fb Login Error");
                    }
                });
            }

            $scope.onFbSecondLogin = function () {
                if($location.absUrl().indexOf("code")>0){
                    setTimeout(
                        function(){
                            $scope.onDelayLoad();
                        },
                        500
                    );
                }
            }

            //me.onAutoLogin();

            $scope.onError = function (msg) {
                me.errmsg = msg;
                $scope.visibility = true;
                $scope.dataLoading = false;
            }

            $scope.onTestLogin = function () {
                var ranUser = Math.floor((Math.random() * 10) + 1);
                if(ranUser==10)
                    ranUser=ranUser-1;
                $scope.email = "guest0" + ranUser+"@webnori.com";
                $scope.password = "webguest";
                $scope.onError("데모계정을 가지고 왔습니다. 로그인시도로 로그인 가능합니다. 데모 계정은,중복 로그인으로 정상적인 게임진행에 제한이 있을수 있습니다.");
            }

            $scope.onFBLogin = function () {
                window.location = $scope.fbloginurl;
                /*
                Auth.infb=true;
                Auth.FBLogin(function(isSucced,data){
                    if(isSucced){
                        Auth.SocialAutoLogin(1,data,function(isSuccedLogin,data2){
                            if(isSuccedLogin){
                                $scope.dataLoading = false;
                                $location.path('/Home');
                            }else{
                                $scope.onError(data2);
                            }
                        });
                    }else{
                        $scope.onError(data);
                    }
                }); */
            }

            $scope.onLogin = function(){
                /* TestCode
                Auth.SetLogin(true);
                $location.path('/Home');
                return;
                */

                $scope.dataLoading = true;
                var validInfo = Auth.ValidChk($scope.email,"email");
                if(!validInfo.IsValid)
                    return $scope.onError(validInfo.Reason);

                var validInfo = Auth.ValidChk($scope.password,"password");
                if(!validInfo.IsValid)
                    return $scope.onError(validInfo.Reason);
                Auth.LoginUser(0,$scope.email,$scope.password,function(isLogined,msg){
                    if(isLogined){
                        $scope.dataLoading = false;
                        $location.path('/Home');
                        //$location.url($location.path());
                    }else{
                        $scope.onError(msg);
                    }
                })
            }

            $scope.onLogout = function(){
                Auth.SetLogin(false,null);
                $location.path('/Login');
            }

            $scope.$watch('$viewContentLoaded', function(){
                $scope.onAutoLogin();
                $scope.onFbSecondLogin();
            });
        }
    ]
});