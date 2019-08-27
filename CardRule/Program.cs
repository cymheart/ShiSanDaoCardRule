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
CardFace.Heart_5,
CardFace.RedJoker,
CardFace.Spade_2,
CardFace.Club_A,
CardFace.RedJoker,
CardFace.Club_K,
CardFace.Club_Q,
CardFace.Club_J,
CardFace.Club_10,
CardFace.RedJoker,
CardFace.Spade_J,
CardFace.Spade_6,
CardFace.Diamond_6
              };

            CardsTypeDict dict = CardsTypeDict.Instance;

            /////////////////////////////////////////////////
            //1.获取普通牌型组合
            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateAllCardsTypeArray(cardValues);
            //获取cardValues牌中的三条牌型表
            List<CardsTypeInfo> santiaos = creater.SantiaoList;

            List<CardsTypeInfo> tonghuaShun = creater.TonghuashunList;
            CardFace[] computeCardFaces = CardsTypeCreater.ComputeMaxScoreCardFaces(tonghuaShun[26]);

            List<CardsTypeInfo> shun = creater.ShunziList;
            computeCardFaces = CardsTypeCreater.ComputeMaxScoreCardFaces(shun[6]);


            List<CardsTypeInfo> hulu = creater.HuluList;
            computeCardFaces = CardsTypeCreater.ComputeMaxScoreCardFaces(hulu[0]);

            List<CardsTypeInfo> towdui = creater.TwoduiList;
            computeCardFaces = CardsTypeCreater.ComputeMaxScoreCardFaces(towdui[8]);

            /////////////////////////////////////////////////
            //2.特殊牌型检查
            SpecCardsCheck specCard = new SpecCardsCheck();

            //存储特殊牌型结果（已排好序）
            CardFace[] outCards = new CardFace[13];
            SpecCardsType type = specCard.Check(cardValues, outCards);

            /////////////////////////////////////////////////
            //3.测试牌型估值
            List<SlotCardsEvalInfo> evalInfoList;
            CardsTypeEvaluation eval = new CardsTypeEvaluation();
            eval.SetOptimalSlotCardsEvalInfoCount(4);

            string text = "开始计时";
            Console.WriteLine(text);
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            //
            evalInfoList = eval.Evaluation(cardValues);

            stopwatch.Stop();
            //
            long ms = stopwatch.ElapsedMilliseconds;
            text = "用时:" + ms + "毫秒";
            Console.WriteLine(text);
            Console.ReadLine();
        }

       
    }
}
