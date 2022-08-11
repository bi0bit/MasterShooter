using System;
using UnityEngine;

namespace Game.Character
{
    public abstract class CharacterBase : MonoBehaviour, IAlive
    {
        [SerializeField] protected CharacterData _characterData;
        
        protected CharacterController _controller;
        protected Animator _animator;
        
        protected bool _isAlive = true;

        protected virtual void Start()
        {
            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        public abstract event Action OnDie;
        public abstract void Die();

        public bool IsAlive() => _isAlive;
    }
}