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
            LaiziCardRulesDict.Instance.CreatePaiXingDict();

            RulePukeFaceValue[] faceValues = new RulePukeFaceValue[]
            {
                RulePukeFaceValue.Club_2,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Club_5,
                RulePukeFaceValue.Diamond_3,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Diamond_J,
                RulePukeFaceValue.Heart_10,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Club_3,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Laizi,
                RulePukeFaceValue.Diamond_A,
                RulePukeFaceValue.Laizi
            };

            CommonRulesCheck paixingCheck = new CommonRulesCheck();


            paixingCheck.CreatePaiXingArray(faceValues);

        }
    }
}
