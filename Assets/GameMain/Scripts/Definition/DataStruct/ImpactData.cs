//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Runtime.InteropServices;

namespace FPS
{
    [StructLayout(LayoutKind.Auto)]
    public struct ImpactData
    {
        private readonly CampType m_Camp;
        private readonly int m_HP;


        public ImpactData(CampType camp, int hp)
        {
            m_Camp = camp;
            m_HP = hp;

        }

        public CampType Camp
        {
            get
            {
                return m_Camp;
            }
        }

        public int HP
        {
            get
            {
                return m_HP;
            }
        }
    }
}
