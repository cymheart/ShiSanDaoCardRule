﻿using CardRuleNS;
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
CardFace.Club_A,
CardFace.Heart_6,
CardFace.Diamond_K,
CardFace.BlackJoker,
CardFace.RedJoker,
CardFace.Club_A,
CardFace.Heart_A,
CardFace.Diamond_K,
CardFace.Heart_10,
CardFace.Diamond_8,
CardFace.Club_8,
CardFace.Heart_8,
CardFace.Heart_7,
CardFace.Heart_5,
CardFace.Diamond_4,
CardFace.Club_3
              };

            CardsTypeDict dict = CardsTypeDict.Instance;


            /////////////////////////////////////////////////
            //1.获取普通牌型组合
            CardsTypeCreater creater = new CardsTypeCreater();
            creater.CreateAllCardsTypeArray(cardValues);
            //获取cardValues牌中的三条牌型表
            List<CardsTypeInfo> santiaos = creater.SantiaoList;

            List<CardsTypeInfo> tonghuaShun = creater.TonghuashunList;
            List<CardFace> cardFaceList = new List<CardFace>();
            int idx = 3;

            //for (int i = 0; i < santiaos[idx].cardFaceValues.Length; i++)
            //    cardFaceList.Add(santiaos[idx].cardFaceValues[i]);

            //for (int i = 0; i < santiaos[idx].laiziCount; i++)
            //    cardFaceList.Add(CardFace.RedJoker);

            //CardsTypeMatch match = new CardsTypeMatch();
            //MatchCardFacesInfo info = match.ComputeMatchCardFacesInfo(cardFaceList.ToArray());

           

            /////////////////////////////////////////////////
            //2.特殊牌型检查
            SpecCardsCheck specCard = new SpecCardsCheck();

            //存储特殊牌型结果（已排好序）
            CardFace[] outCards = new CardFace[13];
            CardFace[] outComputedCards = new CardFace[13];
            SpecCardsType type = specCard.Check(cardValues, outCards, outComputedCards);

            /////////////////////////////////////////////////
            //3.测试牌型估值
            List<SlotCardsEvalInfo> evalInfoList;
            CardsTypeEvaluation eval = new CardsTypeEvaluation();
            //eval.SetOptimalSlotCardsEvalInfoCount(10);

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
