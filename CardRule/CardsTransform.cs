using System;
using System.Collections.Generic;

namespace CardRuleNS
{
    //
    public class CardsTransform
    {
        private static CardsTransform instance = null;
        public static CardsTransform Instance
        {
            get
            {
                if (instance == null)
                    instance = new CardsTransform();
                return instance;
            }
        }

        List<CardInfo> cardInfoList = new List<CardInfo>();

        CardsTransform()
        {
            CreatePukeInfoList();
        }

        public CardInfo[] TransToCardInfo(CardFace[] pukeFaceValues)
        {
            CardInfo[] cardInfos = new CardInfo[pukeFaceValues.Length];

            for (int i = 0; i < pukeFaceValues.Length; i++)
            {
                cardInfos[i].value = GetValue(pukeFaceValues[i]);
                cardInfos[i].suit = GetSuit(pukeFaceValues[i]);
            }

            return cardInfos;
        }

        public int GetValue(CardFace pukeFaceValue)
        {
            return cardInfoList[(int)pukeFaceValue].value;
        }

        public int GetSuit(CardFace pukeFaceValue)
        {
            return cardInfoList[(int)pukeFaceValue].suit;
        }

        public CardInfo[] CreateRemoveFaceValues(CardInfo[] cards, CardFace[] cardFaceValues)
        {
            if (cardFaceValues == null || cardFaceValues.Length == 0)
                return cards;

            CardInfo[] cardinfo = TransToCardInfo(cardFaceValues);
            return CreateRemoveCardInfos(cards, cardinfo);
        }

        public CardInfo[] CreateRemoveCardInfos(CardInfo[] cards, CardInfo[] removeCards)
        {
            if (removeCards == null || removeCards.Length == 0)
                return cards;

            List<CardInfo> cardinfoList = new List<CardInfo>(removeCards);
            List<CardInfo> newCards = new List<CardInfo>();
            int idx;

            for (int i = 0; i < cards.Length; i++)
            {
                idx = FindCard(cardinfoList, cards[i].value, cards[i].value);
                if (idx != -1)
                {
                    cardinfoList.RemoveAt(idx);
                    newCards.Add(cards[i]);
                }
            }

            return newCards.ToArray();
        }


        public int FindCard(List<CardInfo> cards, int value, int suit)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].value == value &&
                    cards[i].suit == suit)
                    return i;
            }

            return -1;
        }

        public int FindCard(CardInfo[] cards, int value)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].value == value)
                    return i;
            }

            return -1;
        }

        public CardInfo[] CreateFormatCards(CardFace[] pukeFaceValues, ref int laiziCount)
        {
            List<CardFace> newPukeFaceValueList = new List<CardFace>();
            laiziCount = 0;

            for (int i = 0; i < pukeFaceValues.Length; i++)
            {
                if (pukeFaceValues[i] == CardFace.Laizi)
                    laiziCount++;
                else
                    newPukeFaceValueList.Add(pukeFaceValues[i]);
            }

            CardInfo[] cards = TransToCardInfo(newPukeFaceValueList.ToArray());
            SortCards(cards);

            return cards;
        }

        public void SortCards(CardInfo[] cards)
        {
            Array.Sort(cards,

               delegate (CardInfo p1, CardInfo p2)
               {
                   if (p1.value > p2.value)
                       return 1;
                   else if (p1.value < p2.value)
                       return -1;
                   return 0;
               }
           );
        }


        void CreatePukeInfoList()
        {
            CardInfo cardInfo;

            for (int i = 0; i < (int)CardFace.Count; i++)
            {
                cardInfo = CreateCardInfo((CardFace)i);
                cardInfoList.Add(cardInfo);
            }
        }

        public CardInfo CreateCardInfo(CardFace pukeFaceValue)
        {
            CardInfo pukeInfo = new CardInfo();
            int n = (int)pukeFaceValue;
            int suit = n / 13;
            int value = n % 13 + 1;
            pukeInfo.suit = suit;
            pukeInfo.value = value;
            return pukeInfo;
        }

        public CardFace GetCardFace(int cardValue, int cardSuit)
        {
            return (CardFace)(cardSuit * 13 + cardValue - 1);
        }
    }
}
