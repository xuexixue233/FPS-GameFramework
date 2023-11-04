using System;
using System.Collections.Generic;
using System.Linq;
using GameFramework;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class Equipment : IReference
    {
        private PlayerSaveData playerSaveData;
        
        private EquipmentForm equipmentForm;
        private Weapon weapon;
        private List<RenderTexture> modTextures = new List<RenderTexture>();
        private List<WeaponMod> previewMods = new List<WeaponMod>();
        private Dictionary<Mod,WeaponMod> currentMods = new Dictionary<Mod,WeaponMod>();
        private List<ItemModSelect> activeSelects = new List<ItemModSelect>();
        private Dictionary<Mod,ItemModUI> activeModUIItem=new Dictionary<Mod, ItemModUI>();
        private ItemModUI showedItem;
        private Dictionary<Mod, List<DRWeaponMod>> modItemsDictionary = new Dictionary<Mod, List<DRWeaponMod>>();

        public Equipment()
        {
            modTextures = new List<RenderTexture>();
            previewMods = new List<WeaponMod>();
            currentMods = new Dictionary<Mod,WeaponMod>();
            activeSelects = new List<ItemModSelect>();
            activeModUIItem=new Dictionary<Mod, ItemModUI>();
            modItemsDictionary = new Dictionary<Mod, List<DRWeaponMod>>();
        }

        public void SaveData()
        {
            playerSaveData.playerWeapon.weaponTypeId = weapon.m_WeaponData.TypeId-1;
            playerSaveData.playerWeapon.modTypeIdDictionary.Clear();
            foreach (var mod in weapon.weaponMods)
            {
                playerSaveData.playerWeapon.modTypeIdDictionary.Add(mod.Key,mod.Value.weaponModData.TypeId);
            }
            GameEntry.Setting.SetObject("PlayerSaveData",playerSaveData);
        }

        public void OnEnter()
        {
            ReadAllModWeaponsData();
            GameEntry.Entity.ShowWeapon(new WeaponData(GameEntry.Entity.GenerateSerialId(), playerSaveData.playerWeapon.weaponTypeId+1, 0,
                CampType.Unknown)
                {
                    Position = new Vector3(0.2f,0,0)
                }
            );
        }

        public void OnUpdate()
        {
            
        }

        public void OnLeave()
        {
            SaveData();
            GameEntry.Item.HideAllLoadedItems();
            GameEntry.UI.CloseAllLoadedUIForms();
            GameEntry.Entity.HideAllLoadedEntities();
        }

        public void ShowUI(EntityLogic ne)
        {
            weapon = (Weapon)ne;
            weapon.transform.localScale = new Vector3(100, 100, 100);
            GameEntry.UI.OpenUIForm(UIFormId.EquipmentForm, weapon);
        }

        public void ShowItemModUI(Mod mod)
        {
            //可修改show增加成功回调
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
            SetCircleTransforms();
        }

        public void ShowWeaponMod(Mod mod, int typeId, int ownerId)
        {
            if (!currentMods.TryGetValue(mod,out var weaponMod))
            {
                GameEntry.Entity.ShowWeaponMod(new WeaponModData(GameEntry.Entity.GenerateSerialId(),typeId,ownerId,CampType.Unknown));
            }
        }

        public void AfterShowWeaponMod(EntityLogic ne,object userdata)
        {
            var mod = (WeaponMod)ne;
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
                GameEntry.Item.ShowItemModSelect(GameEntry.Item.GenerateSerialId(), userdata);
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

        public void AfterShowItemModUI(ItemLogic ne)
        {
            var item = (ItemModUI)ne;
            activeModUIItem.Add(item._mod, item);
            item.transform.SetParent(equipmentForm.previewRect);
            var rects = activeModUIItem.Values.Select(rect => rect.transform as RectTransform).ToList();
            MathUtilities.GetRectFormEllipse(600, 300, 0, rects.ToArray());
            SetCircleTransforms();
        }

        public void AfterOpenEquipmentForm(UIFormLogic ne)
        {
            equipmentForm = (EquipmentForm)ne;
            foreach (var mod in playerSaveData.playerWeapon.modTypeIdDictionary)
            {
                ShowWeaponMod(mod.Key, mod.Value, weapon.m_WeaponData.Id);
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

        public void AfterShowItemModSelect(ItemLogic ne , object userdata)
        {
            var item = (ItemModSelect)ne;
            item.modImage.texture = modTextures[^1];
            if (userdata is WeaponModData data)
            {
                if (activeModUIItem.TryGetValue(data.ModType, out var modUI))
                {
                    item.transform.SetParent(modUI.modList.transform);
                }
            }
            else if (userdata is Mod mod)
            {
                if (activeModUIItem.TryGetValue(mod, out var modUI))
                {
                    item.transform.SetParent(modUI.modList.transform);
                }
            }
            activeSelects.Add(item);
        }

        public void HideAllSelectButton()
        {
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

        public void ShowAllSelectButton(Mod mod,ItemModUI itemModUI)
        {
            showedItem = itemModUI;
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
        
        public static Equipment Create(PlayerSaveData playerSaveData,object userdata=null)
        {
            Equipment equipment = ReferencePool.Acquire<Equipment>();
            equipment.playerSaveData = playerSaveData;
            return equipment;
        }
        
        public void Clear()
        {
            playerSaveData = null;
            equipmentForm = null;
            weapon = null;
            modTextures.Clear();
            previewMods.Clear();
            currentMods.Clear();
            activeSelects.Clear();
            activeModUIItem.Clear();
            showedItem = null;
            modItemsDictionary.Clear();
        }
    }
}