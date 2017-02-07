using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonModule.data;

namespace MainService.core
{
    public static class IEnumerableExtensions
    {

        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();

            return target.OrderBy(x => (r.Next()));
        }
    }

    public class GameReferee
    {
        static public int GetWinner(int gameType,string leftValue,string rightValue,ref string leftResult,ref string rightResult)
        {
            int whoWinner = -1; // 0 비김 , 1왼쪽이김, 2오른쪽이김
            switch (gameType)
            {
                case (int)eGameType.HandGame:
                    whoWinner = GetWinner_StoneGame(leftValue, rightValue);
                    break;
                case (int)eGameType.IndianHoldem:
                    whoWinner = GetWinner_IndianHoldem(leftValue, rightValue,ref leftResult,ref rightResult);
                    break;
                case (int)eGameType.QHoldem:                    
                    whoWinner = GetWinner_QHoldem(leftValue, rightValue, ref leftResult, ref rightResult);
                    break;
                
            }
            return whoWinner;
        }

        public static int InRange_StoneGame(int value )
        {
            int from = 0;
            int to = 2;

            if (value >= from && value <= to)
                return value;

            throw new Exception("Not Allow Value Range");
        }

        static public int[] GetCardInfo(int cardData)
        {
            int[] resultValue = new int[2];
            resultValue[0] = cardData % 13;
            resultValue[1] = cardData / 13;
            return resultValue;
        }
        
        static public GameHandInfo GetCardScore(int[] cardArray,int baseCard)
        {
            //Royal Straigt = 100 , Straitgh = 50 , Pair = 30
            GameHandInfo resultValue = new GameHandInfo();
            bool isPair = false;
            bool isStraight = true;
            bool isFulush = true;
            string rootHandStr = "";
            string cardName = string.Format("{0}",baseCard);
            resultValue.HandScore = baseCard;

            if (baseCard == 1)
            {
                resultValue.HandScore = 14;
                cardName = "ACE";
            }                
            

            int[] preCard = null;
            
            foreach (int cardNum in cardArray)
            {
                int[] curCard = GetCardInfo(cardNum);
                if (preCard != null)
                {
                    if (preCard[0] == curCard[0])
                    {
                        isPair = true;
                        rootHandStr = string.Format("{0} Pair", curCard[0]);
                    }                        
                    if (preCard[1] != curCard[1])
                        isFulush = false;
                    if (preCard[0] + 1 != curCard[0])
                        isStraight = false;
                }                
                preCard = (int[])curCard.Clone();
            }

            if (isPair)
                resultValue.HandScore += 30;

            if(isFulush && isStraight)
            {
                resultValue.HandScore += 100;
                rootHandStr = "Straight Flush";

            }
            else if(isStraight)
            {
                resultValue.HandScore += 80;
                rootHandStr = "Straight";
            }            
            if(rootHandStr.Length>0)
                resultValue.HandTxt = string.Format("{0},{1}Card high)", rootHandStr, cardName);
            else
                resultValue.HandTxt = string.Format("{0}Card high", cardName);

            return resultValue;
        }

        static public int GetWinner_IndianHoldem(string leftValue, string rightValue,ref string leftResult,ref string rightResult)
        {
            int whoWinner = -1;
            string[] cardValues = leftValue.Split(':');

            int creatorCard = int.Parse(cardValues[2]);
            int joinerCard = int.Parse(rightValue);

            int[] comunityCard = new int[2];
            comunityCard[0] = int.Parse(cardValues[0]);
            comunityCard[1] = int.Parse(cardValues[1]);

            int[] creatorCards = new int[3];
            int[] joinerCards = new int[3];

            creatorCards[0] = comunityCard[0];
            creatorCards[1] = comunityCard[1];
            creatorCards[2] = creatorCard;

            joinerCards[0] = comunityCard[0];
            joinerCards[1] = comunityCard[1];
            joinerCards[2] = joinerCard;

            int creatorScore = GetCardInfo(creatorCard)[0];
            int joinerScore = GetCardInfo(joinerCard)[0];

            Array.Sort(creatorCards, delegate (int left, int right) {
                int leftScore = GetCardInfo(left)[0];
                int rightScore = GetCardInfo(right)[0];
                return leftScore.CompareTo(rightScore);                
            });

            Array.Sort(joinerCards, delegate (int left, int right) {
                int leftScore = GetCardInfo(left)[0];
                int rightScore = GetCardInfo(right)[0];
                return leftScore.CompareTo(rightScore);
            });
            GameHandInfo creatorHand = GetCardScore(creatorCards, creatorScore);
            GameHandInfo joinerHand = GetCardScore(joinerCards, joinerScore);

            leftResult = creatorHand.HandTxt;
            rightResult = joinerHand.HandTxt;

            creatorScore =  creatorHand.HandScore;
            joinerScore =  joinerHand.HandScore;

            if (creatorScore == joinerScore)
                whoWinner = 0;
            else if (joinerScore < creatorScore)
                whoWinner = 1;
            else
                whoWinner = 2;

            return whoWinner;
        }

        static public int GetWinner_StoneGame(string leftValue, string rightValue)
        {
            // 0 묵 , 1 찌, 2 빠
            int whoWinner = -1;
            int leftData = InRange_StoneGame(int.Parse(leftValue));
            int rightData = InRange_StoneGame(int.Parse(rightValue));

            if (leftData == rightData)
                whoWinner = 0;
            
            if(leftData == 0)
            {
                switch (rightData)
                {
                    case 1:
                        whoWinner = 1;
                        break;
                    case 2:
                        whoWinner = 2;
                        break;
                }
            }

            if (leftData == 1)
            {
                switch (rightData)
                {
                    case 0:
                        whoWinner = 2;
                        break;
                    case 2:
                        whoWinner = 1;
                        break;
                }
            }

            if (leftData == 2)
            {
                switch (rightData)
                {
                    case 0:
                        whoWinner = 1;
                        break;
                    case 1:
                        whoWinner = 2;
                        break;
                }
            }            
            return whoWinner;
        }

        static public void makePokerData(ref int[] dest,string cardStr)
        {
            int idx = 0;
            foreach (string curCard in cardStr.Split(':'))
            {
                int curCardNum = int.Parse(curCard);
                dest[idx] = curCardNum;
                idx++;
            }
        }

        static public int GetWinner_QHoldem(string leftValue, string rightValue, ref string leftResult, ref string rightResult)
        {
            int whoWinner = -1;
            string joinnerCardsString = rightValue.Split('-')[0];
            string creatorCardsString = leftValue.Split('-')[0];
            string joinnerHasCardsString = rightValue.Split('-')[1];
            string creatorHasCardsString = leftValue.Split('-')[1];
            int[] creatorCards = new int[7];
            int[] joinerCards = new int[7];

            int[] creatorHasCards = new int[2];
            int[] joinerHasCards = new int[2];
            

            makePokerData(ref creatorCards, creatorCardsString);
            makePokerData(ref joinerCards, joinnerCardsString);

            makePokerData(ref creatorHasCards, creatorHasCardsString);
            makePokerData(ref joinerHasCards, joinnerHasCardsString);


            Array.Sort(creatorCards, delegate (int left, int right) {
                int leftScore = GetPokerCardInfo(left)[0];
                int rightScore = GetPokerCardInfo(right)[0];
                return leftScore.CompareTo(rightScore);
            });

            Array.Sort(joinerCards, delegate (int left, int right) {
                int leftScore = GetPokerCardInfo(left)[0];
                int rightScore = GetPokerCardInfo(right)[0];
                return leftScore.CompareTo(rightScore);
            });

            GameHandInfo creatorHand = GetPokerCardScore(creatorCards, creatorHasCards);
            GameHandInfo joinerHand = GetPokerCardScore(joinerCards, joinerHasCards);

            leftResult = creatorHand.HandTxt;
            rightResult = joinerHand.HandTxt;

            int creatorScore = creatorHand.HandScore;
            int joinerScore = joinerHand.HandScore;

            if (creatorScore == joinerScore)
                whoWinner = 0;
            else if (joinerScore < creatorScore)
                whoWinner = 1;
            else
                whoWinner = 2;

            return whoWinner;
        }

        static public string GetIndianGameCard(string exceptCards)
        {
            string resultValues=null;
            List<int> cardDeckList = new List<int>() {1,2,3,4,5,6,7,8,9,10,14,15,16,17,18,19,20,21,22,23 };
            List<int> cardDeck = new List<int>();

            foreach (int cardNum in cardDeckList.Randomize())
            {
                cardDeck.Add(cardNum);
            }

            if (exceptCards != null)
            {
                string[] excepCardsArray = exceptCards.Split(':');
                foreach(string cardStr in excepCardsArray)
                {
                    int curCard = int.Parse(cardStr);
                    cardDeck.Remove(curCard);
                }
                resultValues = cardDeck[0].ToString();
            }
            else
            {
                resultValues = string.Format("{0}:{1}:{2}", cardDeck[0], cardDeck[1], cardDeck[2]);
            }            
            return resultValues;
        }

        static public int[] GetPokerCardInfo(int cardData)
        {
            int[] resultValue = new int[2];
            resultValue[0] = cardData % 13 +1;
            resultValue[1] = cardData / 13;
            return resultValue;
        }

        static public GameHandInfo GetPokerCardScore(int[] cardArray, int[] baseCard)
        {
            //Royal Straigt = 100 , Straitgh = 50 , Pair = 30
            GameHandInfo resultValue = new GameHandInfo();            
            string rootHandStr = "";            
            int highCard = Math.Max(GetPokerCardInfo(baseCard[0])[0], GetPokerCardInfo(baseCard[1])[0]);

            if (GetPokerCardInfo(baseCard[0])[0] == 1)
                highCard = 1;

            if (GetPokerCardInfo(baseCard[1])[0] == 1)
                highCard = 1;

            string cardName = string.Format("{0}", highCard);
            resultValue.HandScore = highCard;

            switch (highCard)
            {
                case 1:
                    resultValue.HandScore = 14;
                    cardName = "ACE";
                    break;
                case 11:
                    cardName = "J";
                    break;
                case 12:
                    cardName = "Q";
                    break;
                case 13:
                    cardName = "K";
                    break;
            }            

            int[] preCard = null;            
            bool isStraight = false;
            bool isFulush = false;
            bool isStraightFulush = false;
            bool isFullHouse = false;

            bool isPair = false;
            bool isTriple = false;
            bool isFourCard = false;

            int pairCnt = 0;
            int fulushCnt = 0;
            int straightCnt = 0;

            int pairScore = 0;
            Dictionary<int, bool> pairList = new Dictionary<int, bool>();

            foreach (int cardNum in cardArray)
            {
                int[] curCard = GetPokerCardInfo(cardNum);
                if (preCard != null)
                {
                    if (preCard[0] == curCard[0])
                    {
                        pairScore = (10 * curCard[0]);
                        pairList[curCard[0]] = true;                        
                        pairCnt++;
                        isPair = true;
                        if (pairCnt == 2)
                            isTriple = true;
                        if (pairCnt == 3)
                            isFourCard = true;
                    }
                    else
                    {
                        pairCnt = 0;
                    }

                    if (preCard[1] == curCard[1])
                    {
                        fulushCnt++;
                        if (fulushCnt == 4)
                            isFulush = true;
                    }
                    else
                    {
                        fulushCnt = 0;
                    }
                        
                    if (preCard[0] + 1 == curCard[0])
                    {
                        straightCnt++;
                        if (straightCnt == 4)
                        {
                            isStraight = true;
                            if (isFulush)
                                isStraightFulush = true;
                        }
                    }
                    else
                    {
                        straightCnt = 0;
                    }

                }
                preCard = (int[])curCard.Clone();
            }

            if (isTriple && pairList.Count == 2)
                isFullHouse = true;
            resultValue.HandScore += pairScore;

            if (isStraightFulush)
            {
                resultValue.HandScore += 1000;
                rootHandStr = "Straight Fulush";
            }
            else if(isFourCard)
            {
                resultValue.HandScore += 900;
                rootHandStr = "FourCard";
            }
            else if (isFullHouse)
            {
                resultValue.HandScore += 800;
                rootHandStr = "FullHouse";
            }
            else if (isFulush)
            {
                resultValue.HandScore += 700;
                rootHandStr = "Fulush";
            }
            else if (isStraight)
            {
                resultValue.HandScore += 600;
                rootHandStr = "Straight";
            }
            else if (isTriple)
            {
                resultValue.HandScore += 700;
                rootHandStr = "Triple";

            }
            else if (isPair)
            {                
                resultValue.HandScore += (pairList.Count * 100);
                if (pairList.Count == 1)
                {
                    rootHandStr = "One Pair";                    
                }

                if (pairList.Count == 2)
                {
                    rootHandStr = "Two Pair";
                }
            }
            else
            {
                rootHandStr = "No Pair";
            }

            if (rootHandStr.Length > 0)
                resultValue.HandTxt = string.Format("{0},{1}Card high", rootHandStr, cardName);
            else
                resultValue.HandTxt = string.Format("{0}Card high", cardName);

            return resultValue;
        }

        static public string GetPokerCard(int cardCnt,string exceptCards)
        {
            string resultValues = null;
            List<int> cardDeckList = new List<int>();

            for(int idx = 0; idx < 52; idx++)
            {
                cardDeckList.Add(idx);
            }

            List<int> cardDeck = new List<int>();

            foreach (int cardNum in cardDeckList.Randomize())
            {
                cardDeck.Add(cardNum);
            }

            if (exceptCards != null)
            {
                string[] excepCardsArray = exceptCards.Split(':');
                foreach (string cardStr in excepCardsArray)
                {
                    int curCard = int.Parse(cardStr);
                    cardDeck.Remove(curCard);
                }                
            }            

            for(int cardIdx = 0; cardIdx < cardCnt; cardIdx++)
            {
                resultValues = resultValues + cardDeck[cardIdx];
                if (cardIdx != (cardCnt - 1))
                {
                    resultValues = resultValues + ":";
                }
            }
            
            return resultValues;
        }


    }
}
