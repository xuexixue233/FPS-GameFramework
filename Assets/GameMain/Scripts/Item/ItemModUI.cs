using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class ItemModUI : ItemLogic
    {
        public Button modButton;
        public Transform circleTransform;
        public RectTransform imageTransform;
        public TMP_Text modText;
        public Mod _mod;
        public GameObject modList;
        public GameObject showImage;
        public GameObject hideImage;
        public LineRenderer lineRenderer;
        public bool isOpened=false;
        
        private Entity m_Owner = null;
        public Entity Owner => m_Owner;
        

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            modButton.onClick.RemoveAllListeners();
            modButton.onClick.AddListener((() =>
            {
                GameEntry.Sound.PlayUISound(10001);
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
            GameEntry.Event.Fire(this,HideAllSelectButtonEventArgs.Create());
            GameEntry.Event.Fire(this,ShowAllSelectButtonEventArgs.Create(_mod));
            isOpened = true;
        }

        private void CloseList()
        {
            modList.SetActive(false);
            showImage.SetActive(true);
            hideImage.SetActive(false);
            GameEntry.Event.Fire(this,HideAllSelectButtonEventArgs.Create());
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
            modText.text = _mod.ToString();
        }

        public void SetLine(Vector3 start,Vector3 end)
        {
            lineRenderer.startWidth = 0.002f;
            lineRenderer.endWidth = 0.002f;
            lineRenderer.SetPosition(0,start);
            lineRenderer.SetPosition(1,end);
        }
        
    }
}