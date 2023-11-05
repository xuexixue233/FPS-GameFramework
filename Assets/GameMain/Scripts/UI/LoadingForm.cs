using DG.Tweening;
using GameFramework.Event;
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
            GameEntry.Event.Subscribe(LoadingChangeEventArgs.EventId, OnLoadingChange);
        }

        private void OnLoadingChange(object sender, GameEventArgs e)
        {
            LoadingChangeEventArgs ne = (LoadingChangeEventArgs)e;
            
            if (ne==null)
            {
                return;
            }

            loadingImage.DOFillAmount(ne.CurrentProgress, 0.5f).OnComplete((() =>
            {
                var fill = Mathf.Round(loadingImage.fillAmount * 100) / 100;
                loadingText.text = $"{fill * 100}%";
            }));
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            GameEntry.Event.Unsubscribe(LoadingChangeEventArgs.EventId, OnLoadingChange);
            base.OnClose(isShutdown, userData);
        }
    }
}
