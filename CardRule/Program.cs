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
            CardRuleDict.Instance.CreatePaiXingDict();

            RulePukeFaceValue[] faceValues = new RulePukeFaceValue[]
            {
                RulePukeFaceValue.Club_2,
                RulePukeFaceValue.Club_6,
                RulePukeFaceValue.Club_5,
                RulePukeFaceValue.Club_3,
                RulePukeFaceValue.Club_J,
                RulePukeFaceValue.Club_J,
                RulePukeFaceValue.Club_10,
                RulePukeFaceValue.Diamond_3,
                RulePukeFaceValue.Heart_3,
                RulePukeFaceValue.Club_3,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Club_A,
                RulePukeFaceValue.Spade_3
            };

            CardRuleCommonCheck paixingCheck = new CardRuleCommonCheck();


            paixingCheck.CreatePaiXingArray(faceValues);

        }
    }
}
