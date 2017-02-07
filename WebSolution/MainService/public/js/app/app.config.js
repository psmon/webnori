/**
 * Created by psmon_qapgr0w on 2016-11-22.
 */
var app = angular.module('myapp');

var completedLoad = function(){
    //Todo:Jquery 제거버젼으로 만들기..
    var height = $("#view_top").outerHeight() + 10;
    //$("#view_content").css('margin-top', height);
    $('#view_content').animate({
        'margin-top':height
    });
}

app.config(['$locationProvider', '$routeProvider',
    function config($locationProvider, $routeProvider) {
        $locationProvider.hashPrefix('!');


        $routeProvider.
        when('/Home', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><home-page></home-page></div>'
        }).
        when('/GameList', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
                '<div id="view_content"><mygame-page></mygame-page></div>'
        }).
        when('/Game/GGame', {
            template:
                '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
                '<div id="view_content"><handgame-page></handgame-page></div>'
        }).
        when('/Game/IHGame', {
            template:
            '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><indianholdem-page></indianholdem-page></div>'
        }).
        when('/Game/QHGame', {
            template:
            '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><qholdem-page></qholdem-page></div>'
        }).
        when('/Game/BJGame', {
            template:
            '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><blackjack-page></blackjack-page></div>'
        }).
        when('/Adver', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><spongame-page></spongame-page></div>'
        }).
        when('/Shop', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
                '<div id="view_content"><shop-page></shop-page></div>'
        }).
        when('/MyPublicGames', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><mygame-page></mygame-page></div>'
        }).
        when('/MyPublicGameHand', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><handgame-page></handgame-page></div>'
        }).
        when('/MyPublicGameIHoldem', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><indianholdem-page></indianholdem-page></div>'
        }).
        when('/MyPublicGameQHoldem', {
            template: '<div id="view_top"><header-page></header-page><top-Menu></top-Menu></div>' +
            '<div id="view_content"><qholdem-page></qholdem-page></div>'
        }).
        when('/PublicDetail/:gameID', {
            template:'<div id="view_content"><publichandgamedetail-page></publichandgamedetail-page></div>'
        }).
        when('/Login', {
            template: '<login-page></login-page>'
        }).
        when('/Register', {
            template: '<register-page></register-page>'
        }).
        otherwise('/Home');

    }
]);

app.run(["$rootScope", "$location","Auth","$route","$window",
    function ($rootScope, $location,Auth,$route,$window) {
        var isLogin = Auth.IsLogin();

        $window.fbAsyncInit = function() {
            // Executed when the SDK is loaded
            FB.init({
                appId      : '588833947973720',
                xfbml      : true,
                version    : 'v2.8'
            });
            //FB.AppEvents.logPageView();
        }

        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            var isLogin = Auth.IsLogin();
            var isAllowGuest = $location.path().indexOf('PublicDetail');
            if(isAllowGuest<0){
                var restrictedPage = $.inArray($location.path(), ['/Login', '/Register']) === -1;
                if( restrictedPage && !isLogin)
                    $location.path('/Login');

                if(!restrictedPage && isLogin)
                    $location.path('/Home');
            }
        });

        $rootScope.$on('$locationChangeSuccess', function (event, next, current) {
            setTimeout(
                function(){
                    //completedLoad();
                },
                500
            );
        });
    }
]);
