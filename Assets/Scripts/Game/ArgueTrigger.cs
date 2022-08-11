using System;
using System.Collections.Generic;
using System.Linq;
using Game.Character;
using UnityEngine;

namespace Game
{
    public class ArgueTrigger : MonoBehaviour, IActionTrigger
    {

        private List<IArgue> _arguesEntities;
        private CharacterPlayer _characterPlayer;
        private int _countDie;
        
        public event Action OnEndAction;
        
        
        private void Start()
        {
            _arguesEntities = GetComponentsInChildren<IArgue>().ToList();
            _arguesEntities.ForEach(argueEntity =>
            {
                IAlive alive;
                if ((alive = argueEntity as IAlive) != null)
                {
                    alive.OnDie += OnDieEntity;
                }
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterPlayer alive))
            {
                _characterPlayer = alive;
                _characterPlayer.StartShooting();
                _arguesEntities.ForEach(argueEntity => argueEntity.Argue(alive));
            }
        }
        
        private void OnDieEntity()
        {
            _countDie++;
            if (_countDie == _arguesEntities.Count)
            {
                _characterPlayer.EndShooting();
                OnEndAction?.Invoke();
            }
        }

    }
}