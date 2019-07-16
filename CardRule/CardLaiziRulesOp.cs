using GameAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRule
{
    // <summary>
    /// 扑克面值
    /// </summary>
    public enum RulePukeFaceValue
    {
        /// <summary>
        /// 红方块A
        /// </summary>
        Diamond_A = 0,

        /// <summary>
        /// 红方块2
        /// </summary>
        Diamond_2,

        /// <summary>
        /// 红方块3
        /// </summary>
        Diamond_3,

        /// <summary>
        /// 红方块4
        /// </summary>
        Diamond_4,

        /// <summary>
        /// 红方块5
        /// </summary>
        Diamond_5,

        /// <summary>
        /// 红方块6
        /// </summary>
        Diamond_6,

        /// <summary>
        /// 红方块7
        /// </summary>
        Diamond_7,

        /// <summary>
        /// 红方块8
        /// </summary>
        Diamond_8,

        /// <summary>
        /// 红方块9
        /// </summary>
        Diamond_9,

        /// <summary>
        /// 红方块10
        /// </summary>
        Diamond_10,

        /// <summary>
        /// 红方块J
        /// </summary>
        Diamond_J,

        /// <summary>
        /// 红方块Q
        /// </summary>
        Diamond_Q,

        /// <summary>
        /// 红方块K
        /// </summary>
        Diamond_K,

        /// <summary>
        /// 黑梅花A
        /// </summary>
        Club_A,

        /// <summary>
        /// 黑梅花2
        /// </summary>
        Club_2,

        /// <summary>
        /// 黑梅花3
        /// </summary>
        Club_3,

        /// <summary>
        /// 黑梅花4
        /// </summary>
        Club_4,

        /// <summary>
        /// 黑梅花5
        /// </summary>
        Club_5,

        /// <summary>
        /// 黑梅花6
        /// </summary>
        Club_6,

        /// <summary>
        /// 黑梅花7
        /// </summary>
        Club_7,

        /// <summary>
        /// 黑梅花8
        /// </summary>
        Club_8,

        /// <summary>
        /// 黑梅花9
        /// </summary>
        Club_9,

        /// <summary>
        /// 黑梅花10
        /// </summary>
        Club_10,

        /// <summary>
        /// 黑梅花J
        /// </summary>
        Club_J,

        /// <summary>
        /// 黑梅花Q
        /// </summary>
        Club_Q,

        /// <summary>
        /// 黑梅花K
        /// </summary>
        Club_K,

        /// <summary>
        /// 红心A
        /// </summary>
        Heart_A,

        /// <summary>
        /// 红心2
        /// </summary>
        Heart_2,

        /// <summary>
        /// 红心3
        /// </summary>
        Heart_3,

        /// <summary>
        /// 红心4
        /// </summary>
        Heart_4,

        /// <summary>
        /// 红心5
        /// </summary>
        Heart_5,

        /// <summary>
        /// 红心6
        /// </summary>
        Heart_6,

        /// <summary>
        /// 红心7
        /// </summary>
        Heart_7,

        /// <summary>
        /// 红心8
        /// </summary>
        Heart_8,

        /// <summary>
        /// 红心9
        /// </summary>
        Heart_9,

        /// <summary>
        /// 红心10
        /// </summary>
        Heart_10,

        /// <summary>
        /// 红心J
        /// </summary>
        Heart_J,

        /// <summary>
        /// 红心Q
        /// </summary>
        Heart_Q,

        /// <summary>
        /// 红心K
        /// </summary>
        Heart_K,


        /// <summary>
        /// 黑桃A
        /// </summary>
        Spade_A,

        /// <summary>
        /// 黑桃2
        /// </summary>
        Spade_2,

        /// <summary>
        /// 黑桃3
        /// </summary>
        Spade_3,

        /// <summary>
        /// 黑桃4
        /// </summary>
        Spade_4,

        /// <summary>
        /// 黑桃5
        /// </summary>
        Spade_5,

        /// <summary>
        /// 黑桃6
        /// </summary>
        Spade_6,

        /// <summary>
        /// 黑桃7
        /// </summary>
        Spade_7,

        /// <summary>
        /// 黑桃8
        /// </summary>
        Spade_8,

        /// <summary>
        /// 黑桃9
        /// </summary>
        Spade_9,

        /// <summary>
        /// 黑桃10
        /// </summary>
        Spade_10,

        /// <summary>
        /// 黑桃J
        /// </summary>
        Spade_J,

        /// <summary>
        /// 黑桃Q
        /// </summary>
        Spade_Q,

        /// <summary>
        /// 黑桃K
        /// </summary>
        Spade_K,


        /// <summary>
        /// 赖子
        /// </summary>
        Laizi,


        /// <summary>
        /// 数量
        /// </summary>
        Count
    }

    public class CardLaiziRulesOp
    {
        List<CardInfo> cardInfoList = new List<CardInfo>();
        public CardLaiziRulesOp()
        {
            CreatePukeInfoList();
        }

        public void CreatePaiXingArray(RulePukeFaceValue[] pukeFaceValues)
        {
            List<RulePukeFaceValue> newPukeFaceValueList = new List<RulePukeFaceValue>();
            int laiziCount = 0;

            for (int i = 0; i < pukeFaceValues.Length; i++)
            {
                if (pukeFaceValues[i] == RulePukeFaceValue.Laizi)
                    laiziCount++;
                else
                    newPukeFaceValueList.Add(pukeFaceValues[i]);
            }

            CardInfo[] cards = TransToCardInfo(newPukeFaceValueList.ToArray());
            SortCards(cards);

            HashSet<CardKey> cardkeyHashSet5 = SplitCardsGroup5(cards);
            HashSet<CardKey> cardkeyHashSet4 = SplitCardsGroup4(cards);
            HashSet<CardKey> cardkeyHashSet3 = SplitCardsGroup3(cards);
            HashSet<CardKey> cardkeyHashSet2 = SplitCardsGroup2(cards);

            CreatePaiXingArrayBySplitGroup5(cardkeyHashSet5, laiziCount);
        }


        void CreatePaiXingArrayBySplitGroup5(HashSet<CardKey> cardkeyHashSet5, int laiziCount)
        {
            bool ret;
            int mustLaziCount;
            CardInfo[] cardInfos;

            foreach (var cardkey in cardkeyHashSet5)
            {
                ret = CardLaiziRules.Instance.shunziKeyDict.TryGetValue(cardkey, out mustLaziCount);
                if (ret == true && mustLaziCount <= laiziCount)
                {
                    cardInfos = CardLaiziRules.Instance.CreateCardInfos(cardkey);
                }

                ret = CardLaiziRules.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out mustLaziCount);
                if (ret == true && mustLaziCount <= laiziCount)
                {

                }

                ret = CardLaiziRules.Instance.huluKeyDict.TryGetValue(cardkey, out mustLaziCount);
                if (ret == true && mustLaziCount <= laiziCount)
                {

                }

                ret = CardLaiziRules.Instance.wutongKeyDict.TryGetValue(cardkey, out mustLaziCount);
                if (ret == true && mustLaziCount <= laiziCount)
                {

                }
            }
        }


        HashSet<CardKey> SplitCardsGroup5(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());

            //可能的顺子,五同
            CardKey cardkey;
            for (int i = 0; i < cards.Length - 4; i++)
            {
                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 3].value, cards[i + 3].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 4].value, cards[i + 4].suit);

                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);
            }

            if (cards[0].value == 1 && cards[cards.Length - 1].value == 13)
            {
                int i = cards.Length - 1;
                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i - 3].value, cards[i - 3].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i - 2].value, cards[i - 2].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i - 1].value, cards[i - 1].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[0].value, cards[0].suit);

                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);
            }

            //可能的葫芦
            CardKey tmpCardkey;

            for (int i = 0; i < cards.Length - 2; i++)
            {
                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);

                for (int j = 0; j < cards.Length - 1; j++)
                {
                    if (j < i - 1 || j > i + 2)
                    {
                        tmpCardkey = cardkey;
                        tmpCardkey = CardLaiziRules.Instance.AppendCardToCardKey(tmpCardkey, cards[j].value, cards[j].suit);
                        tmpCardkey = CardLaiziRules.Instance.AppendCardToCardKey(tmpCardkey, cards[j + 1].value, cards[j + 1].suit);
                        if (!cardkeyHashSet.Contains(tmpCardkey))
                            cardkeyHashSet.Add(tmpCardkey);
                    }
                }
            }

            return cardkeyHashSet;
        }

        HashSet<CardKey> SplitCardsGroup4(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());

            //可能的铁枝,顺子
            CardKey cardkey;
            for (int i = 0; i < cards.Length - 3; i++)
            {
                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 3].value, cards[i + 3].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);

                if (i == cards.Length - 4)
                    continue;

                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 3].value, cards[i + 3].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 4].value, cards[i + 4].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);


                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 3].value, cards[i + 3].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 4].value, cards[i + 4].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);


                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 4].value, cards[i + 4].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);


                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 3].value, cards[i + 3].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 4].value, cards[i + 4].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);
            }



            //可能的两对
            CardKey tmpCardkey;

            for (int i = 0; i < cards.Length - 1; i++)
            {
                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);

                for (int j = 0; j < cards.Length - 1; j++)
                {
                    if (j < i - 1 || j > i + 1)
                    {
                        tmpCardkey = cardkey;
                        tmpCardkey = CardLaiziRules.Instance.AppendCardToCardKey(tmpCardkey, cards[j].value, cards[j].suit);
                        tmpCardkey = CardLaiziRules.Instance.AppendCardToCardKey(tmpCardkey, cards[j + 1].value, cards[j + 1].suit);
                        if (!cardkeyHashSet.Contains(tmpCardkey))
                            cardkeyHashSet.Add(tmpCardkey);
                    }
                }
            }

            return cardkeyHashSet;
        }


        HashSet<CardKey> SplitCardsGroup3(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());
            CardKey cardkey;

            for (int i = 0; i < cards.Length - 2; i++)
            {
                for (int j = i + 1; j < cards.Length - 1; j++)
                {
                    for (int k = j + 1; j < cards.Length; k++)
                    {
                        cardkey = new CardKey();
                        cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                        cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                        cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                        if (!cardkeyHashSet.Contains(cardkey))
                            cardkeyHashSet.Add(cardkey);
                    }
                }
            }

            return cardkeyHashSet;
        }


        HashSet<CardKey> SplitCardsGroup2(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());
            CardKey cardkey;

            for (int i = 0; i < cards.Length - 1; i++)
            {
                for (int j = i + 1; j < cards.Length; j++)
                {
                    cardkey = new CardKey();
                    cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                    cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                    if (!cardkeyHashSet.Contains(cardkey))
                        cardkeyHashSet.Add(cardkey);
                }
            }

            return cardkeyHashSet;
        }



        void SortCards(CardInfo[] cards)
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

        CardInfo[] TransToCardInfo(RulePukeFaceValue[] pukeFaceValues)
        {
            CardInfo[] cardInfos = new CardInfo[pukeFaceValues.Length];

            for (int i = 0; i < pukeFaceValues.Length; i++)
            {
                cardInfos[i].value = GetValue(pukeFaceValues[i]);
                cardInfos[i].suit = GetSuit(pukeFaceValues[i]);
            }

            return cardInfos;
        }

        int GetValue(RulePukeFaceValue pukeFaceValue)
        {
            return cardInfoList[(int)pukeFaceValue].value;
        }

        int GetSuit(RulePukeFaceValue pukeFaceValue)
        {
            return cardInfoList[(int)pukeFaceValue].suit;
        }


        void CreatePukeInfoList()
        {
            CardInfo cardInfo;

            for (int i = 0; i < (int)RulePukeFaceValue.Count; i++)
            {
                cardInfo = CreateCardInfo((RulePukeFaceValue)i);
                cardInfoList.Add(cardInfo);
            }
        }

        CardInfo CreateCardInfo(RulePukeFaceValue pukeFaceValue)
        {
            CardInfo pukeInfo = new CardInfo();

            switch (pukeFaceValue)
            {
                case RulePukeFaceValue.Diamond_A: pukeInfo.suit = 0; pukeInfo.value = 1; break;
                case RulePukeFaceValue.Diamond_2: pukeInfo.suit = 0; pukeInfo.value = 2; break;
                case RulePukeFaceValue.Diamond_3: pukeInfo.suit = 0; pukeInfo.value = 3; break;
                case RulePukeFaceValue.Diamond_4: pukeInfo.suit = 0; pukeInfo.value = 4; break;
                case RulePukeFaceValue.Diamond_5: pukeInfo.suit = 0; pukeInfo.value = 5; break;
                case RulePukeFaceValue.Diamond_6: pukeInfo.suit = 0; pukeInfo.value = 6; break;
                case RulePukeFaceValue.Diamond_7: pukeInfo.suit = 0; pukeInfo.value = 7; break;
                case RulePukeFaceValue.Diamond_8: pukeInfo.suit = 0; pukeInfo.value = 8; break;
                case RulePukeFaceValue.Diamond_9: pukeInfo.suit = 0; pukeInfo.value = 9; break;
                case RulePukeFaceValue.Diamond_10: pukeInfo.suit = 0; pukeInfo.value = 10; break;
                case RulePukeFaceValue.Diamond_J: pukeInfo.suit = 0; pukeInfo.value = 11; break;
                case RulePukeFaceValue.Diamond_Q: pukeInfo.suit = 0; pukeInfo.value = 12; break;
                case RulePukeFaceValue.Diamond_K: pukeInfo.suit = 0; pukeInfo.value = 13; break;

                //
                case RulePukeFaceValue.Club_A: pukeInfo.suit = 1; pukeInfo.value = 1; break;
                case RulePukeFaceValue.Club_2: pukeInfo.suit = 1; pukeInfo.value = 2; break;
                case RulePukeFaceValue.Club_3: pukeInfo.suit = 1; pukeInfo.value = 3; break;
                case RulePukeFaceValue.Club_4: pukeInfo.suit = 1; pukeInfo.value = 4; break;
                case RulePukeFaceValue.Club_5: pukeInfo.suit = 1; pukeInfo.value = 5; break;
                case RulePukeFaceValue.Club_6: pukeInfo.suit = 1; pukeInfo.value = 6; break;
                case RulePukeFaceValue.Club_7: pukeInfo.suit = 1; pukeInfo.value = 7; break;
                case RulePukeFaceValue.Club_8: pukeInfo.suit = 1; pukeInfo.value = 8; break;
                case RulePukeFaceValue.Club_9: pukeInfo.suit = 1; pukeInfo.value = 9; break;
                case RulePukeFaceValue.Club_10: pukeInfo.suit = 1; pukeInfo.value = 10; break;
                case RulePukeFaceValue.Club_J: pukeInfo.suit = 1; pukeInfo.value = 11; break;
                case RulePukeFaceValue.Club_Q: pukeInfo.suit = 1; pukeInfo.value = 12; break;
                case RulePukeFaceValue.Club_K: pukeInfo.suit = 1; pukeInfo.value = 13; break;

                //
                case RulePukeFaceValue.Heart_A: pukeInfo.suit = 2; pukeInfo.value = 1; break;
                case RulePukeFaceValue.Heart_2: pukeInfo.suit = 2; pukeInfo.value = 2; break;
                case RulePukeFaceValue.Heart_3: pukeInfo.suit = 2; pukeInfo.value = 3; break;
                case RulePukeFaceValue.Heart_4: pukeInfo.suit = 2; pukeInfo.value = 4; break;
                case RulePukeFaceValue.Heart_5: pukeInfo.suit = 2; pukeInfo.value = 5; break;
                case RulePukeFaceValue.Heart_6: pukeInfo.suit = 2; pukeInfo.value = 6; break;
                case RulePukeFaceValue.Heart_7: pukeInfo.suit = 2; pukeInfo.value = 7; break;
                case RulePukeFaceValue.Heart_8: pukeInfo.suit = 2; pukeInfo.value = 8; break;
                case RulePukeFaceValue.Heart_9: pukeInfo.suit = 2; pukeInfo.value = 9; break;
                case RulePukeFaceValue.Heart_10: pukeInfo.suit = 2; pukeInfo.value = 10; break;
                case RulePukeFaceValue.Heart_J: pukeInfo.suit = 2; pukeInfo.value = 11; break;
                case RulePukeFaceValue.Heart_Q: pukeInfo.suit = 2; pukeInfo.value = 12; break;
                case RulePukeFaceValue.Heart_K: pukeInfo.suit = 2; pukeInfo.value = 13; break;

                //
                case RulePukeFaceValue.Spade_A: pukeInfo.suit = 3; pukeInfo.value = 1; break;
                case RulePukeFaceValue.Spade_2: pukeInfo.suit = 3; pukeInfo.value = 2; break;
                case RulePukeFaceValue.Spade_3: pukeInfo.suit = 3; pukeInfo.value = 3; break;
                case RulePukeFaceValue.Spade_4: pukeInfo.suit = 3; pukeInfo.value = 4; break;
                case RulePukeFaceValue.Spade_5: pukeInfo.suit = 3; pukeInfo.value = 5; break;
                case RulePukeFaceValue.Spade_6: pukeInfo.suit = 3; pukeInfo.value = 6; break;
                case RulePukeFaceValue.Spade_7: pukeInfo.suit = 3; pukeInfo.value = 7; break;
                case RulePukeFaceValue.Spade_8: pukeInfo.suit = 3; pukeInfo.value = 8; break;
                case RulePukeFaceValue.Spade_9: pukeInfo.suit = 3; pukeInfo.value = 9; break;
                case RulePukeFaceValue.Spade_10: pukeInfo.suit = 3; pukeInfo.value = 10; break;
                case RulePukeFaceValue.Spade_J: pukeInfo.suit = 3; pukeInfo.value = 11; break;
                case RulePukeFaceValue.Spade_Q: pukeInfo.suit = 3; pukeInfo.value = 12; break;
                case RulePukeFaceValue.Spade_K: pukeInfo.suit = 3; pukeInfo.value = 13; break;

                //
                case RulePukeFaceValue.Laizi: pukeInfo.suit = -1; pukeInfo.value = 14; break;
            }

            return pukeInfo;
        }
    }
}
