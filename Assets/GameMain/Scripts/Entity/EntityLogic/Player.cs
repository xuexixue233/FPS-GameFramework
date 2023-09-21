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
        
        private FirstPersonController m_FirstPersonController;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_FirstPersonController = GetComponentInChildren<FirstPersonController>();
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

            StartCoroutine(WeaponStart());

            m_PlayerData.HP = 100;
        }

        public IEnumerator WeaponStart()
        {
            yield return new WaitForSecondsRealtime(1);
            m_Weapons[0].inventory.Controller = m_FirstPersonController;
            m_Weapons[0].inventory.OnStart();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

        }
    }
}