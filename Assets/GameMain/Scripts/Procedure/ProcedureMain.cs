using System.Collections.Generic;
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
        private const float GameOverDelayedSeconds = 2f;
        
        private string SceneName;
        private bool _ChangeScene;
        
        private float m_ShowGameOverSeconds = 0f;
        public PlayerSaveData playerSaveData;
        private PlayerForm playerForm;
        private Player player;
        private bool GameOver;

        private GameObject enemySpawnPoints;
        public GameObject playerSpawnPoint;

        public override bool UseNativeDialog => false;
        

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            playerSpawnPoint=GameObject.Find("PlayerSpawn");
            playerSaveData=GameEntry.Setting.GetObject<PlayerSaveData>("PlayerSaveData");
            GameEntry.Event.Subscribe(ChangeSceneEventArgs.EventId,OnChangeScene);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId,OnOpenUIFormSuccess);
            
            
            _ChangeScene = false;
            //生成玩家
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "Player",
                
            });

            GameOver = false;
            player = null;
            GameEntry.UI.OpenUIForm(UIFormId.PlayerForm);
            
            //生成敌人
            var enemyPoints = enemySpawnPoints.GetComponentsInChildren<Transform>();
            for (var i = 1; i < enemyPoints.Length; i++)
            {
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(),10001)
                {
                    Name = $"Enemy{i}",
                    Position = enemyPoints[i].position
                });
            }
        }
        

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (!GameOver)
            {
                // if (player != null && player.IsDead)
                // {
                //     GameOver = true;
                //     m_ShowGameOverSeconds = 0;
                //     Cursor.lockState = CursorLockMode.None;
                //     return;
                // }
                return;
            }

            if (m_ShowGameOverSeconds>GameOverDelayedSeconds)
            {
                playerForm.ShowGameOver();
            }

            m_ShowGameOverSeconds += elapseSeconds;
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
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId,OnOpenUIFormSuccess);
            base.OnLeave(procedureOwner, isShutdown);
        }
        
        private void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(Player))
            {
                player = (Player)ne.Entity.Logic;
                
                GameEntry.UI.OpenUIForm(UIFormId.PlayerForm);
            }
            else if (ne.EntityLogicType==typeof(Weapon))
            {
                var weapon = (Weapon)ne.Entity.Logic;
                
            }
            else if (ne.EntityLogicType==typeof(WeaponMod))
            {
                var weaponMod = (WeaponMod)ne.Entity.Logic;
                playerForm.PlayerCard.OnInit(player);
            }
        }
        
        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UIForm.Logic is PlayerForm _playerForm)
            {
                playerForm = _playerForm;
                playerForm.procedureMain = this;
                return;
            }
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

        private void OnShowItemSuccess(object sender, GameEventArgs e)
        {
            ShowItemSuccessEventArgs ne = (ShowItemSuccessEventArgs)e;
            if (ne.ItemLogicType==typeof(ItemBulletHole))
            {
                
            }
        }
    }
}