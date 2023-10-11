using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class WeaponExData : MonoBehaviour
    {
        [Header("Weapon Sway")]
        public Transform weaponSwayTransform;
        public float swayAmountA=1;
        public float swayAmountB=2;
        public float swayScale = 600;
        public float swayLerpSpeed = 14;
        
        [Header("Weapon Recoil")]
        public float recoilX;
        public float recoilY;
        public float recoilZ;

        public float kickBackZ;
        public float snappiness;
        public float returnAmount;

        public Transform sightTarget;
        public float sightOffset;
        public float aimingInTime;
        public Transform CameraTransform;
        public Scr_Models.WeaponSettingsModel settings;
    }
}