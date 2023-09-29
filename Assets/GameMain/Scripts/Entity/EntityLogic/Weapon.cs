using FPSFramework;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class Weapon : Entity
    {
        private const string AttachPoint = "Weapon Point";
        
        [SerializeField]
        private WeaponData m_WeaponData = null;
        private WeaponExData m_WeaponExData;
        public Animator weaponAnimator;
        public int bulletNum;
        public int leftBulletNum;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_WeaponExData = GetComponent<WeaponExData>();
            weaponAnimator = GetComponent<Animator>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_WeaponData = userData as WeaponData;
            if (m_WeaponData == null)
            {
                Log.Error("Weapon data is invalid.");
                return;
            }

            var parent = GameEntry.Entity.GetGameEntity(m_WeaponData.OwnerId) as Soldier;

            if (parent==null)
            {
                return;
            }

            GameEntry.Entity.AttachEntity(Entity,m_WeaponData.OwnerId, parent.soldierExData.WeaponTransform);
            parent.showedWeapon = this;
            transform.transform.localPosition = m_WeaponExData.CameraTransform.localPosition*-1;
            transform.localScale=Vector3.one;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            
            Name = Utility.Text.Format("Weapon of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }
        public void TryAttack()
        {
            
        }
    }
}