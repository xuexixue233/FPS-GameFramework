﻿using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace FPS
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;
        
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;
        public PlayerSaveData playerSaveData;
        public PlayerForm playerForm;
        public Player player;
        public bool GameOver;

        public override bool UseNativeDialog => false;

        public void GotoMenu()
        {
            m_GotoMenu = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            playerSaveData=GameEntry.Setting.GetObject<PlayerSaveData>("PlayerSaveData");
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId,OnOpenUIFormSuccess);
            
            m_GotoMenu = false;
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "Player",
                Position = new Vector3(0,10,2)
            });
            GameOver = false;
            player = null;
            GameEntry.UI.OpenUIForm(UIFormId.PlayerForm);
        }
        

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!GameOver)
            {
                if (player != null && player.IsDead)
                {
                    GameOver = true;
                    return;
                }
                return;
            }

            if (!m_GotoMenu)
            {
                m_GotoMenu = true;
                m_GotoMenuDelaySeconds = 0;
            }

            m_GotoMenuDelaySeconds += elapseSeconds;
            if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId,OnOpenUIFormSuccess);
            base.OnLeave(procedureOwner, isShutdown);
        }
        
        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Player))
            {
                player = (Player)ne.Entity.Logic;
                
                GameEntry.UI.OpenUIForm(UIFormId.PlayerForm);
            }
            else if (ne.EntityLogicType==typeof(Weapon))
            {
                var weapon = (Weapon)ne.Entity.Logic;
                
            }
            else if (ne.EntityLogicType==typeof(WeaponMod))
            {
                var weaponMod = (WeaponMod)ne.Entity.Logic;
                
            }
        }
        
        protected virtual void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UIForm.Logic is PlayerForm _playerForm)
            {
                playerForm = _playerForm;
                playerForm.procedureMain = this;
                return;
            }
        }
    }
}