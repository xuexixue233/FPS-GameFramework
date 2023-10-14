using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class ProcedureSelectWeapon : ProcedureBase
    {
        private EquipmentForm equipmentForm;
        private Weapon weapon;
        public override bool UseNativeDialog { get; }

        private bool _BackMenu;

        private void ReadUserData()
        {
            //Todo:读取玩家数据
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenEquipmentFormSuccess);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, ChangeSceneSuccess);

            _BackMenu = false;
            GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GenerateSerialId(), 30000, 0, CampType.Unknown));
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
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
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, ChangeSceneSuccess);
            
            GameEntry.UI.CloseAllLoadedUIForms();
            equipmentForm=null;
        }
        
        private void OnOpenEquipmentFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            equipmentForm=(EquipmentForm)ne.UIForm.Logic;
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
                weapon = (Weapon)ne.Entity.Logic;
                weapon.transform.localScale = new Vector3(100, 100, 100);
                GameEntry.UI.OpenUIForm(UIFormId.EquipmentForm,weapon);
            }
            else if (ne.EntityLogicType == typeof(WeaponMod))
            {
                var mod = (WeaponMod)ne.Entity.Logic;
                foreach (var nextMod in mod.weaponModData.NextModType)
                {
                    equipmentForm.ShowModUIItem(nextMod);
                    MathUtilities.GetRectFormEllipse(600,300,0,equipmentForm.activeModUIItemRect.ToArray());
                    equipmentForm.RefreshText();
                    equipmentForm.RefreshImage();
                }
            }
        }
    }
}