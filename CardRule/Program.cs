using CardRuleNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardRule
{
    class Program
    {
        static void Main(string[] args)
        {
            float var = SolveVariance(new float[] { 860, 610.02f, 0.0009f });

            float var2 = SolveVariance(new float[] { 860, 310, 102 });


            //0, 0.3f, 1f, 1
            float val = InOutCubic(0.3f, 0, 1, 1);



            CardFace[] cardValues = new CardFace[]
              {
CardFace.Club_10,
CardFace.Club_10,
CardFace.Club_10,
CardFace.Club_2,
CardFace.Club_3,
CardFace.Spade_4,
CardFace.Diamond_7,
CardFace.Diamond_9,
CardFace.Heart_5,
CardFace.Diamond_6,
CardFace.Heart_8,
CardFace.Heart_4,
CardFace.Heart_9
              };

            CardsTypeDict dict = CardsTypeDict.Instance;

            //获取普通牌型组合
            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateAllCardsTypeArray(cardValues, new[] {CardFace.BlackJoker, CardFace.RedJoker });
            //获取cardValues牌中的顺子组合
            List<CardsTypeInfo> santiaos = creater.SantiaoList;

            //特殊牌型检查
            SpecCardsCheck specCard = new SpecCardsCheck();

            //存储特殊牌型结果（已排好序）
            CardFace[] outCards = new CardFace[13];
            SpecCardsType type = specCard.Check(cardValues, new[] { CardFace.BlackJoker, CardFace.RedJoker }, outCards);


            //
            CardsTypeEvaluation.Instance.Evaluation(cardValues, new[] { CardFace.BlackJoker, CardFace.RedJoker });

           

        }

        static float SolveVariance(float[] nums)
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

        static float InOutCubic(float t, float b, float c, float d)
        {
            t /= d / 2;

            if (t < 1)
                return c / 2 * (float)Math.Pow(t, 3) + b;

            return c / 2 * ((float)Math.Pow(t - 2, 3) + 2) + b;
        }

    }
}
