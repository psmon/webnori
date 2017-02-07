// Register `phoneList` component, along with its associated controller and template
var myApp = angular.module('profilePage');

myApp.component('profilePage', {
    templateUrl: '/js/app/view/profile-page/profile-page.template.html',
    controller: ['$routeParams','Auth','$scope','$location',
        function ProfilePageController($routeParams,Auth,$scope,$location) {
            $scope.lang=$lang;
            $scope.avatarList = null;
            Auth.GetMyInfo(function(succed,data){
                if(succed){
                    $scope.userInfo = data;
                }
            });

            $scope.errmsg=null;

            $scope.OnCloseProfile = function () {
                Auth.parentScope.OnCloseProfile();
            }

            $scope.OnGetAvarList = function () {
                if($scope.avatarList!=null){
                    $scope.avatarList=null;
                }else{
                    Auth.GetAvarTartList(function(isSucced,data){
                        $scope.avatarList = data;

                    });
                }
            }

            $scope.OnSetAvarTar = function (avartar) {
                Auth.SetAvarTar(avartar.itemNo,function(isSucced,data){
                    if(isSucced){
                        $scope.userInfo.picUrl=avartar.imgUrl;
                    }
                });
            }

            $scope.OnUnJoin = function () {
                Auth.DeleteUser(function(isSucced,data){
                    if(isSucced==false){
                        $scope.errmsg = data;
                    }else{
                        $location.path('/Login');
                    }
                });

            }
        }
    ]
  });
/*
myApp.directive('bgImage', function(){
        return function(scope, element, attrs) {
            scope.$watch(attrs.bgImage, function(value) {
                element.css({
                    'background-image': 'url(' + value +')',
                    'background-size' : 'cover'
                });
            });
        };
    });*/