using System.Collections.Generic;

namespace CardRuleNS
{

    /// <summary>
    /// 用于把手牌数据打包成紧凑的cardkey类型
    /// </summary>
    public struct CardKey
    {
        public uint bit_207_192;
        public uint bit_191_160;
        public uint bit_159_128;
        public uint bit_127_96;
        public uint bit_95_64;
        public uint bit_63_32;
        public uint bit_31_0;
        public CardKey(
            uint bit_207_192, uint bit_191_160, uint bit_159_128,
            uint bit_127_96, uint bit_95_64, uint bit_63_32, uint bit_31_0)
        {
            this.bit_207_192 = bit_207_192;
            this.bit_191_160 = bit_191_160;
            this.bit_159_128 = bit_159_128;
            this.bit_127_96 = bit_127_96;
            this.bit_95_64 = bit_95_64;
            this.bit_63_32 = bit_63_32;
            this.bit_31_0 = bit_31_0;
        }

        public void Clear()
        {
            bit_207_192 = 0;
            bit_191_160 = 0;
            bit_159_128 = 0;
            bit_127_96 = 0;
            bit_95_64 = 0;
            bit_63_32 = 0;
            bit_31_0 = 0;
        }

        public class EqualityComparer : IEqualityComparer<CardKey>
        {
            public bool Equals(CardKey x, CardKey y)
            {
                return x.bit_31_0 == y.bit_31_0 &&
                    x.bit_63_32 == y.bit_63_32 &&
                     x.bit_95_64 == y.bit_95_64 &&
                     x.bit_127_96 == y.bit_127_96 &&
                    x.bit_159_128 == y.bit_159_128 &&
                    x.bit_191_160 == y.bit_191_160 &&
                     x.bit_207_192 == y.bit_207_192;
            }

            public int GetHashCode(CardKey obj)
            {
                int hash = (int)obj.bit_31_0;
                hash = hash * 31 + (int)obj.bit_63_32;
                hash = hash * 31 + (int)obj.bit_95_64;
                hash = hash * 31 + (int)obj.bit_127_96;
                hash = hash * 31 + (int)obj.bit_159_128;
                hash = hash * 31 + (int)obj.bit_191_160;
                hash = hash * 31 + (int)obj.bit_207_192;
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
        bool isCheckSuitCount = false;
        int limitSameSuitCount = 2;

        /// <summary>
        /// 是否生成同花字典
        /// </summary>
        bool isCreateAllTonghuaDict = false;

        /// <summary>
        /// 是否生成单张5个字典
        /// </summary>
        bool isCreateSingle5Dict = false;

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

            if(isCreateAllTonghuaDict)
                CreateTongHuaDict();

            if (isCreateSingle5Dict)
                CreateSingle5Dict();
        }

        void AddToDict(Dictionary<CardKey, CardsTypeCombInfo> dict, CardKey cardkey, int laiziCount, float score)
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
                dict[cardkey] = new CardsTypeCombInfo(laiziCount, score);
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
            int n = (cardValue - 1) * 16;
            int m = cardSuit * 4;

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
            else if (n <= 127)
            {
                n -= 96;
                cardkey.bit_127_96 = FlagKeyBit(cardkey.bit_127_96, n, m);
            }
            else if (n <= 159)
            {
                n -= 128;
                cardkey.bit_159_128 = FlagKeyBit(cardkey.bit_159_128, n, m);
            }
            else if (n <= 191)
            {
                n -= 160;
                cardkey.bit_191_160 = FlagKeyBit(cardkey.bit_191_160, n, m);
            }
            else if (n <= 207)
            {
                n -= 192;
                cardkey.bit_207_192 = FlagKeyBit(cardkey.bit_207_192, n, m);
            }

            return cardkey;
        }

        uint FlagKeyBit(uint key, int n, int m)
        {
            uint bitflag = (uint)(0xF << n);
            bitflag <<= m;
            uint result = key & bitflag;
            result = (result >> m) >> n;
            result++;

            uint zeroMask = 0xFFFFFFFF ^ bitflag;
            key &= zeroMask;
            key |= ((result << n) << m);

            return key;
        }

        public bool IsEqual(CardKey cardkeyA, CardKey cardkeyB)
        {
            if(cardkeyA.Equals(cardkeyB))
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
            CreateCardInfoToList((int)cardkey.bit_63_32, 2, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_95_64, 4, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_127_96, 6, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_159_128, 8, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_191_160, 10, cardInfoList);
            CreateCardInfoToList((int)cardkey.bit_207_192, 12, cardInfoList);
            return cardInfoList.ToArray();
        }


