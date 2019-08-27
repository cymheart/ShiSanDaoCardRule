using System;
using System.Collections.Generic;

namespace CardRuleNS
{

    public class SlotCardsEvalInfo
    {
        public List<CardFace>[] slotCardFaceList = new List<CardFace>[3]
        {
            new List<CardFace>(),new List<CardFace>(),new List<CardFace>()
        };

        public CardsType[] slotCardsType = new CardsType[3];

        public float[] slotScore = new float[3];
        public float totalScore;

        public float[] slotShuiScore = new float[3];
        public float totalShuiScore;

        public float scoreAndShuiEval;
        public float variance;

        /// <summary>
        /// 综合估值
        /// </summary>
        public float compEval;
    }

    /// <summary>
    /// 手牌估值
    /// </summary>
    public class CardsTypeEvaluation
    {
        /// <summary>
        /// 分值水数权重，1:偏向牌型分值加权， 0：偏向水数加权
        /// </summary>
        float scoreAndShuiWeight = 0.9f;

        /// <summary>
        /// 每道牌型偏离方差倍率的上限
        /// </summary>
        float varianceLimit = 2f;

        /// <summary>
        /// 方差在InOutCubic曲线上的取值范围
        /// 例:0~0.3
        /// </summary>
        float varianceCubicRange = 0.3f;

        /// <summary>
        /// 最大分数
        /// </summary>
        float maxScore = 2750f;

        /// <summary>
        /// 最大水数
        /// </summary>
        float maxShui = 33f;

        CardFace[] newCardFaces = new CardFace[13];

        CardFace[] laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };

        Comparer comparer;
        CardEvalComparer cardEvalComparer = new CardEvalComparer();
        CardsTypeCreater optimalCardCreater = new CardsTypeCreater();
        CardsTypeCreater nextSlotCreater = new CardsTypeCreater();
       

        int optimalSlotCardsEvalInfoCount = 50;

        private struct EvalFuncParamDatas
        {
            public CardFace[] cardFaces;
            public CardsTypeInfo? curtSlotCardTypeInfo;
            public List<SlotCardsEvalInfo> slotCardsEvalGroup;
            public List<CardFace>[] evalDatas;
            public CardsTypeInfo?[] cardsTypeInfos;
            public int slotDepth;
        }

        private class Comparer : IComparer<CardsTypeInfo>
        {
            CardsTypeEvaluation cardsTypeEval;
            public Comparer(CardsTypeEvaluation cardsTypeEval)
            {
                this.cardsTypeEval = cardsTypeEval;
            }

            public int Compare(CardsTypeInfo c1, CardsTypeInfo c2)
            {
                return cardsTypeEval.Cmp(c1, null, c2, null);
            }
        }

        private class CardEvalComparer : IComparer<SlotCardsEvalInfo>
        {
            public int Compare(SlotCardsEvalInfo info1, SlotCardsEvalInfo info2)
            {
                if (info1.compEval > info2.compEval)
                    return -1;
                else if (info1.compEval < info2.compEval)
                    return 1;

                return 0;
            }
        }

        public CardsTypeEvaluation()
        {
            comparer = new Comparer(this);
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

            optimalCardCreater.SetLaizi(laizi);
            nextSlotCreater.SetLaizi(laizi);
        }


        /// <summary>
        /// 设置获取最优牌型解的数量
        /// </summary>
        /// <param name="count"></param>
        public void SetOptimalSlotCardsEvalInfoCount(int count)
        {
            optimalSlotCardsEvalInfoCount = count;
        }


        /// <summary>
        /// 设置分值水数权重
        /// 1:偏向牌型分值加权， 0：偏向水数加权
        /// 默认0.9
        /// </summary>
        /// <param name="weight"></param>
        public void SetScoreAndShuiWeight(float weight = 0.9f)
        {
            scoreAndShuiWeight = weight;
        }

        /// <summary>
        /// 设置每道牌型偏离方差倍率的上限
        /// 默认2倍偏离
        /// </summary>
        /// <param name="mulLimit"></param>
        public void SetVarianceMulLimit(float mulLimit = 2)
        {
            varianceLimit = mulLimit;
        }


