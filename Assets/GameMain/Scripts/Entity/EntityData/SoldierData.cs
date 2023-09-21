using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public abstract class SoldierData : TargetableObjectData
    {
        [SerializeField]
        private List<WeaponData> m_WeaponDatas = new List<WeaponData>();
        
        [SerializeField]
        private int m_MaxHP = 0;
        
        [SerializeField]
        private int speed = 0;
        
        [SerializeField]
        private int m_DeadEffectId = 0;

        [SerializeField]
        private int m_DeadSoundId = 0;
        
        protected SoldierData(int entityId, int typeId, CampType camp) : base(entityId, typeId, camp)
        {
            IDataTable<DRSoldier> dtAircraft = GameEntry.DataTable.GetDataTable<DRSoldier>();
            DRSoldier drAircraft = dtAircraft.GetDataRow(TypeId);
            if (drAircraft == null)
            {
                return;
            }
            
            for (int index = 0, weaponId = 0; (weaponId = drAircraft.GetWeaponIdAt(index)) > 0; index++)
            {
                AttachWeaponData(new WeaponData(GameEntry.Entity.GenerateSerialId(), weaponId, Id, Camp));
            }
            
            m_DeadEffectId = drAircraft.DeadEffectId;
            m_DeadSoundId = drAircraft.DeadSoundId;

            HP = m_MaxHP;
        }
        
        /// <summary>
        /// 最大生命。
        /// </summary>
        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }
        
        /// <summary>
        /// 速度。
        /// </summary>
        public float Speed
        {
            get
            {
                return speed;
            }
        }
        
        public int DeadEffectId
        {
            get
            {
                return m_DeadEffectId;
            }
        }

        public int DeadSoundId
        {
            get
            {
                return m_DeadSoundId;
            }
        }
        
        public List<WeaponData> GetAllWeaponDatas()
        {
            return m_WeaponDatas;
        }
        
        public void AttachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            if (m_WeaponDatas.Contains(weaponData))
            {
                return;
            }

            m_WeaponDatas.Add(weaponData);
        }

        public void DetachWeaponData(WeaponData weaponData)
        {
            if (weaponData == null)
            {
                return;
            }

            m_WeaponDatas.Remove(weaponData);
        }
        
        private void RefreshData()
        {
            m_MaxHP = 100;

            if (HP > m_MaxHP)
            {
                HP = m_MaxHP;
            }
        }
    }
}