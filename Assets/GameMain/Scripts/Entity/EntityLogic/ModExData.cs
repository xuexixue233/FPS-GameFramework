using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class ModExData : MonoBehaviour
    {
        [Header("Mods TransForms")]
        public SerializableDictionary<Mod,Transform> nextModsTransforms;
    }
}