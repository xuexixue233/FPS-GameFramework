using UnityEngine;

namespace FPS
{
    public class PlayerExData : SoldierExData
    {
        public Transform cameraHolder;
        public Transform cameraTransform;
        public Scr_Models.PlayerSettingModel playerSetting;
        public LayerMask playerMask;
        public LayerMask groundMask;
    }
}