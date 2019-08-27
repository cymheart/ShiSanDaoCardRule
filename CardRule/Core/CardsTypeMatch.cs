using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardRuleNS
{
    public struct MatchCardFacesInfo
    {
        public CardFace[] computedCardFaces;
        public CardFace[] laiziCardFaces;
    }
    public class CardsTypeMatch
    {
        CardFace[] laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };

        /// <summary>
        /// 设置赖子
        /// </summary>
        /// <param name="_laizi"></param>
        public void SetLaizi(CardFace[] _laizi = null)
        {
            if (_laizi == null)
            {
                laizi = new CardFace[] { CardFace.BlackJoker, CardFace.RedJoker };
                return;
            }

            laizi = _laizi;
        }

        /// <summary>
        /// 计算确定的牌型MatchCardFacesInfo
        /// </summary>
        /// <returns></returns>
        public MatchCardFacesInfo ComputeMatchCardFacesInfo(CardFace[] cardFaces, CardsType cardsType = CardsType.None)
        {
            CardFace[] computedCardFaces = ComputeCardFaces(cardFaces, cardsType);
            CardFace[] laiziCardFaces = new CardFace[computedCardFaces.Length];
            int idx;
            CardFace[] removeLaizi = new CardFace[1];

            for (int i=0; i<computedCardFaces.Length; i++)
            {
                idx = CardsTransform.Instance.FindCardFace(cardFaces, computedCardFaces[i]);
                if (idx != -1)
                {
                    laiziCardFaces[i] = cardFaces[idx];
                    cardFaces = CardsTransform.Instance.CreateDelFaceValues(cardFaces, new CardFace[] { cardFaces[idx] });         
                }
                else
                {
                    cardFaces = CardsTransform.Instance.RemoveLaiziByCount(cardFaces, laizi, 1, removeLaizi);
                    laiziCardFaces[i] = removeLaizi[0];
                }
            }

            MatchCardFacesInfo matchInfo = new MatchCardFacesInfo()
            {
                computedCardFaces = computedCardFaces,
                laiziCardFaces = laiziCardFaces
            };

            return matchInfo;
        }


        /// <summary>
        /// 计算确定的牌型CardFaces
        /// </summary>
        /// <returns></returns>
        public CardFace[] ComputeCardFaces(CardFace[] cardFaces, CardsType cardsType = CardsType.None)
        {
            if (cardsType == CardsType.None)
            {
                CardsTypeCreater creater = new CardsTypeCreater();
                creater.SetLaizi(laizi);
                creater.CreateAllCardsTypeArray(cardFaces);
                CardsTypeInfo typeInfo = creater.GetMaxScoreCardsTypeInfo();
                return ComputeCardFaces(typeInfo);
            }

            int laiziCount = 0;
            CardInfo[] cards = CardsTransform.Instance.CreateFormatCards(cardFaces, laizi, ref laiziCount);

            CardsTypeInfo info = new CardsTypeInfo()
            {
                cardFaceValues = CardsTransform.Instance.CreateCardFaces(cards),
                laiziCount = laiziCount,
                type = cardsType
            };

            return ComputeCardFaces(info);
        }

        /// <summary>
        /// 计算确定的牌型CardFaces
        /// </summary>
        /// <returns></returns>

        public CardFace[] ComputeCardFaces(CardsTypeInfo info)
        {
            if (info.laiziCount == 0)
                return info.cardFaceValues;

            switch (info.type)
            {
                case CardsType.WuTong:
                    {
                        CardFace[] cardFaces = new CardFace[5];

                        if (info.cardFaceValues == null || info.cardFaceValues.Length == 0)
                        {
                            for (int i = 0; i < 5; i++)
                                cardFaces[i] = CardFace.Heart_A;
                            return cardFaces;
                        }

                        int idx = 0;
                        for (int i = 0; i < info.cardFaceValues.Length; i++)
                            cardFaces[idx++] = info.cardFaceValues[i];

                        for (int i = 0; i < info.laiziCount; i++)
                        {
                            cardFaces[idx++] = cardFaces[0];
                        }

                        return cardFaces;
                    }
                case CardsType.TongHuaShun:
                    {
                        CardFace[] cardFaces = new CardFace[5];
                        if (info.cardFaceValues == null || info.cardFaceValues.Length == 0)
                        {
                            cardFaces[0] = CardFace.Heart_10;
                            cardFaces[1] = CardFace.Heart_J;
                            cardFaces[2] = CardFace.Heart_Q;
                            cardFaces[3] = CardFace.Heart_K;
                            cardFaces[4] = CardFace.Heart_A;
                            return cardFaces;
                        }

                        CardFace cardface = info.cardFaceValues[0];
                        CardInfo cardinfo = CardsTransform.Instance.CreateCardInfo(cardface);

                        if (cardinfo.value == 1)
                        {
                            for (int i = 0; i < info.cardFaceValues.Length; i++)
                            {
                                if (CardsTransform.Instance.GetValue(info.cardFaceValues[i]) >= 10)
                                {
                                    cardFaces[0] = CardsTransform.Instance.GetCardFace(10, cardinfo.suit);
                                    cardFaces[1] = CardsTransform.Instance.GetCardFace(11, cardinfo.suit);
                                    cardFaces[2] = CardsTransform.Instance.GetCardFace(12, cardinfo.suit);
                                    cardFaces[3] = CardsTransform.Instance.GetCardFace(13, cardinfo.suit);
                                    cardFaces[4] = cardface;
                                    return cardFaces;
                                }
                            }

                            cardFaces[0] = cardface;
                            cardFaces[1] = CardsTransform.Instance.GetCardFace(2, cardinfo.suit);
                            cardFaces[2] = CardsTransform.Instance.GetCardFace(3, cardinfo.suit);
                            cardFaces[3] = CardsTransform.Instance.GetCardFace(4, cardinfo.suit);
                            cardFaces[4] = CardsTransform.Instance.GetCardFace(5, cardinfo.suit);
                            return cardFaces;
                        }

                        int value = cardinfo.value;
                        if (cardinfo.value >= 10)
                        {
                            cardFaces[0] = CardsTransform.Instance.GetCardFace(10, cardinfo.suit);
                            cardFaces[1] = CardsTransform.Instance.GetCardFace(11, cardinfo.suit);
                            cardFaces[2] = CardsTransform.Instance.GetCardFace(12, cardinfo.suit);
                            cardFaces[3] = CardsTransform.Instance.GetCardFace(13, cardinfo.suit);
                            cardFaces[4] = CardsTransform.Instance.GetCardFace(1, cardinfo.suit);
                        }
                        else
                        {
                            cardFaces[0] = cardface;
                            cardFaces[1] = CardsTransform.Instance.GetCardFace(cardinfo.value + 1, cardinfo.suit);
                            cardFaces[2] = CardsTransform.Instance.GetCardFace(cardinfo.value + 2, cardinfo.suit);
                            cardFaces[3] = CardsTransform.Instance.GetCardFace(cardinfo.value + 3, cardinfo.suit);
                            cardFaces[4] = CardsTransform.Instance.GetCardFace(cardinfo.value + 4, cardinfo.suit);
                        }

                        return cardFaces;

                    }
                case CardsType.Bomb:
                    {
                        CardFace[] cardFaces = new CardFace[4];
                        if (info.cardFaceValues == null || info.cardFaceValues.Length == 0)
                        {
                            cardFaces[0] = CardFace.Heart_A;
                            cardFaces[1] = CardFace.Heart_A;
                            cardFaces[2] = CardFace.Heart_A;
                            cardFaces[3] = CardFace.Heart_A;
                            return cardFaces;
                        }

                        CardInfo[] cardinfo = CardsTransform.Instance.CreateCardInfos(info.cardFaceValues);
                        int idx;

                        cardFaces[0] = info.cardFaceValues[0];
                        for (int j = 1; j < 4; j++)
                        {
                            idx = CardsTransform.Instance.FindCard(cardinfo, cardinfo[0].value);
                            if (idx != -1) cardFaces[j] = info.cardFaceValues[idx];
                            else cardFaces[j] = CardsTransform.Instance.GetCardFace(cardinfo[0].value, 0);
                        }
                        return cardFaces;
                    }

                case CardsType.HuLu:
                    {
                        CardInfo[] cardinfos = CardsTransform.Instance.CreateCardInfos(info.cardFaceValues);
                        int a = 0;
                        int b = -1;

                        for (int i = 1; i < cardinfos.Length; i++)
                        {
                            if (cardinfos[i].value != cardinfos[0].value)
                            {
                                b = i;
                                break;
                            }
                        }

                        CardFace[] cardFaces = new CardFace[5];
                        int[] aCardIdxs = CardsTransform.Instance.FindCards(cardinfos, cardinfos[a].value);
                        int[] bCardIdxs = b > 0 ? CardsTransform.Instance.FindCards(cardinfos, cardinfos[b].value) : null;

                        if (bCardIdxs == null)
                        {
                            if (info.laiziCount >= 3)
                            {
                                cardFaces[0] = CardFace.Heart_A;
                                cardFaces[1] = CardFace.Heart_A;
                                cardFaces[2] = CardFace.Heart_A;

                                if (aCardIdxs.Length == 1)
                                {
                                    cardFaces[3] = info.cardFaceValues[aCardIdxs[0]];
                                    cardFaces[4] = cardFaces[3];
                                }
                                else
                                {
                                    cardFaces[3] = info.cardFaceValues[aCardIdxs[0]];
                                    cardFaces[4] = info.cardFaceValues[aCardIdxs[1]];
                                }
                            }
                            else
                            {
                                int idx = 0;
                                for (int i = 0; i < aCardIdxs.Length; i++)
                                    cardFaces[idx++] = info.cardFaceValues[aCardIdxs[i]];
                                for (int i = 0; i < 3 - aCardIdxs.Length; i++)
                                    cardFaces[idx++] = info.cardFaceValues[0];

                                cardFaces[3] = CardFace.Heart_A;
                                cardFaces[4] = CardFace.Heart_A;
                            }

                            return cardFaces;
                        }


                        if (aCardIdxs.Length == 3)
                        {
                            for (int i = 0; i < aCardIdxs.Length; i++)
                                cardFaces[i] = info.cardFaceValues[aCardIdxs[i]];

                            cardFaces[3] = CardsTransform.Instance.GetCardFace(cardinfos[bCardIdxs[0]].value, cardinfos[bCardIdxs[0]].suit);
                            if (bCardIdxs.Length == 1) cardFaces[4] = cardFaces[3];
                            else cardFaces[4] = CardsTransform.Instance.GetCardFace(cardinfos[bCardIdxs[1]].value, cardinfos[bCardIdxs[1]].suit);

                            return cardFaces;
                        }
                        else if (bCardIdxs.Length == 3)
                        {
                            for (int i = 0; i < aCardIdxs.Length; i++)
                                cardFaces[i] = info.cardFaceValues[bCardIdxs[i]];

                            cardFaces[3] = CardsTransform.Instance.GetCardFace(cardinfos[aCardIdxs[0]].value, cardinfos[aCardIdxs[0]].suit);
                            if (bCardIdxs.Length == 1) cardFaces[4] = cardFaces[3];
                            else cardFaces[4] = CardsTransform.Instance.GetCardFace(cardinfos[aCardIdxs[1]].value, cardinfos[aCardIdxs[1]].suit);
                            return cardFaces;
                        }
                        else
                        {
                            if (cardinfos[aCardIdxs[0]].value > cardinfos[bCardIdxs[0]].value)
                            {
                                int idx = 0;
                                for (int i = 0; i < aCardIdxs.Length; i++)
                                    cardFaces[idx++] = info.cardFaceValues[aCardIdxs[i]];
                                for (int i = 0; i < 3 - aCardIdxs.Length; i++)
                                    cardFaces[idx++] = cardFaces[0];

                                for (int i = 0; i < bCardIdxs.Length; i++)
                                    cardFaces[idx++] = info.cardFaceValues[bCardIdxs[i]];
                                for (int i = 0; i < 2 - bCardIdxs.Length; i++)
                                    cardFaces[idx++] = cardFaces[3];
                            }
                            else
                            {
                                int idx = 0;
                                for (int i = 0; i < bCardIdxs.Length; i++)
                                    cardFaces[idx++] = info.cardFaceValues[bCardIdxs[i]];
                                for (int i = 0; i < 3 - bCardIdxs.Length; i++)
                                    cardFaces[idx++] = cardFaces[0];

                                for (int i = 0; i < aCardIdxs.Length; i++)
                                    cardFaces[idx++] = info.cardFaceValues[aCardIdxs[i]];
                                for (int i = 0; i < 2 - aCardIdxs.Length; i++)
                                    cardFaces[idx++] = cardFaces[3];
                            }

                            return cardFaces;
                        }
                    }


                case CardsType.TwoDui:
                    {
                        CardInfo[] cardinfos = CardsTransform.Instance.CreateCardInfos(info.cardFaceValues);
                        int a = 0;
                        int b = -1;

                        for (int i = 1; i < cardinfos.Length; i++)
                        {
                            if (cardinfos[i].value != cardinfos[0].value)
                            {
                                b = i;
                                break;
                            }
                        }

                        CardFace[] cardFaces = new CardFace[4];

                        int[] cardIdxs = CardsTransform.Instance.FindCards(cardinfos, cardinfos[a].value);
                        cardFaces[0] = CardsTransform.Instance.GetCardFace(cardinfos[cardIdxs[0]].value, cardinfos[cardIdxs[0]].suit);
                        if (cardIdxs.Length == 1) cardFaces[1] = cardFaces[0];
                        else cardFaces[1] = CardsTransform.Instance.GetCardFace(cardinfos[cardIdxs[1]].value, cardinfos[cardIdxs[1]].suit);

                        if (b > 0)
                        {
                            cardIdxs = CardsTransform.Instance.FindCards(cardinfos, cardinfos[b].value);
                            cardFaces[2] = CardsTransform.Instance.GetCardFace(cardinfos[cardIdxs[0]].value, cardinfos[cardIdxs[0]].suit);
                            if (cardIdxs.Length == 1) cardFaces[3] = cardFaces[2];
                            else cardFaces[3] = CardsTransform.Instance.GetCardFace(cardinfos[cardIdxs[1]].value, cardinfos[cardIdxs[1]].suit);
                        }
                        else
                        {
                            cardFaces[2] = CardFace.Heart_A;
                            cardFaces[3] = CardFace.Heart_A;
                        }
                        return cardFaces;
                    }

                case CardsType.TongHua:
                    {
                        CardFace[] cardFaces = new CardFace[5];
                        if (info.cardFaceValues == null || info.cardFaceValues.Length == 0)
                        {
                            cardFaces[0] = CardFace.Heart_A;
                            cardFaces[1] = CardFace.Heart_A;
                            cardFaces[2] = CardFace.Heart_A;
                            cardFaces[3] = CardFace.Heart_A;
                            return cardFaces;
                        }

                        int idx = 0;
                        for (int i = 0; i < info.cardFaceValues.Length; i++)
                            cardFaces[idx++] = info.cardFaceValues[i];

                        for (int i = 0; i < info.laiziCount; i++)
                            cardFaces[idx++] = CardFace.Heart_A;
                        return cardFaces;
                    }

                case CardsType.SanTiao:
                    {
                        CardFace[] cardFaces = new CardFace[3];

                        if (info.cardFaceValues == null || info.cardFaceValues.Length == 0)
                        {
                            for (int i = 0; i < 3; i++)
                                cardFaces[i] = CardFace.Heart_A;
                            return cardFaces;
                        }

                        int idx = 0;
                        for (int i = 0; i < info.cardFaceValues.Length; i++)
                            cardFaces[idx++] = info.cardFaceValues[i];

                        for (int i = 0; i < info.laiziCount; i++)
                        {
                            cardFaces[idx++] = cardFaces[0];
                        }

                        return cardFaces;
                    }

                case CardsType.DuiZi:
                    {
                        CardFace[] cardFaces = new CardFace[2];

                        if (info.cardFaceValues == null || info.cardFaceValues.Length == 0)
                        {
                            for (int i = 0; i < 2; i++)
                                cardFaces[i] = CardFace.Heart_A;
                            return cardFaces;
                        }

                        int idx = 0;
                        for (int i = 0; i < info.cardFaceValues.Length; i++)
                            cardFaces[idx++] = info.cardFaceValues[i];

                        for (int i = 0; i < info.laiziCount; i++)
                        {
                            cardFaces[idx++] = cardFaces[0];
                        }

                        return cardFaces;
                    }

                case CardsType.ShunZi:
                    {
                        CardFace[] cardFaces = new CardFace[5];
                        if (info.cardFaceValues == null || info.cardFaceValues.Length == 0)
                        {
                            cardFaces[0] = CardFace.Heart_10;
                            cardFaces[1] = CardFace.Heart_J;
                            cardFaces[2] = CardFace.Heart_Q;
                            cardFaces[3] = CardFace.Heart_K;
                            cardFaces[4] = CardFace.Heart_A;
                            return cardFaces;
                        }

                        CardInfo[] cardinfo = CardsTransform.Instance.CreateCardInfos(info.cardFaceValues);
                        int idx;

                        if (cardinfo[0].value == 1)
                        {
                            for (int i = 0; i < info.cardFaceValues.Length; i++)
                            {
                                if (CardsTransform.Instance.GetValue(info.cardFaceValues[i]) >= 10)
                                {
                                    for (int j = 0; j < 4; j++)
                                    {
                                        idx = CardsTransform.Instance.FindCard(cardinfo, 10 + j);
                                        if (idx != -1) cardFaces[j] = info.cardFaceValues[idx];
                                        else cardFaces[j] = CardsTransform.Instance.GetCardFace(10 + j, 0);
                                    }

                                    cardFaces[4] = info.cardFaceValues[0];
                                    return cardFaces;
                                }
                            }

                            cardFaces[0] = info.cardFaceValues[0];
                            for (int j = 1; j < 5; j++)
                            {
                                idx = CardsTransform.Instance.FindCard(cardinfo, 1 + j);
                                if (idx != -1) cardFaces[j] = info.cardFaceValues[idx];
                                else cardFaces[j] = CardsTransform.Instance.GetCardFace(1 + j, 0);
                            }
                            return cardFaces;
                        }


                        if (cardinfo[0].value >= 10)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                idx = CardsTransform.Instance.FindCard(cardinfo, 10 + j);
                                if (idx != -1) cardFaces[j] = info.cardFaceValues[idx];
                                else cardFaces[j] = CardsTransform.Instance.GetCardFace(10 + j, 0);
                            }

                            idx = CardsTransform.Instance.FindCard(cardinfo, 1);
                            if (idx != -1) cardFaces[4] = info.cardFaceValues[idx];
                            else cardFaces[4] = CardsTransform.Instance.GetCardFace(1, 0);
                        }
                        else
                        {
                            cardFaces[0] = info.cardFaceValues[0];
                            for (int j = 1; j < 5; j++)
                            {
                                idx = CardsTransform.Instance.FindCard(cardinfo, cardinfo[0].value + j);
                                if (idx != -1) cardFaces[j] = info.cardFaceValues[idx];
                                else cardFaces[j] = CardsTransform.Instance.GetCardFace(cardinfo[0].value + j, 0);
                            }
                        }

                        return cardFaces;
                    }


            }

            return null;
        }

    }
}
