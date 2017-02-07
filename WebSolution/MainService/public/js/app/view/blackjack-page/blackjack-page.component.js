/**
 * Created by psmon_qapgr0w on 2016-11-26.
 */
angular.
module('blackjackPage').
component('blackjackPage', {
    templateUrl: '/js/app/view/blackjack-page/blackjack-page.template.html',
    controller: ['$routeParams','Auth','Game','$scope','$location','$cacheFactory',
        function BlackjackPageController($routeParams,Auth,Game,$scope,$location,$cacheFactory) {
            $scope.lang=$lang;
            var self = $scope;
            self.cocoApp= new Application('gameCanvas',"gray");
            self.cocoApp.removeAll();
            self.cocoApp.run();
            self.testY = 0;

            self.screenWidth=380;
            self.screenHeight=400;
            self.screenWidthCenter=self.screenWidth/2;
            self.screenHeightCenter=self.screenHeight/2;

            self.tableCards=new Array();
            self.dealerCards=new Array();

            self.cardScore = 0;
            self.dealercardScore = 0;
            self.betMoney=0;
            self.totalMoney=0;

            self.cardAdd = function(cardNum){
                var cardCount = self.tableCards.length;
                var posX = 70 + cardCount*15;
                var posY = 400 - 210;
                var cardImg = self.cocoApp.addImage('img/carddeck/images/'+cardNum+'.png', self.screenWidthCenter,self.screenHeight);
                cardImg.scale = 0.1;
                cardImg.moveTo({x:posX,y:posY,duration:0.5,angle:180,scale:0.5});
                var curCardScore = cardNum % 13;
                self.cardScore+=curCardScore;
                self.tableCards.push(cardImg);
                self.updateCardScore();
            }

            self.dealerCardAdd = function(cardNum){
                var cardCount = self.dealerCards.length;
                var posX = 70 + cardCount*15 + 100;
                var posY = 400 - 80;
                var cardImg = self.cocoApp.addImage('img/carddeck/images/'+cardNum+'.png', self.screenWidthCenter,self.screenHeight);
                cardImg.scale = 0.1;
                cardImg.moveTo({x:posX,y:posY,duration:0.5,angle:180,scale:0.3,delay:0.6});
                var curCardScore = cardNum % 13;
                self.dealercardScore+=curCardScore;
                self.dealerCards.push(cardImg);
                self.updateCardScore();
            }


            self.updateMoney = function () {
                self.moneyTxt.string="TotalPoint:" + self.totalMoney + " BetPoint:" + self.betMoney;

            }

            self.updateCardScore = function () {
                self.cardScoreTxt.string="Your CardScore:" + self.cardScore + " DealerScardScore:" +self.dealercardScore;

            }

            self.OnStageInit = function () {
                var bg = $scope.cocoApp.addImage('img/game/bj/bg_table.png', self.screenWidthCenter,self.screenHeightCenter);
                bg.scale = 0.8;
                //imgRandom.width=self.screenWidth;
                //imgRandom.height=self.screenHeight;
                self.cardScoreTxt = self.cocoApp.addLabel({string:"",fontName:"Arial",fontSize:12,fontColor:"white" });
                self.cardScoreTxt.position.x=self.screenWidthCenter-170;
                self.cardScoreTxt.position.y=370;

                self.moneyTxt = self.cocoApp.addLabel({string:"",fontName:"Arial",fontSize:12,fontColor:"white" });
                self.moneyTxt.position.x=self.screenWidthCenter-170;
                self.moneyTxt.position.y=390;

                self.updateMoney();
                self.updateCardScore();
            }

            self.OnCanvasTest = function () {
                var label = $scope.cocoApp.addLabel({string:"블랙잭 준비중(Canvas 성능테스트)",fontName:"Arial",fontSize:18,fontColor:"white" });
                label.position.x=150;
                label.position.y=350;

                for(var i=1;i<53;i++){
                    var imgRandom = self.cocoApp.addImage('img/carddeck/images/'+i+'.png', 150,100);
                    imgRandom.scale = 0.5;
                    imgRandom.opacity = 50;
                    var fixX = 100+( parseInt(i / 13) )*50;
                    var fixY = 250-i*0.4 + self.testY;
                    //x=250-(i*0.5);
                    imgRandom.moveTo({x:fixX,y:250-i*0.4,duration:1.5,delay:i*0.1,angle:180,opacity:100,scale:0.3});
                }
            }


            self.OnTest=function () {
                self.cardAdd(2);
                self.dealerCardAdd(3);
                self.OnCanvasTest();
            }

            //$scope.OnCanvasTest();

            self.OnStageInit();


        }
    ]
});