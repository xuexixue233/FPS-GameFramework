using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class ShowEffectSuccessEventArgs : GameEventArgs
    {
        private static readonly int EventId = typeof(ShowEffectSuccessEventArgs).GetHashCode();
        
        public override int Id => EventId;

        public EffectData EffectData
        {
            get;
            private set;
        }

        public object UserData
        {
            get;
            private set;
        }

        public ShowEffectSuccessEventArgs()
        {
            EffectData = null;
            UserData = null;
        }
        
        public static ShowEffectSuccessEventArgs Create(EffectData effectData, object userData = null)
        {
            ShowEffectSuccessEventArgs loadLevelEventArgs = ReferencePool.Acquire<ShowEffectSuccessEventArgs>();
            loadLevelEventArgs.EffectData = effectData;
            loadLevelEventArgs.UserData = userData;
            return loadLevelEventArgs;
        }
        
        
        public override void Clear()
        {
            EffectData = null;
            UserData = null;
        }
    }
}