        void CreateCardInfoToList(int key, int baseValue, List<CardInfo> cardInfoList)
        {
            int count;
            int _value, _suit;
            CardInfo cardinfo;

            for (int i = 0; i <= 31; i += 4)
            {
                count = key & (0xF << i);
                count >>= i;

                _value = baseValue + i / 16 + 1;
                _suit = i % 16 / 4;

                for (int j = 0; j < count; j++)
                {
                    cardinfo = new CardInfo()
                    {
                        value = _value,
                        suit = _suit
                    };

                    cardInfoList.Add(cardinfo);
                }
            }
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

            float score = CardsTypeEvaluation.GetShunZiBaseScore(10);
            AddShunziKeyToList(null, 0, score);

            for (int i = 1; i <= 10; i++)
            {
                score = CardsTypeEvaluation.GetShunZiBaseScore(i);

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


        /// <summary>
        /// 生成葫芦字典
        /// </summary>
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

                                score = CardsTypeEvaluation.GetHuLuBaseScore(a, b);

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
                score2 = CardsTypeEvaluation.GetHuLuBaseScore(cards[3].value, cards[0].value);

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

                            score = CardsTypeEvaluation.GetTwoDuiBaseScore(a, b);

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

            float score = CardsTypeEvaluation.GetWuTongBaseScore(14);
            AddWuTongKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;
                cards[3].value = i;
                cards[4].value = i;

                if (i == 1)
                    score = CardsTypeEvaluation.GetWuTongBaseScore(14);
                else
                    score = CardsTypeEvaluation.GetWuTongBaseScore(i);

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

            float score = CardsTypeEvaluation.GetTieZhiBaseScore(14);
            AddTieZhiKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;
                cards[3].value = i;

                if (i == 1)
                    score = CardsTypeEvaluation.GetTieZhiBaseScore(14);
                else
                    score = CardsTypeEvaluation.GetTieZhiBaseScore(i);

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

            float score = CardsTypeEvaluation.GetSanTiaoBaseScore(14);
            AddSanTiaoKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;
                cards[2].value = i;

                if(i == 1)
                    score = CardsTypeEvaluation.GetSanTiaoBaseScore(14);
                else
                    score = CardsTypeEvaluation.GetSanTiaoBaseScore(i);

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

            float score = CardsTypeEvaluation.GetDuiziBaseScore(14);
            AddDuiZiKeyToList(null, score);

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;
                cards[1].value = i;

                if(i == 1)
                    score = CardsTypeEvaluation.GetDuiziBaseScore(14);
                else
                    score = CardsTypeEvaluation.GetDuiziBaseScore(i);

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

        /// <summary>
        /// 用于初始手牌一次生成同花字典(和具体手牌相关)
        /// </summary>
        /// <param name="handCardFaces"></param>
        /// <param name="laizi"></param>
        public void CreateTongHuaDict(CardFace[] handCardFaces, CardFace[] laizi = null)
        {
            tonghuaKeyDict.Clear();

            CardsTypeCreater creater = new CardsTypeCreater();
            creater.SetLaizi(laizi);

            int laiziCount = 0;
            CardInfo[] cards = CardsTransform.Instance.CreateFormatCards(handCardFaces, laizi, ref laiziCount);

            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>();

            List<CardInfo>[] suitCards = new List<CardInfo>[]
          {
                new List<CardInfo>(),new List<CardInfo>(),
                new List<CardInfo>(),new List<CardInfo>(),
          };

            for (int i = 0; i < cards.Length; i++)
            {
                suitCards[cards[i].suit].Add(cards[i]);
            }

            CardKey cardkey = new CardKey();
            List<CardInfo> suitcards;

            //5张同花
            for (int a = 0; a < 4; a++)
            {
                suitcards = suitCards[a];
                for (int i = 0; i < suitcards.Count - 4; i++)
                    for (int j = i + 1; j < suitcards.Count - 3; j++)
                        for (int k = j + 1; k < suitcards.Count - 2; k++)
                            for (int m = k + 1; m < suitcards.Count - 1; m++)
                                for (int n = m + 1; n < suitcards.Count; n++)
                                {
                                    cardkey.Clear();
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[i].value, suitcards[i].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[j].value, suitcards[j].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[k].value, suitcards[k].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[m].value, suitcards[m].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[n].value, suitcards[n].suit);

                                    if (suitcards[i].value == suitcards[j].value && suitcards[i].value == suitcards[k].value &&
                                        suitcards[i].value == suitcards[m].value && suitcards[i].value == suitcards[n].value)
                                        continue;

                                    if (!cardkeyHashSet.Contains(cardkey))
                                    {
                                        CardsTypeCombInfo combInfo;
                                        bool ret = tongHuaShunKeyDict.TryGetValue(cardkey, out combInfo);
                                        if (ret == true && combInfo.laiziCount <= laiziCount)
                                            continue;

                                        ret = shunziKeyDict.TryGetValue(cardkey, out combInfo);
                                        if (ret == true && combInfo.laiziCount <= laiziCount)
                                            continue;

                                        ret = huluKeyDict.TryGetValue(cardkey, out combInfo);
                                        if (ret == true && combInfo.laiziCount <= laiziCount)
                                            continue;

                                        ret = tiezhiKeyDict.TryGetValue(cardkey, out combInfo);
                                        if (ret == true && combInfo.laiziCount <= laiziCount)
                                            continue;

                                        cardkeyHashSet.Add(cardkey);

                                        CardInfo[] cardInfos = CreateCardInfos(cardkey);
                                        CardFace[] cardfaces = CardsTransform.Instance.CreateCardFaces(cardInfos);
                                        creater.CreateUsedTonghuaCmpCardsTypeArray(cardfaces);
                                        CardsTypeInfo info = creater.GetMaxScoreCardsTypeInfo();

                                        float score;
                                        switch (info.type)
                                        {
                                            case CardsType.SanTiao: score = 60 + info.score; break;
                                            case CardsType.TwoDui: score = 40 + info.score; break;
                                            case CardsType.DuiZi: score = 20 + info.score; break;
                                            default: score = info.score; break;
                                        }

                                        AddToDict(tonghuaKeyDict, cardkey, 0, score);
                                    }
                                }
            }


            //4张同花
            if (laiziCount >= 1)
            {
                CardFace[] tmpCardFaces = new CardFace[5];

                for (int a = 0; a < 4; a++)
                {
                    suitcards = suitCards[a];
                    for (int j = 0; j < suitcards.Count - 3; j++)
                        for (int k = j + 1; k < suitcards.Count - 2; k++)
                            for (int m = k + 1; m < suitcards.Count - 1; m++)
                                for (int n = m + 1; n < suitcards.Count; n++)
                                {
                                    cardkey = new CardKey();
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[j].value, suitcards[j].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[k].value, suitcards[k].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[m].value, suitcards[m].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[n].value, suitcards[n].suit);

                                    if (suitcards[j].value == suitcards[k].value &&
                                           suitcards[j].value == suitcards[m].value &&
                                           suitcards[j].value == suitcards[n].value)
                                        continue;


                                    if (!cardkeyHashSet.Contains(cardkey))
                                    {

                                        CardsTypeCombInfo combInfo;
                                        bool ret = CardsTypeDict.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out combInfo);
                                        if (ret == true && combInfo.laiziCount <= laiziCount)
                                            continue;

                                        ret = CardsTypeDict.Instance.shunziKeyDict.TryGetValue(cardkey, out combInfo);
                                        if (ret == true && combInfo.laiziCount <= laiziCount)
                                            continue;

                                        ret = CardsTypeDict.Instance.huluKeyDict.TryGetValue(cardkey, out combInfo);
                                        if (ret == true && combInfo.laiziCount <= laiziCount)
                                            continue;

                                        cardkeyHashSet.Add(cardkey);

                                        CardInfo[] cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                                        CardFace[] cardfaces = CardsTransform.Instance.CreateCardFaces(cardInfos);
                                        for (int i = 0; i < cardfaces.Length; i++)
                                            tmpCardFaces[i] = cardfaces[i];
                                        tmpCardFaces[4] = laizi[0];
                                        creater.CreateUsedTonghuaCmpCardsTypeArray(tmpCardFaces);
                                        CardsTypeInfo info = creater.GetMaxScoreCardsTypeInfo();

                                        float score;
                                        switch (info.type)
                                        {
                                            case CardsType.SanTiao: score = 60 + info.score; break;
                                            case CardsType.TwoDui: score = 40 + info.score; break;
                                            case CardsType.DuiZi: score = 20 + info.score; break;
                                            default: score = info.score; break;
                                        }

                                        AddToDict(tonghuaKeyDict, cardkey, 1, score);

                                    }
                                }
                }
            }


