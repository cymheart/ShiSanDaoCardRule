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

        public CardInfo[] TransToCardInfo(CardFace[] cardFaces)
        {
            CardInfo[] cardInfos = new CardInfo[cardFaces.Length];

            for (int i = 0; i < cardFaces.Length; i++)
            {
                cardInfos[i].value = GetValue(cardFaces[i]);
                cardInfos[i].suit = GetSuit(cardFaces[i]);
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

        public CardInfo[] CreateRemoveFaceValues(CardFace[] cardFaces, CardFace[] removeCardFaceValues)
        {
            CardInfo[] cards = TransToCardInfo(cardFaces);
            if (removeCardFaceValues == null || removeCardFaceValues.Length == 0)
                return cards;

            CardInfo[] cardinfo = TransToCardInfo(removeCardFaceValues);
            return CreateRemoveCardInfos(cards, cardinfo);
        }

        public CardInfo[] CreateRemoveFaceValues(CardInfo[] cards, CardFace[] removeCardFaceValues)
        {
            if (removeCardFaceValues == null || removeCardFaceValues.Length == 0)
                return cards;

            CardInfo[] cardinfo = TransToCardInfo(removeCardFaceValues);
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
                idx = FindCard(cardinfoList, cards[i].value, cards[i].suit);
                if (idx == -1)      
                    newCards.Add(cards[i]);
                else
                    cardinfoList.RemoveAt(idx);
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

        public int FindCardFace(CardFace[] cardfaces, CardFace cardface)
        {
            for (int i = 0; i < cardfaces.Length; i++)
            {
                if (cardfaces[i] == cardface)
                    return i;
            }

            return -1;
        }


        /// <summary>
        /// 生成格式化后的手牌信息
        /// </summary>
        /// <param name="cardFaces">原始带赖子手牌的cardface信息</param>
        /// <param name="laiziCount">返回手牌中赖子的数量</param>
        /// <returns>返回值为移除赖子牌的排序后的牌的cardinfo信息</returns>
        public CardInfo[] CreateFormatCards(CardFace[] cardFaces, CardFace[] laiziCardFaces, ref int laiziCount)
        {
            CardFace[] newCardFaces;
            laiziCount = 0;
            newCardFaces = RemoveLaizi(cardFaces, laiziCardFaces, ref laiziCount);
            CardInfo[] cards = TransToCardInfo(newCardFaces);
            SortCards(cards);
            return cards;
        }

        /// <summary>
        /// 移除牌中的赖子，获取没有赖子的cardfaces
        /// </summary>
        /// <param name="cardFaces"></param>
        /// <param name="laiziCount"></param>
        /// <returns></returns>
        public CardFace[] RemoveLaizi(CardFace[] cardFaces, CardFace[] laiziCardFaces, ref int laiziCount)
        {
            List<CardFace> newCardFaceList = new List<CardFace>();
            laiziCount = 0;

            for (int i = 0; i < cardFaces.Length; i++)
            {
                if(FindCardFace(laiziCardFaces, cardFaces[i]) != -1)
                    laiziCount++;
                else
                    newCardFaceList.Add(cardFaces[i]);
            }

            return newCardFaceList.ToArray();
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

        public CardInfo[] CreateCardInfos(CardFace[] cardFaces)
        {
            CardInfo[] cards = new CardInfo[cardFaces.Length];
            for (int i = 0; i < cardFaces.Length; i++)
                cards[i] = CreateCardInfo(cardFaces[i]);
            return cards;
        }

        public CardInfo CreateCardInfo(CardFace cardFaces)
        {
            CardInfo pukeInfo = new CardInfo();

            if (cardFaces == CardFace.BlackJoker)
            {
                pukeInfo.suit = -1;
                pukeInfo.value = 14;
                return pukeInfo;
            }
            else if (cardFaces == CardFace.RedJoker)
            {
                pukeInfo.suit = -1;
                pukeInfo.value = 15;
                return pukeInfo;
            }
            else if (cardFaces == CardFace.Laizi)
            {
                pukeInfo.suit = -1;
                pukeInfo.value = 16;
                return pukeInfo;
            }

            int n = (int)cardFaces;
            int suit = n / 13;
            int value = n % 13 + 1;
            pukeInfo.suit = suit;
            pukeInfo.value = value;
            return pukeInfo;
        }

        public CardFace GetCardFace(int cardValue, int cardSuit)
        {
            if (cardValue == 14)
                return CardFace.BlackJoker;
            else if(cardValue == 15)
                return CardFace.RedJoker;

            return (CardFace)(cardSuit * 13 + cardValue - 1);
        }
    }
}
