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
            ItemModUI = null;
        }
        
        public Mod ShowMod
        {
            get;
            private set;
        }

        public ItemModUI ItemModUI
        {
            get;
            private set;
        }
        
        public override int Id => EventId;
        
        public ShowAllSelectButtonEventArgs()
        {
            ShowMod = Mod.None;
            ItemModUI = null;
        }
        
        public static ShowAllSelectButtonEventArgs Create(Mod mod,ItemModUI itemModUI, object userData = null)
        {
            ShowAllSelectButtonEventArgs showAllSelectButtonEventArgs = ReferencePool.Acquire<ShowAllSelectButtonEventArgs>();
            showAllSelectButtonEventArgs.ShowMod = mod;
            showAllSelectButtonEventArgs.ItemModUI = itemModUI;
            return showAllSelectButtonEventArgs;
        }
    }
}