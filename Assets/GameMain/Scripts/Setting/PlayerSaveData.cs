using System;
using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    
    [Serializable]
    public class PlayerWeapon
    {
        [SerializeField]
        public int weaponTypeId;
        [SerializeField]
        public SerializableDictionary<Mod,int> modTypeIdDictionary=new SerializableDictionary<Mod, int>();

        public PlayerWeapon()
        {
            
        }
    }
    
    [Serializable]
    public class PlayerSaveData
    {
        [SerializeField]
        public PlayerWeapon playerWeapon=new PlayerWeapon();

        public PlayerSaveData()
        {
            
        }
    }
}