        /// <summary>
        /// 设置方差在InOutCubic曲线上的取值范围
        /// 默认范围:0~0.3
        /// </summary>
        public void SetVarianceCubicRange(float range = 0.3f)
        {
            varianceCubicRange = range;
        }


        /// <summary>
        /// 手牌估值
        /// </summary>
        /// <param name="cardFaces"></param>
        /// <param name="laizi"></param>
        /// <returns></returns>
        public List<SlotCardsEvalInfo> Evaluation(CardFace[] cardFaces)
        {
            //排序
            int laiziCount = 0;
            CardInfo[] cardInfos = CardsTransform.Instance.CreateFormatCards(cardFaces, null, ref laiziCount);
            cardFaces = CardsTransform.Instance.CreateCardFaces(cardInfos);

            //
            List<SlotCardsEvalInfo> slotCardsEvalGroup = new List<SlotCardsEvalInfo>();

            List<CardFace>[] evalDatas = new List<CardFace>[3]
            {
                  new List<CardFace>(),
                  new List<CardFace>(),
                  new List<CardFace>(),
            };

            CardsTypeInfo?[] cardsTypeInfos = new CardsTypeInfo?[3];

            EvalFuncParamDatas paramDatas = new EvalFuncParamDatas()
            {
                cardFaces = cardFaces,
                curtSlotCardTypeInfo = null,
                slotCardsEvalGroup = slotCardsEvalGroup,
                evalDatas = evalDatas,
                cardsTypeInfos = cardsTypeInfos,
                slotDepth = -1,
            };

            CreateEvalInfo(paramDatas);

            //
            slotCardsEvalGroup.Sort(cardEvalComparer);
            List<SlotCardsEvalInfo> optimalSlotCardsEvalGroup = GetOptimalSlotCardsEvalInfoList(slotCardsEvalGroup);


            return optimalSlotCardsEvalGroup;
        }


        void CreateEvalInfo(EvalFuncParamDatas paramDatas)
        {
            CardFace[] cardFaces = paramDatas.cardFaces;
            CardsTypeInfo? curtSlotCardTypeInfo = paramDatas.curtSlotCardTypeInfo;
            List<SlotCardsEvalInfo> slotCardsEvalGroup = paramDatas.slotCardsEvalGroup;
            List<CardFace>[] evalDatas = paramDatas.evalDatas;
            CardsTypeInfo?[] cardsTypeInfos = paramDatas.cardsTypeInfos;
            int slotDepth = paramDatas.slotDepth;


            if (curtSlotCardTypeInfo != null)
            {
                //去除当道牌型大于前道牌型的组合
                if (slotDepth >= 1 &&
                    curtSlotCardTypeInfo.Value.type > cardsTypeInfos[slotDepth - 1].Value.type)
                {
                    return;
                }

                //根据赖子牌使用数量，移除当前槽相同数量的赖子牌
                CardFace[] removeLaizi = new CardFace[5];
                if (curtSlotCardTypeInfo.Value.laiziCount > 0)
                    cardFaces = CardsTransform.Instance.RemoveLaiziByCount(cardFaces, laizi, curtSlotCardTypeInfo.Value.laiziCount, removeLaizi);

                //移除当前槽已使用的牌型牌
                CardInfo[] cardInfos = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, curtSlotCardTypeInfo.Value.cardFaceValues);
                cardFaces = CardsTransform.Instance.CreateCardFaces(cardInfos);

                //添加数据
                evalDatas[slotDepth].AddRange(curtSlotCardTypeInfo.Value.cardFaceValues);
                for (int i = 0; i < curtSlotCardTypeInfo.Value.laiziCount; i++)
                    evalDatas[slotDepth].Add(removeLaizi[i]);

                // 
                cardsTypeInfos[slotDepth] = curtSlotCardTypeInfo;
            }

