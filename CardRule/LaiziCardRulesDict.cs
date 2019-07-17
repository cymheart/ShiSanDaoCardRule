using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRuleNS
{
    public struct CardInfo
    {
        public int value;
        public int suit;
    }

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

    /// <summary>
    /// 十三道赖子牌型字典
    /// </summary>
    public class LaiziCardRulesDict
    {
        private static LaiziCardRulesDict instance = null;
        public static LaiziCardRulesDict Instance
        {
            get
            {
                if (instance == null)
                    instance = new LaiziCardRulesDict();
                return instance;
            }
        }

        public Dictionary<CardKey, int> shunziKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> tongHuaShunKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> huluKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> twoduiKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> wutongKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> tiezhiKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> santiaoKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> duiziKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());
        public Dictionary<CardKey, int> tonghuaKeyDict = new Dictionary<CardKey, int>(new CardKey.EqualityComparer());

        /// <summary>
        /// 生成牌型查询字典
        /// </summary>
        public void CreatePaiXingDict()
        {
            CreateShunziDict();
            CreateHuluDict();
            CreateTwoDuiDict();
            CreateWuTongDict();
            CreateTieZhiDict();
            CreateSanTiaoDict();
            CreateDuiZiDict();
            CreateTongHuaDict();
        }

        void CreateShunziDict()
        {
            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

            for (int i = 1; i <= 10; i++)
            {
                for (int s0 = -1; s0 < 4; s0++)
                {
                    if (s0 != -1)
                    {
                        cards[0].value = i;
                        cards[0].suit = s0;
                        AddShunziKeyToList(cards, 1);
                    }
                    else
                        cards[0].value = -1;

                    for (int s1 = -1; s1 < 4; s1++)
                    {
                        if (s1 != -1)
                        {
                            cards[1].value = i + 1;
                            cards[1].suit = s1;
                            AddShunziKeyToList(cards, 2);
                        }
                        else
                            cards[1].value = -1;

                        for (int s2 = -1; s2 < 4; s2++)
                        {
                            if (s2 != -1)
                            {
                                cards[2].value = i + 2;
                                cards[2].suit = s2;
                                AddShunziKeyToList(cards, 3);
                            }
                            else
                                cards[2].value = -1;

                            for (int s3 = -1; s3 < 4; s3++)
                            {
                                if (s3 != -1)
                                {
                                    cards[3].value = i + 3;
                                    cards[3].suit = s3;
                                    AddShunziKeyToList(cards, 4);
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

                                    AddShunziKeyToList(cards, 5);
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddShunziKeyToList(CardInfo[] cards, int count)
        {
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

            if (realCount == 1)
                return;


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
                tongHuaShunKeyDict[cardkey] = 5 - realCount;
            else
            {
                if (realCount <= 2)
                    return;

                shunziKeyDict[cardkey] = 5 - realCount;
            }
        }

        void CreateHuluDict()
        {
            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

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

                                for (int m0 = 0; m0 < 4; m0++)
                                {
                                    cards[3].suit = m0;

                                    for (int m1 = 0; m1 < 4; m1++)
                                    {
                                        cards[4].suit = m1;

                                        AddHuluKeyToList(cards);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddHuluKeyToList(CardInfo[] cards)
        {
            int[] suitCount = new int[4] { 0, 0, 0, 0 };
            for (int i = 0; i < cards.Length; i++)
            {
                suitCount[cards[i].suit]++;
                if (suitCount[cards[i].suit] > 2)
                    return;
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

            huluKeyDict[cardkey] = 0;

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

                huluKeyDict[cardkey] = 1;
                cards[i].value = tmp1;
            }
        }

        void CreateTwoDuiDict()
        {
            CardInfo[] cards = new CardInfo[4]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
            };

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

                        for (int j = 0; j <= 13; j++)
                        {
                            cards[2].value = j;
                            cards[3].value = j;

                            for (int m0 = 0; m0 < 4; m0++)
                            {
                                cards[2].suit = m0;

                                for (int m1 = 0; m1 < 4; m1++)
                                {
                                    cards[3].suit = m1;

                                    AddTwoDuiKeyToList(cards);
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddTwoDuiKeyToList(CardInfo[] cards)
        {
            int[] suitCount = new int[4] { 0, 0, 0, 0 };
            for (int i = 0; i < cards.Length; i++)
            {
                suitCount[cards[i].suit]++;
                if (suitCount[cards[i].suit] > 2)
                    return;
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

            twoduiKeyDict[cardkey] = 0;

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

                twoduiKeyDict[cardkey] = 1;
                cards[i].value = tmp1;
            }


            int[] m = new int[] { 0, 2, 0, 3, 1, 2, 1, 3 };

            for(int i=0; i<m.Length; i+=2)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[m[i]].value, cards[m[i]].suit);
                cardkey = AppendCardToCardKey(cardkey, cards[m[i+1]].value, cards[m[i+1]].suit);
                twoduiKeyDict[cardkey] = 2;
            }
        }

        void CreateWuTongDict()
        {
            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;
                cards[3].value = i;
                cards[4].value = i;

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

                                    AddWuTongKeyToList(cards);
                                }
                            }
                        }
                    }
                }
            }
        }

        void AddWuTongKeyToList(CardInfo[] cards)
        {
            int[] suitCount = new int[4] { 0, 0, 0, 0 };
            for (int i = 0; i < cards.Length; i++)
            {
                suitCount[cards[i].suit]++;
                if (suitCount[cards[i].suit] > 2)
                    return;
            }


            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            wutongKeyDict[cardkey] = 0;

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

                wutongKeyDict[cardkey] = 1;
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

                    wutongKeyDict[cardkey] = 2;
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

                        wutongKeyDict[cardkey] = 3;
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
                wutongKeyDict[cardkey] = 4;
            }
        }


        void CreateTieZhiDict()
        {
            CardInfo[] cards = new CardInfo[4]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
            };

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;
                cards[3].value = i;
     
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
                                AddTieZhiKeyToList(cards);
                            }
                        }
                    }
                }
            }
        }

        void AddTieZhiKeyToList(CardInfo[] cards)
        {
            int[] suitCount = new int[4] { 0, 0, 0, 0 };
            for (int i = 0; i < cards.Length; i++)
            {
                suitCount[cards[i].suit]++;
                if (suitCount[cards[i].suit] > 2)
                    return;
            }


            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            tiezhiKeyDict[cardkey] = 0;

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

                tiezhiKeyDict[cardkey] = 1;
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

                    tiezhiKeyDict[cardkey] = 2;
                    cards[j].value = tmp2;
                }

                cards[i].value = tmp1;
            }

            //
            for (int i = 0; i < 4; i++)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                tiezhiKeyDict[cardkey] = 3;
            }
        }

        void CreateSanTiaoDict()
        {
            CardInfo[] cards = new CardInfo[3]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),
            };

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
                            AddSanTiaoKeyToList(cards);  
                        }
                    }
                }
            }
        }

        void AddSanTiaoKeyToList(CardInfo[] cards)
        {
            int[] suitCount = new int[4] { 0, 0, 0, 0 };
            for (int i = 0; i < cards.Length; i++)
            {
                suitCount[cards[i].suit]++;
                if (suitCount[cards[i].suit] > 2)
                    return;
            }


            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            santiaoKeyDict[cardkey] = 0;

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

                santiaoKeyDict[cardkey] = 1;
                cards[i].value = tmp1;
            }

            
            //
            for (int i = 0; i < 3; i++)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                santiaoKeyDict[cardkey] = 2;
            }
        }

        void CreateDuiZiDict()
        {
            CardInfo[] cards = new CardInfo[2]
            {
                new CardInfo(),new CardInfo(),
            };

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
                        AddDuiZiKeyToList(cards);
                    }
                }
            }
        }

        void AddDuiZiKeyToList(CardInfo[] cards)
        {
            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            duiziKeyDict[cardkey] = 0;

            //
            for (int i = 0; i < 2; i++)
            {
                cardkey = new CardKey();
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                duiziKeyDict[cardkey] = 1;
            }
        }


        void CreateTongHuaDict()
        {
            CardInfo[] cards = new CardInfo[5]
            {
                new CardInfo(),new CardInfo(),
                new CardInfo(),new CardInfo(),
                new CardInfo()
            };

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

                                cards[4].value = n;

                                for (int y = 0; y < 4; y++)
                                {
                                    for (int x = 0; x < 5; x++)
                                        cards[x].suit = y;

                                    AddTongHuaKeyToList(cards);
                                }

                            }
                        }
                    }
                }
            }    
        }

    
        void AddTongHuaKeyToList(CardInfo[] cards)
        {
            CardKey cardkey = new CardKey();
            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
            }

            tonghuaKeyDict[cardkey] = 0;

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
                    tonghuaKeyDict[cardkey] = 1;
    
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
                        tonghuaKeyDict[cardkey] = 2;

                    cards[j].value = tmp2;
                }

                cards[i].value = tmp1;
            }
        }

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

        public bool IsContains(CardKey cardkey, CardKey checkCardKey)
        {
            uint a = cardkey.bit_31_0 & checkCardKey.bit_31_0;
            uint b = cardkey.bit_63_32 & checkCardKey.bit_63_32;
            uint c = cardkey.bit_95_64 & checkCardKey.bit_95_64;
            uint d = cardkey.bit_103_96 & checkCardKey.bit_103_96;

            if(a == checkCardKey.bit_31_0 && 
                b == checkCardKey.bit_63_32 &&
                c == checkCardKey.bit_95_64 &&
                d == checkCardKey.bit_103_96)
            {
                return true;
            }

            return false;
        }

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


        public CardInfo[] CreateCardInfos(CardKey cardkey)
        {
            List<CardInfo> cardInfoList = new List<CardInfo>();
            CreateCardInfoToList((int)cardkey.bit_31_0, 0, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_63_32, 4, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_95_64, 8, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_103_96, 12, cardInfoList);
            return cardInfoList.ToArray();
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