using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "BulletData", menuName = "Assets/BulletData", order = 0)]
    public class BulletData : ScriptableObject
    {
        [SerializeField] private float _speed;

        public float Speed => _speed;
    }
}