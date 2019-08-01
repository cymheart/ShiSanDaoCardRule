using System;
using System.Collections.Generic;


namespace CardRuleNS
{
    /// <summary>
    /// 牌型生成器
    /// </summary>
    public class CardsTypeCreater
    {    
        /// <summary>
        /// 五同牌型组
        /// </summary>
        public List<CardsTypeInfo> WutongList = new List<CardsTypeInfo>();

        /// <summary>
        /// 同花顺牌型组
        /// </summary>
        public List<CardsTypeInfo> TonghuashunList = new List<CardsTypeInfo>();
      
        /// <summary>
        /// 葫芦牌型组
        /// </summary>
        public List<CardsTypeInfo> HuluList = new List<CardsTypeInfo>();

        /// <summary>
        /// 铁枝牌型组
        /// </summary>
        public List<CardsTypeInfo> TiezhiList = new List<CardsTypeInfo>();

        /// <summary>
        /// 顺子牌型组
        /// </summary>
        public List<CardsTypeInfo> ShunziList = new List<CardsTypeInfo>();

        /// <summary>
        /// 两对牌型组
        /// </summary>
        public List<CardsTypeInfo> TwoduiList = new List<CardsTypeInfo>();

        /// <summary>
        /// 三条牌型组
        /// </summary>
        public List<CardsTypeInfo> SantiaoList = new List<CardsTypeInfo>();

        /// <summary>
        /// 对子牌型组
        /// </summary>
        public List<CardsTypeInfo> DuiziList = new List<CardsTypeInfo>();
       
        /// <summary>
        /// 同花牌型组
        /// </summary>
        public List<CardsTypeInfo> TonghuaList = new List<CardsTypeInfo>();

        /// <summary>
        /// 生成所有牌型组数据
        /// </summary>
        /// <param name="cardFaces">手牌数据</param>
        public void CreateAllCardsTypeArray(CardFace[] cardFaces, CardFace[] laizi = null)
        {
            Clear();

            if (laizi == null)
                laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };

            int laiziCount = 0;
            CardInfo[] cards = CardsTransform.Instance.CreateFormatCards(cardFaces, laizi, ref laiziCount);

            HashSet<CardKey> cardkeyHashSet5 = SplitCardsGroup5(cards);
            HashSet<CardKey> cardkeyHashSet4 = SplitCardsGroup4(cards);
            HashSet<CardKey> cardkeyHashSet3 = SplitCardsGroup3(cards);
            HashSet<CardKey> cardkeyHashSet2 = SplitCardsGroup2(cards);
            HashSet<CardKey> cardkeyHashSet1 = SplitCardsGroup1(cards);

            CreateCardsTypeArrayBySplitGroup(cardkeyHashSet5, laiziCount, 5);
            CreateCardsTypeArrayBySplitGroup(cardkeyHashSet4, laiziCount, 4);
            CreateCardsTypeArrayBySplitGroup(cardkeyHashSet3, laiziCount, 3);
            CreateCardsTypeArrayBySplitGroup(cardkeyHashSet2, laiziCount, 2);
            CreateCardsTypeArrayBySplitGroup(cardkeyHashSet1, laiziCount, 1);
        }


        /// <summary>
        /// 生成顺子牌型数组（包括同花顺）
        /// </summary>
        /// <param name="cardFaces"></param>
        public void CreateShunziArray(CardFace[] cardFaces, CardFace[] laizi = null)
        {
            ShunziList.Clear();

            if (laizi == null)
                laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };

