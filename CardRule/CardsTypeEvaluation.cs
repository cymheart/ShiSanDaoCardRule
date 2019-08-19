﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRuleNS
{

    public class SlotCardsEvalInfo
    {
        public List<CardFace>[] slotCardFaceList = new List<CardFace>[3]
        {
            new List<CardFace>(),new List<CardFace>(),new List<CardFace>()
        };

        public float[] slotScore = new float[3];
        public float totalScore;

        public float[] slotShuiScore = new float[3];
        public float totalShuiScore;

        public float eval;
        public float variance;
        public float variance_eval;
    }

    public class CardsTypeEvaluation
    {
        /// <summary>
        /// 分值水数权重，1:偏向牌型分值加权， 0：偏向水数加权
        /// </summary>
        float scoreAndShuiWeight = 0.9f;
        float varianceLimit = 1.5f;

        private static CardsTypeEvaluation instance = null;
        public static CardsTypeEvaluation Instance
        {
            get
            {
                if (instance == null)
                    instance = new CardsTypeEvaluation();
                return instance;
            }
        }
        private struct EvalFuncParamDatas
        {
            public CardFace[] cardFaces;
            public CardsTypeInfo? curtSlotCardTypeInfo;
            public List<SlotCardsEvalInfo> slotCardsEvalGroup;
            public List<CardFace>[] evalDatas;
            public CardsTypeInfo?[] cardsTypeInfos;
            public int slotDepth;
            public CardFace[] refLaizi;
        }

        private class Comparer : IComparer<CardsTypeInfo>
        {
            public int Compare(CardsTypeInfo c1, CardsTypeInfo c2)
            {
                return instance.Cmp(c1, null, c2, null);
            }
        }

        private class CardEvalComparer : IComparer<SlotCardsEvalInfo>
        {
            public int Compare(SlotCardsEvalInfo info1, SlotCardsEvalInfo info2)
            {
                if (info1.variance_eval > info2.variance_eval)
                    return -1;
                else if (info1.variance_eval < info2.variance_eval)
                    return 1;

                return 0;
            }
        }

        public List<SlotCardsEvalInfo> Evaluation(CardFace[] cardFaces, CardFace[] laizi = null)
        {
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
                refLaizi = laizi
            };

            CreateEvalInfo(paramDatas);


            //
            slotCardsEvalGroup.Sort(new CardEvalComparer());

            return slotCardsEvalGroup;
        }


        void CreateEvalInfo(EvalFuncParamDatas paramDatas)
        {
            CardFace[] cardFaces = paramDatas.cardFaces;
            CardsTypeInfo? curtSlotCardTypeInfo = paramDatas.curtSlotCardTypeInfo;
            List<SlotCardsEvalInfo> slotCardsEvalGroup = paramDatas.slotCardsEvalGroup;
            List<CardFace>[] evalDatas = paramDatas.evalDatas;
            CardsTypeInfo?[] cardsTypeInfos = paramDatas.cardsTypeInfos;
            int slotDepth = paramDatas.slotDepth;
            CardFace[] refLaizi = paramDatas.refLaizi;

            if (curtSlotCardTypeInfo != null)
            {
                //根据赖子牌使用数量，移除当前槽相同数量的赖子牌
                CardFace[] removeLaizi = new CardFace[5];
                if (curtSlotCardTypeInfo.Value.laiziCount > 0)
                    cardFaces = CardsTransform.Instance.RemoveLaiziByCount(cardFaces, refLaizi, curtSlotCardTypeInfo.Value.laiziCount, removeLaizi);

                //移除当前槽已使用的牌型牌
                CardInfo[] cardInfos = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, curtSlotCardTypeInfo.Value.cardFaceValues);
                cardFaces = CardsTransform.Instance.CreateCardFaces(cardInfos);

                //添加数据
                evalDatas[slotDepth].AddRange(curtSlotCardTypeInfo.Value.cardFaceValues);
                for (int i = 0; i < curtSlotCardTypeInfo.Value.laiziCount; i++)
                    evalDatas[slotDepth].Add(removeLaizi[i]);

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

                //
                SlotCardsEvalInfo evalInfo = new SlotCardsEvalInfo();
                evalInfo.slotCardFaceList[0].AddRange(evalDatas[0]);
                for (int i = 0; i < 5 - evalDatas[0].Count; i++)
                {
                    value[valueIdx++] = CardsTransform.Instance.GetValue(cardFaces[n]);
                    evalInfo.slotCardFaceList[0].Add(cardFaces[n++]);
                }
                evalInfo.slotScore[0] = CalCardsScore(cardsTypeInfos[0], value);
                evalInfo.slotShuiScore[0] = GetCardsTypeShuiScore(cardsTypeInfos[0], 0);


                //
                valueIdx = 0;
                Array.Clear(value, 0, value.Length);
                evalInfo.slotCardFaceList[1].AddRange(evalDatas[1]);
                for (int i = 0; i < 5 - evalDatas[1].Count; i++)
                {
                    value[valueIdx++] = CardsTransform.Instance.GetValue(cardFaces[n]);
                    evalInfo.slotCardFaceList[1].Add(cardFaces[n++]);
                }
                evalInfo.slotScore[1] = CalCardsScore(cardsTypeInfos[1], value);
                evalInfo.slotShuiScore[1] = GetCardsTypeShuiScore(cardsTypeInfos[1], 1);

                //
                valueIdx = 0;
                Array.Clear(value, 0, value.Length);
                evalInfo.slotCardFaceList[2].AddRange(evalDatas[2]);
                for (int i = 0; i < 3 - evalDatas[2].Count; i++)
                {
                    value[valueIdx++] = CardsTransform.Instance.GetValue(cardFaces[n]);
                    evalInfo.slotCardFaceList[2].Add(cardFaces[n++]);
                }
                evalInfo.slotScore[2] = CalCardsScore(cardsTypeInfos[2], value);
                evalInfo.slotShuiScore[2] = GetCardsTypeShuiScore(cardsTypeInfos[2], 2);

                //
                evalInfo.totalScore = evalInfo.slotScore[0] + evalInfo.slotScore[1] + evalInfo.slotScore[2];
                evalInfo.totalShuiScore = evalInfo.slotShuiScore[0] + evalInfo.slotShuiScore[1] + evalInfo.slotShuiScore[2];

                evalInfo.eval = 
                    evalInfo.totalScore / 2200f * scoreAndShuiWeight +
                    evalInfo.totalShuiScore / 24f * (1 - scoreAndShuiWeight);

                evalInfo.variance = SolveVariance(evalInfo.slotScore);

                float noramlVar = evalInfo.variance / varianceLimit;
                float v = 1 - InOutCubic(noramlVar, 0.3f, 0.7f, 1);
                evalInfo.variance_eval = v * evalInfo.eval;

                //
                slotCardsEvalGroup.Add(evalInfo);

                return;
            }

            //为下一个槽准备数据
            CardsTypeCreater nextSlotCreater = new CardsTypeCreater();
            nextSlotCreater.CreateAllCardsTypeArray(cardFaces, refLaizi);

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
                        refLaizi = refLaizi
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
                    refLaizi = refLaizi
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


        //1f, 0.3f, 0.7f, 1
        float InOutCubic(float t, float b, float c, float d)
        {
            if (t > 1) { t = 1; }
            else if (t < 0) { t = 0; }

            t /= d / 2;

            if (t < 1)
                return c / 2 * (float)Math.Pow(t, 3) + b;

            return c / 2 * ((float)Math.Pow(t - 2, 3) + 2) + b;
        }

        public void SortCardsTypes(List<CardsTypeInfo> cardsTypeInfoList)
        {
            if (cardsTypeInfoList != null && cardsTypeInfoList.Count > 0)
                cardsTypeInfoList.Sort(new Comparer());
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

                score += max * 0.0001f;
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
            cardCreater.CreateAllCardsTypeArray(cardFaces);
            CardsTypeInfo info = cardCreater.GetMaxScoreCardsTypeInfo();
            CardInfo[] otherCards = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, info.cardFaceValues);

            return CalCardsScore(info, otherCards);
        }

        public float GetCardsTypeShuiScore(CardsTypeInfo? cardsTypeInfo, int slotIdx)
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
            public float GetCardsTypeShuiScore(CardsType cardsType, int slotIdx)
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


        public float GetShunZiBaseScore(float startCardValue)
        {
            float score = 0;
            for (int i = 0; i < 5; i++)
                score += startCardValue + i;
            return score;
        }

        public float GetHuLuBaseScore(float firstCardValue, float secondCardValue)
        {
            float score = firstCardValue + 0.01f * secondCardValue;
            return score;
        }

        public float GetTwoDuiBaseScore(float firstCardValue, float secondCardValue)
        {
            float score = firstCardValue + secondCardValue;
            return score;
        }

        public float GetWuTongBaseScore(float cardValue)
        {
            return cardValue;
        }

        public float GetTieZhiBaseScore(float cardValue)
        {
            return cardValue;
        }

        public float GetSanTiaoBaseScore(float cardValue)
        {
            return cardValue;
        }

        public float GetDuiziBaseScore(float cardValue)
        {
            return cardValue;
        }

        public float GetTongHuaBaseScore(float[] cardValues)
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