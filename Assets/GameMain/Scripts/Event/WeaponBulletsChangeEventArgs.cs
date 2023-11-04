using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class WeaponBulletsChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(WeaponBulletsChangeEventArgs).GetHashCode();
        
        public override void Clear()
        {
            LastBullets = 0;
            CurrentBullets = 0;
        }

        public override int Id => EventId;

        public int LastBullets
        {
            get;
            private set;
        }
        
        public int CurrentBullets
        {
            get;
            private set;
        }

        public WeaponBulletsChangeEventArgs()
        {
            LastBullets = 0;
            CurrentBullets = 0;
        }
        
        public static WeaponBulletsChangeEventArgs Create(int lastBullets, int currentBullets, object userData = null)
        {
            WeaponBulletsChangeEventArgs weaponBulletsChangeEventArgs = ReferencePool.Acquire<WeaponBulletsChangeEventArgs>();
            weaponBulletsChangeEventArgs.LastBullets = lastBullets;
            weaponBulletsChangeEventArgs.CurrentBullets = currentBullets;
            return weaponBulletsChangeEventArgs;
        }
        
    }
}