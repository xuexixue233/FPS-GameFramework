using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class ItemModUI : ItemLogic
    {
        public Button modButton;
        public RawImage modImage;
        public TMP_Text modText;
        public Mod _mod;
        public GameObject modList;
        public GameObject showImage;
        public GameObject hideImage;
        public bool isOpened=false;
        
        private Entity m_Owner = null;
        public Entity Owner => m_Owner;
        

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            modButton.onClick.RemoveAllListeners();
            modButton.onClick.AddListener((() =>
            {
                if (isOpened)
                {
                    CloseList();
                }
                else
                {
                    OpenList();
                }
            }));
        }

        private void OpenList()
        {
            modList.SetActive(true);
            showImage.SetActive(false);
            hideImage.SetActive(true);
            if (GameEntry.Procedure.CurrentProcedure is ProcedureSelectWeapon procedure)
            {
                procedure.ShowAllSelectButton(_mod);
                procedure.equipmentForm.showedItem = this;
            }
            isOpened = true;
        }

        private void CloseList()
        {
            modList.SetActive(false);
            showImage.SetActive(true);
            hideImage.SetActive(false);
            if (GameEntry.Procedure.CurrentProcedure is ProcedureSelectWeapon procedure)
            {
                procedure.HideAllSelectButton();
            }
            isOpened = false;
        }

        public void OutCloseList()
        {
            modList.SetActive(false);
            showImage.SetActive(true);
            hideImage.SetActive(false);
            isOpened = false;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            _mod = userData is Mod mod ? mod : Mod.None;
        }
        
    }
}