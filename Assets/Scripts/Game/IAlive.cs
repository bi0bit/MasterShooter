using System;

namespace Game
{
    public interface IAlive
    {
        event Action OnDie;
        
        bool IsAlive();
        void Die();
    }
}