using GameFramework;
using GameFramework.Event;

namespace FPS
{
    public class PlayerHPChangeEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(PlayerHPChangeEventArgs).GetHashCode();
        
        public override void Clear()
        {
            LastHP = 0;
            CurrentHP = 0;
        }
        
        public int LastHP
        {
            get;
            private set;
        }
        
        public int CurrentHP
        {
            get;
            private set;
        }

        public override int Id => EventId;
        
        public PlayerHPChangeEventArgs()
        {
            LastHP = 0;
            CurrentHP = 0;
        }
        
        public static PlayerHPChangeEventArgs Create(int lastHP, int currentHP, object userData = null)
        {
            PlayerHPChangeEventArgs playerHPChangeEventArgs = ReferencePool.Acquire<PlayerHPChangeEventArgs>();
            playerHPChangeEventArgs.LastHP = lastHP;
            playerHPChangeEventArgs.CurrentHP = currentHP;
            return playerHPChangeEventArgs;
        }
    }
}