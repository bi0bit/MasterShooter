using System;
using UnityEngine;

namespace Game.Character
{
    public class Girl : CharacterBase
    {
        private static readonly int DanceAnimParam = Animator.StringToHash("Dance");

        
        public override event Action OnDie;
        public override void Die()
        {
            Debug.Log("Girl die not implemented");
        }

        public void Dance()
        {
            _animator.SetTrigger(DanceAnimParam);       
        }
    }
}