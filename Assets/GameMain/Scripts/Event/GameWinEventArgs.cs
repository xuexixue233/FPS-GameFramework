using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class GameWinEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameWinEventArgs).GetHashCode();
        
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
        
        public static GameWinEventArgs Create(int levelId, object userData = null)
        {
            GameWinEventArgs gameOverEventArgs = ReferencePool.Acquire<GameWinEventArgs>();
            gameOverEventArgs.LevelId = levelId;
            return gameOverEventArgs;
        }

        public GameWinEventArgs()
        {
            LevelId = 0;
        }
    }
}