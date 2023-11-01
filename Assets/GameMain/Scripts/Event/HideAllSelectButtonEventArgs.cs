using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class HideAllSelectButtonEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HideAllSelectButtonEventArgs).GetHashCode();
        
        public override void Clear()
        {
            
        }


        public override int Id => EventId;
        
        public HideAllSelectButtonEventArgs()
        {
            
        }
        
        public static HideAllSelectButtonEventArgs Create(object userData = null)
        {
            HideAllSelectButtonEventArgs hideAllSelectButtonEventArgs = ReferencePool.Acquire<HideAllSelectButtonEventArgs>();
            return hideAllSelectButtonEventArgs;
        }
    }
}