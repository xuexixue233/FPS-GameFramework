﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static UserDataComponent UserData
        {
            get;
            private set;
        }

        public static ItemComponent Item
        {
            get;
            private set;
        }
        
        private static void InitCustomComponents()
        {
            UserData=UnityGameFramework.Runtime.GameEntry.GetComponent<UserDataComponent>();
            Item=UnityGameFramework.Runtime.GameEntry.GetComponent<ItemComponent>();
        }
    }
}
