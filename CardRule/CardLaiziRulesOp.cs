using System;
using System.Collections.Generic;

namespace CardRuleNS
{

    /// <summary>
    /// 牌型类型
    /// </summary>
    public enum RulePaiXingType
    {
        /// <summary>
        /// 不存在
        /// </summary>
        None,

        /// <summary>
        /// 单张
        /// </summary>
        Single,

        /// <summary>
        /// 对子
        /// </summary>
        DuiZi,

        /// <summary>
        /// 两对
        /// </summary>
        TwoDui,

        /// <summary>
        /// 三条
        /// </summary>
        SanTiao,

        /// <summary>
        /// 顺子
        /// </summary>
        ShunZi,

        /// <summary>
        /// 同花
        /// </summary>
        TongHua,

        /// <summary>
        /// 葫芦
        /// </summary>
        HuLu,

        /// <summary>
        /// 炸弹
        /// </summary>
        Bomb,

        /// <summary>
        /// 同花顺
        /// </summary>
        TongHuaShun,

        /// <summary>
        /// 五同
        /// </summary>
        WuTong,

    }

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


    /// <summary>
    /// 牌型数据
    /// </summary>
    public struct PaiXingInfo
    {
        /// <summary>
        /// 牌型
        /// </summary>
        public RulePaiXingType paiXingType;

        /// <summary>
        /// 牌型中所有牌的面值组合
        /// </summary>
        public RulePukeFaceValue[] cardFaceValues;

        /// <summary>
        /// 需要的赖子数量
        /// </summary>
        public int laiziCount;
    }

    public class CardLaiziRulesOp
    {
       
        /// <summary>
        /// 五同牌型组
        /// </summary>
        public List<PaiXingInfo> wutongList = new List<PaiXingInfo>();

        /// <summary>
        /// 同花顺牌型组
        /// </summary>
        public List<PaiXingInfo> tonghuashunList = new List<PaiXingInfo>();

        /// <summary>
        /// 顺子牌型组
        /// </summary>
        public List<PaiXingInfo> shunziList = new List<PaiXingInfo>();

        /// <summary>
        /// 葫芦牌型组
        /// </summary>
        public List<PaiXingInfo> huluList = new List<PaiXingInfo>();

        /// <summary>
        /// 铁枝牌型组
        /// </summary>
        public List<PaiXingInfo> tiezhiList = new List<PaiXingInfo>();

        /// <summary>
        /// 两对牌型组
        /// </summary>
        public List<PaiXingInfo> twoduiList = new List<PaiXingInfo>();

        /// <summary>
        /// 三条牌型组
        /// </summary>
        public List<PaiXingInfo> santiaoList = new List<PaiXingInfo>();

        /// <summary>
        /// 对子牌型组
        /// </summary>
        public List<PaiXingInfo> duiziList = new List<PaiXingInfo>();


        List<CardInfo> cardInfoList = new List<CardInfo>();

        public CardLaiziRulesOp()
        {
            CreatePukeInfoList();
        }


        /// <summary>
        /// 生成牌型组数据
        /// </summary>
        /// <param name="pukeFaceValues">手牌数据</param>
        public void CreatePaiXingArray(RulePukeFaceValue[] pukeFaceValues)
        {
            Clear();

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

            CardKey orgcardkey = CardLaiziRules.Instance.CreateCardKey(cards);

            CreateShunziList(orgcardkey, laiziCount);
            CreateTonghuashunList(orgcardkey, laiziCount);


            HashSet<CardKey> cardkeyHashSet5 = SplitCardsGroup5(cards);
            HashSet<CardKey> cardkeyHashSet4 = SplitCardsGroup4(cards);
            HashSet<CardKey> cardkeyHashSet3 = SplitCardsGroup3(cards);
            HashSet<CardKey> cardkeyHashSet2 = SplitCardsGroup2(cards);
            HashSet<CardKey> cardkeyHashSet1 = SplitCardsGroup1(cards);

            CreatePaiXingArrayBySplitGroup(cardkeyHashSet5, laiziCount, 5);
            CreatePaiXingArrayBySplitGroup(cardkeyHashSet4, laiziCount, 4);
            CreatePaiXingArrayBySplitGroup(cardkeyHashSet3, laiziCount, 3);
            CreatePaiXingArrayBySplitGroup(cardkeyHashSet2, laiziCount, 2);
            CreatePaiXingArrayBySplitGroup(cardkeyHashSet1, laiziCount, 1);

        }

        void CreateShunziList(CardKey cardKey, int laiziCount)
        {
            bool ret;
            CardInfo[] cardInfos;
            PaiXingInfo paiXingInfo;

            foreach (var item in CardLaiziRules.Instance.shunziKeyDict)
            {
                ret = CardLaiziRules.Instance.IsContains(cardKey, item.Key);

                if(ret == true && item.Value <= laiziCount)
                {
                    cardInfos = CardLaiziRules.Instance.CreateCardInfos(item.Key);
                    paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.ShunZi, item.Value);
                    shunziList.Add(paiXingInfo);
                }
            }
        }

