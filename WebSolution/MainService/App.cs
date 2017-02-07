using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.Owin.Hosting;

using CommonModule;
using CommonModule.data;
using MainService.core;
namespace MainService
{
    class App
    {

        static void BeforeTest()
        {
            //DBInfomationHandler testDB = new DBInfomationHandler(ConfigurationManager.AppSettings.Get("ConectionDBString"));
            //testDB.GetHandGameList(0, 100);
            DBLoginInfo info =  FBAPI.GetUserData("EAACEdEose0cBAOVYZANbS25LytZBtVhG0Gs6dhYkXN3rpXpkxNZAiYhZByg8ocHCBRJqJjmZA66x7BZC3A7M4Fmr1eg5RtGrOJz1GFM7x6O5a4bw8iRlOa7iwVOR6HOoKYD6UvjvuWyLduJuM77MQDLL7N7ZABdWaU2Qp25bpyruwZDZD");
            int ibreak = 999;

            string left="";
            string right="";
            GameReferee.GetWinner_IndianHoldem("4:18:3", "6", ref left, ref right);
            

            int[] commcard = { 29,45,27,21,50,30,44 };
            int[] usercard = { 30, 44 };

            Array.Sort(commcard, delegate (int left1, int right1) {
                int leftScore = GameReferee.GetPokerCardInfo(left1)[0];
                int rightScore = GameReferee.GetPokerCardInfo(right1)[0];
                return leftScore.CompareTo(rightScore);
            });

            GameHandInfo gamehInfo= GameReferee.GetPokerCardScore(commcard,usercard);

            ibreak = 9;
        }

        static void RunService()
        {
            //var url = "http://+:9000";
            var url = ConfigurationManager.AppSettings.Get("ServiceURL");            
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
#if DEBUG
            BeforeTest();
#endif
            RunService();
        }
    }
}
