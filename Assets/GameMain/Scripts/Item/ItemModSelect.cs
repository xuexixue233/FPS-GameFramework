using System;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class ItemModSelect : ItemLogic
    {
        private EquipmentForm _equipmentForm;
        public int modId;
        public Mod mod;
        public Button selectButton;
        public RawImage modImage;
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            selectButton.onClick.RemoveAllListeners();
            selectButton.onClick.AddListener((() =>
            {
                GameEntry.Entity.ShowWeaponMod(new WeaponModData(GameEntry.Entity.GenerateSerialId(),modId,_equipmentForm.showedWeapon.Id,CampType.Unknown));
            }));
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            var data = userData as WeaponModData;
            if (data==null)
            {
                return;
            }
            mod = data.ModType;
            modId = data.TypeId;
            _equipmentForm = GameEntry.UI.GetUIForm(UIFormId.EquipmentForm) as EquipmentForm;
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
        
    }
}