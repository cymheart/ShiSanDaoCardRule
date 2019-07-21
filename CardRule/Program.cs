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

            RulePukeFaceValue[] cardValues = new RulePukeFaceValue[]
            {
                RulePukeFaceValue.Club_2,
                RulePukeFaceValue.Club_6,
                RulePukeFaceValue.Club_5,
                RulePukeFaceValue.Club_3,
                RulePukeFaceValue.Club_J,
                RulePukeFaceValue.Club_J,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Club_A,
                RulePukeFaceValue.Spade_3
            };

            //获取普通牌型组合
            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateAllCardsTypeArray(cardValues);
            //获取cardValues牌中的顺子组合
            List<CardsTypeInfo> shunziList = creater.ShunziList;

            //特殊牌型检查
            SpecCardsCheck specCard = new SpecCardsCheck();

            //存储特殊牌型结果（已排好序）
            RulePukeFaceValue[] outCards = new RulePukeFaceValue[13];
            SpecCardsType type = specCard.Check(cardValues, outCards);
        }
    }
}
