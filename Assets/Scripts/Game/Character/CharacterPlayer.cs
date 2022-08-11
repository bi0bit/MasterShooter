using System;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Game.Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterPlayer : CharacterBase, IMove
    {
        
        private static readonly int DyingAnimParam = Animator.StringToHash("Dying");
        private static readonly int IdleAnimParam = Animator.StringToHash("Idle");
        private static readonly int RunAnimParam = Animator.StringToHash("Run");
        private static readonly int DanceAnimParam = Animator.StringToHash("Dance");

        [SerializeField] private Transform _aimTarget;
        [SerializeField] private ObjectPoolContainer _bulletPool;
        
        private Vector3 _goalShoot;
        private Quaternion _aimTowards;
        
        private bool _isMoving;
        private bool _isShooting;

        public bool IsShooting => _isShooting;
        
        
        public event Action<WayPoint> OnReachedPoint;
        public event Action OnStartShooting;
        public event Action OnEndShooting;
        public override event Action OnDie;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_goalShoot, .5f);
            if(_aimTarget != null)
                Gizmos.DrawLine(_goalShoot, _aimTarget.position);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (_isShooting)
            {
                if (_goalShoot != default)
                {
                    _animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1);
                    _animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1);  
                    _animator.SetIKPosition(AvatarIKGoal.RightHand,_goalShoot);
                    _animator.SetIKRotation(AvatarIKGoal.RightHand,_aimTowards);
                }
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0); 
                _animator.SetLookAtWeight(0);
            }
        }
        
        private void UpdateRun()
        {
            if (!_isMoving)
            {
                _isMoving = true;
                _animator.SetTrigger(RunAnimParam);
            }
            
        }

        public void Move(WayPoint wayPoint)
        {
            Vector3 direction = wayPoint.Position - transform.position;
            if (direction.magnitude >= .2f)
            {
                UpdateRun();
                _controller.Move(direction.normalized * (_characterData.Speed * Time.fixedDeltaTime));
                if (direction != Vector3.zero)
                {
                    Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                    transform.rotation =
                        Quaternion.RotateTowards(transform.rotation, toRotation, 60 * Time.fixedDeltaTime);
                }
            }
            else
            {
                OnReachedPoint?.Invoke(wayPoint);
            }
            
        }
        
        public void Stop()
        {
            _animator.SetTrigger(IdleAnimParam);
            _isMoving = false;
        }


        public override void Die()
        {
            _isAlive = false;
            _animator.SetTrigger(DyingAnimParam);
            OnDie?.Invoke();
        }

        public void UpdateGoalShoot(Vector3 goal)
        {
            _goalShoot = goal;
            var aimDirection = Vector3.forward;
            var targetDirection = _goalShoot - _aimTarget.position;
            _aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        }

        public void StartShooting()
        {
            OnStartShooting?.Invoke();
            _isShooting = true;
        }

        public void EndShooting()
        {
            OnEndShooting?.Invoke();
            _aimTowards = default;
            _goalShoot = default;
            _isShooting = false;
        }

        public void Shoot()
        {
            var bullet = _bulletPool.ObjectPool.Get<Bullet>(_aimTarget.position);
            bullet.Direction = (_goalShoot - _aimTarget.position).normalized;
        }

        public void Dance()
        {
            _animator.SetTrigger(DanceAnimParam);   
        }
    }
}