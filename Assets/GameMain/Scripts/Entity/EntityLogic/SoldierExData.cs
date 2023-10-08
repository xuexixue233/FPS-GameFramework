using UnityEngine;

namespace FPS
{
    public class SoldierExData : MonoBehaviour
    {
        public Transform WeaponTransform;
        public Transform DropLocation;
        public Transform soldierTransform;
        public Transform feetTransform;
        public Transform leanPivot;
        public float leanAngle;
        public float leanSmoothing;
        public Scr_Models.PlayerStance playerStance;
        public float playerStanceSmoothing;
        public Scr_Models.CharacterStance standStance;
        public Scr_Models.CharacterStance crouchStance;
        public Scr_Models.CharacterStance proneStance;
    }
}