using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class WayMove : MonoBehaviour
    {
        private IMove _targetObject;
        
        private List<WayPoint> _wayPoints;
        private List<IActionTrigger> _actionTriggers;

        private int _currentPoint = -1;
        private int _nextPoint;
        private bool _isFinish;
        private bool _isStarted;

        private WayPoint _lastReached;

        public event Action OnReachedFinish;


        private void Awake()
        {
            _wayPoints = GetComponentsInChildren<WayPoint>().ToList();
            _actionTriggers = GetComponentsInChildren<IActionTrigger>().ToList();
        }

        private void OnEnable()
        {
            AddListener();
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void AddListenerOnReachedPoint()
        {
            if(_targetObject != null)
                _targetObject.OnReachedPoint += OnReachedPoint;
        }
        
        private void RemoveListenerOnReachedPoint()
        {
            if(_targetObject != null)
                _targetObject.OnReachedPoint -= OnReachedPoint;
        }


        private void AddListener()
        {
            AddListenerOnReachedPoint();
            _actionTriggers.ForEach(actionTriggers =>
            {
                actionTriggers.OnEndAction += NextPoint;
            });
        }

        private void RemoveListener()
        {
            RemoveListenerOnReachedPoint();
            _actionTriggers.ForEach(actionTriggers =>
            {
                actionTriggers.OnEndAction -= NextPoint;
            });
        }
        
        private void FixedUpdate()
        {
            if (_wayPoints.Count == 0)
            {
                Debug.LogWarning("Not exiting way points in way");
                return;
            }

            if (_targetObject == null)
            {
                Debug.LogWarning("Target not set");
                return;
            }
            if (_isStarted && _nextPoint > _currentPoint && !_isFinish)
            {
                _targetObject.Move(_wayPoints[_nextPoint]);
            }
        }

        private void OnReachedPoint(WayPoint point)
        {
            if (point == _lastReached) return;
            _lastReached = point;
            if (point.ActionPoint)
            {
                _targetObject.Stop();
            }
            else
            {
                _nextPoint++;
            }
            _currentPoint++;
            if (_currentPoint == _wayPoints.Count - 1)
            {
                _isFinish = true;
                OnReachedFinish?.Invoke();
            }
        }

        public void StartWay()
        {
            _isStarted = true;
        }
        
        public void SetTarget(IMove target)
        {
            _targetObject = target;
            RemoveListenerOnReachedPoint();
            AddListenerOnReachedPoint();
        }

        public void NextPoint()
        {
            _nextPoint++;
        }
        
    }
}