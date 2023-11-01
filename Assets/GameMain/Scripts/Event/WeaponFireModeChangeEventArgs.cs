using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class WeaponFireModeChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WeaponFireModeChangeEventArgs).GetHashCode();
        
        public override void Clear()
        {
            LastFireMode = FireMode.None;
            CurrentFireMode = FireMode.None;
        }

        public override int Id => EventId;

        public FireMode LastFireMode
        {
            get;
            private set;
        }
        
        public FireMode CurrentFireMode
        {
            get;
            private set;
        }

        public WeaponFireModeChangeEventArgs()
        {
            LastFireMode = FireMode.None;
            CurrentFireMode = FireMode.None;
        }
        
        public static WeaponFireModeChangeEventArgs Create(FireMode lastFireMode, FireMode currentFireMode, object userData = null)
        {
            WeaponFireModeChangeEventArgs weaponFireModeChangeEventArgs = ReferencePool.Acquire<WeaponFireModeChangeEventArgs>();
            weaponFireModeChangeEventArgs.LastFireMode = lastFireMode;
            weaponFireModeChangeEventArgs.CurrentFireMode = currentFireMode;
            return weaponFireModeChangeEventArgs;
        }
    }
}