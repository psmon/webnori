/**
 * Created by psmon_qapgr0w on 2016-11-24.
 */
angular.
module('core.auth').
factory('Auth', ['$resource','$http',
    function($resource,$http) {
        var AuthService = {};
        //member
        AuthService.isLogin = false;
        AuthService.authToken = "";
        AuthService.userInfo = null;
        AuthService.userInfoCache={};
        AuthService.isInGame = false;
        AuthService.infoMessage = null;
        AuthService.parentScope = null;
        AuthService.infb=false;
        AuthService.isRunTabBonus=false;
        AuthService.isRunDayBonus=false;

        //func
        AuthService.IsLogin = IsLogin;
        AuthService.SetLogin = SetLogin;
        AuthService.ValidChk = ValidChk;
        AuthService.GetAuthToken = GetAuthToken;
        AuthService.LoginUser = LoginUser;
        AuthService.CreateUser = CreateUser;
        AuthService.DeleteUser = DeleteUser;
        AuthService.Encript = Encript;
        AuthService.GetMyInfo = GetMyInfo;
        AuthService.GetUserInfo = GetUserInfo;
        AuthService.AutoLogin = AutoLogin;
        AuthService.GetMyInfoLocal = GetMyInfoLocal;
        AuthService.IsInGame = IsInGame;
        AuthService.SetInGame = SetInGame;
        AuthService.GetAvarTartList = GetAvarTartList;
        AuthService.SetAvarTar = SetAvarTar;
        AuthService.SetTabBonus = SetTabBonus;
        AuthService.SetDayBonus = SetDayBonus;
        AuthService.GetAdverList = GetAdverList;
        //FB
        AuthService.FBLogin =FBLogin;
        AuthService.FBLogout =FBLogout;
        AuthService.SocialAutoLogin=SocialAutoLogin;
        return AuthService;

        function Encript (plainText){
            key = "1020304050607080";
            var C = CryptoJS;
            plainText = C.enc.Utf8.parse(plainText);
            key = C.enc.Utf8.parse(key);
            var aes = C.algo.AES.createEncryptor(key, {
                mode: C.mode.CBC,
                padding: C.pad.Pkcs7,
                iv: key
            });
            var encrypted = aes.finalize(plainText);
            return C.enc.Base64.stringify(encrypted);

        }

        function GetAuthToken(){
            return AuthService.authToken;
        }

        function IsLogin(){
            return AuthService.isLogin;
        }

        function IsInGame(){
            return AuthService.isInGame;
        }

        function SetInGame(inGame) {
            AuthService.isInGame = inGame;
        }

        function SetLogin(bLogin,_authToken){
            AuthService.isLogin = bLogin;
            AuthService.authToken = _authToken;
            localStorage.setItem('tmpToken',_authToken);
        }

        function AutoLogin(callback){
            var _authToken = localStorage.getItem('tmpToken');
            var resuestData = {
                method: 'POST',
                url: '/api/login/validate',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'authtoken':_authToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    AuthService.SetLogin(true,_authToken);
                    AuthService.SetInGame(false);
                    callback(true,"AutoLogin Succed");
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;
                    callback(false,showErrorMsg);
                    AuthService.SetLogin(false,null);
                }
            }, function(response){
                callback(false,"api call failed");
                AuthService.SetLogin(false,null);
            });
        }

        function GetMyInfoLocal(){
            return AuthService.userInfo;
        }

        function GetMyInfo(callback) {

            var _authToken = AuthService.GetAuthToken();
            var resuestData = {
                method: 'POST',
                url: '/api/info/me',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'authtoken':_authToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                    AuthService.userInfo = data;
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;
                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });
        }

        function GetUserInfo(userNo,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/info/user',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'userNo':userNo}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,data);
                    AuthService.userInfo = data;
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;
                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });
        }

        function GetAvarTartList(callback) {
            var _authToken = AuthService.GetAuthToken();

            var resuestData = {
                method: 'POST',
                url: '/api/info/avatar/getlist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'authtoken':_authToken}
            }

            $http(resuestData).then(function(response){
                callback(true,response.data);
            }, function(response){
                callback(false,"api call failed");
            });
        }

        function GetAdverList(callback) {
            var _authToken = AuthService.GetAuthToken();
            var resuestData = {
                method: 'POST',
                url: '/api/info/adver/getlist',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'authtoken':_authToken}
            }

            $http(resuestData).then(function(response){
                callback(true,response.data);
            }, function(response){
                callback(false,"api call failed");
            });
        }

        function SetAvarTar(itemNo,callback){
            var _authToken = AuthService.GetAuthToken();
            var resuestData = {
                method: 'POST',
                url: '/api/info/avatar/set',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'authtoken':_authToken,'itemNo':itemNo}
            }

            $http(resuestData).then(function(response){
                callback(true,response.data);
            }, function(response){
                callback(false,"api call failed");
            });
        }
        //deleteUser

        function DeleteUser(callback){
            var _authToken = AuthService.GetAuthToken();
            var resuestData = {
                method: 'POST',
                url: '/api/login/deleteUser',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'authtoken':_authToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    AuthService.SetLogin(false,null);
                    AuthService.infoMessage=$lang.api.unjoincomplete;
                    callback(true,data);
                }else{
                    var showErrorMsg="api call failed(internal error) :" + data.statusCode;
                    if(data.statusCode == 500 || data.statusCode == 401 ){
                        showErrorMsg = data.message;
                    }
                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"api call failed");
            });

        }

        function LoginUser(socialType,socialID,pw,callback){
            pw = Encript(pw);
            var resuestData = {
                method: 'POST',
                url: '/api/login/loginUser',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'socialType':socialType, 'socialID':socialID, 'pw':pw}
            }
            AuthService.SetLogin(false,null);
            var me = this;
            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    AuthService.SetLogin(true,data.authToken);
                    callback(true,"login succed");
                }else{
                    var showErrorMsg=$lang.api.loginerror1;
                    switch (data.statusCode){
                        case 2:
                            showErrorMsg=$lang.faultemailtype;
                            break;
                    }
                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"login failed");
            });
        }

        function CreateUser(reqdata,callback) {
            reqdata.pw =  Encript(reqdata.pw);

            var resuestData = {
                method: 'POST',
                url: '/api/login/createUser',
                headers:{'Content-Type':'application/json'},
                data:reqdata
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    callback(true,"created succed");
                }else{
                    var showErrorMsg=$lang.api.createusererror;
                    switch (data.statusCode){
                        case -2:
                            showErrorMsg=$lang.api.sameaccount;
                            break;
                        case -3:
                            showErrorMsg=$lang.api.samenick;
                            break;
                        case 2:
                            showErrorMsg=$lang.api.faultemailtype;
                            break;
                    }

                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"created failed");
            });
        }

        function ValidChk(data,dataType){
            var result={IsValid:true,Reason:""};
            if(dataType=="email"){
                if(data==undefined){
                    result = {IsValid:false,Reason:$lang.api.emailmin};
                }
                else if(data.length<5){
                    result = {IsValid:false,Reason:$lang.api.emailmin};
                }
            }
            if(dataType=="password"){
                if(data==undefined){
                    result = {IsValid:false,Reason:$lang.api.pwmin};
                }
                else if(data.length<5){
                    result = {IsValid:false,Reason:$lang.api.pwmin};
                }
            }

            if(dataType=="nickname"){
                if(data==undefined){
                    result = {IsValid:false,Reason:$lang.api.nikmin};
                }
                else if(data.length<4){
                    result = {IsValid:false,Reason:$lang.api.nikmin};
                }
            }
            return result;
        }

        function SetTabBonus(callback){
            if(AuthService.isRunTabBonus==true)
                return;
            var _authToken = AuthService.GetAuthToken();
            var resuestData = {
                method: 'POST',
                url: '/api/game/tabbonus',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'playerAuthToken':_authToken}
            }

            $http(resuestData).then(function(response){
                callback(true,response.data);
            }, function(response){
                callback(false,"api call failed");
            });

            AuthService.isRunTabBonus=true;
        }

        function SetDayBonus(callback) {
            if(AuthService.isRunDayBonus==true)
                return;
            var _authToken = AuthService.GetAuthToken();
            var resuestData = {
                method: 'POST',
                url: '/api/game/daybonus',
                headers:{'Content-Type':'application/json',
                    'Charset':'utf-8'},
                data:{'playerAuthToken':_authToken}
            }

            $http(resuestData).then(function(response){
                callback(true,response.data);
            }, function(response){
                callback(false,"api call failed");
            });
            AuthService.isRunDayBonus=true;
        }


        function SocialAutoLogin(socialType,authToken,callback) {
            var resuestData = {
                method: 'POST',
                url: '/api/login/loginUserSocial',
                headers:{'Content-Type':'application/json'},
                data:{'socialType':socialType,'authToken':authToken}
            }

            $http(resuestData).then(function(response){
                var data = response.data;
                if(data.statusCode == 200){
                    AuthService.SetLogin(true,data.authToken);
                    callback(true,"login succed");
                }else{
                    var showErrorMsg=$lang.api.loginerror1;
                    callback(false,showErrorMsg);
                }
            }, function(response){
                callback(false,"login failed");
            });
        }

        function FBLogin(callback) {
            var self = this;
            FB.login(function(response){
                if (response.status === 'connected') {
                    // 페이스북과 앱에 같이 로그인되어 있다.
                    FB.getLoginStatus(function(response) {
                        if (response.status === 'connected') {
                            callback(true,response.authResponse.accessToken);
                        }
                    });
                } else if (response.status === 'not_authorized') {
                    // 페이스북에는 로그인 되어있으나, 앱에는 로그인 되어있지 않다.
                    callback(false,"FB Login Failed-2");
                } else {
                    // 페이스북에 로그인이 되어있지 않아서, 앱에 로그인 되어있는지 불명확하다.
                    callback(false,"FB Login Failed-3");
                }
            },{scope: 'public_profile,email'});
        }

        function FBLogout(callback){
            FB.logout(function(response) {
                callback(true,"logout Succed");
            });
        }

        /*
        return $resource('phones/:phoneId.json', {}, {
            query: {
                method: 'GET',
                params: {phoneId: 'phones'},
                isArray: true
            }
        });*/

    }
]);