            if (slotDepth == 2)
            {
                int mustSingleCardCount = 5 - evalDatas[0].Count + 5 - evalDatas[1].Count + 3 - evalDatas[2].Count;
                if (cardFaces.Length < mustSingleCardCount)
                    return;
             
                int n = 0;
                int valueIdx = 0;
                int[] value = new int[5];

                //尾道
                SlotCardsEvalInfo evalInfo = new SlotCardsEvalInfo();
                evalInfo.slotCardFaceList[0].AddRange(evalDatas[0]);
                for (int i = 0; i < 5 - evalDatas[0].Count; i++)
                {
                    value[valueIdx++] = CardsTransform.Instance.GetValue(cardFaces[n]);
                    if (value[valueIdx - 1] == 1)
                        value[valueIdx - 1] = 14;

                    evalInfo.slotCardFaceList[0].Add(cardFaces[n++]);
                }

                if (cardsTypeInfos[0] == null)
                    evalInfo.slotCardsType[0] = CardsType.Single;
                else
                    evalInfo.slotCardsType[0] = cardsTypeInfos[0].Value.type;

                evalInfo.slotScore[0] = CalCardsScore(cardsTypeInfos[0], value);
                evalInfo.slotShuiScore[0] = GetCardsTypeShuiScore(cardsTypeInfos[0], 0);

                //中道
                valueIdx = 0;
                Array.Clear(value, 0, value.Length);
                evalInfo.slotCardFaceList[1].AddRange(evalDatas[1]);
                for (int i = 0; i < 5 - evalDatas[1].Count; i++)
                {
                    value[valueIdx++] = CardsTransform.Instance.GetValue(cardFaces[n]);
                    if (value[valueIdx - 1] == 1)
                        value[valueIdx - 1] = 14;

                    evalInfo.slotCardFaceList[1].Add(cardFaces[n++]);
                }

                if (cardsTypeInfos[1] == null)
                    evalInfo.slotCardsType[1] = CardsType.Single;
                else
                    evalInfo.slotCardsType[1] = cardsTypeInfos[1].Value.type;

                evalInfo.slotScore[1] = CalCardsScore(cardsTypeInfos[1], value);
                evalInfo.slotShuiScore[1] = GetCardsTypeShuiScore(cardsTypeInfos[1], 1);


                if (evalInfo.slotScore[1] > evalInfo.slotScore[0])
                    return;


                //头道
                valueIdx = 0;
                Array.Clear(value, 0, value.Length);
                evalInfo.slotCardFaceList[2].AddRange(evalDatas[2]);
                CardFace cf;

                if (3 - evalDatas[2].Count > 0 &&
                    CardsTransform.Instance.GetValue(cardFaces[n]) == 1)
                {
                    int m = newCardFaces.Length - 1;
                    newCardFaces[m--] = cardFaces[n];
                    for (int i = cardFaces.Length - 1; i > n; i--)
                    {
                        newCardFaces[m--] = cardFaces[i];
                    }

                    for (int i = 0; i < 3 - evalDatas[2].Count; i++)
                    {
                        cf = newCardFaces[newCardFaces.Length - i - 1];
                        value[valueIdx++] = CardsTransform.Instance.GetValue(cf);
                        if (value[valueIdx - 1] == 1)
                            value[valueIdx - 1] = 14;

                        evalInfo.slotCardFaceList[2].Add(cf);
                    }
                }
                else
                {
                    for (int i = 0; i < 3 - evalDatas[2].Count; i++)
                    {
                        cf = cardFaces[cardFaces.Length - i - 1];
                        value[valueIdx++] = CardsTransform.Instance.GetValue(cf);
                        evalInfo.slotCardFaceList[2].Add(cf);
                    }
                }

                if (cardsTypeInfos[2] == null)
                    evalInfo.slotCardsType[2] = CardsType.Single;
                else
                    evalInfo.slotCardsType[2] = cardsTypeInfos[2].Value.type;

                evalInfo.slotScore[2] = CalCardsScore(cardsTypeInfos[2], value);

                if (evalInfo.slotScore[2] > evalInfo.slotScore[1])
                    return;

                if (cardsTypeInfos[2] != null && cardsTypeInfos[2].Value.type == CardsType.SanTiao)
                {
                    evalInfo.slotScore[2] += 600;
                }

                evalInfo.slotShuiScore[2] = GetCardsTypeShuiScore(cardsTypeInfos[2], 2);


                //综合估值
                evalInfo.totalScore = evalInfo.slotScore[0] + evalInfo.slotScore[1] + evalInfo.slotScore[2];
                evalInfo.totalShuiScore = evalInfo.slotShuiScore[0] + evalInfo.slotShuiScore[1] + evalInfo.slotShuiScore[2];

                evalInfo.scoreAndShuiEval = 
                    evalInfo.totalScore / maxScore * scoreAndShuiWeight +
                    evalInfo.totalShuiScore / maxShui * (1 - scoreAndShuiWeight);

                evalInfo.variance = SolveVariance(evalInfo.slotScore);

                float normalVar = evalInfo.variance / varianceLimit;
                float weight = 1 - InOutCubic(normalVar, 0f, varianceCubicRange, 1);
                evalInfo.compEval = weight * evalInfo.scoreAndShuiEval;

                //
                slotCardsEvalGroup.Add(evalInfo);

                return;
            }

