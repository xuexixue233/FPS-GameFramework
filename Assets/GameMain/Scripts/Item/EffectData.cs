﻿using System;
using GameFramework;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class EffectData : EntityData
    {
        [SerializeField]
        private float m_KeepTime = 0f;
        
        public EffectData(int entityId, int typeId) : base(entityId, typeId)
        {
            m_KeepTime = 3f;
        }
        
        public float KeepTime => m_KeepTime;
    }
}