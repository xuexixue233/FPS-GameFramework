using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class ProcedureSelectWeapon : ProcedureBase
    {
        public override bool UseNativeDialog { get; }

        private bool _BackMenu;

        private Equipment equipment;



        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenEquipmentFormSuccess);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowItemSuccessEventArgs.EventId, OnShowItemSuccess);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, ChangeSceneSuccess);
            GameEntry.Event.Subscribe(ShowWeaponModEventArgs.EventId,ShowWeaponMod);
            GameEntry.Event.Subscribe(ShowAllSelectButtonEventArgs.EventId,ShowAllSelectButton);
            GameEntry.Event.Subscribe(HideAllSelectButtonEventArgs.EventId,HideAllSelectButton);
            GameEntry.Event.Subscribe(ShowItemModUIEventArgs.EventId,ShowItemModUI);
            GameEntry.Event.Subscribe(HideItemModUIEventArgs.EventId,HideItemModUI);
            
            _BackMenu = false;
            
            if (!GameEntry.Setting.HasSetting("PlayerSaveData"))
            {
                // GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GenerateSerialId(), 30001, 0,
                //     CampType.Unknown));
                var playerSaveData = new PlayerSaveData();
                equipment = Equipment.Create(playerSaveData);
            }
            else
            {
                var playerSaveData = GameEntry.Setting.GetObject<PlayerSaveData>("PlayerSaveData");
                equipment = Equipment.Create(playerSaveData);
                // GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GenerateSerialId(), playerSaveData.playerWeapon.weaponTypeId+1, 0,
                //     CampType.Unknown));
            }
            equipment.OnEnter();
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds,
            float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (_BackMenu)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenEquipmentFormSuccess);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowItemSuccessEventArgs.EventId, OnShowItemSuccess);
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, ChangeSceneSuccess);
            GameEntry.Event.Unsubscribe(ShowWeaponModEventArgs.EventId,ShowWeaponMod);
            GameEntry.Event.Unsubscribe(ShowAllSelectButtonEventArgs.EventId,ShowAllSelectButton);
            GameEntry.Event.Unsubscribe(HideAllSelectButtonEventArgs.EventId,HideAllSelectButton);
            GameEntry.Event.Unsubscribe(ShowItemModUIEventArgs.EventId,ShowItemModUI);
            GameEntry.Event.Unsubscribe(HideItemModUIEventArgs.EventId,HideItemModUI);
            
            equipment.OnLeave();
            ReferencePool.Release(equipment);
            equipment = null;
        }

        private void OnOpenEquipmentFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne ==null)
            {
                return;
            }
            equipment.AfterOpenEquipmentForm(ne.UIForm.Logic);
        }

        private void ChangeSceneSuccess(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            _BackMenu = true;
            
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;

            if (ne.EntityLogicType == typeof(Weapon))
            {
                equipment.ShowUI(ne.Entity.Logic);
            }
            else if (ne.EntityLogicType == typeof(WeaponMod))
            {
                equipment.AfterShowWeaponMod(ne.Entity.Logic,ne.UserData);
            }
        }

        private void OnShowItemSuccess(object sender, GameEventArgs e)
        {
            ShowItemSuccessEventArgs ne = (ShowItemSuccessEventArgs)e;
            if (ne.ItemLogicType == typeof(ItemModUI))
            {
                equipment.AfterShowItemModUI(ne.Item.Logic);
            }
            else if (ne.ItemLogicType == typeof(ItemModSelect))
            {
                equipment.AfterShowItemModSelect(ne.Item.Logic,ne.UserData);
            }
        }

        private void HideAllSelectButton(object sender,GameEventArgs e)
        {
            HideAllSelectButtonEventArgs ne = (HideAllSelectButtonEventArgs)e;
            if (ne==null)
            {
                return;
            }
            
            equipment.HideAllSelectButton();
        }

        private void ShowAllSelectButton(object sender, GameEventArgs e)
        {
            ShowAllSelectButtonEventArgs ne = (ShowAllSelectButtonEventArgs)e;

            if (ne==null)
            {
                return;
            }
            equipment.ShowAllSelectButton(ne.ShowMod,ne.ItemModUI);
        }

        private void ShowItemModUI(object sender, GameEventArgs e)
        {
            ShowItemModUIEventArgs ne = (ShowItemModUIEventArgs)e;
            
            if (ne==null)
            {
                return;
            }
            
            equipment.ShowItemModUI(ne.ShowMod);
        }

        private void HideItemModUI(object sender, GameEventArgs e)
        {
            HideItemModUIEventArgs ne = (HideItemModUIEventArgs)e;
            if (ne==null)
            {
                return; 
            }
            equipment.HideItemModUI(ne.HideMod);
        }

        private void ShowWeaponMod(object sender, GameEventArgs e)
        {
            ShowWeaponModEventArgs ne = (ShowWeaponModEventArgs)e;
            
            if (ne==null)
            {
                return;
            }
            
            equipment.ShowWeaponMod(ne.ShowMod, ne.TypeId, ne.OwnerId);
        }
    }
}