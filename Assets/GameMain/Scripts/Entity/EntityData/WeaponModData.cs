using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using UnityEngine;

namespace FPS
{
    [Serializable]
    public class WeaponModData : AccessoryObjectData
    {
        [SerializeField] private string _ModName;

        [SerializeField] private float _Weight;

        [SerializeField] private int _Recoil;

        [SerializeField] private int _Ergonomics;

        [SerializeField] private float _Precision;

        [SerializeField] private float _MuzzleVelocity;

        [SerializeField] private Mod _ModType;

        [SerializeField] private List<Mod> _NextModType;

        public WeaponModData(int entityId, int typeId, int ownerId, CampType ownerCamp) : base(entityId, typeId,
            ownerId, ownerCamp)
        {
            IDataTable<DRWeaponMod> dtWeaponMod = GameEntry.DataTable.GetDataTable<DRWeaponMod>();
            DRWeaponMod drWeaponMod = dtWeaponMod.GetDataRow(TypeId);
            if (drWeaponMod == null)
            {
                return;
            }

            _ModName = drWeaponMod.ModName;
            _Weight = drWeaponMod.Weight;
            _Recoil = drWeaponMod.Recoil;
            _Ergonomics = drWeaponMod.Ergonomics;
            _Precision = drWeaponMod.Precision;
            _MuzzleVelocity = drWeaponMod.MuzzleVelocity;
            _ModType = (Mod)Enum.Parse(typeof(Mod), drWeaponMod.ModType);
            _NextModType = new List<Mod>();
            var modName = "";
            for (var index = 0; index < 3 && (modName = drWeaponMod.GetNextModTypeAt(index)) != "None"; index++)
            {
                var modType = (Mod)Enum.Parse(typeof(Mod), modName);
                _NextModType.Add(modType);
            }
        }

        public string ModName => _ModName;

        public float Weight => _Weight;

        public int Recoil => _Recoil;

        public int Ergonomics => _Ergonomics;

        public float Precision => _Precision;

        public float MuzzleVelocity => _MuzzleVelocity;

        public Mod ModType => _ModType;

        public List<Mod> NextModType => _NextModType;
    }
}