using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class ShowEffectSuccessEventArgs : GameEventArgs
    {
        private static readonly int EventId = typeof(ShowEffectSuccessEventArgs).GetHashCode();
        
        public override int Id => EventId;
        
        

        public object UserData
        {
            get;
            private set;
        }
        
        public static ShowEffectSuccessEventArgs Create(object userData = null)
        {
            ShowEffectSuccessEventArgs loadLevelEventArgs = ReferencePool.Acquire<ShowEffectSuccessEventArgs>();
            
            loadLevelEventArgs.UserData = userData;
            return loadLevelEventArgs;
        }
        
        
        public override void Clear()
        {
            
        }
    }
}