﻿

using System.Collections.Generic;

namespace CardRuleNS
{
    /// <summary>
    /// 特殊牌型检查
    /// </summary>
    public class SpecCardsCheck
    {
        /// <summary>
        /// 生成牌型组数据
        /// </summary>
        /// <param name="pukeFaceValues">手牌数据</param>
        public void Check(RulePukeFaceValue[] pukeFaceValues)
        {

        }


        /// <summary>
        /// 判断是否为至尊青龙
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        bool IsZhiZunQinLong(CardInfo[] cards, int laiziCount, RulePukeFaceValue[] outFaceValues)
        {
            for(int i= 1; i < cards.Length; i++)
            {
                if (cards[i].suit != cards[0].suit)
                    return false;
            }

            bool ret = IsYiTiaoLong(cards, laiziCount, outFaceValues);
            return ret; 
        }

        /// <summary>
        /// 判断是否为一条龙
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        bool IsYiTiaoLong(CardInfo[] cards, int laiziCount, RulePukeFaceValue[] outFaceValues)
        {
            int idx;
            for (int i = 2; i < 14; i++)
            {
                if (i == 14)
                    i = 1;

                idx = CardsTransform.Instance.FindCard(cards, i);
                if (idx == -1)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[i] = RulePukeFaceValue.Laizi;
                }
                else
                {
                    outFaceValues[i] = CardsTransform.Instance.GetRulePukeFaceValue(cards[idx].value, cards[idx].suit);
                }
            }

            return true;
        }

        /// <summary>
        /// 判断是否为三同花
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        bool IsSanTongHua(CardInfo[] cards, int laiziCount, RulePukeFaceValue[] outFaceValues)
        {
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


            if (count == 4)
                return false;

            if(count == 3)
            {
                int min = 4;
                int minIdx = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (suitCards[idx[i]].Count > 5)
                        return false;

                    if (suitCards[idx[i]].Count < min)
                    {
                        min = suitCards[idx[i]].Count;
                        minIdx = idx[i];
                    }
                }

                if (min > 3)
                    return false;

                int n = 0;

                for(int i=0; i < suitCards[minIdx].Count; i++)
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(suitCards[minIdx][i].value, suitCards[minIdx][i].suit);

                for (int i = suitCards[minIdx].Count; i < 3; i++)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
                }

