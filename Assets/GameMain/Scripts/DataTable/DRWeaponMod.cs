//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-10-11 23:39:59.102
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
    /// 武器配件表。
    /// </summary>
    public class DRWeaponMod : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取配件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取配件名称。
        /// </summary>
        public string ModName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取重量。
        /// </summary>
        public float Weight
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取后坐力。
        /// </summary>
        public int Recoil
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取人机工效。
        /// </summary>
        public int Ergonomics
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取精准度。
        /// </summary>
        public float Precision
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取枪口初速。
        /// </summary>
        public float MuzzleVelocity
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
            ModName = columnStrings[index++];
            Weight = float.Parse(columnStrings[index++]);
            Recoil = int.Parse(columnStrings[index++]);
            Ergonomics = int.Parse(columnStrings[index++]);
            Precision = float.Parse(columnStrings[index++]);
            MuzzleVelocity = float.Parse(columnStrings[index++]);

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
                    ModName = binaryReader.ReadString();
                    Weight = binaryReader.ReadSingle();
                    Recoil = binaryReader.Read7BitEncodedInt32();
                    Ergonomics = binaryReader.Read7BitEncodedInt32();
                    Precision = binaryReader.ReadSingle();
                    MuzzleVelocity = binaryReader.ReadSingle();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
