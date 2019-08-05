using System;
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

        public float[] slotEval;
        public float totalEval;
    }

    public class CardsTypeEvaluation
    {
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

        class Comparer : IComparer<CardsTypeInfo>
        {
            public int Compare(CardsTypeInfo c1, CardsTypeInfo c2)
            {
                return instance.Cmp(c1, null, c2, null);
            }
        }


        public List<SlotCardsEvalInfo> Evaluation(CardFace[] cardFaces, CardFace[] laizi = null)
        {
            List<SlotCardsEvalInfo> slotCardsEvalGroup = new List<SlotCardsEvalInfo>();
            int evalGroupIdx = 0;
            int slotIdx = 0;

            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateAllCardsTypeArray(cardFaces, laizi);
            CardsTypeInfo[] info;
            if (creater.IsExistNotSingleCardsType())
                info = creater.GetAllCardsTypeInfo();
            else
                info = creater.GetAllCardsTypeInfo(false);

            for (int i=0; i<info.Length; i++)
            {
                evalGroupIdx++;
                slotIdx = 0;
                CreateEvalInfo(cardFaces, info[i], slotCardsEvalGroup, evalGroupIdx, slotIdx, 3, laizi);
            }

            return null;
        }


        void CreateEvalInfo(
            CardFace[] richCardFaces, 
            CardsTypeInfo curtSlotCardTypeInfo,
            List<SlotCardsEvalInfo> slotCardsEvalGroup, int evalGroupIdx, int slotIdx,
            int depth,
            CardFace[] refLaizi)
        {
            //根据赖子牌使用数量，移除当前槽相同数量的赖子牌
            CardFace[] removeLaizi = new CardFace[5];
            CardFace[] cardFaces = CardsTransform.Instance.RemoveLaiziByCount(richCardFaces, refLaizi, curtSlotCardTypeInfo.laiziCount, removeLaizi);

            //移除当前槽已使用的牌型牌
            CardInfo[] tmpCardInfos = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, curtSlotCardTypeInfo.cardFaceValues);

            //添加数据
            slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].AddRange(curtSlotCardTypeInfo.cardFaceValues);
            slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].AddRange(removeLaizi);

            if (depth == 0)
                return;

            //为下一个槽准备数据
            CardsTypeCreater nextSlotCreater = new CardsTypeCreater();
            nextSlotCreater.CreateAllCardsTypeArray(cardFaces, refLaizi);

            CardsTypeInfo[] info;
            if (nextSlotCreater.IsExistNotSingleCardsType())
                info = nextSlotCreater.GetAllCardsTypeInfo();
            else
                info = nextSlotCreater.GetAllCardsTypeInfo(false);

            for (int i = 0; i < info.Length; i++)
            {
                CreateEvalInfo(cardFaces, info[i], slotCardsEvalGroup, evalGroupIdx, slotIdx + 1, depth - 1, refLaizi);
            }
        }

       
        //public List<SlotCardsEvalInfo> Evaluation(CardFace[] cardFaces, CardFace[] laizi = null)
        //{
        //    List<SlotCardsEvalInfo> slotCardsEvalGroup = new List<SlotCardsEvalInfo>();
        //    SlotCardsEvalInfo evalInfo;
        //    int count;
        //    CardFace[] laiziCardFaces = CardsTransform.Instance.GetLaiziCardFaces(cardFaces, laizi);
        //    int laiziIdx = 0;


        //    CardsTypeCreater creater1 = new CardsTypeCreater();
        //    creater1.CreateAllCardsTypeArray(cardFaces, laizi);
        //    CardsTypeInfo[] info1;
        //    if (creater1.IsExistNotSingleCardsType())
        //        info1 = creater1.GetAllCardsTypeInfo();
        //    else
        //        info1 = creater1.GetAllCardsTypeInfo(false);

        //    for (int i = 0; i < info1.Length; i++)
        //    {
        //        count = GetCardsTypeBaseCount(info1[i].type);

        //        if (count == 4)
        //        {
        //            CardFace[] cardFaces2 = CardsTransform.Instance.RemoveLaiziByCount(cardFaces, laizi, info1[i].laiziCount);
        //            CardInfo[] tmpCardInfos2 = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces2, info1[i].cardFaceValues);
        //            CardInfo[] cardInfos2;

        //            for (int m = 0; m < tmpCardInfos2.Length; m++)
        //            {
        //                cardInfos2 = CardsTransform.Instance.CreateRemoveCardInfos(tmpCardInfos2, new CardInfo[] { tmpCardInfos2[m] });
        //                cardFaces2 = CardsTransform.Instance.CreateCardFaces(cardInfos2);
        //                CardsTypeCreater creater2 = new CardsTypeCreater();
        //                creater2.CreateAllCardsTypeArray(cardFaces2, laizi);

        //                CardsTypeInfo[] info2;
        //                if (creater2.IsExistNotSingleCardsType())
        //                    info2 = creater1.GetAllCardsTypeInfo();
        //                else
        //                    info2 = creater1.GetAllCardsTypeInfo(false);

        //                for (int j = 0; j < info2.Length; j++)
        //                {
        //                    count = GetCardsTypeBaseCount(info2[i].type);

        //                    if (count == 4)
        //                    {
        //                        CardFace[] cardFaces3 = CardsTransform.Instance.RemoveLaiziByCount(cardFaces2, laizi, info2[i].laiziCount);
        //                        CardInfo[] tmpCardInfos3 = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces2, info2[i].cardFaceValues);
        //                        CardInfo[] cardInfos3;

        //                        for (int n = 0; n < tmpCardInfos2.Length; n++)
        //                        {
        //                            cardInfos3 = CardsTransform.Instance.CreateRemoveCardInfos(tmpCardInfos3, new CardInfo[] { tmpCardInfos3[n] });
        //                            cardFaces3 = CardsTransform.Instance.CreateCardFaces(cardInfos3);
        //                            CardsTypeCreater creater3 = new CardsTypeCreater();
        //                            creater3.CreateAllCardsTypeArray(cardFaces3, laizi);

        //                        }
        //                    }



        //                    //cardsEvalInfo.slot2CardFacesList.Add(info2[i].cardFaceValues);


        //                }
        //            }
        //        }
        //        else if (count == 3)
        //        {


        //        }
        //        else if (count == 2)
        //        {

        //        }
        //        else
        //        {


        //        }
        //    }

        //    return null;
        //}


        //void CreateEvalInfoByCount4(
        //    CardFace[] richCardFaces,
        //    CardsTypeInfo curtSlotCardTypeInfo,
        //    List<SlotCardsEvalInfo> slotCardsEvalGroup, int evalGroupIdx, int slotIdx,
        //    CardFace[] refLaizi)
        //{

        //    //根据赖子牌使用数量，移除当前槽相同数量的赖子牌
        //    CardFace[] removeLaizi = new CardFace[5];
        //    CardFace[] cardFaces = CardsTransform.Instance.RemoveLaiziByCount(richCardFaces, refLaizi, curtSlotCardTypeInfo.laiziCount, removeLaizi);

        //    //移除当前槽已使用的牌型牌
        //    CardInfo[] tmpCardInfos = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, curtSlotCardTypeInfo.cardFaceValues);
        //    CardInfo[] cardInfos;

        //    for (int i = 0; i < tmpCardInfos.Length; i++)
        //    {
        //        //移除一个牌作为当前槽的组合牌
        //        cardInfos = CardsTransform.Instance.CreateRemoveCardInfos(tmpCardInfos, new CardInfo[] { tmpCardInfos[i] });
        //        cardFaces = CardsTransform.Instance.CreateCardFaces(cardInfos);

        //        //添加数据
        //        slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].AddRange(curtSlotCardTypeInfo.cardFaceValues);
        //        slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].AddRange(removeLaizi);
        //        slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].Add(CardsTransform.Instance.GetCardFace(tmpCardInfos[i]));

        //        //为下一个槽准备数据
        //        CardsTypeCreater nextSlotCreater = new CardsTypeCreater();
        //        nextSlotCreater.CreateAllCardsTypeArray(cardFaces, refLaizi);

        //        CardsTypeInfo[] info;
        //        if (nextSlotCreater.IsExistNotSingleCardsType())
        //            info = nextSlotCreater.GetAllCardsTypeInfo();
        //        else
        //            info = nextSlotCreater.GetAllCardsTypeInfo(false);

        //        for (int j = 0; j < info.Length; j++)
        //        {
        //            SwitchCreateEvalInfoByCount(cardFaces, info[j], slotCardsEvalGroup, evalGroupIdx, slotIdx + 1, refLaizi);
        //        }
        //    }
        //}

        //void CreateEvalInfoByCount3(
        //    CardFace[] richCardFaces,
        //    CardsTypeInfo curtSlotCardTypeInfo,
        //    List<SlotCardsEvalInfo> slotCardsEvalGroup, int evalGroupIdx, int slotIdx,
        //    CardFace[] refLaizi)
        //{
        //    //根据赖子牌使用数量，移除当前槽相同数量的赖子牌
        //    CardFace[] removeLaizi = new CardFace[5];
        //    CardFace[] cardFaces = CardsTransform.Instance.RemoveLaiziByCount(richCardFaces, refLaizi, curtSlotCardTypeInfo.laiziCount, removeLaizi);

        //    //移除当前槽已使用的牌型牌
        //    CardInfo[] tmpCardInfos = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, curtSlotCardTypeInfo.cardFaceValues);
        //    CardInfo[] cardInfos;
        //    CardInfo[] removeCardInfos = new CardInfo[2];

        //    for (int i = 0; i < tmpCardInfos.Length - 1; i++)
        //    {
        //        for (int j = i + 1; j < tmpCardInfos.Length; j++)
        //        {
        //            removeCardInfos[0] = tmpCardInfos[i];
        //            removeCardInfos[1] = tmpCardInfos[j];

        //            //移除两个牌作为当前槽的组合牌
        //            cardInfos = CardsTransform.Instance.CreateRemoveCardInfos(tmpCardInfos, removeCardInfos);
        //            cardFaces = CardsTransform.Instance.CreateCardFaces(cardInfos);

        //            //添加数据
        //            slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].AddRange(curtSlotCardTypeInfo.cardFaceValues);
        //            slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].AddRange(removeLaizi);
        //            slotCardsEvalGroup[evalGroupIdx].slotCardFaceList[slotIdx].AddRange(CardsTransform.Instance.CreateCardFaces(removeCardInfos));

        //            //为下一个槽准备数据
        //            CardsTypeCreater nextSlotCreater = new CardsTypeCreater();
        //            nextSlotCreater.CreateAllCardsTypeArray(cardFaces, refLaizi);

        //            CardsTypeInfo[] info;
        //            if (nextSlotCreater.IsExistNotSingleCardsType())
        //                info = nextSlotCreater.GetAllCardsTypeInfo();
        //            else
        //                info = nextSlotCreater.GetAllCardsTypeInfo(false);

        //            for (int k = 0; k < info.Length; k++)
        //            {
        //                SwitchCreateEvalInfoByCount(cardFaces, info[k], slotCardsEvalGroup, evalGroupIdx, slotIdx + 1, refLaizi);
        //            }
        //        }
        //    }
        //}

        //void SwitchCreateEvalInfoByCount(
        //    CardFace[] cardFaces,
        //    CardsTypeInfo curtSlotCardTypeInfo,
        //    List<SlotCardsEvalInfo> slotCardsEvalGroup, int evalGroupIdx, int slotIdx,
        //    CardFace[] refLaizi)
        //{
        //    int count = GetCardsTypeBaseCount(curtSlotCardTypeInfo.type);

        //    switch (count)
        //    {
        //        case 4:
        //            CreateEvalInfoByCount4(cardFaces, curtSlotCardTypeInfo, slotCardsEvalGroup, evalGroupIdx, slotIdx, refLaizi);
        //            break;

        //        case 3:
        //            CreateEvalInfoByCount3(cardFaces, curtSlotCardTypeInfo, slotCardsEvalGroup, evalGroupIdx, slotIdx, refLaizi);
        //            break;
        //    }
        //}
        int GetCardsTypeBaseCount(CardsType type)
        {
            switch(type)
            {
                case CardsType.TongHuaShun:
                case CardsType.ShunZi:
                case CardsType.WuTong:
                case CardsType.HuLu:
                case CardsType.TongHua:
                    return 5;

                case CardsType.Bomb:
                case CardsType.TwoDui:
                    return 4;

                case CardsType.SanTiao:
                    return 3;

                case CardsType.DuiZi:
                    return 2;

                case CardsType.Single:
                    return 1;
            }

            return 0;
        }


        public void SortCardsTypes(List<CardsTypeInfo> cardsTypeInfoList)
        {
            if(cardsTypeInfoList != null && cardsTypeInfoList.Count > 0)
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

        public float CalCardsScore(CardsTypeInfo cardsTypeInfo, int[] otherCardValue)
        {
            float score = ((float)cardsTypeInfo.type - 1) * 100;
            score += cardsTypeInfo.score;

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

        public float CalCardsScore(CardFace[] cardFaces)
        {
            CardsTypeCreater cardCreater = new CardsTypeCreater();
            cardCreater.CreateAllCardsTypeArray(cardFaces);
            CardsTypeInfo info = cardCreater.GetMaxScoreCardsTypeInfo();
            CardInfo[] otherCards = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces, info.cardFaceValues);

            return CalCardsScore(info, otherCards);
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
