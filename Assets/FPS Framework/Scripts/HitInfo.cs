using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSFramework
{
    /// <summary>
    /// base class for collision in (Projectile or Hitscan)
    /// </summary>
    public class HitInfo
    {
        /// <summary>
        /// The projectile which did hit the collider
        /// </summary>
        public Projectile projectile { get; set; }

        /// <summary>
        /// The raycast hit of the Projectile or Hitscan(Soon)
        /// </summary>
        public RaycastHit raycastHit { get; set; }

        /// <summary>
        /// The position which the collider hit happened
        /// </summary>
        public Vector3 position { get; set; }

        /// <summary>
        /// The rotation which the collider hit happened
        /// </summary>
        public Quaternion rotation { get; set; }

        /// <summary>
        /// The rotation which the collider hit happened as Euler angles in degrees
        /// </summary>
        public Vector3 eulerAngles { get; set; }

        /// <summary>
        /// The amount of damage damagable has received
        /// </summary>
        public float damageReceived { get; set; }

        public HitInfo()
        {

        }

        public HitInfo(Projectile projectile, Vector3 position, Quaternion rotation, Vector3 eulerAngles, RaycastHit hit)
        {
            this.projectile = projectile;
            this.position = position;
            this.rotation = rotation;
            this.eulerAngles = eulerAngles;
            this.raycastHit = hit;
        }
    }
}