            List<CardFace> newPukeFaceValueList = new List<CardFace>();
            int laiziCount = 0;
            CardInfo[] cards = CardsTransform.Instance.CreateFormatCards(cardFaces, laizi, ref laiziCount);
            CreateShunziArray(cards, laiziCount);
        }

        /// <summary>
        /// 生成顺子牌型数组（包括同花顺）
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        public void CreateShunziArray(CardInfo[] formatCards, int laiziCount)
        {
            HashSet<CardKey> cardkeyHashSet5 = SplitCardsGroup5(formatCards);
            HashSet<CardKey> cardkeyHashSet4 = SplitCardsGroup4(formatCards);
            HashSet<CardKey> cardkeyHashSet3 = SplitCardsGroup3(formatCards);
            HashSet<CardKey> cardkeyHashSet2 = SplitCardsGroup2(formatCards);
            HashSet<CardKey> cardkeyHashSet1 = SplitCardsGroup1(formatCards);

            CreateShunziArrayBySplitGroup(cardkeyHashSet5, laiziCount, 5);
            CreateShunziArrayBySplitGroup(cardkeyHashSet4, laiziCount, 4);
            CreateShunziArrayBySplitGroup(cardkeyHashSet3, laiziCount, 3);
            CreateShunziArrayBySplitGroup(cardkeyHashSet2, laiziCount, 2);
            CreateShunziArrayBySplitGroup(cardkeyHashSet1, laiziCount, 1);
        }

        void CreateShunziArrayBySplitGroup(HashSet<CardKey> cardkeyHashSet, int laiziCount, int splitGroup)
        {
            bool ret;
            int mustLaziCount;
            CardInfo[] cardInfos;
            CardsTypeInfo CardsTypeInfo;

            foreach (var cardkey in cardkeyHashSet)
            {
                ret = CardsTypeDict.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out mustLaziCount);
                if (ret == true && mustLaziCount <= laiziCount)
                {
                    cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                    CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TongHuaShun, mustLaziCount);
                    TonghuashunList.Add(CardsTypeInfo);
                }

                ret = CardsTypeDict.Instance.shunziKeyDict.TryGetValue(cardkey, out mustLaziCount);
                if (ret == true && mustLaziCount <= laiziCount)
                {
                    cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                    CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.ShunZi, mustLaziCount);
                    ShunziList.Add(CardsTypeInfo);
                }
            }
        }


        void CreateCardsTypeArrayBySplitGroup(HashSet<CardKey> cardkeyHashSet, int laiziCount, int splitGroup)
        {
            foreach (var cardkey in cardkeyHashSet)
            {
                _CreateCardsTypeArray(cardkey, laiziCount, splitGroup);
            }
        }
       
        void _CreateCardsTypeArray(CardKey cardkey, int laiziCount, int splitGroup)
        {
            bool ret;
            int mustLaziCount;
            CardInfo[] cardInfos;
            CardsTypeInfo CardsTypeInfo;

            ret = CardsTypeDict.Instance.wutongKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.WuTong, mustLaziCount);
                WutongList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TongHuaShun, mustLaziCount);
                TonghuashunList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.shunziKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.ShunZi, mustLaziCount);
                ShunziList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.huluKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.HuLu, mustLaziCount);
                HuluList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.tonghuaKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TongHua, mustLaziCount);
                TonghuaList.Add(CardsTypeInfo);
            }

            if (splitGroup == 5)
                return;

            ret = CardsTypeDict.Instance.tiezhiKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.Bomb, mustLaziCount);
                TiezhiList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.twoduiKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TwoDui, mustLaziCount);
                TwoduiList.Add(CardsTypeInfo);
            }

            if (splitGroup == 4)
                return;

            ret = CardsTypeDict.Instance.santiaoKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.SanTiao, mustLaziCount);
                SantiaoList.Add(CardsTypeInfo);
            }

            if (splitGroup == 3)
                return;

            ret = CardsTypeDict.Instance.duiziKeyDict.TryGetValue(cardkey, out mustLaziCount);
            if (ret == true && mustLaziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.DuiZi, mustLaziCount);
                DuiziList.Add(CardsTypeInfo);
            }
        }

        CardsTypeInfo CreateCardsTypeInfo(CardInfo[] cardInfos, CardsType CardsTypeType, int laiziCount)
        {
            CardsTypeInfo CardsTypeInfo = new CardsTypeInfo();
            CardsTypeInfo.laiziCount = laiziCount;
            CardsTypeInfo.CardsTypeType = CardsTypeType;
            CardsTypeInfo.cardFaceValues = new CardFace[cardInfos.Length];

            CardFace face;

            for (int i=0; i < cardInfos.Length; i++)
            {
                face = CardsTransform.Instance.GetCardFace(cardInfos[i].value, cardInfos[i].suit);
                CardsTypeInfo.cardFaceValues[i] = face;
            }

            return CardsTypeInfo;
        }


        HashSet<CardKey> SplitCardsGroup5(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());

            CardKey cardkey;

            //可能的五同
            for (int i = 0; i < cards.Length - 4; i++)
            {
                cardkey = new CardKey();
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 3].value, cards[i + 3].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 4].value, cards[i + 4].suit);

                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);
            }

  
            //可能的葫芦
            CardKey tmpCardkey;

            for (int i = 0; i < cards.Length - 2; i++)
            {
                cardkey = new CardKey();
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);

                for (int j = 0; j < cards.Length - 1; j++)
                {
                    if (j < i - 1 || j > i + 2)
                    {
                        tmpCardkey = cardkey;
                        tmpCardkey = CardsTypeDict.Instance.AppendCardToCardKey(tmpCardkey, cards[j].value, cards[j].suit);
                        tmpCardkey = CardsTypeDict.Instance.AppendCardToCardKey(tmpCardkey, cards[j + 1].value, cards[j + 1].suit);
                        if (!cardkeyHashSet.Contains(tmpCardkey))
                            cardkeyHashSet.Add(tmpCardkey);
                    }
                }
            }


            //可能的同花
            List<CardInfo>[] suitCards = new List<CardInfo>[] 
            {
                new List<CardInfo>(),new List<CardInfo>(),
                new List<CardInfo>(),new List<CardInfo>(),
            };

            for (int i = 0; i < cards.Length; i++)
            {
                suitCards[cards[i].suit].Add(cards[i]);
            }

            List<CardInfo> suitcards;
            for (int a=0; a<4; a++)
            {
                suitcards = suitCards[a];
                for (int i=0; i< suitcards.Count - 4; i++)
                    for (int j = i+1; j < suitcards.Count - 3; j++)
                        for (int k = j + 1; k < suitcards.Count - 2; k++)
                            for (int m = k+1; m < suitcards.Count - 1; m++)
                                for (int n = m+1; n < suitcards.Count; n++)
                                {
                                    cardkey = new CardKey();
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[i].value, suitcards[i].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[j].value, suitcards[j].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[k].value, suitcards[k].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[m].value, suitcards[m].suit);
                                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, suitcards[n].value, suitcards[n].suit);
                                    if (!cardkeyHashSet.Contains(cardkey))
                                        cardkeyHashSet.Add(cardkey);
                                }
            }

            //所有五张
            for (int i = 0; i < cards.Length - 4; i++)
                for (int j = i + 1; j < cards.Length - 3; j++)
                    for (int k = j + 1; k < cards.Length - 2; k++)
                        for (int m = k + 1; m < cards.Length - 1; m++)
                            for (int n = m + 1; n < cards.Length; n++)
                            {
                                cardkey = new CardKey();
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[m].value, cards[m].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[n].value, cards[n].suit);
                                if (!cardkeyHashSet.Contains(cardkey))
                                    cardkeyHashSet.Add(cardkey);
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
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 2].value, cards[i + 2].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 3].value, cards[i + 3].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);
            }



            //可能的两对
            CardKey tmpCardkey;

            for (int i = 0; i < cards.Length - 1; i++)
            {
                cardkey = new CardKey();
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i + 1].value, cards[i + 1].suit);

                for (int j = 0; j < cards.Length - 1; j++)
                {
                    if (j < i - 1 || j > i + 1)
                    {
                        tmpCardkey = cardkey;
                        tmpCardkey = CardsTypeDict.Instance.AppendCardToCardKey(tmpCardkey, cards[j].value, cards[j].suit);
                        tmpCardkey = CardsTypeDict.Instance.AppendCardToCardKey(tmpCardkey, cards[j + 1].value, cards[j + 1].suit);
                        if (!cardkeyHashSet.Contains(tmpCardkey))
                            cardkeyHashSet.Add(tmpCardkey);
                    }
                }
            }


            //可能的同花
            List<CardInfo>[] suitCards = new List<CardInfo>[]
            {
                new List<CardInfo>(),new List<CardInfo>(),
                new List<CardInfo>(),new List<CardInfo>(),
            };

            for (int i = 0; i < cards.Length; i++)
            {
                suitCards[cards[i].suit].Add(cards[i]);
            }

            List<CardInfo> suitcards;
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
                                    if (!cardkeyHashSet.Contains(cardkey))
                                        cardkeyHashSet.Add(cardkey);
                                }
            }

            //所有四张
            for (int j = 0; j < cards.Length - 3; j++)
                for (int k = j + 1; k < cards.Length - 2; k++)
                    for (int m = k + 1; m < cards.Length - 1; m++)
                        for (int n = m + 1; n < cards.Length; n++)
                        {
                            cardkey = new CardKey();
                            cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                            cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                            cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[m].value, cards[m].suit);
                            cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[n].value, cards[n].suit);
                            if (!cardkeyHashSet.Contains(cardkey))
                                cardkeyHashSet.Add(cardkey);
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
                        cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                        cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                        cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
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
                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                    cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                    if (!cardkeyHashSet.Contains(cardkey))
                        cardkeyHashSet.Add(cardkey);
                }
            }

            return cardkeyHashSet;
        }


        HashSet<CardKey> SplitCardsGroup1(CardInfo[] cards)
        {
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>(new CardKey.EqualityComparer());
            CardKey cardkey = new CardKey();
            cardkeyHashSet.Add(cardkey);

            for (int i = 0; i < cards.Length; i++)
            {
                cardkey = new CardKey();
                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                if (!cardkeyHashSet.Contains(cardkey))
                    cardkeyHashSet.Add(cardkey);
            }

            return cardkeyHashSet;
        }

        void Clear()
        {
            WutongList.Clear();
            TonghuashunList.Clear();
            ShunziList.Clear();
            HuluList.Clear();
            TiezhiList.Clear();
            TwoduiList.Clear();
            SantiaoList.Clear();
            DuiziList.Clear();
            TonghuaList.Clear();
        }
    }
}
