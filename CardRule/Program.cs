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
                CardFace.Club_2,
                CardFace.Club_6,
                CardFace.Club_5,
                CardFace.Club_3,
                CardFace.Club_J,
                CardFace.Club_J,
                CardFace.Laizi,
                CardFace.Laizi,
                CardFace.Laizi,
                CardFace.Laizi,
                CardFace.Laizi,
                CardFace.Club_A,
                CardFace.Spade_3
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
        }
    }
}
