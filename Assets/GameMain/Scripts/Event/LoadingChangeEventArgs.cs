using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class LoadingChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(LoadingChangeEventArgs).GetHashCode();
        
        public override void Clear()
        {
            CurrentProgress = 0;
        }

        public override int Id => EventId;

        public float CurrentProgress
        {
            get;
            private set;
        }

        public LoadingChangeEventArgs()
        {
            CurrentProgress = 0;
        }
        
        public static LoadingChangeEventArgs Create(float currentProgress, object userData = null)
        {
            LoadingChangeEventArgs playerBulletsChangeEventArgs = ReferencePool.Acquire<LoadingChangeEventArgs>();
            playerBulletsChangeEventArgs.CurrentProgress = currentProgress;
            return playerBulletsChangeEventArgs;
        }
    }
}