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
            CardsTypeDict.Instance.CreateDict();

            CardFace[] cardValues = new CardFace[]
            {
                CardFace.Laizi,
                CardFace.Spade_2,
                CardFace.Laizi,
                CardFace.Heart_5,
                CardFace.Laizi,
                CardFace.Heart_10,
                CardFace.Spade_J,
                CardFace.Laizi,
                CardFace.Diamond_K,
                CardFace.Laizi,
                CardFace.Heart_J,
                CardFace.Heart_7,
                CardFace.Laizi,
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
           // SpecCardsType type = specCard.Check(cardValues, outCards);

            //
            int laiziCount = 0;
            bool ret = false;
            CardInfo[] formatCards = CardsTransform.Instance.CreateFormatCards(cardValues, ref laiziCount);
            ret = specCard.IsSanShunZi(formatCards, laiziCount, outCards);


        }
    }
}
