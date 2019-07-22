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
                CardFace.Club_10,
                CardFace.Club_10,
                CardFace.Club_10,
                CardFace.Diamond_7,
                CardFace.Diamond_9,
                CardFace.Laizi,
                CardFace.Heart_9,
                CardFace.Diamond_8,
                CardFace.Laizi,
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

            //获取普通牌型组合
            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateAllCardsTypeArray(cardValues);
            //获取cardValues牌中的顺子组合
            List<CardsTypeInfo> shunziList = creater.ShunziList;

            //特殊牌型检查
            SpecCardsCheck specCard = new SpecCardsCheck();

            //存储特殊牌型结果（已排好序）
            CardFace[] outCards = new CardFace[13];
            SpecCardsType type = specCard.Check(cardValues, outCards);

            //
            //int laiziCount = 0;
            //bool ret = false;
            //CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);
            //ret = specCard.IsSixBomb(formatCards, laiziCount, outCards);


        }
    }
}
