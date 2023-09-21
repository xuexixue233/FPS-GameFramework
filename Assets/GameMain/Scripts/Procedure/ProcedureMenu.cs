using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace FPS
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;


        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void StartGame()
        {
            m_StartGame = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMenuFormSuccess);
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId, ChangeSceneSuccess);

            m_StartGame = false;
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenMenuFormSuccess);
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId, ChangeSceneSuccess);
            
            GameEntry.UI.CloseAllLoadedUIForms();
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Main"));
                procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Survival);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenMenuFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            
            if (ne.UserData != this)
            {
                return;
            }
        }

        private void ChangeSceneSuccess(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne == null)
                return;

            m_StartGame = true;
        }
    }
}