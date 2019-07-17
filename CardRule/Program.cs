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
            CardLaiziRules.Instance.CreatePaiXingDict();

            RulePukeFaceValue[] faceValues = new RulePukeFaceValue[]
            {
                RulePukeFaceValue.Club_2,
                RulePukeFaceValue.Club_3,
                RulePukeFaceValue.Club_5,
                RulePukeFaceValue.Diamond_3,
                RulePukeFaceValue.Diamond_5,
                RulePukeFaceValue.Diamond_J,
                RulePukeFaceValue.Heart_10,
                RulePukeFaceValue.Spade_K,
                RulePukeFaceValue.Spade_Q,
                RulePukeFaceValue.Diamond_4,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Diamond_A,
                RulePukeFaceValue.Laizi
            };

            CardLaiziRulesOp cardop = new CardLaiziRulesOp(faceValues);
            cardop.CreatePaiXingArray();

        }
    }
}