            //为下一个槽准备数据
           // CardsTypeCreater nextSlotCreater = new CardsTypeCreater();
          //  nextSlotCreater.SetLaizi(laizi);
            nextSlotCreater.CreateAllCardsTypeArray(cardFaces);

            CardsTypeInfo[] info;

            if (slotDepth < 1)
            {
                if (nextSlotCreater.IsExistNotSingleCardsType())
                    info = nextSlotCreater.GetAllCardsTypeInfo();
                else
                    info = nextSlotCreater.GetAllCardsTypeInfo(false);
            }
            else
            {
                List<CardsTypeInfo> tmpInfo = new List<CardsTypeInfo>();
                tmpInfo.AddRange(nextSlotCreater.SantiaoList);
                tmpInfo.AddRange(nextSlotCreater.DuiziList);
                info = tmpInfo.ToArray();

                if (info.Length == 0)
                {
                    EvalFuncParamDatas paramDatas2 = new EvalFuncParamDatas()
                    {
                        cardFaces = cardFaces,
                        curtSlotCardTypeInfo = null,
                        slotCardsEvalGroup = slotCardsEvalGroup,
                        evalDatas = evalDatas,
                        cardsTypeInfos = cardsTypeInfos,
                        slotDepth = slotDepth + 1,
                    };

                    CreateEvalInfo(paramDatas2);
                    evalDatas[slotDepth + 1].Clear();
                    cardsTypeInfos[slotDepth + 1] = null;
                    return;
                }
            }

