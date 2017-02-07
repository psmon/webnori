using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainService.core
{
    public enum eBjGameState : int { Init=0,Bet=1,Play=2,End=3 };
    public enum eBjActionType : int { Bet=0,Hit=1,Stand=2 };
    public enum eBjResuestType : int { Bet=0,Action=1, End};

    public enum eBjResultType : int { Normal=0,Bust=1,BlackJack=2};
    public enum eBjWinType : int { Lost=0,Win=1};

    public class BJRes  //Server->Client
    {
        public string pid;
        public int reqAction;   //Request From Server
    }

    public class BJBetRes : BJRes
    {
        public BJBetRes()
        {
            pid = "BJBetRes";
            reqAction = (int)eBjResuestType.Bet;
        }
    }

    public class BJActionRes : BJRes
    {
        public string avableAction;
        public int addPlyCard;
        public int[] addDealerCards;

        public BJActionRes()
        {
            pid = "BJActionRes";
            reqAction = (int)eBjResuestType.Action;
        }
    }

    public class BJEndActionRes : BJRes
    {
        public int addPlyCard;
        public int[] addDealerCards;

        public int dealerResultType;
        public int plyResultType;
        public int dealerWinType;
        public int plyWinType;

        public BJEndActionRes()
        {
            pid = "BJEndActionRes";
            reqAction = (int)eBjResuestType.End;
        }
    }

    public class BJReq //Client->Server
    {
        public string pid;
        public int resAction;   //Response From Client
    }

    public class BJBetReq : BJReq
    {
        public int betAmount;
        public BJBetReq()
        {
            pid = "BJBetReq";
            resAction = (int)eBjResuestType.Bet;
        }
    }

    public class BJActionReq : BJReq
    {
        public int betAmount;
        public BJActionReq()
        {
            pid = "BJActionReq";
            resAction = (int)eBjResuestType.Action;
        }

    }


    public class BlackJackStage
    {
        public int stageNo=0;
        public int gameState = 0;
        public int betAmount = 0;
        public int playerNo = 0;

        public List<int> dealerCards = new List<int>();
        public List<int> userCards = new List<int>();
        public Queue<int> cardDeckQueue = new Queue<int>();
        
        public BlackJackStage(int _stageNo,int _playerNo)
        {
            stageNo = _stageNo;            
            playerNo = _playerNo;
            gameState = (int)eBjGameState.Init;

            List<int> cardDeckList = new List<int>();
            for (int cardNum=1; cardNum < 53; cardNum++)
            {
                cardDeckList.Add(cardNum);
            }
            foreach (int cardNum in cardDeckList.Randomize())
            {
                cardDeckQueue.Enqueue(cardNum);
            }
        }

        public object PlayGame(int gameAction, int _betAmount)
        {
            eBjActionType eGameAction = (eBjActionType)gameAction;
            object result = null;

            switch (eGameAction)
            {
                case eBjActionType.Bet:
                    betAmount = _betAmount;
                    break;
                case eBjActionType.Hit:
                    break;
                case eBjActionType.Stand:
                    break;
            }            
            return result;
        }
        
    }
}