            //3张同花
            if (laiziCount >= 2)
            {
                CardFace[] tmpCardFaces = new CardFace[5];

                for (int a = 0; a < 4; a++)
                {
                    suitcards = suitCards[a];
                    for (int k = 0; k < suitcards.Count - 2; k++)
                        for (int m = k + 1; m < suitcards.Count - 1; m++)
                            for (int n = m + 1; n < suitcards.Count; n++)
                            {
                                cardkey = new CardKey();
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[k].value, suitcards[k].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[m].value, suitcards[m].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[n].value, suitcards[n].suit);
                                if (!cardkeyHashSet.Contains(cardkey))
                                {

                                    CardsTypeCombInfo combInfo;
                                    bool ret = CardsTypeDict.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out combInfo);
                                    if (ret == true && combInfo.laiziCount <= laiziCount)
                                        continue;

                                    ret = CardsTypeDict.Instance.shunziKeyDict.TryGetValue(cardkey, out combInfo);
                                    if (ret == true && combInfo.laiziCount <= laiziCount)
                                        continue;

                                    ret = CardsTypeDict.Instance.tiezhiKeyDict.TryGetValue(cardkey, out combInfo);
                                    if (ret == true && combInfo.laiziCount <= laiziCount)
                                        continue;

                                    ret = CardsTypeDict.Instance.huluKeyDict.TryGetValue(cardkey, out combInfo);
                                    if (ret == true && combInfo.laiziCount <= laiziCount)
                                        continue;


                                    cardkeyHashSet.Add(cardkey);

                                    CardInfo[] cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                                    CardFace[] cardfaces = CardsTransform.Instance.CreateCardFaces(cardInfos);
                                    for (int i = 0; i < cardfaces.Length; i++)
                                        tmpCardFaces[i] = cardfaces[i];
                                    tmpCardFaces[3] = laizi[0];
                                    tmpCardFaces[4] = laizi[0];
                                    creater.CreateUsedTonghuaCmpCardsTypeArray(tmpCardFaces);
                                    CardsTypeInfo info = creater.GetMaxScoreCardsTypeInfo();

                                    float score;
                                    switch (info.type)
                                    {
                                        case CardsType.SanTiao: score = 60 + info.score; break;
                                        case CardsType.TwoDui: score = 40 + info.score; break;
                                        case CardsType.DuiZi: score = 20 + info.score; break;
                                        default: score = info.score; break;
                                    }

                                    AddToDict(tonghuaKeyDict, cardkey, 2, score);
                                }
                            }
                }
            }


            //2张同花
            if (laiziCount >= 3)
            {
                CardFace[] tmpCardFaces = new CardFace[5];

                for (int a = 0; a < 4; a++)
                {
                    suitcards = suitCards[a];
                    for (int m = 0; m < suitcards.Count - 1; m++)
                        for (int n = m + 1; n < suitcards.Count; n++)
                        {
                            cardkey = new CardKey();
                            cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[m].value, suitcards[m].suit);
                            cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[n].value, suitcards[n].suit);
                            if (!cardkeyHashSet.Contains(cardkey))
                            {
                                CardsTypeCombInfo combInfo;
                                bool ret = CardsTypeDict.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out combInfo);
                                if (ret == true && combInfo.laiziCount <= laiziCount)
                                    continue;

                                ret = CardsTypeDict.Instance.shunziKeyDict.TryGetValue(cardkey, out combInfo);
                                if (ret == true && combInfo.laiziCount <= laiziCount)
                                    continue;

                                ret = CardsTypeDict.Instance.tiezhiKeyDict.TryGetValue(cardkey, out combInfo);
                                if (ret == true && combInfo.laiziCount <= laiziCount)
                                    continue;

                                ret = CardsTypeDict.Instance.huluKeyDict.TryGetValue(cardkey, out combInfo);
                                if (ret == true && combInfo.laiziCount <= laiziCount)
                                    continue;

                                cardkeyHashSet.Add(cardkey);

                                CardInfo[] cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                                CardFace[] cardfaces = CardsTransform.Instance.CreateCardFaces(cardInfos);
                                for (int i = 0; i < cardfaces.Length; i++)
                                    tmpCardFaces[i] = cardfaces[i];
                                tmpCardFaces[2] = laizi[0];
                                tmpCardFaces[3] = laizi[0];
                                tmpCardFaces[4] = laizi[0];
                                creater.CreateUsedTonghuaCmpCardsTypeArray(tmpCardFaces);
                                CardsTypeInfo info = creater.GetMaxScoreCardsTypeInfo();

                                float score;
                                switch (info.type)
                                {
                                    case CardsType.SanTiao: score = 60 + info.score; break;
                                    case CardsType.TwoDui: score = 40 + info.score; break;
                                    case CardsType.DuiZi: score = 20 + info.score; break;
                                    default: score = info.score; break;
                                }

                                AddToDict(tonghuaKeyDict, cardkey, 3, score);

                            }
                        }
                }
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

            for (int i = 1; i <= 13; i++)
            {
                cards[0].value = i;

                for (int j = 1; j <= 13; j++)
                {
                    cards[1].value = j;
                    for (int k = 1; k <= 13; k++)
                    {
                        cards[2].value = k;
                        for (int m = 1; m <= 13; m++)
                        {
                            cards[3].value = m;

                            for (int n = 1; n <= 13; n++)
                            {
                                if (i == j + 1 && j == k + 1 &&
                                   k == m + 1 && m == n + 1)
                                    continue;

                                score = CardsTypeEvaluation.GetTongHuaBaseScore(new float[] {i, j, k, m, n });

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
    } 
}