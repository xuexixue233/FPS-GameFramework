using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace FPS
{
    public class Level : IReference
    {

        public Transform playerSpawn;
        public Transform enemySpawn;

        public static Level Create(Transform playerSpawn,Transform enemySpawn,object userdata=null)
        {
            Level level = ReferencePool.Acquire<Level>();
            level.playerSpawn = playerSpawn;
            level.enemySpawn = enemySpawn;
            return level;
        }
        
        

        public void Clear()
        {
            
        }
    }
}