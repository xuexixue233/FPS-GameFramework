using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace FPS
{
    public class ModUIItemObject : ObjectBase
    {
        public static ModUIItemObject Create(object target)
        {
            ModUIItemObject hpBarItemObject = ReferencePool.Acquire<ModUIItemObject>();
            hpBarItemObject.Initialize(target);
            return hpBarItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            ModUIItem hpBarItem = (ModUIItem)Target;
            if (hpBarItem == null)
            {
                return;
            }

            Object.Destroy(hpBarItem.gameObject);
        }
    }
}