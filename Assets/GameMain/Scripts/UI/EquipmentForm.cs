using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        public Weapon showedWeapon;
        public GameObject attributeList;
        
        [Header("Button")]
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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
