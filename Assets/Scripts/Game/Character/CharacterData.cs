using UnityEngine;

namespace Game.Character
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Assets/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private float _speed;

        public float Speed => _speed;
    }
}