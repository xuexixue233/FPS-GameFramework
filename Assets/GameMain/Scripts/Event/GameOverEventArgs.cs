using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class GameOverEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameOverEventArgs).GetHashCode();
        
        public override void Clear()
        {
            LevelId = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        
        public int LevelId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }
        
        public static GameOverEventArgs Create(int levelId, object userData = null)
        {
            GameOverEventArgs gameOverEventArgs = ReferencePool.Acquire<GameOverEventArgs>();
            gameOverEventArgs.LevelId = levelId;
            return gameOverEventArgs;
        }

        public GameOverEventArgs()
        {
            LevelId = 0;
        }
    }
}