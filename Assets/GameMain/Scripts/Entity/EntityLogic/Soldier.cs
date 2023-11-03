using System.Collections.Generic;
using FPSFramework;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public abstract class Soldier : TargetableObject
    {
        [HideInInspector]
        public SoldierExData soldierExData;

        [SerializeField] 
        private SoldierData m_SoldierData = null;

        protected Vector3 newSoldierRotation;

        [SerializeField] protected List<Weapon> m_Weapons = new List<Weapon>();
        public Weapon showedWeapon;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            soldierExData = GetComponent<SoldierExData>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_SoldierData = userData as SoldierData;

            Name = Utility.Text.Format("Soldier ({0})", Id);

            List<WeaponData> weaponDatas = m_SoldierData.GetAllWeaponDatas();

            if (!CompareTag("Enemy"))
            {
                GameEntry.Entity.ShowWeapon(weaponDatas[0]);
            }

        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            if (childEntity is Weapon)
            {
                m_Weapons.Add((Weapon)childEntity);
                return;
            }
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
            if (childEntity is Weapon)
            {
                m_Weapons.Remove((Weapon)childEntity);
                return;
            }
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_SoldierData.Camp, m_SoldierData.HP);
        }
    }
}