
using System.Collections.Generic;

namespace CardRuleNS
{
    #region 枚举数据
    /// <summary>
    /// 牌型类型
    /// </summary>
    public enum CardsType
    {
        /// <summary>
        /// 不存在
        /// </summary>
        None,

        /// <summary>
        /// 单张
        /// </summary>
        Single,

        /// <summary>
        /// 对子
        /// </summary>
        DuiZi,

        /// <summary>
        /// 两对
        /// </summary>
        TwoDui,

        /// <summary>
        /// 三条
        /// </summary>
        SanTiao,

        /// <summary>
        /// 顺子
        /// </summary>
        ShunZi,

        /// <summary>
        /// 同花
        /// </summary>
        TongHua,

        /// <summary>
        /// 葫芦
        /// </summary>
        HuLu,

        /// <summary>
        /// 炸弹
        /// </summary>
        Bomb,

        /// <summary>
        /// 同花顺
        /// </summary>
        TongHuaShun,

        /// <summary>
        /// 五同
        /// </summary>
        WuTong,

    }


    public enum SpecCardsType
    {
        /// <summary>
        /// 普通牌型
        /// </summary>       
        Normal = 0,

        /// <summary>
        /// 至尊一条龙
        /// </summary>
        ZhiZunQinLong,

        /// <summary>
        /// 一条龙
        /// </summary>
        YiTiaoLong,

        /// <summary>
        /// 六对半
        /// </summary>
        LiuDuiBan,

        /// <summary>
        /// 三顺子
        /// </summary>
        SanShunZi,

        /// <summary>
        /// 三同花
        /// </summary>
        SanTongHua,

        /// <summary>
        /// 至尊雷
        /// </summary>
        ZhiZunLei,
      
        /// <summary>
        /// 八炸
        /// </summary>
        EightBomb,

        /// <summary>
        /// 七炸
        /// </summary>
        SevenBomb,

        /// <summary>
        /// 六炸
        /// </summary>
        SixBomb,

        /// <summary>
        /// 四套三条
        /// </summary>
        SiTaoSan,
    }

    // <summary>
    /// 牌面
    /// </summary>
    public enum CardFace
    {
        /// <summary>
        /// 红方块A
        /// </summary>
        Diamond_A = 0,

        /// <summary>
        /// 红方块2
        /// </summary>
        Diamond_2,

        /// <summary>
        /// 红方块3
        /// </summary>
        Diamond_3,

        /// <summary>
        /// 红方块4
        /// </summary>
        Diamond_4,

        /// <summary>
        /// 红方块5
        /// </summary>
        Diamond_5,

        /// <summary>
        /// 红方块6
        /// </summary>
        Diamond_6,

        /// <summary>
        /// 红方块7
        /// </summary>
        Diamond_7,

        /// <summary>
        /// 红方块8
        /// </summary>
        Diamond_8,

        /// <summary>
        /// 红方块9
        /// </summary>
        Diamond_9,

        /// <summary>
        /// 红方块10
        /// </summary>
        Diamond_10,

        /// <summary>
        /// 红方块J
        /// </summary>
        Diamond_J,

        /// <summary>
        /// 红方块Q
        /// </summary>
        Diamond_Q,

        /// <summary>
        /// 红方块K
        /// </summary>
        Diamond_K,

        /// <summary>
        /// 黑梅花A
        /// </summary>
        Club_A,

        /// <summary>
        /// 黑梅花2
        /// </summary>
        Club_2,

        /// <summary>
        /// 黑梅花3
        /// </summary>
        Club_3,

        /// <summary>
        /// 黑梅花4
        /// </summary>
        Club_4,

        /// <summary>
        /// 黑梅花5
        /// </summary>
        Club_5,

        /// <summary>
        /// 黑梅花6
        /// </summary>
        Club_6,

        /// <summary>
        /// 黑梅花7
        /// </summary>
        Club_7,

        /// <summary>
        /// 黑梅花8
        /// </summary>
        Club_8,

        /// <summary>
        /// 黑梅花9
        /// </summary>
        Club_9,

        /// <summary>
        /// 黑梅花10
        /// </summary>
        Club_10,

        /// <summary>
        /// 黑梅花J
        /// </summary>
        Club_J,

        /// <summary>
        /// 黑梅花Q
        /// </summary>
        Club_Q,

        /// <summary>
        /// 黑梅花K
        /// </summary>
        Club_K,

        /// <summary>
        /// 红心A
        /// </summary>
        Heart_A,

        /// <summary>
        /// 红心2
        /// </summary>
        Heart_2,

        /// <summary>
        /// 红心3
        /// </summary>
        Heart_3,

        /// <summary>
        /// 红心4
        /// </summary>
        Heart_4,

        /// <summary>
        /// 红心5
        /// </summary>
        Heart_5,

        /// <summary>
        /// 红心6
        /// </summary>
        Heart_6,

        /// <summary>
        /// 红心7
        /// </summary>
        Heart_7,

        /// <summary>
        /// 红心8
        /// </summary>
        Heart_8,

        /// <summary>
        /// 红心9
        /// </summary>
        Heart_9,

        /// <summary>
        /// 红心10
        /// </summary>
        Heart_10,

        /// <summary>
        /// 红心J
        /// </summary>
        Heart_J,

        /// <summary>
        /// 红心Q
        /// </summary>
        Heart_Q,

        /// <summary>
        /// 红心K
        /// </summary>
        Heart_K,


        /// <summary>
        /// 黑桃A
        /// </summary>
        Spade_A,

        /// <summary>
        /// 黑桃2
        /// </summary>
        Spade_2,

        /// <summary>
        /// 黑桃3
        /// </summary>
        Spade_3,

        /// <summary>
        /// 黑桃4
        /// </summary>
        Spade_4,

        /// <summary>
        /// 黑桃5
        /// </summary>
        Spade_5,

        /// <summary>
        /// 黑桃6
        /// </summary>
        Spade_6,

        /// <summary>
        /// 黑桃7
        /// </summary>
        Spade_7,

        /// <summary>
        /// 黑桃8
        /// </summary>
        Spade_8,

        /// <summary>
        /// 黑桃9
        /// </summary>
        Spade_9,

        /// <summary>
        /// 黑桃10
        /// </summary>
        Spade_10,

        /// <summary>
        /// 黑桃J
        /// </summary>
        Spade_J,

        /// <summary>
        /// 黑桃Q
        /// </summary>
        Spade_Q,

        /// <summary>
        /// 黑桃K
        /// </summary>
        Spade_K,


        /// <summary>
        /// 赖子
        /// </summary>
        Laizi,


        /// <summary>
        /// 数量
        /// </summary>
        Count
    }

    #endregion

    public struct CardInfo
    {
        public int value;
        public int suit;
    }

    /// <summary>
    /// 牌型数据
    /// </summary>
    public struct CardsTypeInfo
    {
        /// <summary>
        /// 牌型
        /// </summary>
        public CardsType CardsTypeType;

        /// <summary>
        /// 牌型中所有牌的面值组合
        /// </summary>
        public CardFace[] cardFaceValues;

        /// <summary>
        /// 需要的赖子数量
        /// </summary>
        public int laiziCount;
    }

}
