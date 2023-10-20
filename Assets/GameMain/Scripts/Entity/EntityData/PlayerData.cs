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
            if (GameEntry.Setting.HasSetting("PlayerSaveData"))
            {
                var playerSaveData = GameEntry.Setting.GetObject<PlayerSaveData>("PlayerSaveData");
                var weaponData = new WeaponData(GameEntry.Entity.GenerateSerialId(), playerSaveData.playerWeapon.weaponTypeId, Id, Camp);
                weaponData.weaponModId=playerSaveData.playerWeapon.modTypeIdDictionary;
                AttachWeaponData(weaponData);
            }
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