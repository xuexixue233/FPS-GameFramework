using System.Collections;
using FPSFramework;
using Unity.VisualScripting;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class Player : Soldier
    {
        [SerializeField] private PlayerData m_PlayerData = null;

        public PlayerCamera playerCamera;
        public PlayerMovement playerMovement;
        public PlayerCameraShaker playerCameraShaker;
        private static readonly int Ammo = Animator.StringToHash("Ammo");
        public float cameraShakeRoughness = 7;
        public float cameraShakeFadeInTime = 0.2f;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            playerCamera = GetComponentInChildren<PlayerCamera>();
            playerMovement = GetComponentInChildren<PlayerMovement>();
            playerCameraShaker = GetComponentInChildren<PlayerCameraShaker>();
            playerMovement.player = this;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_PlayerData = userData as PlayerData;
            if (m_PlayerData == null)
            {
                Log.Error("m_PlayerData is invalid.");
                return;
            }

            playerCamera.OnStart();
            playerMovement.OnStart();
            m_PlayerData.HP = 100;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            playerCamera.OnUpdate();
            playerMovement.OnUpdate();
            if (Input.GetMouseButtonDown(0))
            {
                playerCameraShaker.Shake(1, cameraShakeRoughness, cameraShakeFadeInTime, 1);
            }
        }
    }
}