                for(int i=0; i<3; i++)
                {
                    if (idx[i] == minIdx)
                        continue;

                    for (int j = 0; j < suitCards[idx[i]].Count; j++)
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(suitCards[idx[i]][j].value, suitCards[idx[i]][j].suit);

                    for (int j = suitCards[idx[i]].Count; j < 5; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }
                }
            }
            else if(count == 2)
            {
                if (suitCards[idx[0]].Count > 5 &&
                    suitCards[idx[1]].Count > 5)
                    return false;

                int min = 4;
                int minIdx = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (suitCards[idx[i]].Count <= min)
                    {
                        min = suitCards[idx[i]].Count;
                        minIdx = idx[i];
                    }
                }

                int n = 0;
                int otherIdx = 1 - minIdx;

                if (min <= 3)
                {
                    if (suitCards[otherIdx].Count > 10)
                        return false;

                    for (int i = 0; i < suitCards[minIdx].Count; i++)
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(suitCards[minIdx][i].value, suitCards[minIdx][i].suit);

                    for (int i = suitCards[minIdx].Count; i < 3; i++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }

                    for (int j = 0; j < suitCards[otherIdx].Count; j++)
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(suitCards[otherIdx][j].value, suitCards[otherIdx][j].suit);

                    for (int j = suitCards[otherIdx].Count; j < 10; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }
                }
                else
                {
                    if (suitCards[otherIdx].Count > 8)
                        return false;

                    for (int j = 0; j < suitCards[otherIdx].Count; j++)
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(suitCards[otherIdx][j].value, suitCards[otherIdx][j].suit);

                    for (int j = suitCards[otherIdx].Count; j < 8; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }

                    for (int i = 0; i < suitCards[minIdx].Count; i++)
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(suitCards[minIdx][i].value, suitCards[minIdx][i].suit);

                    for (int i = suitCards[minIdx].Count; i < 5; i++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }
                }
            }
            else
            {
                int n = 0;

                for (int j = 0; j < suitCards[idx[0]].Count; j++)
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(suitCards[idx[0]][j].value, suitCards[idx[0]][j].suit);

                for (int j = suitCards[idx[0]].Count; j < 13; j++)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
                }

            }

            return true;
        }


        /// <summary>
        /// 判断是否为四套三
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        bool IsSiTaoSan(CardInfo[] cards, int laiziCount, RulePukeFaceValue[] outFaceValues)
        {
            List<CardInfo> cardList = new List<CardInfo>();
            cardList.AddRange(cards);

            int n = 0;
            bool isFind;

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
                                outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[i].value, cardList[i].suit);
                                outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[j].value, cardList[j].suit);
                                outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[k].value, cardList[k].suit);
                                cardList.RemoveRange(i, 3);

                                isFind = true;
                                k = cardList.Count;
                                j = cardList.Count;
                                i = cardList.Count;

                                if (n == 11)
                                    isFind = false;
                            }
                        }

                if (isFind == false)
                    break;
            }

            if (n == 11)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
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
                            outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[i].value, cardList[i].suit);
                            outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[j].value, cardList[j].suit);
                            cardList.RemoveRange(i, 2);

                            laiziCount--;
                            if (laiziCount < 0)
                                return false;

                            outFaceValues[n++] = RulePukeFaceValue.Laizi;

                            isFind = true;
                            j = cardList.Count;
                            i = cardList.Count;

                            if (n == 11)
                                isFind = false;
                        }
                    }

                if (isFind == false)
                    break;
            }

            if (n == 11)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    return true;
                }
                return false;
            }


            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count; i++)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[i].value, cardList[i].suit);
                    cardList.RemoveAt(i);

                    for (int j = 0; j < 2; j++)
                    {
                        laiziCount--;
                        if (laiziCount < 0)
                            return false;

                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }

                    isFind = true;
                    i = cardList.Count;
                    if (n == 11)
                        isFind = false;
                }

                if (isFind == false)
                    break;
            }

            if (n == 11)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    return true;
                }
                return false;
            }

            for (int j = n; j < 13; j++)
            {
                laiziCount--;
                if (laiziCount < 0)
                    return false;

                outFaceValues[n++] = RulePukeFaceValue.Laizi;
            }

            return true;
        }


        /// <summary>
        /// 判断是否为六对半
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        bool IsLiuDuiBan(CardInfo[] cards, int laiziCount, RulePukeFaceValue[] outFaceValues)
        {
            List<CardInfo> cardList = new List<CardInfo>();
            cardList.AddRange(cards);

            int n = 0;
            bool isFind;

            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count - 1; i++)
                    for (int j = i + 1; j < cardList.Count; j++)
                    {
                        if (cardList[i].value == cardList[j].value &&
                            cardList[i].value == cardList[j].value)
                        {
                            outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[i].value, cardList[i].suit);
                            outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[j].value, cardList[j].suit);
                            cardList.RemoveRange(i, 2);

                            isFind = true;
                            j = cardList.Count;
                            i = cardList.Count;

                            if (n == 11)
                                isFind = false;
                        }
                    }


                if (isFind == false)
                    break;
            }

            if (n == 11)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    return true;
                }
                return false;
            }

            while (true)
            {
                isFind = false;

                for (int i = 0; i < cardList.Count; i++)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[i].value, cardList[i].suit);
                    cardList.RemoveAt(i);

                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = RulePukeFaceValue.Laizi;

                    isFind = true;
                    i = cardList.Count;
                    if (n == 11)
                        isFind = false;
                }

                if (isFind == false)
                    break;
            }

            if (n == 11)
            {
                if (cardList.Count > 0)
                {
                    outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cardList[0].value, cardList[0].suit);
                    return true;
                }
                else if (laiziCount > 0)
                {
                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    return true;
                }
                return false;
            }

            for (int j = n; j < 13; j++)
            {
                laiziCount--;
                if (laiziCount < 0)
                    return false;

                outFaceValues[n++] = RulePukeFaceValue.Laizi;
            }

            return true;
        }

        /// <summary>
        /// 判断是否为三顺子
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="laiziCount"></param>
        /// <param name="outFaceValues"></param>
        /// <returns></returns>
        bool IsSanShunZi(CardInfo[] cards, int laiziCount, RulePukeFaceValue[] outFaceValues)
        {
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
                    CardInfo[] cards3 = CardsTransform.Instance.CreateRemoveFaceValues(cards2, shunziList2[i].cardFaceValues);
                    int laiziCount3 = laiziCount2 - shunziList2[i].laiziCount;
                    if (laiziCount3 < 0)
                        continue;

                    if(cards3.Length == 3 &&
                        cards3[0].value + 1 == cards3[1].value &&
                        cards3[1].value + 1 == cards3[2].value)
                    {
                        for(int m=0; m<3; m++)
                            outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cards3[m].value, cards3[m].suit);
                    }
                    else if(cards3.Length == 2 && laiziCount3 >= 1 && 
                        (cards3[0].value + 1 == cards3[1].value || cards3[0].value + 2 == cards3[1].value))
                    {
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cards3[0].value, cards3[0].suit);
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cards3[1].value, cards3[1].suit);
                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }
                    else if(cards3.Length == 1 && laiziCount3 >= 2)
                    {
                        outFaceValues[n++] = CardsTransform.Instance.GetRulePukeFaceValue(cards3[0].value, cards3[0].suit);
                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }
                    else if(cards3.Length == 0 && laiziCount3 >= 3)
                    {
                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                    }
                    else
                    {
                        return false;
                    }

                    for(int m = 0; m < shunziList[i].cardFaceValues.Length; m++)
                        outFaceValues[n++] = shunziList[i].cardFaceValues[m];
                    for (int m = shunziList[i].cardFaceValues.Length; m < 5; m++)
                        outFaceValues[n++] =  RulePukeFaceValue.Laizi;

                    for (int m = 0; m < shunziList2[i].cardFaceValues.Length; m++)
                        outFaceValues[n++] = shunziList2[i].cardFaceValues[m];
                    for (int m = shunziList2[i].cardFaceValues.Length; m < 5; m++)
                        outFaceValues[n++] = RulePukeFaceValue.Laizi;
                }
            }

            return true;
        }
    }
}