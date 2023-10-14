using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public enum Mod : int
    {
        Barrel = 40001,
        Charge=40002,
        GasBlock=40003,
        HandGuard=40004,
        Mag=40005,
        Muzzle=40006,
        PistolGrip=40007,
        Receiver=40008,
        Scope=40009,
        Silencer=40010,
        Stock=40012,
        StockLast=40011
    }
    
    public class WeaponMod: Entity
    {
        [SerializeField]
        public WeaponModData weaponModData;
        public ModExData modExData;

        public Weapon weapon;
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            modExData = GetComponent<ModExData>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            weaponModData = userData as WeaponModData;
            if (weaponModData==null)
            {
                Log.Error("WeaponMod Data is invalid");
                return;
            }
            
            weapon=GameEntry.Entity.GetGameEntity(weaponModData.OwnerId) as Weapon;

            if (weapon==null)
            {
                return;
            }

            weapon.m_WeaponExData.nextModsTransforms.TryGetValue(weaponModData.ModType, out var spawnTransform);
            
            GameEntry.Entity.AttachEntity(Entity,weaponModData.OwnerId,spawnTransform);

            transform.localPosition=Vector3.zero;
            transform.localRotation = new Quaternion(0,0,0,0);
        }
        

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }
    }
}