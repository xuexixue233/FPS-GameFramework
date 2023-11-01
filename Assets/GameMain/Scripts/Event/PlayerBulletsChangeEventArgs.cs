using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class PlayerBulletsChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerBulletsChangeEventArgs).GetHashCode();
        
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

        public PlayerBulletsChangeEventArgs()
        {
            LastBullets = 0;
            CurrentBullets = 0;
        }
        
        public static PlayerBulletsChangeEventArgs Create(int lastBullets, int currentBullets, object userData = null)
        {
            PlayerBulletsChangeEventArgs playerBulletsChangeEventArgs = ReferencePool.Acquire<PlayerBulletsChangeEventArgs>();
            playerBulletsChangeEventArgs.LastBullets = lastBullets;
            playerBulletsChangeEventArgs.CurrentBullets = currentBullets;
            return playerBulletsChangeEventArgs;
        }
        
    }
}