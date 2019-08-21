﻿using System;
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
        /// 5张单牌组
        /// </summary>
        public List<CardsTypeInfo> Single5List = new List<CardsTypeInfo>();

        /// <summary>
        /// 3张单牌组
        /// </summary>
        public List<CardsTypeInfo> Single3List = new List<CardsTypeInfo>();

        int laiziCount;
        CardInfo[] cards;

        /// <summary>
        /// 生成所有牌型组数据
        /// </summary>
        /// <param name="cardFaces">手牌数据</param>
        public void CreateAllCardsTypeArray(CardFace[] cardFaces, CardFace[] laizi = null)
        {
            Clear();

            if (laizi == null)
                laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };

            cards = CardsTransform.Instance.CreateFormatCards(cardFaces, laizi, ref laiziCount);

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

            //
            CreateSingle5Dict();
            //CreateSingle3Dict();

            //
            SortDictCardsTypeInfo();
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

            //
            SortDictCardsTypeInfo();
        }

        /// <summary>
        /// 获取所有能生成的牌型信息
        /// </summary>
        /// <returns></returns>
        public CardsTypeInfo[] GetAllCardsTypeInfo(bool isExcludeSingle5 = true)
        {
            List<CardsTypeInfo> infoList = new List<CardsTypeInfo>();

            infoList.AddRange(WutongList);
            infoList.AddRange(TonghuashunList);
            infoList.AddRange(TiezhiList);
            infoList.AddRange(HuluList);
            infoList.AddRange(TonghuaList);
            infoList.AddRange(ShunziList);
            infoList.AddRange(SantiaoList);
            infoList.AddRange(TwoduiList);
            infoList.AddRange(DuiziList);

            if(!isExcludeSingle5)
                infoList.AddRange(Single5List);

            return infoList.ToArray();
        }

    
        /// <summary>
        /// 存在非单张的牌型组合
        /// </summary>
        /// <returns></returns>
        public bool IsExistNotSingleCardsType()
        {
            if (WutongList.Count > 0 || TonghuashunList.Count > 0 ||
                TiezhiList.Count > 0 || HuluList.Count > 0 ||
                TonghuaList.Count > 0 || ShunziList.Count > 0 ||
                SantiaoList.Count > 0 || TwoduiList.Count > 0 ||
                DuiziList.Count > 0)
                return true;

            return false;
        }

        public CardInfo[] GetFormatCards()
        {
            return cards;
        }
  
        public CardsTypeInfo GetMaxScoreCardsTypeInfo()
        {
            if(WutongList.Count > 0)
            {
                return WutongList[WutongList.Count - 1];
            }
            else if(TonghuashunList.Count > 0)
            {
                return TonghuashunList[TonghuashunList.Count - 1];
            }
            else if (TiezhiList.Count > 0)
            {
                return TiezhiList[TiezhiList.Count - 1];
            }
            else if (HuluList.Count > 0)
            {
                return HuluList[HuluList.Count - 1];
            }
            else if (TonghuaList.Count > 0)
            {
                return TonghuaList[TonghuaList.Count - 1];
            }
            else if (ShunziList.Count > 0)
            {
                return ShunziList[ShunziList.Count - 1];
            }
            else if (SantiaoList.Count > 0)
            {
                return SantiaoList[SantiaoList.Count - 1];
            }
            else if (TwoduiList.Count > 0)
            {
                return TwoduiList[TwoduiList.Count - 1];
            }
            else if (DuiziList.Count > 0)
            {
                return DuiziList[DuiziList.Count - 1];
            }

            CardsTypeInfo info;
            if (laiziCount > 0)
            {
                info = new CardsTypeInfo();
                info.type = CardsType.Single;
                info.cardFaceValues = new CardFace[] { CardFace.Club_A };
                return info;
            }
            else if (cards.Length > 0)
            {
                info = new CardsTypeInfo();
                info.type = CardsType.Single;
                if (cards[0].value == 1)
                    info.cardFaceValues = new CardFace[] { CardFace.Club_A };
                else
                {
                    CardFace face = CardsTransform.Instance.GetCardFace(cards[cards.Length - 1].value, cards[cards.Length - 1].suit);
                    info.cardFaceValues = new CardFace[] { face };
                }
                return info;
            }

            info = new CardsTypeInfo();
            info.type = CardsType.None;
            return info;
        }


        void SortDictCardsTypeInfo()
        {
            CardsTypeEvaluation.Instance.SortCardsTypes(WutongList);
            CardsTypeEvaluation.Instance.SortCardsTypes(TonghuashunList);
            CardsTypeEvaluation.Instance.SortCardsTypes(TiezhiList);
            CardsTypeEvaluation.Instance.SortCardsTypes(HuluList);
            CardsTypeEvaluation.Instance.SortCardsTypes(TonghuaList);
            CardsTypeEvaluation.Instance.SortCardsTypes(ShunziList);
            CardsTypeEvaluation.Instance.SortCardsTypes(SantiaoList);
            CardsTypeEvaluation.Instance.SortCardsTypes(TwoduiList);
            CardsTypeEvaluation.Instance.SortCardsTypes(DuiziList);
            CardsTypeEvaluation.Instance.SortCardsTypes(Single5List);
        }

        void CreateShunziArrayBySplitGroup(HashSet<CardKey> cardkeyHashSet, int laiziCount, int splitGroup)
        {
            bool ret;
            CardsTypeCombInfo combInfo;
            CardInfo[] cardInfos;
            CardsTypeInfo CardsTypeInfo;

            foreach (var cardkey in cardkeyHashSet)
            {
                ret = CardsTypeDict.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out combInfo);
                if (ret == true && combInfo.laiziCount <= laiziCount)
                {
                    cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                    CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TongHuaShun, combInfo.laiziCount, combInfo.baseScore);
                    TonghuashunList.Add(CardsTypeInfo);
                }

                ret = CardsTypeDict.Instance.shunziKeyDict.TryGetValue(cardkey, out combInfo);
                if (ret == true && combInfo.laiziCount <= laiziCount)
                {
                    cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                    CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.ShunZi, combInfo.laiziCount, combInfo.baseScore);
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
            CardsTypeCombInfo combInfo;
            CardInfo[] cardInfos;
            CardsTypeInfo CardsTypeInfo;

            ret = CardsTypeDict.Instance.wutongKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.WuTong, combInfo.laiziCount, combInfo.baseScore);
                WutongList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.tongHuaShunKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TongHuaShun, combInfo.laiziCount, combInfo.baseScore);
                TonghuashunList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.shunziKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.ShunZi, combInfo.laiziCount, combInfo.baseScore);
                ShunziList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.huluKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.HuLu, combInfo.laiziCount, combInfo.baseScore);
                HuluList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.tonghuaKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TongHua, combInfo.laiziCount, combInfo.baseScore);
                TonghuaList.Add(CardsTypeInfo);
            }

            if (splitGroup == 5)
                return;

            ret = CardsTypeDict.Instance.tiezhiKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.Bomb, combInfo.laiziCount, combInfo.baseScore);
                TiezhiList.Add(CardsTypeInfo);
            }

            ret = CardsTypeDict.Instance.twoduiKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.TwoDui, combInfo.laiziCount, combInfo.baseScore);
                TwoduiList.Add(CardsTypeInfo);
            }

            if (splitGroup == 4)
                return;

            ret = CardsTypeDict.Instance.santiaoKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.SanTiao, combInfo.laiziCount, combInfo.baseScore);
                SantiaoList.Add(CardsTypeInfo);
            }

            if (splitGroup == 3)
                return;

            ret = CardsTypeDict.Instance.duiziKeyDict.TryGetValue(cardkey, out combInfo);
            if (ret == true && combInfo.laiziCount <= laiziCount)
            {
                cardInfos = CardsTypeDict.Instance.CreateCardInfos(cardkey);
                CardsTypeInfo = CreateCardsTypeInfo(cardInfos, CardsType.DuiZi, combInfo.laiziCount, combInfo.baseScore);
                DuiziList.Add(CardsTypeInfo);
            }
        }

        CardsTypeInfo CreateCardsTypeInfo(CardInfo[] cardInfos, CardsType CardsTypeType, int laiziCount, float score)
        {
            CardsTypeInfo cardsTypeInfo = new CardsTypeInfo();
            cardsTypeInfo.laiziCount = laiziCount;
            cardsTypeInfo.score = score;
            cardsTypeInfo.type = CardsTypeType;
            cardsTypeInfo.cardFaceValues = new CardFace[cardInfos.Length];

            CardFace face;

            for (int i=0; i < cardInfos.Length; i++)
            {
                face = CardsTransform.Instance.GetCardFace(cardInfos[i].value, cardInfos[i].suit);
                cardsTypeInfo.cardFaceValues[i] = face;
            }

            return cardsTypeInfo;
        }

        void CreateSingle5Dict()
        {
            float _score;
            CardKey cardkey;
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>();
            CardFace[] cardfaces = CardsTransform.Instance.CreateCardFaces(cards);

            for (int i = 0; i < cards.Length - 4; i++)
            {
                for (int j = i + 1; j < cards.Length - 3; j++)
                {
                    if (cards[j].value == cards[i].value)
                        continue;

                    for (int k = j + 1; k < cards.Length - 2; k++)
                    {
                        if (cards[k].value == cards[j].value)
                            continue;

                        for (int m = k + 1; m < cards.Length - 1; m++)
                        {
                            if (cards[m].value == cards[k].value)
                                continue;

                            for (int n = m + 1; n < cards.Length; n++)
                            {
                                if (cards[n].value == cards[m].value)
                                    continue;

                                cardkey = new CardKey();
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[m].value, cards[m].suit);
                                cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[n].value, cards[n].suit);

                                if (CardsTypeDict.Instance.tongHuaShunKeyDict.ContainsKey(cardkey))
                                    continue;

                                if (CardsTypeDict.Instance.tonghuaKeyDict.ContainsKey(cardkey))
                                    continue;

                                if (CardsTypeDict.Instance.shunziKeyDict.ContainsKey(cardkey))
                                    continue;

                                if (cardkeyHashSet.Contains(cardkey))
                                    continue;

                                if (cards[i].value == 1) _score = 14;
                                else _score = cards[n].value;

                                CardsTypeInfo info = new CardsTypeInfo()
                                {
                                    type = CardsType.Single,
                                    cardFaceValues = new CardFace[] { cardfaces[i], cardfaces[j], cardfaces[k], cardfaces[m], cardfaces[n] },
                                    laiziCount = 0,
                                    score = _score
                                };

                                cardkeyHashSet.Add(cardkey);
                                Single5List.Add(info);
                            }
                        }
                    }
                }
            }
        }


        void CreateSingle3Dict()
        {
            float _score;
            CardKey cardkey;
            HashSet<CardKey> cardkeyHashSet = new HashSet<CardKey>();
            CardFace[] cardfaces = CardsTransform.Instance.CreateCardFaces(cards);

            for (int i = 0; i < cards.Length - 2; i++)
            {
                for (int j = i + 1; j < cards.Length - 1; j++)
                {
                    if (cards[j].value == cards[i].value)
                        continue;

                    for (int k = j + 1; k < cards.Length; k++)
                    {
                        if (cards[k].value == cards[j].value)
                            continue;

                        cardkey = new CardKey();
                        cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[i].value, cards[i].suit);
                        cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[j].value, cards[j].suit);
                        cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, cards[k].value, cards[k].suit);

                        if (CardsTypeDict.Instance.santiaoKeyDict.ContainsKey(cardkey))
                            continue;

                        if (cardkeyHashSet.Contains(cardkey))
                            continue;

                        if (cards[i].value == 1) _score = 14;
                        else _score = cards[k].value;

                        CardsTypeInfo info = new CardsTypeInfo()
                        {
                            type = CardsType.Single,
                            cardFaceValues = new CardFace[] { cardfaces[i], cardfaces[j], cardfaces[k]},
                            laiziCount = 0,
                            score = _score
                        };

                        cardkeyHashSet.Add(cardkey);
                        Single3List.Add(info);

                    }
                }
            }



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
            Single5List.Clear();
            Single3List.Clear();
        }
    }
}