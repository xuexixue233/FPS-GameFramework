using System;
using System.Collections.Generic;
using System.Linq;
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
        public PlayerSaveData playerSaveData;
        
        private EquipmentForm equipmentForm;
        private Weapon weapon;
        private List<RenderTexture> modTextures = new List<RenderTexture>();
        private List<WeaponMod> previewMods = new List<WeaponMod>();
        private Dictionary<Mod,WeaponMod> currentMods = new Dictionary<Mod,WeaponMod>();
        private List<ItemModSelect> activeSelects = new List<ItemModSelect>();
        private Dictionary<Mod,ItemModUI> activeModUIItem=new Dictionary<Mod, ItemModUI>();
        public ItemModUI showedItem;

        public override bool UseNativeDialog { get; }

        private bool _BackMenu;

        private Dictionary<Mod, List<DRWeaponMod>> modItemsDictionary = new Dictionary<Mod, List<DRWeaponMod>>();
        
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
            GameEntry.Event.Subscribe(ShowAllSelectButtonEventArgs.EventId,ShowAllSelectButton);
            GameEntry.Event.Subscribe(HideAllSelectButtonEventArgs.EventId,HideAllSelectButton);
            _BackMenu = false;
            ReadAllModWeaponsData();
            if (!GameEntry.Setting.HasSetting("PlayerSaveData"))
            {
                GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GenerateSerialId(), 30001, 0,
                    CampType.Unknown));
                playerSaveData = new PlayerSaveData();
            }
            else
            {
                playerSaveData = GameEntry.Setting.GetObject<PlayerSaveData>("PlayerSaveData");
                GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GenerateSerialId(), playerSaveData.playerWeapon.weaponTypeId+1, 0,
                    CampType.Unknown));
            }
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
            GameEntry.Event.Unsubscribe(ShowAllSelectButtonEventArgs.EventId,ShowAllSelectButton);
            GameEntry.Event.Unsubscribe(HideAllSelectButtonEventArgs.EventId,HideAllSelectButton);
            modTextures.Clear();
            previewMods.Clear();
            currentMods.Clear();
            activeSelects.Clear();
            activeModUIItem.Clear();
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
            foreach (var mod in playerSaveData.playerWeapon.modTypeIdDictionary)
            {
                ShowWeaponMod(mod.Key, mod.Value, weapon.m_WeaponData.Id);
            }
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
                        ShowItemModUI(nextMod);
                        equipmentForm.RefreshText();
                        equipmentForm.RefreshImage();
                    }
                }
            }
        }

        private void OnShowItemSuccess(object sender, GameEventArgs e)
        {
            ShowItemSuccessEventArgs ne = (ShowItemSuccessEventArgs)e;
            if (ne.ItemLogicType == typeof(ItemModUI))
            {
                var item = (ItemModUI)ne.Item.Logic;
                activeModUIItem.Add(item._mod, item);
                item.transform.SetParent(equipmentForm.previewRect);
                var rects = activeModUIItem.Values.Select(rect => rect.transform as RectTransform).ToList();
                MathUtilities.GetRectFormEllipse(600, 300, 0, rects.ToArray());
                SetCircleTransforms();
            }
            else if (ne.ItemLogicType == typeof(ItemModSelect))
            {
                var item = (ItemModSelect)ne.Item.Logic;
                item.modImage.texture = modTextures[^1];
                if (ne.UserData is WeaponModData data)
                {
                    if (activeModUIItem.TryGetValue(data.ModType, out var modUI))
                    {
                        item.transform.SetParent(modUI.modList.transform);
                    }
                }
                else if (ne.UserData is Mod mod)
                {
                    if (activeModUIItem.TryGetValue(mod, out var modUI))
                    {
                        item.transform.SetParent(modUI.modList.transform);
                    }
                }
                activeSelects.Add(item);
            }
        }
        
        public void HideAllSelectButton(object sender,GameEventArgs e)
        {
            HideAllSelectButtonEventArgs ne = (HideAllSelectButtonEventArgs)e;
            if (ne==null)
            {
                return;
            }
            
            foreach (var select in activeSelects)
            {
                GameEntry.Item.HideItem(select.Item);
            }

            foreach (var mod in previewMods)
            {
                GameEntry.Entity.HideEntity(mod);
            }

            if (showedItem)
            {
                showedItem.OutCloseList();
                showedItem = null;
            }

            activeSelects.Clear();
            previewMods.Clear();
            modTextures.Clear();
        }
        
        public void ShowAllSelectButton(object sender, GameEventArgs e)
        {
            ShowAllSelectButtonEventArgs ne = (ShowAllSelectButtonEventArgs)e;

            if (ne==null)
            {
                return;
            }
            var mod = ne.ShowMod;
            showedItem = sender as ItemModUI;
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
                    i++;
                }
            }
            GameEntry.Item.ShowItemModSelect(GameEntry.Item.GenerateSerialId(),mod);
        }

        public void ShowItemModUI(Mod mod)
        {
            GameEntry.Item.ShowItemModUI(GameEntry.Item.GenerateSerialId(),mod);
        }

        public void HideItemModUI(Mod mod)
        {
            if (currentMods.TryGetValue(mod,out var weaponMod))
            {
                foreach (var modType in weaponMod.weaponModData.NextModType)
                {
                    if (activeModUIItem.TryGetValue(modType,out var itemModUI))
                    {
                        HideItemModUI(modType);
                        GameEntry.Item.HideItem(itemModUI.Item);
                        activeModUIItem.Remove(modType);
                    }
                }
                GameEntry.Entity.HideEntity(weaponMod);
                currentMods.Remove(mod);
            }
            var rects = activeModUIItem.Values.Select(rect => rect.transform as RectTransform).ToList();
            MathUtilities.GetRectFormEllipse(600, 300, 0, rects.ToArray());
        }

        public void ShowWeaponMod(Mod mod,int typeId, int ownerId)
        {
            if (!currentMods.TryGetValue(mod,out var weaponMod))
            {
                GameEntry.Entity.ShowWeaponMod(new WeaponModData(GameEntry.Entity.GenerateSerialId(),typeId,ownerId,CampType.Unknown));
            }
        }

        private void SetCircleTransforms()
        {
            foreach (var item in activeModUIItem.Values)
            {
                if (Camera.main == null) continue;
                var temp = Camera.main.WorldToScreenPoint(weapon.m_WeaponExData.nextModsTransforms[item._mod].position);
                item.circleTransform.position=new Vector2(temp.x,temp.y);
                var transformItem = Camera.main.ScreenToWorldPoint(item.imageTransform.position);
                item.SetLine(weapon.m_WeaponExData.nextModsTransforms[item._mod].position,transformItem);
            }
        }
    }
}