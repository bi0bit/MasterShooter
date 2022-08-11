using System.Collections;
using Game.Character;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterPlayer))]
    public class PlayerShooterControl : MonoBehaviour
    {
        [SerializeField] private float _shootDelay;

        private bool _canShoot = true;
        
        private CharacterPlayer _player;
        private Camera _camera;

        private void Awake()
        {
            _player = GetComponent<CharacterPlayer>();
            _camera = Camera.main;
        }

        private IEnumerator DelayShoot()
        {
            yield return new WaitForEndOfFrame();
            _canShoot = false;
            yield return new WaitForSeconds(_shootDelay);
            _canShoot = true;
        }
        
        private void Update()
        {
            if (!_canShoot) return;
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
            if (Input.GetMouseButton((int) MouseButton.LeftMouse))
            {
                StartCoroutine(DelayShoot());
                var mousePos = Input.mousePosition;
                var ray = _camera.ScreenPointToRay(mousePos);
                var target = ray.GetPoint(10);
                _player.UpdateGoalShoot(target);
            }
#else
            
#endif
        }

        private void LateUpdate()
        {
            if (!_canShoot) return;
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
            if (Input.GetMouseButton((int) MouseButton.LeftMouse))
            {
                _player.Shoot();
            }
#else
            
#endif
        }
    }
}