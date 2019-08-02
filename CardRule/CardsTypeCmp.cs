using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRuleNS
{
    public class CardsTypeCmp
    {
        private static CardsTypeCmp instance = null;
        public static CardsTypeCmp Instance
        {
            get
            {
                if (instance == null)
                    instance = new CardsTypeCmp();
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
        public void SortCardsTypes(List<CardsTypeInfo> cardsTypeInfoList)
        {
            cardsTypeInfoList.Sort(new Comparer());
        }

        public int Cmp(
            CardsTypeInfo cardsTypeInfo1, int[] richCardValue1,
            CardsTypeInfo cardsTypeInfo2, int[] richCardValue2)
        {
            if (cardsTypeInfo1.CardsTypeType < cardsTypeInfo2.CardsTypeType)
                return -1;
            else if (cardsTypeInfo1.CardsTypeType > cardsTypeInfo2.CardsTypeType)
                return 1;

            switch(cardsTypeInfo1.CardsTypeType)
            {
                case CardsType.TongHuaShun:
                case CardsType.ShunZi:
                case CardsType.WuTong:
                case CardsType.HuLu:
                case CardsType.TongHua:
                    {
                        if (cardsTypeInfo1.score > cardsTypeInfo2.score)
                            return 1;
                        else if (cardsTypeInfo1.score < cardsTypeInfo2.score)
                            return -1;
                    }
                    break;

                case CardsType.Bomb:
                case CardsType.TwoDui:
                    {
                        if (cardsTypeInfo1.score > cardsTypeInfo2.score)
                            return 1;
                        else if (cardsTypeInfo1.score < cardsTypeInfo2.score)
                            return -1;
                        else if (richCardValue1 != null && richCardValue2 != null)
                        {
                            if (richCardValue1[0] > richCardValue2[0])
                                return 1;
                            else if (richCardValue1[0] < richCardValue2[0])
                                return 1;
                        }
                    }
                    break;

                case CardsType.SanTiao:
                    {
                        if (cardsTypeInfo1.score > cardsTypeInfo2.score)
                            return 1;
                        else if (cardsTypeInfo1.score < cardsTypeInfo2.score)
                            return -1;
                        else if (richCardValue1 != null && richCardValue2 != null)
                        {
                            int max1 = Math.Max(richCardValue1[0], richCardValue1[1]);
                            int max2 = Math.Max(richCardValue2[0], richCardValue2[1]);

                            if (max1 > max2)
                                return 1;
                            else if (max1 < max2)
                                return -1;
                        }
                    }
                    break;

                case CardsType.DuiZi:
                    {
                        if (cardsTypeInfo1.score > cardsTypeInfo2.score)
                            return 1;
                        else if (cardsTypeInfo1.score < cardsTypeInfo2.score)
                            return -1;
                        else if (richCardValue1 != null && richCardValue2 != null)
                        {
                            int max1 = Math.Max(richCardValue1[0], richCardValue1[1]);
                            max1 = Math.Max(max1, richCardValue1[2]);

                            int max2 = Math.Max(richCardValue2[0], richCardValue2[1]);
                            max2 = Math.Max(max2, richCardValue2[2]);

                            if (max1 > max2)
                                return 1;
                            else if (max1 < max2)
                                return -1;
                        }
                    }
                    break;
            }

            return 0;
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
