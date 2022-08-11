
using System;
using UnityEngine;

namespace Game
{
    public interface IMove
    {
        event Action<WayPoint> OnReachedPoint; 
        
        void Move(WayPoint targetPosition);
        void Stop();
    }
}
