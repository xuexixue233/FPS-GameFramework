using TMPro;
using UnityEngine.UI;

namespace FPS
{
    public class PlayerForm : UGuiForm
    {
        public ProcedureMain procedureMain;
        public PlayerCard PlayerCard;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            // PlayerCard.Refresh(procedureMain.player);
        }

        protected override void OnClose(bool isShutdown, object userData)
        {
            base.OnClose(isShutdown, userData);
        }
    }
}