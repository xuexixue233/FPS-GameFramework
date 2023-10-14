using System;
using UnityEngine;

namespace FPS
{
    public static class Scr_Models
    {
        #region -Player-

        public enum PlayerStance
        {
            Stand,
            Crouch,
            Prone
        }

        [Serializable]
        public class PlayerSettingModel
        {
            [Header("View Setting")] 
            public float ViewXSensitivity;
            public float ViewYSensitivity;

            public float AimingSensitivityEffector;

            public bool ViewXInverted;
            public bool ViewYInverted;

            [Header("Movement Setting")] 
            public bool SprintingHold;
            public float MovementSmoothing;

            [Header("Jumping")] 
            public float JumpingHeight;
            public float JumpingFalloff;
            public float FallingSmoothing;

            [Header("Speed Effectors")] 
            public float SpeedEffector = 1;
            public float CrouchSpeedEffector;
            public float ProneSpeedEffector;
            public float FallingSpeedEffector;
            public float AimingSpeedEffector;

            [Header("Is Grounded / Falling")] 
            public float isGroundedRadius;
            public float isFallingSpeed;
        }
        
        [Serializable]
        public class CharacterStance
        {
            public float CameraHeight;
            public float CharacterHeight;
            public Vector3 CharacterCenter;
        }

        #endregion

        #region -Weapon-
        
        [Serializable]
        public class WeaponSettingsModel
        {
            [Header("Weapon Sway")] 
            public float SwayAmount;
            public bool SwayYInverted;
            public bool SwayXInverted;
            public float SwaySmoothing;
            public float SwayResetSmoothing;
            public float SwayClampX;
            public float SwayClampY;

            [Header("Weapon Movement Sway")]
            public float MovementSwayX;
            public float MovementSwayY;
            public bool MovementSwayXInverted;
            public bool MovementSwayYInverted;
            public float MovementSwaySmoothing;
        }

        #endregion
    }
}
