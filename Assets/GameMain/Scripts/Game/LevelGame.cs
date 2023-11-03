using GameFramework;
using UnityEngine;

namespace FPS
{
    public class LevelGame : IReference
    {
        public PlayerSaveData playerSaveData;
        public PlayerForm playerForm;
        public Player player;
        public bool GameOver;

        public LevelGame()
        {
            
        }

        public static LevelGame Create( object userdata=null)
        {
            LevelGame levelGame = ReferencePool.Acquire<LevelGame>();
            return levelGame;
        }
        
        public void Clear()
        {
            
        }
    }
}