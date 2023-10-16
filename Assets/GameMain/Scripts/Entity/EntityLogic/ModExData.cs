using System.Collections.Generic;
using UnityEngine;

namespace FPS
{
    public class ModExData : MonoBehaviour
    {
        [Header("Mods TransForms")] 
        public Transform modTransform;
        public SerializableDictionary<Mod,Transform> nextModsTransforms;
        public Vector3 previewPosition;
        public Vector3 previewRotation;
    }
}