        void CreateTonghuashunList(CardKey cardKey, int laiziCount)
        {
            bool ret;
            CardInfo[] cardInfos;
            PaiXingInfo paiXingInfo;

            foreach (var item in CardLaiziRules.Instance.tongHuaShunKeyDict)
            {
                ret = CardLaiziRules.Instance.IsContains(cardKey, item.Key);

                if (ret == true && item.Value <= laiziCount)
                {
                    cardInfos = CardLaiziRules.Instance.CreateCardInfos(item.Key);
                    paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.TongHuaShun, item.Value);
                    tonghuashunList.Add(paiXingInfo);
                }
            }
        }


        void CreatePaiXingArrayBySplitGroup(HashSet<CardKey> cardkeyHashSet, int laiziCount, int splitGroup)
        {
            foreach (var cardkey in cardkeyHashSet)
            {
                _CreatePaiXingArray(cardkey, laiziCount, splitGroup);
            }
        }
       
        void _CreatePaiXingArray(CardKey cardkey, int laiziCount, int splitGroup)
        {
            bool ret;
            int mustLaziCount;
            CardInfo[] cardInfos;
            PaiXingInfo paiXingInfo;

            ret = CardLaiziRules.Instance.wutongKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardLaiziRules.Instance.CreateCardInfos(cardkey);
                paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.WuTong, mustLaziCount);
                wutongList.Add(paiXingInfo);
            }

            ret = CardLaiziRules.Instance.huluKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardLaiziRules.Instance.CreateCardInfos(cardkey);
                paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.HuLu, mustLaziCount);
                huluList.Add(paiXingInfo);
            }

            if (splitGroup == 5)
                return;

            ret = CardLaiziRules.Instance.tiezhiKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardLaiziRules.Instance.CreateCardInfos(cardkey);
                paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.Bomb, mustLaziCount);
                tiezhiList.Add(paiXingInfo);
            }

            ret = CardLaiziRules.Instance.twoduiKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardLaiziRules.Instance.CreateCardInfos(cardkey);
                paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.TwoDui, mustLaziCount);
                twoduiList.Add(paiXingInfo);
            }

            if (splitGroup == 4)
                return;

            ret = CardLaiziRules.Instance.santiaoKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardLaiziRules.Instance.CreateCardInfos(cardkey);
                paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.SanTiao, mustLaziCount);
                santiaoList.Add(paiXingInfo);
            }

            if (splitGroup == 3)
                return;

            ret = CardLaiziRules.Instance.duiziKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardLaiziRules.Instance.CreateCardInfos(cardkey);
                paiXingInfo = CreatePaiXingInfo(cardInfos, RulePaiXingType.DuiZi, mustLaziCount);
                duiziList.Add(paiXingInfo);
            }
        }

        PaiXingInfo CreatePaiXingInfo(CardInfo[] cardInfos, RulePaiXingType paiXingType, int laiziCount)
        {
            PaiXingInfo paiXingInfo = new PaiXingInfo();
            paiXingInfo.laiziCount = laiziCount;
            paiXingInfo.paiXingType = paiXingType;
            paiXingInfo.cardFaceValues = new RulePukeFaceValue[cardInfos.Length];

            RulePukeFaceValue face;

            for (int i=0; i < cardInfos.Length; i++)
            {
                face = GetRulePukeFaceValue(cardInfos[i].value, cardInfos[i].suit);
                paiXingInfo.cardFaceValues[i] = face;
            }

            return paiXingInfo;
        }


        HashSet<CardKey> SplitCardsGroup5(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());

            //可能的五同
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

            //可能的铁枝
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
                    for (int k = j + 1; k < cards.Length; k++)
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


        HashSet<CardKey> SplitCardsGroup1(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());
            CardKey cardkey;

            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = new CardKey();
                cardkey = CardLaiziRules.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);
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
            int n = (int)pukeFaceValue;
            int suit = n / 13;
            int value = n % 13 + 1;
            pukeInfo.suit = suit;
            pukeInfo.value = value;
            return pukeInfo;
        }

        RulePukeFaceValue GetRulePukeFaceValue(int cardValue, int cardSuit)
        {
            return (RulePukeFaceValue)(cardSuit * 13 + cardValue - 1);
        }

        void Clear()
        {
            wutongList.Clear();
            tonghuashunList.Clear();
            shunziList.Clear();
            huluList.Clear();
            tiezhiList.Clear();
            twoduiList.Clear();
            santiaoList.Clear();
            duiziList.Clear();
        }
    }
}
