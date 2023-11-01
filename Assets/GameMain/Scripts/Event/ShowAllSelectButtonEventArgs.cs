using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class ShowAllSelectButtonEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowAllSelectButtonEventArgs).GetHashCode();
        
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
        
        public ShowAllSelectButtonEventArgs()
        {
            ShowMod = Mod.None;
        }
        
        public static ShowAllSelectButtonEventArgs Create(Mod mod, object userData = null)
        {
            ShowAllSelectButtonEventArgs showAllSelectButtonEventArgs = ReferencePool.Acquire<ShowAllSelectButtonEventArgs>();
            showAllSelectButtonEventArgs.ShowMod = mod;
            return showAllSelectButtonEventArgs;
        }
    }
}