using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class EnemyDeadEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(EnemyDeadEventArgs).GetHashCode();
        
        public override void Clear()
        {
            enemy = null;
        }
        
        public Enemy enemy
        {
            get;
            private set;
        }

        public override int Id => EventId;
        
        public EnemyDeadEventArgs()
        {
            enemy = null;
        }
        
        public static EnemyDeadEventArgs Create(Enemy enemy, object userData = null)
        {
            EnemyDeadEventArgs enemyDeadEventArgs = ReferencePool.Acquire<EnemyDeadEventArgs>();
            enemyDeadEventArgs.enemy = enemy;
            return enemyDeadEventArgs;
        }
    }
}