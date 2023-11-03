using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class ShowWeaponModEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(ShowWeaponModEventArgs).GetHashCode();
        
        public override void Clear()
        {
            ShowMod = Mod.None;
        }
        
        public Mod ShowMod
        {
            get;
            private set;
        }

        public int TypeId
        {
            get;
            private set;
        }
        
        public int OwnerId
        {
            get;
            private set;
        }
        
        public override int Id => EventId;
        
        public ShowWeaponModEventArgs()
        {
            ShowMod = Mod.None;
        }
        
        public static ShowWeaponModEventArgs Create(Mod mod,int typeId,int ownerId , object userData = null)
        {
            ShowWeaponModEventArgs showWeaponModEventArgs = ReferencePool.Acquire<ShowWeaponModEventArgs>();
            showWeaponModEventArgs.ShowMod = mod;
            showWeaponModEventArgs.TypeId = typeId;
            showWeaponModEventArgs.OwnerId = ownerId;
            return showWeaponModEventArgs;
        }
    }
}