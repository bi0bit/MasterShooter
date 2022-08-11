using System;
using DG.Tweening;
using UnityEngine;

namespace Utils.Animations
{
    public class Rotate360Loop : MonoBehaviour
    {
        [SerializeField] private float _rotateForSeconds;

        private void Start()
        {
            transform
                .DORotate(Vector3.up * 360, _rotateForSeconds, RotateMode.WorldAxisAdd)
                .SetLoops(-1, LoopType.Restart);
        }
    }
}