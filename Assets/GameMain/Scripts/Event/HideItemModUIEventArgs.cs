using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class HideItemModUIEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(HideItemModUIEventArgs).GetHashCode();
        
        public override void Clear()
        {
            HideMod = Mod.None;
        }
        
        public Mod HideMod
        {
            get;
            private set;
        }
        
        public override int Id => EventId;
        
        public HideItemModUIEventArgs()
        {
            HideMod = Mod.None;
        }
        
        public static HideItemModUIEventArgs Create(Mod mod, object userData = null)
        {
            HideItemModUIEventArgs hideItemModUIEventArgs = ReferencePool.Acquire<HideItemModUIEventArgs>();
            hideItemModUIEventArgs.HideMod = mod;
            return hideItemModUIEventArgs;
        }
    }
}