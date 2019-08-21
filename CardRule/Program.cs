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
            creater.CreateAllCardsTypeArray(cardValues);
            //获取cardValues牌中的顺子组合
            List<CardsTypeInfo> santiaos = creater.SantiaoList;

            //特殊牌型检查
            SpecCardsCheck specCard = new SpecCardsCheck();

            //存储特殊牌型结果（已排好序）
            CardFace[] outCards = new CardFace[13];
            SpecCardsType type = specCard.Check(cardValues, outCards);


            //
            CardsTypeEvaluation.Instance.Evaluation(cardValues);

          
        }

       
    }
}
