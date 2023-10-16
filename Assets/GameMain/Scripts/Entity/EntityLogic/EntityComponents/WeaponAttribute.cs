
using UnityEngine.PlayerLoop;

namespace FPS
{
    public class WeaponAttribute
    {
        public string WeaponName;
        public float Durability;
        public float Weight;
        public int Ergonomics;
        public float Precision;
        public int ShootingGallery;
        public int VerticalRecoil;
        public int HorizontalRecoil;
        public int MuzzleVelocity;
        public string FiringMode;
        public string Caliber;
        public int FiringRate;
        public int EffectiveFiringRange;

        public WeaponAttribute()
        {
            
        }

        public WeaponAttribute(WeaponData weaponData)
        {
            WeaponName = weaponData.WeaponName;
            Durability = weaponData.Durability;
            Weight = weaponData.Weight;
            Ergonomics = weaponData.Ergonomics;
            Precision = weaponData.Precision;
            ShootingGallery = weaponData.ShootingGallery;
            VerticalRecoil = weaponData.VerticalRecoil;
            HorizontalRecoil = weaponData.HorizontalRecoil;
            MuzzleVelocity = weaponData.MuzzleVelocity;
            FiringMode = weaponData.FiringMode;
            Caliber = weaponData.Caliber;
            FiringRate = weaponData.FiringRate;
            EffectiveFiringRange = weaponData.EffectiveFiringRange;
        }

        public void Init(WeaponData weaponData)
        {
            WeaponName = weaponData.WeaponName;
            Durability = weaponData.Durability;
            Weight = weaponData.Weight;
            Ergonomics = weaponData.Ergonomics;
            Precision = weaponData.Precision;
            ShootingGallery = weaponData.ShootingGallery;
            VerticalRecoil = weaponData.VerticalRecoil;
            HorizontalRecoil = weaponData.HorizontalRecoil;
            MuzzleVelocity = weaponData.MuzzleVelocity;
            FiringMode = weaponData.FiringMode;
            Caliber = weaponData.Caliber;
            FiringRate = weaponData.FiringRate;
            EffectiveFiringRange = weaponData.EffectiveFiringRange;
        }

        public void AddModToRefresh(WeaponMod mod)
        {
            Weight += mod.weaponModData.Weight;
            VerticalRecoil = (int)(VerticalRecoil*(100f + mod.weaponModData.Recoil) / 100f);
            HorizontalRecoil = (int)(HorizontalRecoil*(100f + mod.weaponModData.Recoil) / 100f);
            Ergonomics+=mod.weaponModData.Ergonomics;
            Precision += mod.weaponModData.Precision;
            MuzzleVelocity += (int)mod.weaponModData.MuzzleVelocity;
        }

        public void RemoveToRefresh(WeaponMod mod)
        {
            Weight -= mod.weaponModData.Weight;
            VerticalRecoil = (int)(VerticalRecoil/(100f + mod.weaponModData.Recoil) / 100f);
            HorizontalRecoil = (int)(HorizontalRecoil/(100f + mod.weaponModData.Recoil) / 100f);
            Ergonomics-=mod.weaponModData.Ergonomics;
            Precision -= mod.weaponModData.Precision;
            MuzzleVelocity -= (int)mod.weaponModData.MuzzleVelocity;
        }
    }
}