//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace FPS
{
    /// <summary>
    /// 阵营类型。
    /// </summary>
    public enum CampType : byte
    {
        Unknown = 0,

        /// <summary>
        /// 玩家阵营。
        /// </summary>
        Player,

        /// <summary>
        /// 敌人阵营。
        /// </summary>
        Enemy,

        /// <summary>
        /// 中立阵营。
        /// </summary>
        Neutral,
        
    }
}
