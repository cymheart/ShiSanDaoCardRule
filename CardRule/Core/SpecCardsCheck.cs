﻿using System.Collections.Generic;

namespace CardRuleNS
{
    /// <summary>
    /// 特殊牌型检查(带赖子检查算法)
    /// </summary>
    public class SpecCardsCheck
    {
        HashSet<SpecCardsType> checkSpeckCardSet = new HashSet<SpecCardsType>();
        CardFace[] laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };

        struct SpecCardsInfo
        {
            public SpecCardsType specCardsType;
            public CardFace[] faceValues;
        }

        private class SpecCardsComparer : IComparer<SpecCardsInfo>
        {
            public int Compare(SpecCardsInfo info1, SpecCardsInfo info2)
            {
                if (info1.specCardsType > info2.specCardsType)
                    return -1;
                else if (info1.specCardsType < info2.specCardsType)
                    return 1;

                return 0;
            }
        }


        public SpecCardsCheck()
        {
            ReSetCheckSpecCardTypesDefault();
        }

        /// <summary>
        /// 设置赖子
        /// </summary>
        /// <param name="_laizi"></param>
        public void SetLaizi(CardFace[] _laizi = null)
        {
            if (_laizi == null)
            {
                laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };
                return;
            }

            laizi = _laizi;
        }

        public void SetCheckSpecCardTypes(SpecCardsType[] specCardsTypes)
        {
            checkSpeckCardSet.Clear();
            for (int i = 0; i < specCardsTypes.Length; i++)
                checkSpeckCardSet.Add(specCardsTypes[i]);
        }

        /// <summary>
        /// 设置只检查这些特殊牌型: 6炸、7炸、8炸、至尊雷、一条龙、至尊一条龙
        /// </summary>
        public void PreSetCheckSpecCardTypes1()
        {
            SpecCardsType[] specCardsTypes = new SpecCardsType[]
            {
                 SpecCardsType.SixBomb, SpecCardsType.SevenBomb, SpecCardsType.EightBomb,
                SpecCardsType.ZhiZunLei76, SpecCardsType.ZhiZunLei66, SpecCardsType.YiTiaoLong, SpecCardsType.ZhiZunQinLong
            };

            SetCheckSpecCardTypes(specCardsTypes);
        }

        public void ReSetCheckSpecCardTypesDefault()
        {
            checkSpeckCardSet.Clear();
            for (SpecCardsType i = SpecCardsType.Normal + 1; i < SpecCardsType.Count; i++)
            {
                checkSpeckCardSet.Add(i);
            }
        }

        /// <summary>
        /// 检查是否为特殊牌型
        /// </summary>
        /// <param name="cardFaces">手牌数据</param>
        public SpecCardsType Check(CardFace[] cardFaces, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            if (cardFaces.Length == 13)
                return Check13Count(cardFaces, outFaceValues, outComputedFaceValues);

            if (cardFaces.Length != 16)
                return SpecCardsType.Normal;


            List<CardFace[]> removeCardFacesList = CreateRemoveOut13CountCards(cardFaces);
            SpecCardsType _specCardsType;
            List<SpecCardsInfo> specCardsInfoList = new List<SpecCardsInfo>();

            for (int i = 0; i < removeCardFacesList.Count; i++)
            {
                CardFace[] newCardFaces = CardsTransform.Instance.CreateDelFaceValues(cardFaces, removeCardFacesList[i]);
                int laiziCount = 0;
                CardInfo[] cards = CardsTransform.Instance.CreateFormatCards(newCardFaces, laizi, ref laiziCount);

                CardFace[] newOutFaceValues = new CardFace[13];
                _specCardsType = IsSpecCards(cards, laiziCount, newOutFaceValues, outComputedFaceValues);
                if (_specCardsType == SpecCardsType.Normal)
                    continue;

                SpecCardsInfo info = new SpecCardsInfo
                {
                    specCardsType = _specCardsType,
                    faceValues = newOutFaceValues
                };

                specCardsInfoList.Add(info);
            }

            if (specCardsInfoList.Count > 0)
            {
                specCardsInfoList.Sort(new SpecCardsComparer());

                for (int i = 0; i < outFaceValues.Length; i++)
                    outFaceValues[i] = specCardsInfoList[0].faceValues[i];

                return specCardsInfoList[0].specCardsType;
            }


            return SpecCardsType.Normal;

        }

        SpecCardsType Check13Count(CardFace[] cardFaces, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            int laiziCount = 0;
            CardInfo[] cards = CardsTransform.Instance.CreateFormatCards(cardFaces, laizi, ref laiziCount);
            return IsSpecCards(cards, laiziCount, outFaceValues, outComputedFaceValues);
        }

        List<CardFace[]> CreateRemoveOut13CountCards(CardFace[] cardFaces)
        {
            if (cardFaces.Length != 16)
                return null;

            List<CardFace[]> cardFacesList = new List<CardFace[]>();
            CardKey cardkey = new CardKey();
            HashSet<CardKey> cardkeySet = new HashSet<CardKey>();

            for (int i = 0; i < cardFaces.Length - 2; i++)
            {
                for (int j = i + 1; j < cardFaces.Length - 1; j++)
                {
                    for (int k = j + 1; k < cardFaces.Length; k++)
                    {
                        CardFace[] tmpCardFaces = new CardFace[3]
                        {
                            cardFaces[i], cardFaces[j], cardFaces[k]
                        };

                        for (int m = 0; m < tmpCardFaces.Length; m++)
                        {
                            CardInfo card = CardsTransform.Instance.CreateCardInfo(tmpCardFaces[m]);
                            cardkey = new CardKey();
                            cardkey = CardsTypeDict.Instance.AppendCardToCardKey(cardkey, card.value, card.suit);
                        }

                        if (!cardkeySet.Contains(cardkey))
                        {
                            cardFacesList.Add(tmpCardFaces);
                            cardkeySet.Add(cardkey);
                        }
                    }
                }
            }

            return cardFacesList;

        }


        SpecCardsType IsSpecCards(CardInfo[] cards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            SpecCardsType type = SpecCardsType.Normal;
      
            if (checkSpeckCardSet.Contains(SpecCardsType.ZhiZunQinLong) &&
                IsZhiZunQinLong(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.ZhiZunQinLong;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.YiTiaoLong) &&
                IsYiTiaoLong(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.YiTiaoLong;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.ZhiZunLei76) &&
                IsZhiZunLei(cards, laiziCount, outFaceValues, outComputedFaceValues) == 1)
            {
                type = SpecCardsType.ZhiZunLei76;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.ZhiZunLei66) &&
                IsZhiZunLei(cards, laiziCount, outFaceValues, outComputedFaceValues) == 2)
            {
                type = SpecCardsType.ZhiZunLei66;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.EightBomb) &&
                IsEightBomb(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.EightBomb;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.SevenBomb) &&
                IsSevenBomb(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.SevenBomb;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.SixBomb) &&
                IsSixBomb(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.SixBomb;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.SanShunZi) &&
                IsSanShunZi(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.SanShunZi;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.SanTongHua) &&
                IsSanTongHua(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.SanTongHua;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.SiTaoSan) &&
                IsSiTaoSan(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.SiTaoSan;
            }
            else if (checkSpeckCardSet.Contains(SpecCardsType.LiuDuiBan) &&
                IsLiuDuiBan(cards, laiziCount, outFaceValues, outComputedFaceValues))
            {
                type = SpecCardsType.LiuDuiBan;
            }

            return type;
        }


        /// <summary>
        /// 判断是否为至尊青龙
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsZhiZunQinLong(CardInfo[] formatCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;

            for (int i = 1; i < cards.Length; i++)
            {
                if (cards[i].suit != cards[0].suit)
                    return false;
            }

            bool ret = IsYiTiaoLong(cards, laiziCount, outFaceValues, outComputedFaceValues);
            return ret;
        }

        /// <summary>
        /// 判断是否为一条龙
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsYiTiaoLong(CardInfo[] formatCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;
            int n = 0;
            int m = 0;
            int idx;
            int computedSuit = 0;

            for (int i = 2; i <= 14; i++)
            {
                if (i == 14)
                    i = 1;

                idx = CardsTransform.Instance.FindCard(cards, i);

                if (idx == -1)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(i, 0);
                }
                else
                {
                    computedSuit = cards[idx].suit;
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cards[idx].value, cards[idx].suit);
                }

                if (computedSuit != 0)
                {
                    int value;
                    for (int j = 0; j < outComputedFaceValues.Length; j++)
                    {
                        value = CardsTransform.Instance.GetValue(outComputedFaceValues[j]);
                        outComputedFaceValues[j] = CardsTransform.Instance.GetCardFace(value, computedSuit);
                    }
                }

                if (i == 1)
                    break;
            }

            return true;
        }

        /// <summary>
        /// 判断是否为三同花
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsSanTongHua(CardInfo[] formatCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;

            List<CardInfo>[] suitCards = new List<CardInfo>[]
            {
                new List<CardInfo>(),new List<CardInfo>(),
                new List<CardInfo>(),new List<CardInfo>(),
            };

            for (int i = 0; i < cards.Length; i++)
            {
                suitCards[cards[i].suit].Add(cards[i]);
            }

            int[] idx = new int[4];
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                if (suitCards[i].Count > 0)
                    idx[count++] = i;
            }

            //有4中花色
            if (count == 4)
                return false;

            //只有3种花色
            if (count == 3)
            {
                int min = 4; //最小数量花色牌的数量
                int minIdx = 0;  //最小数量花色的idx
                for (int i = 0; i < 3; i++)
                {
                    //其中任何一个花色牌的数量大于5个
                    if (suitCards[idx[i]].Count > 5)
                        return false;

                    if (suitCards[idx[i]].Count < min)
                    {
                        min = suitCards[idx[i]].Count;
                        minIdx = idx[i];
                    }
                }

                //最少数量的花色的个数大于3个
                if (min > 3)
                    return false;

                int n = 0;
                int m = 0;

                for (int i = 0; i < suitCards[minIdx].Count; i++)
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(suitCards[minIdx][i].value, suitCards[minIdx][i].suit);

                for (int i = suitCards[minIdx].Count; i < 3; i++)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, suitCards[minIdx][0].suit);
                }

                //对其他两种花色处理
                for (int i = 0; i < 3; i++)
                {
                    if (idx[i] == minIdx)  //上面已处理的最小数量的花色，不作处理
                        continue;

                    for (int j = 0; j < suitCards[idx[i]].Count; j++)
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(suitCards[idx[i]][j].value, suitCards[idx[i]][j].suit);

                    for (int j = suitCards[idx[i]].Count; j < 5; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = CardFace.Laizi;
                        outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, suitCards[idx[i]][0].suit);
                    }
                }
            }
            else if (count == 2)
            {
                if (suitCards[idx[0]].Count > 5 &&
                    suitCards[idx[1]].Count > 5)
                    return false;

                int min = 5;
                int minIdx = 0;
                int otherIdx = 0;

                for (int i = 0; i < 2; i++)
                {
                    if (suitCards[idx[i]].Count <= min)
                    {
                        min = suitCards[idx[i]].Count;
                        minIdx = idx[i];
                        otherIdx = idx[1 - i];
                    }
                }

                int n = 0;
                int m = 0;

                if (min <= 3)
                {
                    if (suitCards[otherIdx].Count > 10)
                        return false;

                    for (int i = 0; i < suitCards[minIdx].Count; i++)
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(suitCards[minIdx][i].value, suitCards[minIdx][i].suit);

                    for (int i = suitCards[minIdx].Count; i < 3; i++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = CardFace.Laizi;
                        outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, suitCards[idx[minIdx]][0].suit);
                    }

                    for (int j = 0; j < suitCards[otherIdx].Count; j++)
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(suitCards[otherIdx][j].value, suitCards[otherIdx][j].suit);

                    for (int j = suitCards[otherIdx].Count; j < 10; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = CardFace.Laizi;
                        outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, suitCards[otherIdx][0].suit);
                    }
                }
                else
                {
                    if (suitCards[otherIdx].Count > 8)
                        return false;

                    for (int j = 0; j < suitCards[otherIdx].Count; j++)
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(suitCards[otherIdx][j].value, suitCards[otherIdx][j].suit);

                    for (int j = suitCards[otherIdx].Count; j < 8; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = CardFace.Laizi;
                        outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, suitCards[otherIdx][0].suit);
                    }

                    for (int i = 0; i < suitCards[minIdx].Count; i++)
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(suitCards[minIdx][i].value, suitCards[minIdx][i].suit);

                    for (int i = suitCards[minIdx].Count; i < 5; i++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = CardFace.Laizi;
                        outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, suitCards[minIdx][0].suit);
                    }
                }
            }
            else
            {
                int n = 0;
                int m = 0;

                for (int j = 0; j < suitCards[idx[0]].Count; j++)
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(suitCards[idx[0]][j].value, suitCards[idx[0]][j].suit);

                for (int j = suitCards[idx[0]].Count; j < 13; j++)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, suitCards[idx[0]][0].suit);
                }

            }

            return true;
        }


        /// <summary>
        /// 判断是否为四套三
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsSiTaoSan(CardInfo[] formatCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;

            List<CardInfo> cardList = new List<CardInfo>();
            cardList.AddRange(cards);

            int n = 0;
            int m = 0;
            bool isFind;
            CardInfo tmpCardInfo;

            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count - 2; i++)
                    for (int j = i + 1; j < cardList.Count - 1; j++)
                        for (int k = j + 1; k < cardList.Count; k++)
                        {
                            if (cardList[i].value == cardList[j].value &&
                                cardList[i].value == cardList[k].value)
                            {
                                outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[i].value, cardList[i].suit);
                                outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[j].value, cardList[j].suit);
                                outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[k].value, cardList[k].suit);
                                cardList.RemoveRange(i, 3);

                                isFind = true;
                                k = cardList.Count;
                                j = cardList.Count;
                                i = cardList.Count;

                                if (n == 12)
                                    isFind = false;
                            }
                        }

                if (isFind == false)
                    break;
            }

            if (n == 12)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
                    return true;
                }
                return false;
            }


            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count - 1; i++)
                    for (int j = i + 1; j < cardList.Count; j++)
                    {
                        if (cardList[i].value == cardList[j].value)
                        {
                            outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[i].value, cardList[i].suit);
                            outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[j].value, cardList[j].suit);
                            tmpCardInfo = cardList[i];
                            cardList.RemoveRange(i, 2);

                            laiziCount--;
                            if (laiziCount < 0)
                                return false;

                            outFaceValues[n++] = CardFace.Laizi;
                            outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(tmpCardInfo);

                            isFind = true;
                            j = cardList.Count;
                            i = cardList.Count;

                            if (n == 12)
                                isFind = false;
                        }
                    }

                if (isFind == false)
                    break;
            }

            if (n == 12)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
                    return true;
                }
                return false;
            }


            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count; i++)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[i].value, cardList[i].suit);
                    tmpCardInfo = cardList[i];
                    cardList.RemoveAt(i);

                    for (int j = 0; j < 2; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = CardFace.Laizi;
                        outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(tmpCardInfo);
                    }

                    isFind = true;
                    i = cardList.Count;
                    if (n == 12)
                        isFind = false;
                }

                if (isFind == false)
                    break;
            }

            if (n == 12)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
                    return true;
                }
                return false;
            }

            for (int j = n; j < 13; j++)
            {
                laiziCount--;
                if (laiziCount < 0)
                    return false;

                outFaceValues[n++] = CardFace.Laizi;
                outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
            }

            return true;
        }


        /// <summary>
        /// 判断是否为六对半
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsLiuDuiBan(CardInfo[] formatCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;

            List<CardInfo> cardList = new List<CardInfo>();
            cardList.AddRange(cards);

            int n = 0;
            int m = 0;
            bool isFind;
            CardInfo tmpCardInfo;

            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count - 1; i++)
                    for (int j = i + 1; j < cardList.Count; j++)
                    {
                        if (cardList[i].value == cardList[j].value &&
                            cardList[i].value == cardList[j].value)
                        {
                            outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[i].value, cardList[i].suit);
                            outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[j].value, cardList[j].suit);
                            cardList.RemoveRange(i, 2);

                            isFind = true;
                            j = cardList.Count;
                            i = cardList.Count;

                            if (n == 12)
                                isFind = false;
                        }
                    }


                if (isFind == false)
                    break;
            }

            if (n == 12)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
                    return true;
                }
                return false;
            }

            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count; i++)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[i].value, cardList[i].suit);
                    tmpCardInfo = cardList[i];
                    cardList.RemoveAt(i);

                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(tmpCardInfo);

                    isFind = true;
                    i = cardList.Count;
                    if (n == 12)
                        isFind = false;
                }

                if (isFind == false)
                    break;
            }

            if (n == 12)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
                    return true;
                }
                return false;
            }

            for (int j = n; j < 13; j++)
            {
                laiziCount--;
                if (laiziCount < 0)
                    return false;

                outFaceValues[n++] = CardFace.Laizi;
                outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
            }

            return true;
        }

        /// <summary>
        /// 判断是否为三顺子
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsSanShunZi(CardInfo[] formatCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;

            bool ret = false;
            int n = 0;
            List<CardsTypeInfo> shunziList = new List<CardsTypeInfo>();
            List<CardsTypeInfo> shunziList2 = new List<CardsTypeInfo>();

            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateShunziArray(cards, laiziCount);
            shunziList.Clear();
            shunziList.AddRange(creater.ShunziList);
            shunziList.AddRange(creater.TonghuashunList);

            for (int i = 0; i < shunziList.Count; i++)
            {
                CardInfo[] cards2 = CardsTransform.Instance.CreateRemoveFaceValues(cards, shunziList[i].cardFaceValues);
                int laiziCount2 = laiziCount - shunziList[i].laiziCount;
                if (laiziCount2 < 0)
                    continue;

                CardsTypeCreater creater2 = new CardsTypeCreater();
                creater2.CreateShunziArray(cards2, laiziCount2);
                shunziList2.Clear();
                shunziList2.AddRange(creater2.ShunziList);
                shunziList2.AddRange(creater2.TonghuashunList);

                for (int j = 0; j < shunziList2.Count; j++)
                {
                    CardInfo[] cards3 = CardsTransform.Instance.CreateRemoveFaceValues(cards2, shunziList2[j].cardFaceValues);
                    int laiziCount3 = laiziCount2 - shunziList2[j].laiziCount;
                    if (laiziCount3 < 0)
                        continue;

                    if (cards3.Length == 3 &&
                        cards3[0].value + 1 == cards3[1].value &&
                        cards3[1].value + 1 == cards3[2].value)
                    {
                        for (int m = 0; m < 3; m++)
                            outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cards3[m].value, cards3[m].suit);
                    }
                    else if (cards3.Length == 2 && laiziCount3 >= 1 &&
                        (cards3[0].value + 1 == cards3[1].value || cards3[0].value + 2 == cards3[1].value))
                    {
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cards3[0].value, cards3[0].suit);
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cards3[1].value, cards3[1].suit);
                        outFaceValues[n++] = CardFace.RedJoker;
                    }
                    else if (cards3.Length == 1 && laiziCount3 >= 2)
                    {
                        outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cards3[0].value, cards3[0].suit);
                        outFaceValues[n++] = CardFace.RedJoker;
                        outFaceValues[n++] = CardFace.RedJoker;
                    }
                    else if (cards3.Length == 0 && laiziCount3 >= 3)
                    {
                        outFaceValues[n++] = CardFace.RedJoker;
                        outFaceValues[n++] = CardFace.RedJoker;
                        outFaceValues[n++] = CardFace.RedJoker;
                    }
                    else
                    {
                        continue;
                    }

                    for (int m = 0; m < shunziList[i].cardFaceValues.Length; m++)
                        outFaceValues[n++] = shunziList[i].cardFaceValues[m];
                    for (int m = shunziList[i].cardFaceValues.Length; m < 5; m++)
                        outFaceValues[n++] = CardFace.RedJoker;

                    for (int m = 0; m < shunziList2[j].cardFaceValues.Length; m++)
                        outFaceValues[n++] = shunziList2[j].cardFaceValues[m];
                    for (int m = shunziList2[j].cardFaceValues.Length; m < 5; m++)
                        outFaceValues[n++] = CardFace.RedJoker;

                    ret = true;
                    break;
                }

                if (ret == true)
                    break;
            }

            if (ret == false)
                return false;

            //
            CardsTypeMatch match = new CardsTypeMatch();
            match.SetLaizi(laizi);

            List<CardFace> cardFaceList = new List<CardFace>();

            for (int i = 0; i <= 2; i++)
                cardFaceList.Add(outFaceValues[i]);
            MatchCardFacesInfo matchInfo1 = match.ComputeMatchCardFacesInfo(cardFaceList.ToArray(), CardsType.ShunZi);

            //
            cardFaceList.Clear();
            for (int i = 3; i <= 7; i++)
                cardFaceList.Add(outFaceValues[i]);
            MatchCardFacesInfo matchInfo2 = match.ComputeMatchCardFacesInfo(cardFaceList.ToArray(), CardsType.ShunZi);

            //
            cardFaceList.Clear();
            for (int i = 8; i <= 12; i++)
                cardFaceList.Add(outFaceValues[i]);
            MatchCardFacesInfo matchInfo3 = match.ComputeMatchCardFacesInfo(cardFaceList.ToArray(), CardsType.ShunZi);

            int k = 0;
            n = 0;

            for (int i = 0; i < matchInfo1.laiziCardFaces.Length; i++)
            {
                if (matchInfo1.laiziCardFaces[i] == CardFace.RedJoker)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[k++] = matchInfo1.computedCardFaces[i];
                }
                else
                    outFaceValues[n++] = matchInfo1.laiziCardFaces[i];
            }

            for (int i = 0; i < matchInfo2.laiziCardFaces.Length; i++)
            {
                if (matchInfo2.laiziCardFaces[i] == CardFace.RedJoker)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[k++] = matchInfo2.computedCardFaces[i];
                }
                else
                    outFaceValues[n++] = matchInfo2.laiziCardFaces[i];
            }

            for (int i = 0; i < matchInfo3.laiziCardFaces.Length; i++)
            {
                if (matchInfo3.laiziCardFaces[i] == CardFace.RedJoker)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[k++] = matchInfo3.computedCardFaces[i];
                }
                else
                    outFaceValues[n++] = matchInfo3.laiziCardFaces[i];
            }

            return true;
        }

        /// <summary>
        /// 判断是否为至尊雷
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public int IsZhiZunLei(CardInfo[] formatCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;

            List<CardInfo>[] cardsList = new List<CardInfo>[]
            {
                 new List<CardInfo>(), new List<CardInfo>()
            };

            int[] cardCount = new int[14];
            for (int i = 0; i < cards.Length; i++)
                cardCount[cards[i].value]++;

            for (int j = 0; j < 2; j++)
            {
                int max = cardCount[0];
                int maxIdx = 0;
                for (int i = 0; i < cardCount.Length; i++)
                {
                    if (cardCount[i] > max)
                    {
                        max = cardCount[i];
                        maxIdx = i;
                    }
                }

                if (max != 0)
                {
                    int s = CardsTransform.Instance.FindCard(cards, maxIdx);
                    for (int i = s; i < s + max; i++)
                        cardsList[j].Add(cards[i]);
                }

                cardCount[maxIdx] = 0;
            }

            if (cardsList[0].Count > 7 || cardsList[1].Count > 6)
                return 0;

            if (cardsList[0].Count == 7)
            {
                int mustLaiziCount = 13 - (cardsList[0].Count + cardsList[1].Count);
                if (laiziCount < mustLaiziCount)
                    return 0;

                int n = 0;
                int m = 0;
                for (int i = 0; i < cardsList[0].Count; i++)
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardsList[0][i].value, cardsList[0][i].suit);
                for (int i = cardsList[0].Count; i < 7; i++)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(cardsList[0][0].value, cardsList[0][0].suit);
                }

                for (int i = 0; i < cardsList[1].Count; i++)
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardsList[1][i].value, cardsList[1][i].suit);
                for (int i = cardsList[1].Count; i < 6; i++)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(cardsList[1][0].value, cardsList[1][0].suit);
                }
                return 1;  //7+6
            }
            else
            {
                int mustLaiziCount = 12 - (cardsList[0].Count + cardsList[1].Count);
                if (laiziCount < mustLaiziCount)
                    return 0;

                int n = 0;
                int m = 0;
                for (int i = 0; i < cardsList[0].Count; i++)
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardsList[0][i].value, cardsList[0][i].suit);
                for (int i = cardsList[0].Count; i < 6; i++)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(cardsList[0][0].value, cardsList[0][0].suit);
                }

                for (int i = 0; i < cardsList[1].Count; i++)
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardsList[1][i].value, cardsList[1][i].suit);
                for (int i = cardsList[1].Count; i < 6; i++)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(cardsList[1][0].value, cardsList[1][0].suit);
                }

                int max = cardCount[0];
                int maxIdx = 0;
                for (int i = 0; i < cardCount.Length; i++)
                {
                    if (cardCount[i] > max)
                    {
                        max = cardCount[i];
                        maxIdx = i;
                    }
                }

                if (max != 0)
                {
                    int s = CardsTransform.Instance.FindCard(cards, maxIdx);
                    outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cards[s].value, cards[s].suit);
                }
                else
                {
                    if (laiziCount - mustLaiziCount >= 1)
                    {
                        outFaceValues[n++] = CardFace.Laizi;
                        outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
                    }
                    else
                        return 0;
                }

                return 2; //6+6
            }
        }

        /// <summary>
        /// 判断是否为八炸
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsEightBomb(CardInfo[] foramtCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            return IsNBomb(foramtCards, laiziCount, 8, outFaceValues, outComputedFaceValues);
        }

        /// <summary>
        /// 判断是否为七炸
        /// </summary>
        ///<param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsSevenBomb(CardInfo[] foramtCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            return IsNBomb(foramtCards, laiziCount, 7, outFaceValues, outComputedFaceValues);
        }

        /// <summary>
        /// 判断是否为六炸
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        public bool IsSixBomb(CardInfo[] foramtCards, int laiziCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            return IsNBomb(foramtCards, laiziCount, 6, outFaceValues, outComputedFaceValues);
        }

        /// <summary>
        /// 判断是否为N炸
        /// </summary>
        /// <param name="formatCards">CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);</param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>  
        public bool IsNBomb(CardInfo[] formatCards, int laiziCount, int bombCount, CardFace[] outFaceValues, CardFace[] outComputedFaceValues)
        {
            CardInfo[] cards = formatCards;

            List<CardInfo> cardsList = new List<CardInfo>();

            int[] cardCount = new int[15];
            for (int i = 0; i < cards.Length; i++)
            {
                if(cards[i].value == 1)
                    cardCount[14]++;
                else
                    cardCount[cards[i].value]++;
            }


            int max = 0;
            int maxIdx = 0;
            int mustLaiziCount;
            for (int i = cardCount.Length - 1; i > 1; i--)
            {
                mustLaiziCount = bombCount - cardCount[i];
                if (laiziCount < mustLaiziCount)
                    continue;

                max = cardCount[i];

                if (i == 14)
                    maxIdx = 1;
                else
                    maxIdx = i;

                break;
            }

            if (max != 0)
            {
                int s = CardsTransform.Instance.FindCard(cards, maxIdx);
                for (int i = s; i < s + max; i++)
                    cardsList.Add(cards[i]);
            }
            else
            {
                return false;
            }

            mustLaiziCount = bombCount - cardsList.Count;
            int n = 0;
            int m = 0;
            for (int i = 0; i < cardsList.Count; i++)
                outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cardsList[i].value, cardsList[i].suit);


            for (int i = cardsList.Count; i < bombCount; i++)
            {
                outFaceValues[n++] = CardFace.Laizi;
                outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(cardsList[0].value, cardsList[0].suit);
            }

            CardInfo[] cards2 = CardsTransform.Instance.CreateRemoveCardInfos(cards, cardsList.ToArray());
            for (int i = 0; i < cards2.Length; i++)
                outFaceValues[n++] = CardsTransform.Instance.GetCardFace(cards2[i].value, cards2[i].suit);

            if (13 - n <= laiziCount - mustLaiziCount)
            {
                for (int i = n; i < 13; i++)
                {
                    outFaceValues[n++] = CardFace.Laizi;
                    outComputedFaceValues[m++] = CardsTransform.Instance.GetCardFace(1, 0);
                }
            }
            else
            {
                return false;
            }

            return true;
        }

    }
}