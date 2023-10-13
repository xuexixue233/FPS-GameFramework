using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using GameFramework.ObjectPool;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class WeaponModUI
    {
        public Mod mod;
        public string Name;
        public Image modImage;
    }
    
    public class EquipmentForm : UGuiForm
    {
        private IObjectPool<ModUIItemObject> _objectPool;

        public Weapon showedWeapon;
        public GameObject attributeList;

        [Header("Button")] 
        public Button backButton;
        public Button listButton;
        public Image showImage;
        public Image hideImage;

        [Header("List-Fill")] 
        public Image durabilityImage;
        public Image weightImage;
        public Image ergonomicsImage;
        public Image precisionImage;
        public Image shootingGalleryImage;
        public Image verticalRecoilImage;
        public Image horizontalRecoilImage;
        public Image muzzleVelocityImage;

        [Header("list-Text")] 
        public TMP_Text durabilityText;
        public TMP_Text weightText;
        public TMP_Text ergonomicsText;
        public TMP_Text precisionText;
        public TMP_Text shootingGalleryText;
        public TMP_Text verticalRecoilText;
        public TMP_Text horizontalRecoilText;
        public TMP_Text muzzleVelocityText;

        public TMP_Text firingModeText;
        public TMP_Text caliberText;
        public TMP_Text firingRateText;
        public TMP_Text effectiveFiringRange;

        public ModUIItem modUIItemTemplate;
        public GameObject circleGameObject;
        public RectTransform previewRect;

        public Dictionary<Mod,ModUIItem> activeModUIItem;
        public List<RectTransform> activeModUIItemRect=new List<RectTransform>();

        public void ShowModUIItem(Mod mod)
        {
            ModUIItem modUIItem = GetActiveModUIItem(mod);
            if (modUIItem==null)
            {
                modUIItem = CreateModUIItem(mod);
                activeModUIItem.Add(mod,modUIItem);
                activeModUIItemRect.Add(modUIItem.transform as RectTransform);
            }
            modUIItem.Init(mod,showedWeapon);
        }
        
        public void HideModUIItem(Mod  mod)
        {
            if (activeModUIItem.TryGetValue(mod,out var modUIItem))
            {
                activeModUIItem.Remove(mod);
                _objectPool.Unspawn(modUIItem);
            }
        }
        
        public ModUIItem GetActiveModUIItem(Mod mod)
        {
            if (activeModUIItem.TryGetValue(mod,out var modUIItem))
            {
                return modUIItem;
            }

            return null;

        }

        public ModUIItem CreateModUIItem(Mod mod)
        {
            ModUIItem modUIItem = null;
            ModUIItemObject modUIItemObject = _objectPool.Spawn();
            if (modUIItemObject!=null)
            {
                modUIItem = (ModUIItem)modUIItemObject.Target;
            }
            else
            {
                modUIItem = Instantiate(modUIItemTemplate);
                Transform modUITransform = modUIItem.GetComponent<Transform>();
                //TODO:设置位置
                modUITransform.SetParent(previewRect);
                
                _objectPool.Register(ModUIItemObject.Create(modUIItem),true);
            }

            return modUIItem;
        }

        private void BackToMenu()
        {
            GameEntry.Event.Fire(this,ChangeSceneEventArgs.Create(1));
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(BackToMenu);
            
            listButton.onClick.RemoveAllListeners();
            listButton.onClick.AddListener((() =>
            {
                attributeList.SetActive(!attributeList.activeSelf);
                showImage.gameObject.SetActive(!showImage.gameObject.activeSelf);
                hideImage.gameObject.SetActive(!hideImage.gameObject.activeSelf);
            }));
            _objectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<ModUIItemObject>("ModUIItem", 10);
            activeModUIItem = new Dictionary<Mod, ModUIItem>();
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            
            showedWeapon = (Weapon)userData;
            durabilityText.text = "100/100(100)";
            OpenText();
            OpenImage();
            foreach (var mod in showedWeapon.m_WeaponData.NextMods)
            {
                ShowModUIItem(mod);
            }

            UITransformToModTransform();
            MathUtilities.GetRectFormEllipse(600,300,0,activeModUIItemRect.ToArray());
        }

        private void UITransformToModTransform()
        {
            var newTransform = new List<Transform>();

            foreach (var mod in showedWeapon.m_WeaponData.NextMods)
            {
                var temp = showedWeapon.m_WeaponExData.previewCamera.WorldToScreenPoint(showedWeapon.m_WeaponExData.nextModsTransforms[mod].position);
                var modUIItem = Instantiate(circleGameObject, previewRect, true);
                modUIItem.GetComponent<RectTransform>().anchoredPosition=new Vector2(temp.x,temp.y);
            }
            
        }

        private void OpenText()
        {
            weightText.text = showedWeapon.m_WeaponData.Weight.ToString(CultureInfo.InvariantCulture);
            ergonomicsText.text = showedWeapon.m_WeaponData.Ergonomics.ToString();
            precisionText.text = showedWeapon.m_WeaponData.Precision.ToString(CultureInfo.InvariantCulture);
            shootingGalleryText.text = showedWeapon.m_WeaponData.ShootingGallery.ToString();
            verticalRecoilText.text = showedWeapon.m_WeaponData.VerticalRecoil.ToString();
            horizontalRecoilText.text = showedWeapon.m_WeaponData.HorizontalRecoil.ToString();
            muzzleVelocityText.text = showedWeapon.m_WeaponData.MuzzleVelocity.ToString();
            
            firingModeText.text = showedWeapon.m_WeaponData.FiringMode;
            caliberText.text = showedWeapon.m_WeaponData.Caliber;
            firingRateText.text = showedWeapon.m_WeaponData.FiringRate.ToString();
            effectiveFiringRange.text = showedWeapon.m_WeaponData.EffectiveFiringRange.ToString();
        }

        private void OpenImage()
        {
            weightImage.fillAmount = showedWeapon.m_WeaponData.Weight / 20;
            ergonomicsImage.fillAmount = (float)showedWeapon.m_WeaponData.Ergonomics / 100;
            precisionImage.fillAmount = 1-showedWeapon.m_WeaponData.Precision / 50;
            shootingGalleryImage.fillAmount = (float)showedWeapon.m_WeaponData.ShootingGallery / 10000;
            verticalRecoilImage.fillAmount = (float)showedWeapon.m_WeaponData.VerticalRecoil / 500;
            horizontalRecoilImage.fillAmount = (float)showedWeapon.m_WeaponData.HorizontalRecoil / 500;
            muzzleVelocityImage.fillAmount = (float)showedWeapon.m_WeaponData.MuzzleVelocity / 2000;
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
        
    }
}
