using System;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace FPS
{
    public class ProcedureSelectWeapon : ProcedureBase
    {
        public EquipmentForm equipmentForm;
        private Weapon weapon;
        private List<RenderTexture> modTextures = new List<RenderTexture>();
        private List<WeaponMod> previewMods = new List<WeaponMod>();
        public Dictionary<Mod,WeaponMod> currentMods = new Dictionary<Mod,WeaponMod>();

        public override bool UseNativeDialog { get; }

        private bool _BackMenu;

        private Dictionary<Mod, List<DRWeaponMod>> modItemsDictionary = new Dictionary<Mod, List<DRWeaponMod>>();

        private void ReadUserData()
        {
            //Todo:读取玩家数据
        }

        public void HideItemModUI(Mod mod)
        {
            if (equipmentForm.activeModUIItem.TryGetValue(mod,out var item))
            {
                GameEntry.Item.HideItem(item.Item);
                equipmentForm.activeModUIItem.Remove(mod);
            }
        }

        public void ShowAllSelectButton(Mod mod)
        {
            HideAllSelectButton();
            int i = 1;
            if (modItemsDictionary.TryGetValue(mod, out var list))
            {
                foreach (var data in list)
                {
                    GameEntry.Entity.ShowWeaponMod(
                        new WeaponModData(GameEntry.Entity.GenerateSerialId(), data.Id, 0, CampType.Unknown)
                        {
                            Position = new Vector3(i * 100, 0, 0)
                        });
                }
            }
            GameEntry.Item.ShowItemModSelect(GameEntry.Item.GenerateSerialId(),mod);
        }

        public void HideAllSelectButton()
        {
            foreach (var select in equipmentForm.activeSelects)
            {
                GameEntry.Item.HideItem(select.Item);
            }

            foreach (var mod in previewMods)
            {
                GameEntry.Entity.HideEntity(mod);
            }

            if (equipmentForm.showedItem)
            {
                equipmentForm.showedItem.OutCloseList();
                equipmentForm.showedItem = null;
            }

            equipmentForm.activeSelects.Clear();
            previewMods.Clear();
            modTextures.Clear();
        }


        private void ReadAllModWeaponsData()
        {
            IDataTable<DRWeaponMod> dtItem = GameEntry.DataTable.GetDataTable<DRWeaponMod>();
            var drItems = dtItem.GetAllDataRows();
            foreach (var drItem in drItems)
            {
                var modType = (Mod)Enum.Parse(typeof(Mod), drItem.ModType);
                if (modItemsDictionary.TryGetValue(modType, out var list))
                {
                    list.Add(drItem);
                }
                else
                {
                    modItemsDictionary.Add(modType, new List<DRWeaponMod> { drItem });
                }
            }
        }

        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenEquipmentFormSuccess);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowItemSuccessEventArgs.EventId, OnShowItemSuccess);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, ChangeSceneSuccess);
            ReadAllModWeaponsData();
            _BackMenu = false;
            GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GenerateSerialId(), 30000, 0,
                CampType.Unknown));
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

            GameEntry.UI.CloseAllLoadedUIForms();
            equipmentForm = null;
        }

        private void OnOpenEquipmentFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne ==null)
            {
                return;
            }
            equipmentForm = (EquipmentForm)ne.UIForm.Logic;

            equipmentForm.procedureSelectWeapon = this;
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
                GameEntry.UI.OpenUIForm(UIFormId.EquipmentForm, weapon);
            }
            else if (ne.EntityLogicType == typeof(WeaponMod))
            {
                var mod = (WeaponMod)ne.Entity.Logic;
                if (mod.weaponModData.OwnerId == 0)
                {

                    var gameObject = new GameObject();
                    gameObject.transform.SetParent(mod.transform); 
                    var camera = gameObject.AddComponent<Camera>();
                    camera.nearClipPlane = 0.001f;
                    camera.transform.localPosition = Vector3.zero;
                    var child = mod.GetComponent<ModExData>().modTransform;
                    child.localPosition = mod.GetComponent<ModExData>().previewPosition;
                    child.rotation = Quaternion.Euler(mod.GetComponent<ModExData>().previewRotation);
                    previewMods.Add(mod);
                    var texture = new RenderTexture(800, 800, 10);
                    modTextures.Add(texture);
                    camera.targetTexture = texture;
                    camera.clearFlags = CameraClearFlags.SolidColor;
                    GameEntry.Item.ShowItemModSelect(GameEntry.Item.GenerateSerialId(), ne.UserData);
                }
                else
                {
                    currentMods.Add(mod.weaponModData.ModType,mod);
                    foreach (var nextMod in mod.weaponModData.NextModType)
                    {
                        GameEntry.Item.ShowItemModUI(GameEntry.Item.GenerateSerialId(), nextMod);
                        equipmentForm.RefreshText();
                        equipmentForm.RefreshImage();
                    }
                }
            }
        }

        private void OnHideEntityComplete(object sender, GameEventArgs e)
        {
            HideEntityCompleteEventArgs ne = (HideEntityCompleteEventArgs)e;
            
        }

        private void OnShowItemSuccess(object sender, GameEventArgs e)
        {
            ShowItemSuccessEventArgs ne = (ShowItemSuccessEventArgs)e;
            if (ne.ItemLogicType == typeof(ItemModUI))
            {
                var item = (ItemModUI)ne.Item.Logic;
                equipmentForm.activeModUIItem.Add(item._mod, item);
                item.transform.SetParent(equipmentForm.previewRect);
                equipmentForm.activeModUIItemRect.Add(item.transform as RectTransform);
                MathUtilities.GetRectFormEllipse(600, 300, 0, equipmentForm.activeModUIItemRect.ToArray());
            }
            else if (ne.ItemLogicType == typeof(ItemModSelect))
            {
                var item = (ItemModSelect)ne.Item.Logic;
                item.modImage.texture = modTextures[^1];
                if (ne.UserData is WeaponModData data)
                {
                    if (equipmentForm.activeModUIItem.TryGetValue(data.ModType, out var modUI))
                    {
                        item.transform.SetParent(modUI.modList.transform);
                    }
                }
                else if (ne.UserData is Mod mod)
                {
                    if (equipmentForm.activeModUIItem.TryGetValue(mod, out var modUI))
                    {
                        item.transform.SetParent(modUI.modList.transform);
                    }
                }
                equipmentForm.activeSelects.Add(item);
            }
        }
    }
}