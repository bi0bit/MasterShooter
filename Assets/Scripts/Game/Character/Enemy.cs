using System;
using System.Collections;
using UnityEngine;

namespace Game.Character
{
    public class Enemy : CharacterBase, IArgue
    {
        private static readonly int RunAnimParam = Animator.StringToHash("Run");
        private static readonly int HookAnimParam = Animator.StringToHash("Hook");
        private static readonly int VictoryAnimParam = Animator.StringToHash("Victory");

        private IAlive _target;
        
        private bool _isMoving;
        private bool _isAttacking;

        private Coroutine _delayAttack;

        public override event Action OnDie;

        protected override void Start()
        {
            base.Start();
            DisableRagdoll();
        }

        private void FixedUpdate()
        {
            MonoBehaviour monoBehaviour;
            if (_isAlive && _target != null && _target.IsAlive() && (monoBehaviour = _target as MonoBehaviour) != null)
            {
                Vector3 direction = monoBehaviour.transform.position - transform.position;
                if (direction.magnitude > 2.3f)
                {
                    UpdateMove();
                    _controller.Move(direction.normalized * (_characterData.Speed * Time.fixedDeltaTime));
                    if (direction != Vector3.zero)
                    {
                        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                        transform.rotation =
                            Quaternion.RotateTowards(transform.rotation, toRotation, 60 * Time.fixedDeltaTime);
                    }
                }
                else if(!_isAttacking)
                {
                    _isAttacking = true;
                    _animator.SetTrigger(HookAnimParam);
                    _delayAttack = StartCoroutine(DelayAttack());
                }

            }
        }

        private void UpdateMove()
        {
            if (!_isMoving)
            {
                _isMoving = true;
                _animator.SetTrigger(RunAnimParam);
            }
        }

        private void Victory()
        {
            _animator.SetTrigger(VictoryAnimParam);
        }

        private IEnumerator DelayAttack()
        {
            yield return new WaitForSeconds(1.6f);
            if(_target.IsAlive())
                _target.Die();
        }

        
        private void SetRagdollState(bool state)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (var cld in colliders)
            {
                cld.enabled = state;
            }
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (var rg in rigidbodies)
            {
                rg.isKinematic = !state;
            }
        }

        private void DisableRagdoll()
        {
            SetRagdollState(false);
            _animator.enabled = true;
            _controller.enabled = true;
        }
        
        private void EnableRagdoll()
        {
            SetRagdollState(true);
            _animator.enabled = false;
            _controller.enabled = false;
        }
        
        public override void Die()
        {
            if(_delayAttack != null)
                StopCoroutine(_delayAttack);
            if (_target != null)
                _target.OnDie -= Victory;
            EnableRagdoll();
            OnDie?.Invoke();
            _isAlive = false;
        }
        
        
        public void Argue(IAlive aliveEntity)
        {
            if (aliveEntity.IsAlive())
            {
                _target = aliveEntity;
                _target.OnDie += Victory;
            }
        }
    }
}