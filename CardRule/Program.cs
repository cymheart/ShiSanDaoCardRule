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
CardFace.RedJoker,
CardFace.RedJoker,
CardFace.RedJoker,
CardFace.RedJoker,
CardFace.RedJoker,
CardFace.RedJoker,
CardFace.RedJoker,
CardFace.Diamond_9,
CardFace.RedJoker,
CardFace.Spade_2,
CardFace.Heart_8,
CardFace.Heart_4,
CardFace.Heart_6
              };

            CardsTypeDict dict = CardsTypeDict.Instance;

            //获取普通牌型组合
            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateAllCardsTypeArray(cardValues);
            //获取cardValues牌中的三条牌型表
            List<CardsTypeInfo> santiaos = creater.SantiaoList;

            //特殊牌型检查
            SpecCardsCheck specCard = new SpecCardsCheck();

            //存储特殊牌型结果（已排好序）
            CardFace[] outCards = new CardFace[13];
            SpecCardsType type = specCard.Check(cardValues, outCards);


            //测试牌型估值
            List<SlotCardsEvalInfo> evalInfoList;
            CardsTypeEvaluation eval = new CardsTypeEvaluation();
            eval.SetOptimalSlotCardsEvalInfoCount(50);

            string text = "开始计时";
            Console.WriteLine(text);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //
            evalInfoList = eval.Evaluation(cardValues);

            //
            long ms = stopwatch.ElapsedMilliseconds;
            text = "用时:" + ms + "毫秒";
            Console.WriteLine(text);
            Console.ReadLine();
        }

       
    }
}
