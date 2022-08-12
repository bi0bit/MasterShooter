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

        private APlayerInputBase _input;

        private void Awake()
        {
            _player = GetComponent<CharacterPlayer>();
            _camera = Camera.main;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
            _input = new PlayerInputPC();
#elif UNITY_ANDROID || UNITY_IOS
            _input = new PlayerInputMobile();
#endif
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
            if (_input != null && _input.CheckInput(out Vector2 position))
            {
                StartCoroutine(DelayShoot());
                var ray = _camera.ScreenPointToRay(position);
                var target = ray.GetPoint(10);
                _player.UpdateGoalShoot(target);
            }
        }

        private void LateUpdate()
        {
            if (!_canShoot) return;
            if (_input != null && _input.CheckInput(out Vector2 position))
            {
                _player.Shoot();
            }
        }
    }
}