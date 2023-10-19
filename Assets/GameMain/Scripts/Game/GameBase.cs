using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode
        {
            get;
        }

        // protected ScrollableBackground SceneBackground
        // {
        //     get;
        //     private set;
        // }

        public bool GameOver
        {
            get;
            protected set;
        }

        protected Player m_Player = null;
        protected PlayerForm _playerForm;

        public virtual void ReadData()
        {
            GameEntry.Setting.GetObject<Weapon>("PlayerWeapon");
        }

        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId,OnOpenUIFormSuccess);

            // SceneBackground = Object.FindObjectOfType<ScrollableBackground>();
            // if (SceneBackground == null)
            // {
            //     Log.Warning("Can not find scene background.");
            //     return;
            // }
            //
            // SceneBackground.VisibleBoundary.gameObject.GetOrAddComponent<HideByBoundary>();
            
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "Player",
                Position = new Vector3(0,0,0)
            });

            GameOver = false;
            m_Player = null;
        }

        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_Player != null && m_Player.IsDead)
            {
                GameOver = true;
                return;
            }
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Player))
            {
                m_Player = (Player)ne.Entity.Logic;
                GameEntry.UI.OpenUIForm(UIFormId.PlayerForm);
            }
        }

        protected virtual void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UIForm.Logic is PlayerForm playerForm)
            {
                _playerForm = playerForm;
                return;
            }
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }
    }
}