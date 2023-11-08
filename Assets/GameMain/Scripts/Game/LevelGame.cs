using System.Collections.Generic;
using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace FPS
{
    public class LevelGame : IReference
    {
        private int levelId;//以后设置levelData存储关卡信息 
        
        private Player player;
        private List<Enemy> enemies=new List<Enemy>();

        private PlayerForm playerForm;

        private Transform playerSpawn;
        private Transform enemySpawn;

        private float m_ShowSeconds;
        private float DelayedSeconds = 0.5f;
        
        private bool _ChangeScene;
        private bool gameOver;
        private bool gameWin;

        public LevelGame()
        {

        }

        public void ShowGameOver()
        {
            m_ShowSeconds = 0;
            gameOver = true;
            Cursor.lockState = CursorLockMode.None;
            foreach (var enemy in enemies)
            {
                enemy._behaviorTree.DisableBehavior();
            }
        }
        
        public void ShowGameWin()
        {
            m_ShowSeconds = 0;
            gameWin = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void OnEnable()
        {
            _ChangeScene = false;
            
            //生成玩家
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "Player",
            });

            gameOver = false;
            gameWin = false;
            player = null;

            //生成敌人
            var enemyPoints = enemySpawn.GetComponentsInChildren<Transform>();
            for (var i = 1; i < enemyPoints.Length; i++)
            {
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(),10001)
                {
                    Name = $"Enemy{i}",
                    Position = enemyPoints[i].position,
                    Rotation = enemyPoints[i].rotation
                });
            }
        }

        public void ReStart()
        {
            playerForm.gameOver.gameObject.SetActive(false);
            playerForm.gameWin.gameObject.SetActive(false);
            Clear();
            GameEntry.Entity.HideAllLoadedEntities();
            GameEntry.UI.CloseAllLoadedUIForms();
            
            GameEntry.UI.OpenUIForm(UIFormId.LoadingForm);
            
            gameOver = false;
            gameWin = false;
            player = null;
            
            GameEntry.Entity.ShowPlayer(new PlayerData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "Player",
            });

            var enemyPoints = enemySpawn.GetComponentsInChildren<Transform>();
            for (var i = 1; i < enemyPoints.Length; i++)
            {
                GameEntry.Entity.ShowEnemy(new EnemyData(GameEntry.Entity.GenerateSerialId(),10001)
                {
                    Name = $"Enemy{i}",
                    Position = enemyPoints[i].position,
                    Rotation = enemyPoints[i].rotation
                });
            }
        }

        public void OnUpdate(float elapseSeconds)
        {

            if (!gameOver&&!gameWin)
            {
                return;
            }

            m_ShowSeconds += elapseSeconds;
            if (m_ShowSeconds>DelayedSeconds)
            {
                if (gameOver)
                {
                    playerForm.ShowGameOver();
                }
                else if (gameWin)
                {
                    playerForm.ShowGameWin();
                }
            }
        }

        public void OnLeave()
        {
            
        }

        public void ShowPlayer(EntityLogic entityLogic)
        {
            player = entityLogic as Player;
            var form = GameEntry.UI.GetUIForm(UIFormId.LoadingForm);
            GameEntry.UI.CloseUIForm(form);
            GameEntry.UI.OpenUIForm(UIFormId.PlayerForm);
            if (player != null)
            {
                player.transform.position = playerSpawn.position;
                player.m_characterController.enabled = true;
            }
        }

        public void ShowEnemy(EntityLogic entityLogic)
        {
            var enemy = entityLogic as Enemy;
            enemies.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
            if (enemies.Count==0)
            {
                GameEntry.Event.Fire(this,GameWinEventArgs.Create(1));
            }
        }

        public void ShowPlayerForm(UIFormLogic uiFormLogic)
        {
            playerForm = uiFormLogic as PlayerForm;
            
        }

        public void ShowWeapon()
        {
            
        }

        public void ShowWeaponMod()
        {
            PlayerFormInit();
        }

        private void PlayerFormInit()
        {
            playerForm.PlayerCard.OnInit(player);
        }

        public static LevelGame Create(PlayerSaveData playerSaveData,Transform playerSpawn,Transform enemySpawn, object userdata=null)
        {
            LevelGame levelGame = ReferencePool.Acquire<LevelGame>();
            levelGame.playerSpawn = playerSpawn;
            levelGame.enemySpawn = enemySpawn;
            return levelGame;
        }

        public void Clear()
        {
            player = null;
            enemies.Clear();
            playerForm = null;
            gameOver = false;
            gameWin = false;
        }
    }
}