            for (int i = 0; i < info.Length; i++)
            {
                EvalFuncParamDatas paramDatas2 = new EvalFuncParamDatas()
                {
                    cardFaces = cardFaces,
                    curtSlotCardTypeInfo = info[i],
                    slotCardsEvalGroup = slotCardsEvalGroup,
                    evalDatas = evalDatas,
                    cardsTypeInfos = cardsTypeInfos,
                    slotDepth = slotDepth + 1,
                };

                CreateEvalInfo(paramDatas2);
                evalDatas[slotDepth + 1].Clear();
                cardsTypeInfos[slotDepth + 1] = null;
            }
        }


        float SolveVariance(float[] nums)
        {
            float score = 0;
            for (int i = 0; i < nums.Length; i++)
                score += nums[i];

            float avg = score / nums.Length;
            float sub;
            float var = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                sub = (nums[i] - avg) / avg;
                var += sub * sub;
            }

            float variance = var / nums.Length;
            variance = (float)Math.Sqrt(variance);
            return variance;
        }


        //
        float InOutCubic(float t, float b, float c, float d)
        {
            if (t > 1) { t = 1; }
            else if (t < 0) { t = 0; }

            t /= d / 2;

            if (t < 1)
                return c / 2 * (float)Math.Pow(t, 3) + b;

            return c / 2 * ((float)Math.Pow(t - 2, 3) + 2) + b;
        }


        /// <summary>
        /// 获取最优评估牌型信息
        /// </summary>
        /// <param name="slotCardsEvalInfoList"></param>
        /// <returns></returns>
        List<SlotCardsEvalInfo> GetOptimalSlotCardsEvalInfoList(List<SlotCardsEvalInfo> slotCardsEvalInfoList)
        {
            List<SlotCardsEvalInfo> newSlotCardsEvalInfo = new List<SlotCardsEvalInfo>();
            int count = 0;
            SlotCardsEvalInfo evalInfo;
            CardsTypeInfo info;

            for (int i = 0; i < slotCardsEvalInfoList.Count; i++)
            {
                evalInfo = slotCardsEvalInfoList[i];

                //排除非最大牌型(针对补全赖子牌后的牌型歧义)
                optimalCardCreater.CreateAllCardsTypeArray(evalInfo.slotCardFaceList[2].ToArray());
                info = optimalCardCreater.GetMaxScoreCardsTypeInfo();
                if (info.type != evalInfo.slotCardsType[2])
                    continue;

                optimalCardCreater.CreateAllCardsTypeArray(evalInfo.slotCardFaceList[1].ToArray());
                info = optimalCardCreater.GetMaxScoreCardsTypeInfo();
                if (info.type != evalInfo.slotCardsType[1])
                    continue;

                optimalCardCreater.CreateAllCardsTypeArray(evalInfo.slotCardFaceList[0].ToArray());
                info = optimalCardCreater.GetMaxScoreCardsTypeInfo();
                if (info.type != evalInfo.slotCardsType[0])
                    continue;

                PostSort(evalInfo);
                newSlotCardsEvalInfo.Add(evalInfo);


                count++;

                if (count == optimalSlotCardsEvalInfoCount)
                    break;
            }

            return newSlotCardsEvalInfo;
        }

        void PostSort(SlotCardsEvalInfo evalInfo)
        {
            for (int i = 0; i < 2; i++)
            {
                if (evalInfo.slotCardsType[i] == CardsType.HuLu)
                {
                    List<CardFace> facelist = evalInfo.slotCardFaceList[i];


                    if(CardsTransform.Instance.GetValue(facelist[0]) == CardsTransform.Instance.GetValue(facelist[1]) &&
                       CardsTransform.Instance.GetValue(facelist[1]) == CardsTransform.Instance.GetValue(facelist[2]))
                    {
                        continue;
                    }
                    else
                    {
                        CardFace a = facelist[0];
                        CardFace b = facelist[1];
                        facelist[0] = facelist[2];
                        facelist[1] = facelist[3];
                        facelist[2] = facelist[4];
                        facelist[3] = a;
                        facelist[4] = b;
                    }
                }   
            }
        }


        public void SortCardsTypes(List<CardsTypeInfo> cardsTypeInfoList)
        {
            if (cardsTypeInfoList != null && cardsTypeInfoList.Count > 0)
                cardsTypeInfoList.Sort(comparer);
        }

        int Cmp(
          CardsTypeInfo cardsTypeInfo1, int[] richCardValue1,
          CardsTypeInfo cardsTypeInfo2, int[] richCardValue2)
        {
            float score1 = CalCardsScore(cardsTypeInfo1, richCardValue1);
            float score2 = CalCardsScore(cardsTypeInfo2, richCardValue2);

            if (score1 > score2)
                return -1;
            else if (score1 < score2)
                return 1;

            return 0;
        }

        /// <summary>
        /// 计算牌的分值
        /// </summary>
        /// <param name="cardsTypeInfo">牌的牌型详细信息</param>
        /// <param name="otherCardValue">包含的其它杂牌</param>
        /// <returns></returns>
        public float CalCardsScore(CardsTypeInfo? cardsTypeInfo, int[] otherCardValue)
        {
            if(cardsTypeInfo == null)
            {
                if (otherCardValue != null && otherCardValue.Length > 0)
                {
                    int max = otherCardValue[0];
                    for (int i = 1; i < otherCardValue.Length; i++)
                    {
                        if (otherCardValue[i] > max)
                            max = otherCardValue[i];
                    }
                    return max;
                }
            }
            else if(cardsTypeInfo != null && cardsTypeInfo.Value.type == CardsType.Single)
            {
                float max = cardsTypeInfo.Value.score;
                if (otherCardValue != null && otherCardValue.Length > 0)
                {
                    for (int i = 1; i < otherCardValue.Length; i++)
                    {
                        if (otherCardValue[i] > max)
                            max = otherCardValue[i];
                    }
                }

                return max;
            }

            float score = 0;
            if (cardsTypeInfo != null)
            {
                score = ((float)cardsTypeInfo.Value.type - 1) * 100;
                score += cardsTypeInfo.Value.score;
            }

            if (otherCardValue != null && otherCardValue.Length > 0)
            {
                int max = otherCardValue[0];
                for (int i = 1; i < otherCardValue.Length; i++)
                {
                    if (otherCardValue[i] > max)
                        max = otherCardValue[i];
                }

                score += max * 0.001f;
            }

            return score;
        }

        /// <summary>
        /// 计算牌的分值
        /// </summary>
        /// <param name="cardsTypeInfo">牌的牌型详细信息</param>
        /// <param name="otherCardValue">包含的其它杂牌</param>
        /// <returns></returns>
        public float CalCardsScore(CardsTypeInfo cardsTypeInfo, CardInfo[] otherCardInfos)
        {
            int[] values = null;
            if (otherCardInfos != null && otherCardInfos.Length > 0)
            {
                values = new int[otherCardInfos.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = otherCardInfos[i].value;
                    if (values[i] == 1)
                        values[i] = 14;
                }
            }

            return CalCardsScore(cardsTypeInfo, values);
        }


        /// <summary>
        /// 计算牌的分值
        /// </summary>
        /// <param name="cardFaces">手牌</param>
        /// <returns></returns>
        public float CalCardsScore(CardFace[] cardFaces)
        {
            CardsTypeCreater cardCreater = new CardsTypeCreater();
            cardCreater.SetLaizi(laizi);
            cardCreater.CreateAllCardsTypeArray(cardFaces);
            CardsTypeInfo info = cardCreater.GetMaxScoreCardsTypeInfo();
            CardInfo[] otherCards = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, info.cardFaceValues);

            return CalCardsScore(info, otherCards);
        }

        static public float GetCardsTypeShuiScore(CardsTypeInfo? cardsTypeInfo, int slotIdx)
        {
            if(cardsTypeInfo == null)
            {
               return GetCardsTypeShuiScore(CardsType.Single, slotIdx);
            }

            return GetCardsTypeShuiScore(cardsTypeInfo.Value.type, slotIdx);
        }


        /// <summary>
        /// 获取牌型所在对应槽位的水数
        /// </summary>
        /// <param name="cardsType"></param>
        /// <param name="slotIdx"></param>
        /// <returns></returns>
        static public float GetCardsTypeShuiScore(CardsType cardsType, int slotIdx)
        {
            switch (cardsType)
            {
                case CardsType.Single:
                case CardsType.DuiZi:
                case CardsType.TwoDui:
                case CardsType.ShunZi:
                case CardsType.TongHua:
                    return 1;

                case CardsType.SanTiao:
                    if (slotIdx == 2)
                        return 3;
                    return 1;
 
                case CardsType.HuLu:
                    if (slotIdx == 0)
                        return 1;
                    else if (slotIdx == 1)
                        return 2;
                    break;

                case CardsType.Bomb:
                    if (slotIdx == 0)
                        return 4;
                    else if (slotIdx == 1)
                        return 8;
                    break;

                case CardsType.TongHuaShun:
                    if (slotIdx == 0)
                        return 5;
                    else if (slotIdx == 1)
                        return 10;
                    break;

                case CardsType.WuTong:
                    if (slotIdx == 0)
                        return 10;
                    else if (slotIdx == 1)
                        return 20;
                    break;

            }

            return 1;
        }

        static public float GetShunZiBaseScore(float startCardValue)
        {
            float score = 0;
            for (int i = 0; i < 5; i++)
                score += startCardValue + i;
            return score;
        }

        static public float GetHuLuBaseScore(float firstCardValue, float secondCardValue)
        {
            float score = firstCardValue + 0.01f * secondCardValue;
            return score;
        }

        static public float GetTwoDuiBaseScore(float firstCardValue, float secondCardValue)
        {
            float score = firstCardValue + secondCardValue;
            return score;
        }

        static public float GetWuTongBaseScore(float cardValue)
        {
            return cardValue;
        }

        static public float GetTieZhiBaseScore(float cardValue)
        {
            return cardValue;
        }

        static public float GetSanTiaoBaseScore(float cardValue)
        {
            return cardValue;
        }

        static public float GetDuiziBaseScore(float cardValue)
        {
            return cardValue;
        }

        static public float GetTongHuaBaseScore(float[] cardValues)
        {
            float max = cardValues[0];
            for (int i = 1; i < cardValues.Length; i++)
            {
                if (cardValues[i] > max)
                    max = cardValues[i];
            }
            return max;
        }

    }
}