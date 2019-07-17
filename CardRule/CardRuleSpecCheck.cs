

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
            for (int i=1; i < 13; i++)
            {
                idx = FindCard(cards, 1);
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