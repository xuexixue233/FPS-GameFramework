//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2023-10-14 20:58:49.240
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
    /// 武器表。
    /// </summary>
    public class DRWeapon : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取武器编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取武器名字。
        /// </summary>
        public string WeaponName
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
        /// 获取射击场。
        /// </summary>
        public int ShootingGallery
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取垂直后坐力。
        /// </summary>
        public int VerticalRecoil
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取水平后座力。
        /// </summary>
        public int HorizontalRecoil
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取膛口初速。
        /// </summary>
        public int MuzzleVelocity
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取开火模式。
        /// </summary>
        public string FiringMode
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取口径。
        /// </summary>
        public string Caliber
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取射速。
        /// </summary>
        public int FiringRate
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取有效射程。
        /// </summary>
        public int EffectiveFiringRange
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下一个配件类型1。
        /// </summary>
        public string NextModType1
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下一个配件类型2。
        /// </summary>
        public string NextModType2
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下一个配件类型3。
        /// </summary>
        public string NextModType3
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下一个配件类型4。
        /// </summary>
        public string NextModType4
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下一个配件类型5。
        /// </summary>
        public string NextModType5
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
            WeaponName = columnStrings[index++];
            Weight = float.Parse(columnStrings[index++]);
            Ergonomics = int.Parse(columnStrings[index++]);
            Precision = float.Parse(columnStrings[index++]);
            ShootingGallery = int.Parse(columnStrings[index++]);
            VerticalRecoil = int.Parse(columnStrings[index++]);
            HorizontalRecoil = int.Parse(columnStrings[index++]);
            MuzzleVelocity = int.Parse(columnStrings[index++]);
            FiringMode = columnStrings[index++];
            Caliber = columnStrings[index++];
            FiringRate = int.Parse(columnStrings[index++]);
            EffectiveFiringRange = int.Parse(columnStrings[index++]);
            NextModType1 = columnStrings[index++];
            NextModType2 = columnStrings[index++];
            NextModType3 = columnStrings[index++];
            NextModType4 = columnStrings[index++];
            NextModType5 = columnStrings[index++];

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
                    WeaponName = binaryReader.ReadString();
                    Weight = binaryReader.ReadSingle();
                    Ergonomics = binaryReader.Read7BitEncodedInt32();
                    Precision = binaryReader.ReadSingle();
                    ShootingGallery = binaryReader.Read7BitEncodedInt32();
                    VerticalRecoil = binaryReader.Read7BitEncodedInt32();
                    HorizontalRecoil = binaryReader.Read7BitEncodedInt32();
                    MuzzleVelocity = binaryReader.Read7BitEncodedInt32();
                    FiringMode = binaryReader.ReadString();
                    Caliber = binaryReader.ReadString();
                    FiringRate = binaryReader.Read7BitEncodedInt32();
                    EffectiveFiringRange = binaryReader.Read7BitEncodedInt32();
                    NextModType1 = binaryReader.ReadString();
                    NextModType2 = binaryReader.ReadString();
                    NextModType3 = binaryReader.ReadString();
                    NextModType4 = binaryReader.ReadString();
                    NextModType5 = binaryReader.ReadString();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        private KeyValuePair<int, string>[] m_NextModType = null;

        public int NextModTypeCount
        {
            get
            {
                return m_NextModType.Length;
            }
        }

        public string GetNextModType(int id)
        {
            foreach (KeyValuePair<int, string> i in m_NextModType)
            {
                if (i.Key == id)
                {
                    return i.Value;
                }
            }

            throw new GameFrameworkException(Utility.Text.Format("GetNextModType with invalid id '{0}'.", id));
        }

        public string GetNextModTypeAt(int index)
        {
            if (index < 0 || index >= m_NextModType.Length)
            {
                throw new GameFrameworkException(Utility.Text.Format("GetNextModTypeAt with invalid index '{0}'.", index));
            }

            return m_NextModType[index].Value;
        }

        private void GeneratePropertyArray()
        {
            m_NextModType = new KeyValuePair<int, string>[]
            {
                new KeyValuePair<int, string>(1, NextModType1),
                new KeyValuePair<int, string>(2, NextModType2),
                new KeyValuePair<int, string>(3, NextModType3),
                new KeyValuePair<int, string>(4, NextModType4),
                new KeyValuePair<int, string>(5, NextModType5),
            };
        }
    }
}
