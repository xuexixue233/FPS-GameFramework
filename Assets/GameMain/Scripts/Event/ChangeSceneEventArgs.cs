using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class ChangeSceneEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ChangeSceneEventArgs).GetHashCode();
        
        public override void Clear()
        {
            SceneId = 0;
        }

        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        
        public int SceneId
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }
        
        public static ChangeSceneEventArgs Create(int sceneId, object userData = null)
        {
            ChangeSceneEventArgs changeSceneEventArgs = ReferencePool.Acquire<ChangeSceneEventArgs>();
            changeSceneEventArgs.SceneId = sceneId;
            changeSceneEventArgs.UserData = userData;
            return changeSceneEventArgs;
        }
    }
}