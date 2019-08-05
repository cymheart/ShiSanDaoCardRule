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
                CardFace.Diamond_10,
                CardFace.Club_4,
                CardFace.Club_5,
                CardFace.Diamond_7,
                CardFace.Diamond_2,
                CardFace.Diamond_2,
                CardFace.Heart_9,
                CardFace.Diamond_8,
                CardFace.Heart_3,
                CardFace.Diamond_9,
            };

            CardFace[] cardValues2 = new CardFace[]
            {
                CardFace.Laizi,
                CardFace.Spade_2,
                CardFace.Laizi,
                CardFace.Heart_5,
                CardFace.Laizi,       
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
            //int laiziCount = 0;
            //bool ret = false;
            //CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);
            //ret = specCard.IsSixBomb(formatCards, laiziCount, outCards);


        }
    }
}
