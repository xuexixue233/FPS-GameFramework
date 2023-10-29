//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-10-29 16:52:16.097
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    /// <summary>
    /// 士兵表。
    /// </summary>
    public class DRSoldier : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取士兵编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取武器编号0。
        /// </summary>
        public int WeaponId0
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器编号1。
        /// </summary>
        public int WeaponId1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取武器编号2。
        /// </summary>
        public int WeaponId2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡特效编号。
        /// </summary>
        public int DeadEffectId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取死亡声音编号。
        /// </summary>
        public int DeadSoundId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取行走前进速度。
        /// </summary>
        public float WalkingForwardSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取行走后退速度。
        /// </summary>
        public float WalkingBackSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取行走平移速度。
        /// </summary>
        public float WalkingStrafeSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取奔跑前进速度。
        /// </summary>
        public float RunningForwardSpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取奔跑平移速度。
        /// </summary>
        public float RunningStrafeSpeed
        {
            get;
            private set;
        }

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnStrings.Length; i++)
            {
                columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnStrings[index++]);
            index++;
            WeaponId0 = int.Parse(columnStrings[index++]);
            WeaponId1 = int.Parse(columnStrings[index++]);
            WeaponId2 = int.Parse(columnStrings[index++]);
            DeadEffectId = int.Parse(columnStrings[index++]);
            DeadSoundId = int.Parse(columnStrings[index++]);
            WalkingForwardSpeed = float.Parse(columnStrings[index++]);
            WalkingBackSpeed = float.Parse(columnStrings[index++]);
            WalkingStrafeSpeed = float.Parse(columnStrings[index++]);
            RunningForwardSpeed = float.Parse(columnStrings[index++]);
            RunningStrafeSpeed = float.Parse(columnStrings[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            using (MemoryStream memoryStream = new MemoryStream(dataRowBytes, startIndex, length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.Read7BitEncodedInt32();
                    WeaponId0 = binaryReader.Read7BitEncodedInt32();
                    WeaponId1 = binaryReader.Read7BitEncodedInt32();
                    WeaponId2 = binaryReader.Read7BitEncodedInt32();
                    DeadEffectId = binaryReader.Read7BitEncodedInt32();
                    DeadSoundId = binaryReader.Read7BitEncodedInt32();
                    WalkingForwardSpeed = binaryReader.ReadSingle();
                    WalkingBackSpeed = binaryReader.ReadSingle();
                    WalkingStrafeSpeed = binaryReader.ReadSingle();
                    RunningForwardSpeed = binaryReader.ReadSingle();
                    RunningStrafeSpeed = binaryReader.ReadSingle();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, int>[] m_WeaponId = null;

        public int WeaponIdCount
        {
            get
            {
                return m_WeaponId.Length;
            }
        }

        public int GetWeaponId(int id)
        {
            foreach (KeyValuePair<int, int> i in m_WeaponId)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetWeaponId with invalid id '{0}'.", id));
        }

        public int GetWeaponIdAt(int index)
        {
            if (index < 0 || index >= m_WeaponId.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetWeaponIdAt with invalid index '{0}'.", index));
            }

            return m_WeaponId[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_WeaponId = new KeyValuePair<int, int>[]
            {
                new KeyValuePair<int, int>(0, WeaponId0),
                new KeyValuePair<int, int>(1, WeaponId1),
                new KeyValuePair<int, int>(2, WeaponId2),
            };
        }
    }
}
