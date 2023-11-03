using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class ShowItemModUIEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowItemModUIEventArgs).GetHashCode();
        
        public override void Clear()
        {
            ShowMod = Mod.None;
        }
        
        public Mod ShowMod
        {
            get;
            private set;
        }
        
        public override int Id => EventId;
        
        public ShowItemModUIEventArgs()
        {
            ShowMod = Mod.None;
        }
        
        public static ShowItemModUIEventArgs Create(Mod mod, object userData = null)
        {
            ShowItemModUIEventArgs showItemModUIEventArgs = ReferencePool.Acquire<ShowItemModUIEventArgs>();
            showItemModUIEventArgs.ShowMod = mod;
            return showItemModUIEventArgs;
        }
    }
}