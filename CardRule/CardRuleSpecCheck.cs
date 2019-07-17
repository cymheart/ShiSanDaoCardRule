

using System.Collections.Generic;

namespace CardRuleNS
{
    /// <summary>
    /// 特殊牌型检查
    /// </summary>
    public class CardRuleSpecCheck
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
            for(int i=1; i< cards.Length; i++)
            {
                if (cards[i].suit != cards[0].suit)
                    return false;
            }

            int idx;
            for (int i=2; i < 14; i++)
            {
                if (i == 14)
                    i = 1;

                idx = FindCard(cards, i);
                if(idx == -1)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[i] = RulePukeFaceValue.Laizi;
                }
                else
                {
                    outFaceValues[i] = GetRulePukeFaceValue(cards[idx].value, cards[idx].suit);
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
                for (int i=0; i<3; i++)
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
                    outFaceValues[n++] = GetRulePukeFaceValue(suitCards[minIdx][i].value, suitCards[minIdx][i].suit);

                for (int i = suitCards[minIdx].Count; i < 3; i++)
                {
                    laiziCount--;
                    if (laiziCount < 0)
                        return false;

                    outFaceValues[n++] = RulePukeFaceValue.Laizi;
                }

            }

     

            return true;
        }



        int FindCard(CardInfo[] cards, int value)
        {
            for(int i=0; i<cards.Length; i++)
            {
                if (cards[i].value == value)
                    return i;
            }

            return -1;
        }

        RulePukeFaceValue GetRulePukeFaceValue(int cardValue, int cardSuit)
        {
            return (RulePukeFaceValue)(cardSuit * 13 + cardValue - 1);
        }

    }
}