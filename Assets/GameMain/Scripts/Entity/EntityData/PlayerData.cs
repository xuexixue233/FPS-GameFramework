using System;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class PlayerData : SoldierData
    {
        [SerializeField]
        private string m_Name = null;
        
        public PlayerData(int entityId, int typeId) : base(entityId, typeId, CampType.Player)
        {
            GameEntry.Setting.GetObject<WeaponData>("PlayerWeaponData");
        }
        
        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get => m_Name;
            set => m_Name = value;
        }
    }
}