using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRuleNS
{
    /// <summary>
    /// 用于把手牌数据打包成紧凑的cardkey类型
    /// </summary>
    public struct CardKey
    {
        public uint bit_103_96;
        public uint bit_95_64;
        public uint bit_63_32;
        public uint bit_31_0;

        public CardKey(uint bit_103_96, uint bit_95_64, uint bit_63_32, uint bit_31_0)
        {
            this.bit_103_96 = bit_103_96;
            this.bit_95_64 = bit_95_64;
            this.bit_63_32 = bit_63_32;
            this.bit_31_0 = bit_31_0;
        }

        public class EqualityComparer : IEqualityComparer<CardKey>
        {
            public bool Equals(CardKey x, CardKey y)
            {
                return x.bit_31_0 == y.bit_31_0 &&
                    x.bit_63_32 == y.bit_63_32 &&
                     x.bit_95_64 == y.bit_95_64 &&
                     x.bit_103_96 == y.bit_103_96;
            }

            public int GetHashCode(CardKey obj)
            {
                int hash = (int)obj.bit_31_0;
                hash = hash * 31 + (int)obj.bit_63_32;
                hash = hash * 31 + (int)obj.bit_95_64;
                hash = hash * 31 + (int)obj.bit_103_96;
                return hash;
            }
        }
    }

    public struct CardsTypeCombInfo
    {
        public int laiziCount;
        public float baseScore;

        public CardsTypeCombInfo(int laiziCount, float baseScore)
        {
            this.laiziCount = laiziCount;
            this.baseScore = baseScore;
        }
    }

    /// <summary>
    /// 十三道赖子牌型字典
    /// </summary>
    public class CardsTypeDict
    {
        private static CardsTypeDict instance = null;
        public static CardsTypeDict Instance
        {
            get
            {
                if (instance == null)
                    instance = new CardsTypeDict();
                return instance;
            }
        }

        public Dictionary<CardKey, CardsTypeCombInfo> shunziKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> tongHuaShunKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> huluKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> twoduiKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> wutongKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> tiezhiKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> santiaoKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> duiziKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> tonghuaKeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, CardsTypeCombInfo> single5KeyDict = new Dictionary<CardKey, CardsTypeCombInfo>(new CardKey.EqualityComparer());
      
        //
        bool isCheckSuitCount = true;
        int limitSameSuitCount = 2;

        CardsTypeDict()
        {
            CreateDict();
        }

        /// <summary>
        /// 生成牌型查询字典
        /// </summary>
        void CreateDict()
        {
            CreateShunziDict();
            CreateHuluDict();
            CreateTwoDuiDict();
            CreateWuTongDict();
            CreateTieZhiDict();
            CreateSanTiaoDict();
            CreateDuiZiDict();
            CreateTongHuaDict();
           // CreateSingle5Dict();
        }

        void AddToDict(Dictionary<CardKey, CardsTypeCombInfo> dict, CardKey cardkey, int count, float score)
        {
            CardsTypeCombInfo old;
            if (dict.TryGetValue(cardkey, out old))
            {
                if (old.baseScore < score)
                    old.baseScore = score;
                dict[cardkey] = old;
            }
            else
            {
                dict[cardkey] = new CardsTypeCombInfo(count, score);
            }
        }

        /// <summary>
        /// 添加牌数据到现有cardkey
        /// </summary>
        /// <param name="cardkey"></param>
        /// <param name="cardValue"></param>
        /// <param name="cardSuit"></param>
        /// <returns></returns>
        public CardKey AppendCardToCardKey(CardKey cardkey, int cardValue, int cardSuit)
        {
            int n = (cardValue - 1) * 8;
            int m = cardSuit * 2;

            if (n <= 31)
            {
                cardkey.bit_31_0 = FlagKeyBit(cardkey.bit_31_0, n, m);
            }
            else if (n <= 63)
            {
                n -= 32;
                cardkey.bit_63_32 = FlagKeyBit(cardkey.bit_63_32, n, m);
            }
            else if (n <= 95)
            {
                n -= 64;
                cardkey.bit_95_64 = FlagKeyBit(cardkey.bit_95_64, n, m);
            }
            else
            {
                n -= 96;
                cardkey.bit_103_96 = FlagKeyBit(cardkey.bit_103_96, n, m);
            }

            return cardkey;
        }

        /// <summary>
        /// 检查cardkey是否包含checkCardKey的牌数据
        /// </summary>
        /// <param name="cardkey"></param>
        /// <param name="checkCardKey"></param>
        /// <returns></returns>
        public bool IsContains(CardKey cardkey, CardKey checkCardKey)
        {
            uint a = cardkey.bit_31_0 & checkCardKey.bit_31_0;
            uint b = cardkey.bit_63_32 & checkCardKey.bit_63_32;
            uint c = cardkey.bit_95_64 & checkCardKey.bit_95_64;
            uint d = cardkey.bit_103_96 & checkCardKey.bit_103_96;

            if (a == checkCardKey.bit_31_0 &&
                b == checkCardKey.bit_63_32 &&
                c == checkCardKey.bit_95_64 &&
                d == checkCardKey.bit_103_96)
            {
                return true;
            }

            return false;
        }

        public bool IsEqual(CardKey cardkeyA, CardKey cardkeyB)
        {
            if (cardkeyA.bit_31_0 == cardkeyB.bit_31_0 &&
                cardkeyA.bit_63_32 == cardkeyB.bit_63_32 &&
                cardkeyA.bit_95_64 == cardkeyB.bit_95_64 &&
                cardkeyA.bit_103_96 == cardkeyB.bit_103_96)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 通过CardInfo[]生成CardKey
        /// </summary>
        /// <param name="cardinfos"></param>
        /// <returns></returns>
        public CardKey CreateCardKey(CardInfo[] cardinfos)
        {
            CardKey cardkey = new CardKey();
            if (cardinfos == null || cardinfos.Length == 0)
                return cardkey;

            for (int i = 0; i < cardinfos.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cardinfos[i].value, cardinfos[i].suit);
            }

            return cardkey;
        }

        /// <summary>
        /// 根据cardkey还原出CardInfo
        /// </summary>
        /// <param name="cardkey"></param>
        /// <returns></returns>
        public CardInfo[] CreateCardInfos(CardKey cardkey)
        {
            List<CardInfo> cardInfoList = new List<CardInfo>();
            CreateCardInfoToList((int)cardkey.bit_31_0, 0, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_63_32, 4, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_95_64, 8, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_103_96, 12, cardInfoList);
            return cardInfoList.ToArray();
        }

        void CreateShunziDict()
        {
            shunziKeyDict.Clear();

            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

            float score = CardsTypeEvaluation.Instance.GetShunZiBaseScore(10);
            AddShunziKeyToList(null, 0, score);

            for (int i = 1; i <= 10; i++)
            {
                score = CardsTypeEvaluation.Instance.GetShunZiBaseScore(i);

                for (int s0 = -1; s0 < 4; s0++)
                {            
                    if (s0 != -1)
                    {
                        cards[0].value = i;
                        cards[0].suit = s0;                   
                        AddShunziKeyToList(cards, 1, score);
                    }
                    else
                        cards[0].value = -1;

                    for (int s1 = -1; s1 < 4; s1++)
                    {
                        if (s1 != -1)
                        {
                            cards[1].value = i + 1;
                            cards[1].suit = s1;
                            AddShunziKeyToList(cards, 2, score);
                        }
                        else
                            cards[1].value = -1;

                        for (int s2 = -1; s2 < 4; s2++)
                        {
                            if (s2 != -1)
                            {
                                cards[2].value = i + 2;
                                cards[2].suit = s2;
                                AddShunziKeyToList(cards, 3, score);
                            }
                            else
                                cards[2].value = -1;

                            for (int s3 = -1; s3 < 4; s3++)
                            {
                                if (s3 != -1)
                                {
                                    cards[3].value = i + 3;
                                    cards[3].suit = s3;
                                    AddShunziKeyToList(cards, 4, score);
                                }
                                else
                                    cards[3].value = -1;


                                for (int s4 = 0; s4 < 4; s4++)
                                {
                                    cards[4].suit = s4;
                                    if (i + 4 == 14)
                                        cards[4].value = 1;
                                    else
                                        cards[4].value = i + 4;

                                    AddShunziKeyToList(cards, 5, score);
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddShunziKeyToList(CardInfo[] cards, int count, float score)
        {
            if (cards == null)
            {
                CardKey key = new CardKey();
                tongHuaShunKeyDict[key] = new CardsTypeCombInfo(5, score);
                return;
            }

            int realCount = 0;
            CardKey cardkey = new CardKey();
            for (int i = 0; i < count; i++)
            {
                if (cards[i].value != -1)
                {
                    cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                    realCount++;
                }
            }

            bool isEqual = true;
            int prevCardSuit = -1;
            for (int i = 0; i < count; i++)
            {
                if (cards[i].value == -1)
                    continue;

                if (prevCardSuit == -1)
                    prevCardSuit = cards[i].suit;

                if (prevCardSuit != cards[i].suit)
                {
                    isEqual = false;
                    break;
                }
            }

            if (isEqual)
            {
                AddToDict(tongHuaShunKeyDict, cardkey, 5 - realCount, score);        
            }
            else
            {
                AddToDict(shunziKeyDict, cardkey, 5 - realCount, score);
            }
        }

        void CreateHuluDict()
        {
            huluKeyDict.Clear();

            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

            float score = 0;
            float a, b;

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;

                for (int s0 = 0; s0 < 4; s0++)
                {
                    cards[0].suit = s0;

                    for (int s1 = 0; s1 < 4; s1++)
                    {
                        cards[1].suit = s1;

                        for (int s2 = 0; s2 < 4; s2++)
                        {
                            cards[2].suit = s2;

                            for (int j = 1; j <= 13; j++)
                            {
                                cards[3].value = j;
                                cards[4].value = j;

                                if (i == 1) a = 14;
                                else a = i;

                                if (j == 1) b = 14;
                                else b = j;

                                score = CardsTypeEvaluation.Instance.GetHuLuBaseScore(a, b);

                                for (int m0 = 0; m0 < 4; m0++)
                                {
                                    cards[3].suit = m0;

                                    for (int m1 = 0; m1 < 4; m1++)
                                    {
                                        cards[4].suit = m1;

                                        AddHuluKeyToList(cards, score);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddHuluKeyToList(CardInfo[] cards, float score)
        {
            if (isCheckSuitCount)
            {
                int[] value = new int[14];
                List<int>[] suitCounts = new List<int>[4]
                {
                    new List<int>(value), new List<int>(value), new List<int>(value),new List<int>(value)
                };

                for (int i = 0; i < cards.Length; i++)
                {
                    suitCounts[cards[i].suit][cards[i].value]++;
                    if (suitCounts[cards[i].suit][cards[i].value] > limitSameSuitCount)
                        return;
                }
            }


            if (cards[0].value == cards[1].value &&
               cards[0].value == cards[2].value &&
               cards[0].value == cards[3].value &&
               cards[0].value == cards[4].value)
                return;

            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            AddToDict(huluKeyDict, cardkey, 0, score);
       
            //
            int tmp1;
            float score2 = score;
            float tmpScore;
            if (cards[0].value < cards[3].value)
                score2 = CardsTypeEvaluation.Instance.GetHuLuBaseScore(cards[3].value, cards[0].value);

            for (int i = 0; i < 4; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                cardkey = new CardKey();
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j].value == -1)
                        continue;

                    cardkey = AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                }

                if (i > 2) { tmpScore = score2; }
                else { tmpScore = score; }

                AddToDict(huluKeyDict, cardkey, 1, tmpScore); 
                cards[i].value = tmp1;
            }
        }

        void CreateTwoDuiDict()
        {
            twoduiKeyDict.Clear();

            CardInfo[] cards = new CardInfo[4]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
            };

            float score;
            float a, b;

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;


                for (int s0 = 0; s0 < 4; s0++)
                {
                    cards[0].suit = s0;

                    for (int s1 = 0; s1 < 4; s1++)
                    {
                        cards[1].suit = s1;

                        for (int j = 1; j <= 13; j++)
                        {
                            cards[2].value = j;
                            cards[3].value = j;

                            if (i == 1) a = 14;
                            else a = i;

                            if (j == 1) b = 14;
                            else b = j;

                            score = CardsTypeEvaluation.Instance.GetTwoDuiBaseScore(a, b);

                            for (int m0 = 0; m0 < 4; m0++)
                            {
                                cards[2].suit = m0;

                                for (int m1 = 0; m1 < 4; m1++)
                                {
                                    cards[3].suit = m1;

                                    AddTwoDuiKeyToList(cards, score);
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddTwoDuiKeyToList(CardInfo[] cards, float score)
        {
            if (isCheckSuitCount)
            {
                int[] value = new int[14];
                List<int>[] suitCounts = new List<int>[4]
                {
                    new List<int>(value), new List<int>(value), new List<int>(value),new List<int>(value)
                };

                for (int i = 0; i < cards.Length; i++)
                {
                    suitCounts[cards[i].suit][cards[i].value]++;
                    if (suitCounts[cards[i].suit][cards[i].value] > limitSameSuitCount)
                        return;
                }
            }

            if (cards[0].value == cards[1].value &&
               cards[0].value == cards[2].value &&
               cards[0].value == cards[3].value)
                return;


            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            AddToDict(twoduiKeyDict, cardkey, 0, score);

            //
            int tmp1;
            for (int i = 0; i < 4; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                cardkey = new CardKey();
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j].value == -1)
                        continue;

                    cardkey = AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                }

                AddToDict(twoduiKeyDict, cardkey, 1, score);
                cards[i].value = tmp1;
            }


            int[] m = new int[] { 0, 2, 0, 3, 1, 2, 1, 3 };

            for(int i=0; i<m.Length; i+=2)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[m[i]].value, cards[m[i]].suit);
                cardkey = AppendCardToCardKey(cardkey, cards[m[i+1]].value, cards[m[i+1]].suit);
                AddToDict(twoduiKeyDict, cardkey, 2, score);
            }
        }

        void CreateWuTongDict()
        {
            wutongKeyDict.Clear();

            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

            float score = CardsTypeEvaluation.Instance.GetWuTongBaseScore(14);
            AddWuTongKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;
                cards[3].value = i;
                cards[4].value = i;

                if (i == 1)
                    score = CardsTypeEvaluation.Instance.GetWuTongBaseScore(14);
                else
                    score = CardsTypeEvaluation.Instance.GetWuTongBaseScore(i);

                for (int s0 = 0; s0 < 4; s0++)
                {
                    cards[0].suit = s0;

                    for (int s1 = 0; s1 < 4; s1++)
                    {
                        cards[1].suit = s1;

                        for (int s2 = 0; s2 < 4; s2++)
                        {
                            cards[2].suit = s2;

                            for (int s3 = 0; s3 < 4; s3++)
                            {
                                cards[3].suit = s3;

                                for (int s4 = 0; s4 < 4; s4++)
                                {
                                    cards[4].suit = s4;

                                    AddWuTongKeyToList(cards, score);
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddWuTongKeyToList(CardInfo[] cards, float score)
        {
            if(cards == null)
            {
                CardKey key = new CardKey();
                AddToDict(wutongKeyDict, key, 5, score);
                return;
            }

            if (isCheckSuitCount)
            {
                int[] suitCount = new int[4] { 0, 0, 0, 0 };
                for (int i = 0; i < cards.Length; i++)
                {
                    suitCount[cards[i].suit]++;
                    if (suitCount[cards[i].suit] > limitSameSuitCount)
                        return;
                }
            }


            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            AddToDict(wutongKeyDict, cardkey, 0, score);
            //
            int tmp1, tmp2, tmp3;
            for (int i = 0; i < 5; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                cardkey = new CardKey();
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j].value == -1)
                        continue;

                    cardkey = AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                }

                AddToDict(wutongKeyDict, cardkey, 1, score);
                cards[i].value = tmp1;
            }

            for (int i = 0; i < 4; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                for (int j = i + 1; j < 5; j++)
                {
                    tmp2 = cards[j].value;
                    cards[j].value = -1;

                    cardkey = new CardKey();
                    for (int k = 0; k < cards.Length; k++)
                    {
                        if (cards[k].value == -1)
                            continue;

                        cardkey = AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                    }

                    AddToDict(wutongKeyDict, cardkey, 2, score);
                    cards[j].value = tmp2;
                }

                cards[i].value = tmp1;
            }

            //
            for (int i = 0; i < 3; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                for (int j = i+1; j < 4; j++)
                {
                    tmp2 = cards[j].value;
                    cards[j].value = -1;

                    for (int m = j+1; m < 5; m++)
                    {
                        tmp3 = cards[m].value;
                        cards[m].value = -1;

                        cardkey = new CardKey();
                        for (int k = 0; k < cards.Length; k++)
                        {
                            if (cards[k].value == -1)
                                continue;

                            cardkey = AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                        }

                        AddToDict(wutongKeyDict, cardkey, 3, score);
                        cards[m].value = tmp3;
                    }

                    cards[j].value = tmp2;
                }

                cards[i].value = tmp1;
            }


            //
            for(int i=0; i<5; i++)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                AddToDict(wutongKeyDict, cardkey, 4, score);
            }
        }


        void CreateTieZhiDict()
        {
            tiezhiKeyDict.Clear();

            CardInfo[] cards = new CardInfo[4]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
            };

            float score = CardsTypeEvaluation.Instance.GetTieZhiBaseScore(14);
            AddTieZhiKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;
                cards[3].value = i;

                if (i == 1)
                    score = CardsTypeEvaluation.Instance.GetTieZhiBaseScore(14);
                else
                    score = CardsTypeEvaluation.Instance.GetTieZhiBaseScore(i);

                for (int s0 = 0; s0 < 4; s0++)
                {
                    cards[0].suit = s0;

                    for (int s1 = 0; s1 < 4; s1++)
                    {
                        cards[1].suit = s1;

                        for (int s2 = 0; s2 < 4; s2++)
                        {
                            cards[2].suit = s2;

                            for (int s3 = 0; s3 < 4; s3++)
                            {
                                cards[3].suit = s3;
                                AddTieZhiKeyToList(cards, score);
                            }
                        }
                    }
                }
            }
        }

        void AddTieZhiKeyToList(CardInfo[] cards, float score)
        {
            if (cards == null)
            {
                CardKey key = new CardKey();
                AddToDict(tiezhiKeyDict, key, 4, score);
                return;
            }

            if (isCheckSuitCount)
            {
                int[] suitCount = new int[4] { 0, 0, 0, 0 };
                for (int i = 0; i < cards.Length; i++)
                {
                    suitCount[cards[i].suit]++;
                    if (suitCount[cards[i].suit] > limitSameSuitCount)
                        return;
                }
            }


            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            AddToDict(tiezhiKeyDict, cardkey, 0, score);

            //
            int tmp1, tmp2;
            for (int i = 0; i < 4; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                cardkey = new CardKey();
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j].value == -1)
                        continue;

                    cardkey = AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                }

                AddToDict(tiezhiKeyDict, cardkey, 1, score);

                cards[i].value = tmp1;
            }

            for (int i = 0; i < 3; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                for (int j = i + 1; j < 4; j++)
                {
                    tmp2 = cards[j].value;
                    cards[j].value = -1;

                    cardkey = new CardKey();
                    for (int k = 0; k < cards.Length; k++)
                    {
                        if (cards[k].value == -1)
                            continue;

                        cardkey = AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                    }

                    AddToDict(tiezhiKeyDict, cardkey, 2, score);
                    cards[j].value = tmp2;
                }

                cards[i].value = tmp1;
            }

            //
            for (int i = 0; i < 4; i++)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                AddToDict(tiezhiKeyDict, cardkey, 3, score);
            }
        }

        void CreateSanTiaoDict()
        {
            santiaoKeyDict.Clear();

            CardInfo[] cards = new CardInfo[3]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),
            };

            float score = CardsTypeEvaluation.Instance.GetSanTiaoBaseScore(14);
            AddSanTiaoKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;

                if(i == 1)
                    score = CardsTypeEvaluation.Instance.GetSanTiaoBaseScore(14);
                else
                    score = CardsTypeEvaluation.Instance.GetSanTiaoBaseScore(i);

                for (int s0 = 0; s0 < 4; s0++)
                {
                    cards[0].suit = s0;

                    for (int s1 = 0; s1 < 4; s1++)
                    {
                        cards[1].suit = s1;

                        for (int s2 = 0; s2 < 4; s2++)
                        {
                            cards[2].suit = s2;
                            AddSanTiaoKeyToList(cards, score);  
                        }
                    }
                }
            }
        }

        void AddSanTiaoKeyToList(CardInfo[] cards, float score)
        {
            if (cards == null)
            {
                CardKey key = new CardKey();
                AddToDict(santiaoKeyDict, key, 3, score);
                return;
            }

            if (isCheckSuitCount)
            {
                int[] suitCount = new int[4] { 0, 0, 0, 0 };
                for (int i = 0; i < cards.Length; i++)
                {
                    suitCount[cards[i].suit]++;
                    if (suitCount[cards[i].suit] > limitSameSuitCount)
                        return;
                }
            }


            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            AddToDict(santiaoKeyDict, cardkey, 0, score);

            //
            int tmp1;
            for (int i = 0; i < 3; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                cardkey = new CardKey();
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j].value == -1)
                        continue;

                    cardkey = AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                }

                AddToDict(santiaoKeyDict, cardkey, 1, score);
                cards[i].value = tmp1;
            }

            
            //
            for (int i = 0; i < 3; i++)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                AddToDict(santiaoKeyDict, cardkey, 2, score);
            }
        }

        void CreateDuiZiDict()
        {
            duiziKeyDict.Clear();

            CardInfo[] cards = new CardInfo[2]
            {
                new CardInfo(),new CardInfo(),
            };

            float score = CardsTypeEvaluation.Instance.GetDuiziBaseScore(14);
            AddDuiZiKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;

                if(i == 1)
                    score = CardsTypeEvaluation.Instance.GetDuiziBaseScore(14);
                else
                    score = CardsTypeEvaluation.Instance.GetDuiziBaseScore(i);

                for (int s0 = 0; s0 < 4; s0++)
                {
                    cards[0].suit = s0;

                    for (int s1 = 0; s1 < 4; s1++)
                    {
                        cards[1].suit = s1;
                        AddDuiZiKeyToList(cards, score);
                    }
                }
            }
        }

        void AddDuiZiKeyToList(CardInfo[] cards, float score)
        {
            if (cards == null)
            {
                CardKey key = new CardKey();
                AddToDict(duiziKeyDict, key, 2, score);
                return;
            }

            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            AddToDict(duiziKeyDict, cardkey, 0, score);

            //
            for (int i = 0; i < 2; i++)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                AddToDict(duiziKeyDict, cardkey, 1, score);
            }
        }

        void CreateTongHuaDict()
        {
            tonghuaKeyDict.Clear();

            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

            float score;

            for (int i = 1; i <= 9; i++)
            {
                cards[0].value = i;

                for (int j = i; j <= 10; j++)
                {
                    cards[1].value = j;
                    for (int k = j; k <= 11; k++)
                    {
                        cards[2].value = k;
                        for (int m = k; m <= 12; m++)
                        {
                            cards[3].value = m;

                            for (int n = m; n <= 13; n++)
                            {
                                if ((i == j && i == k) || (i == k && i == m ) || (i == m && i == n) ||
                                    (j == k && j == m) || (j == m && j == n) || (k == m && k == n))
                                    continue;

                                else if (i == j + 1 && j == k + 1 &&
                                    k == m + 1 && m == n + 1)
                                    continue;

                                score = CardsTypeEvaluation.Instance.GetTongHuaBaseScore(new float[] {i, j, k, m, n });

                                cards[4].value = n;

                                for (int y = 0; y < 4; y++)
                                {
                                    for (int x = 0; x < 5; x++)
                                        cards[x].suit = y;

                                    AddTongHuaKeyToList(cards, score);
                                }

                            }
                        }
                    }
                }
            }    
        }
 
        void AddTongHuaKeyToList(CardInfo[] cards, float score)
        {
            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            AddToDict(tonghuaKeyDict, cardkey, 0, score);

            //
            int tmp1, tmp2;
            for (int i = 0; i < 5; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                cardkey = new CardKey();
                for (int j = 0; j < cards.Length; j++)
                {
                    if (cards[j].value == -1)
                        continue;

                    cardkey = AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                }

                if (!tongHuaShunKeyDict.ContainsKey(cardkey))
                {
                    AddToDict(tonghuaKeyDict, cardkey, 1, score);
                }
    
                cards[i].value = tmp1;
            }


            for (int i = 0; i < 4; i++)
            {
                tmp1 = cards[i].value;
                cards[i].value = -1;

                for (int j = i + 1; j < 5; j++)
                {
                    tmp2 = cards[j].value;
                    cards[j].value = -1;

                    cardkey = new CardKey();
                    for (int k = 0; k < cards.Length; k++)
                    {
                        if (cards[k].value == -1)
                            continue;

                        cardkey = AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                    }

                    if (!tongHuaShunKeyDict.ContainsKey(cardkey))
                    {
                        AddToDict(tonghuaKeyDict, cardkey, 2, score);
                    }

                    cards[j].value = tmp2;
                }

                cards[i].value = tmp1;
            }
        }

        void CreateSingle5Dict()
        {
            single5KeyDict.Clear();

            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),new CardInfo(),new CardInfo(),new CardInfo(),
            };

            float score;

            for (int i = 1; i <= 13 - 4; i++)
            {
                cards[0].value = i;
                for (int s0 = 0; s0 < 4; s0++)
                {
                    cards[0].suit = s0;
                    for (int j = i + 1; j <= 13 - 3; j++)
                    {
                        cards[1].value = j;
                        for (int s1 = 0; s1 < 4; s1++)
                        {
                            cards[1].suit = s1;
                            for (int k = j + 1; k <= 13 - 2; k++)
                            {
                                cards[2].value = k;
                                for (int s2 = 0; s2 < 4; s2++)
                                {
                                    cards[2].suit = s2;
                                    for (int m = k + 1; m <= 13 - 1; m++)
                                    {
                                        cards[3].value = m;

                                        for (int s3 = 0; s3 < 4; s3++)
                                        {
                                            cards[3].suit = s3;
                                            for (int n = m + 1; n <= 13; n++)
                                            {
                                                cards[4].value = n;

                                                for (int s4 = 0; s4 < 4; s4++)
                                                {
                                                    cards[4].suit = s4;

                                                    if (i == 1) score = 14;
                                                    else score = n;

                                                    AddSingle5KeyToList(cards, score);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddSingle5KeyToList(CardInfo[] cards, float score)
        {
            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            if (tongHuaShunKeyDict.ContainsKey(cardkey))
                return;

            if (tonghuaKeyDict.ContainsKey(cardkey))
                return;

            if (shunziKeyDict.ContainsKey(cardkey))
                return;

        
            AddToDict(single5KeyDict, cardkey, 0, score);
        }



        uint FlagKeyBit(uint key, int n, int m)
        {
            uint bitflag = (uint)(1 << n);
            bitflag <<= m;
            uint result = key & bitflag;

            if (result == 0)
            {
                key |= bitflag;
            }
            else
            {
                bitflag <<= 1;
                key |= bitflag;
            }

            return key;
        }
  
        void CreateCardInfoToList(int key, int baseValue, List<CardInfo> cardInfoList)
        {
            int mask = 0x1;
            int flag;
            int _value, _suit;
            CardInfo cardinfo;

            for (int i = 0; i <= 31; i++)
            {
                flag = key & (mask << i);
                if (flag == 0)
                    continue;

                _value = baseValue + i / 8 + 1;
                _suit = i % 8 / 2;

                cardinfo = new CardInfo()
                {
                    value = _value,
                    suit = _suit
                };

                cardInfoList.Add(cardinfo);
            }
        }
    }

    
}