using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRuleNS
{

    public struct SlotCardsEvalInfo
    {
        public CardFace[] slot1CardFaces;
        public CardFace[] slot2CardFaces;
        public CardFace[] slot3CardFaces;
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
            List<SlotCardsEvalInfo> slotCardsEval = new List<SlotCardsEvalInfo>(); 
            int count;

            CardsTypeCreater creater1 = new CardsTypeCreater();
            creater1.CreateAllCardsTypeArray(cardFaces, laizi);
            CardsTypeInfo[] info1;
            if (creater1.IsExistNotSingleCardsType())
                info1 = creater1.GetAllCardsTypeInfo();
            else
                info1 = creater1.GetAllCardsTypeInfo(false);

            for (int i=0; i<info1.Length; i++)
            {
                count = GetCardsTypeBaseCount(info1[i].type);

                if(count == 4)
                {
                    CardFace[] cardFaces2 = CardsTransform.Instance.RemoveLaiziByCount(cardFaces, laizi, info1[i].laiziCount);
                    CardInfo[] tmpCardInfos2 = CardsTransform.Instance.CreateRemoveFaceValues(cardFaces2, info1[i].cardFaceValues);
                    CardInfo[] cardInfos2;

                    for (int m = 0; m < tmpCardInfos2.Length; m++)
                    {
                        cardInfos2 = CardsTransform.Instance.CreateRemoveCardInfos(tmpCardInfos2, new CardInfo[] { tmpCardInfos2[m] });
                        cardFaces2 = CardsTransform.Instance.CreateCardFaces(cardInfos2);
                        CardsTypeCreater creater2 = new CardsTypeCreater();
                        creater2.CreateAllCardsTypeArray(cardFaces2, laizi);

                        CardsTypeInfo[] info2;
                        if (creater2.IsExistNotSingleCardsType())
                            info2 = creater1.GetAllCardsTypeInfo();
                        else
                            info2 = creater1.GetAllCardsTypeInfo(false);

                        for (int j = 0; j < info2.Length; j++)
                        {

                            //cardsEvalInfo.slot2CardFacesList.Add(info2[i].cardFaceValues);

                        }
                    }
                }
                else if(count == 3)
                {


                }
                else if(count == 2)
                {

                }
                else
                {


                }
            }

            return null;
        }

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
