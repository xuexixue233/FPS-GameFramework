using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public enum Mod
    {
        Barrel,
        Charge,
        GasBlock,
        HandGuard,
        Mag,
        Muzzle,
        PistolGrip,
        Reciever,
        Scope,
        Silencer,
        Stock,
        StockLast
    }
    
    public class WeaponMod: Entity
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
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