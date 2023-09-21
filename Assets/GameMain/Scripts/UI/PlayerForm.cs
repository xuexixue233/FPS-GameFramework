using TMPro;
using UnityEngine.UI;

namespace FPS
{
    public class PlayerForm : UGuiForm
    {
        public PlayerCard PlayerCard;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            PlayerCard.PlayerInit();
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