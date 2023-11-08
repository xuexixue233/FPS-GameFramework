using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class GameReStartEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(GameReStartEventArgs).GetHashCode();
        
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
        
        public static GameReStartEventArgs Create(int levelId, object userData = null)
        {
            GameReStartEventArgs gameReStartEventArgs = ReferencePool.Acquire<GameReStartEventArgs>();
            gameReStartEventArgs.LevelId = levelId;
            return gameReStartEventArgs;
        }

        public GameReStartEventArgs()
        {
            LevelId = 0;
        }
    }
}