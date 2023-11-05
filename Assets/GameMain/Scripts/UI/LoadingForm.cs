using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FPS
{
    public class LoadingForm : UGuiForm
    {
        public TMP_Text loadingText;
        public Image loadingImage;
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            loadingImage.fillAmount = 0;
            loadingText.text = "0.00%";
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}
