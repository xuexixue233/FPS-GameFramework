using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Procedure;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace FPS
{
    public class ProcedureMain : ProcedureBase
    {
        private const float DelayedSeconds = 0.5f;
        
        private string SceneName;
        private bool _ChangeScene;
        
        private float m_ShowSeconds = 0f;

        private LevelGame levelGame;

        public override bool UseNativeDialog => false;
        

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            var playerSpawnPoint=GameObject.Find("PlayerSpawn").transform;
            var enemySpawnPoints=GameObject.Find("EnemySpawn").transform;
            var playerSaveData=GameEntry.Setting.GetObject<PlayerSaveData>("PlayerSaveData");
            
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId,OnChangeScene);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(EnemyDeadEventArgs.EventId,OnEnemyDead);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId,OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(GameOverEventArgs.EventId,OnGameOver);
            GameEntry.Event.Subscribe(GameWinEventArgs.EventId,OnGameWin);
            GameEntry.Event.Subscribe(GameReStartEventArgs.EventId,OnGameRestart);

            levelGame = LevelGame.Create(playerSaveData,playerSpawnPoint,enemySpawnPoints);
            
            levelGame.OnEnable();
        }

        private void OnGameRestart(object sender, GameEventArgs e)
        {
            GameReStartEventArgs ne = (GameReStartEventArgs)e;
            if (ne==null)
            {
                return;
            }
            
            levelGame.ReStart();
        }

        private void OnGameOver(object sender, GameEventArgs e)
        {
            GameOverEventArgs ne = (GameOverEventArgs)e;
            
            if (ne==null)
            {
                return;
            }
            
            levelGame.ShowGameOver();
        }

        private void OnGameWin(object sender, GameEventArgs e)
        {
            GameWinEventArgs ne = (GameWinEventArgs)e;
            if (ne==null)
            {
                return;
            }
            
            levelGame.ShowGameWin();
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            
            levelGame.OnUpdate(elapseSeconds);
            
            if (_ChangeScene)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt(SceneName));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(ChangeSceneEventArgs.EventId,OnChangeScene);
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(EnemyDeadEventArgs.EventId,OnEnemyDead);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId,OnOpenUIFormSuccess);
            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId,OnGameOver);
            GameEntry.Event.Unsubscribe(GameWinEventArgs.EventId,OnGameWin);
            GameEntry.Event.Unsubscribe(GameReStartEventArgs.EventId,OnGameRestart);
            levelGame.OnLeave();
            ReferencePool.Release(levelGame);
            base.OnLeave(procedureOwner, isShutdown);
        }

        private void OnEnemyDead(object sender, GameEventArgs e)
        {
            EnemyDeadEventArgs ne = (EnemyDeadEventArgs)e;
            if (ne==null)
            {
                return;
            }

            levelGame.RemoveEnemy(ne.enemy);
        }

        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Player))
            {
                levelGame.ShowPlayer(ne.Entity.Logic);
            }
            else if (ne.EntityLogicType==typeof(Weapon))
            {
                levelGame.ShowWeapon();
            }
            else if (ne.EntityLogicType==typeof(WeaponMod))
            {
                levelGame.ShowWeaponMod();
            }
            else if (ne.EntityLogicType==typeof(Enemy))
            {
                levelGame.ShowEnemy(ne.Entity.Logic);
            }
        }
        
        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne==null)
            {
                return;
            }
            levelGame.ShowPlayerForm(ne.UIForm.Logic);
        }

        private void OnChangeScene(object sender, GameEventArgs e)
        {
            ChangeSceneEventArgs ne = (ChangeSceneEventArgs)e;
            if (ne==null)
            {
                return;
            }
            
            SceneName = ne.SceneId switch
            {
                1 => "Scene.Menu",
                2 => "Scene.Main",
                3 => "Scene.Equipment",
                _ => SceneName
            };
            _ChangeScene = true;
        }
    }
}