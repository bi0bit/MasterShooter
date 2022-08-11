using System;
using System.Collections;
using UnityEngine;
using Utils;

namespace Game
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private BulletData _bulletData;

        public Vector3 Direction { get; set; }
        
        private Rigidbody _rigidbody;
        private ObjectPoolContainer _objectPoolContainer;
        private TrailRenderer _trailRenderer;

        private Coroutine _delayDestroyCoroutine;
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _trailRenderer = GetComponent<TrailRenderer>();
            _objectPoolContainer = GetComponentInParent<ObjectPoolContainer>();
        }

        private void OnEnable()
        {
            _delayDestroyCoroutine = StartCoroutine(DelayRelease());
        }

        private void OnDisable()
        {
            if(_delayDestroyCoroutine != null)
                StopCoroutine(_delayDestroyCoroutine);
            _trailRenderer.Clear();
        }

        private IEnumerator DelayRelease()
        {
            yield return new WaitForSeconds(7f);
            Release();
        }

        private void Release()
        {
            if (_objectPoolContainer != null)
            {
                _objectPoolContainer.ObjectPool.Release(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IAlive alive))
            {
                alive.Die();
            }
            Release();
        }

        private void FixedUpdate()
        {
            _rigidbody.MovePosition(transform.position + Direction * _bulletData.Speed * Time.fixedDeltaTime);
        }
    }
}