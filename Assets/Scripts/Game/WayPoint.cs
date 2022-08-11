using UnityEngine;

namespace Game
{
    public class WayPoint : MonoBehaviour
    {
        [SerializeField] private bool _actionPoint;

        public bool ActionPoint => _actionPoint;

        public Vector3 Position => transform.position;
    }
}