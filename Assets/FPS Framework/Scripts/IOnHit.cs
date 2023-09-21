using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSFramework
{
    /// <summary>
    /// contais OnHit() which gets called on hit from any firearm (Projectile or Hitscan)
    /// </summary>
    public interface IOnHit
    {
        void OnHit(HitInfo info);
    }
}