using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public abstract class TargetableObject : Entity
    {
        [SerializeField]
        private TargetableObjectData m_TargetableObjectData = null;
        
        public abstract ImpactData GetImpactData();
        
        public bool IsDead
        {
            get
            {
                return m_TargetableObjectData.HP <= 0;
            }
        }
        public virtual void ApplyDamage(Entity attacker, int damageHP)
        {
            m_TargetableObjectData.HP -= damageHP;
            if (m_TargetableObjectData.HP<=0)
            {
                OnDead(attacker);
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            gameObject.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_TargetableObjectData = userData as TargetableObjectData;
            if (m_TargetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }

        protected virtual void OnDead(Entity attacker)
        {
            GameEntry.Entity.HideEntity(this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}