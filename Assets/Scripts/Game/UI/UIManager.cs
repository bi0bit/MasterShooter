using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _startPanel;
        [SerializeField] private RectTransform _endPanel;
        [SerializeField] private RectTransform _gameOverPanel;

        [SerializeField] private UnityEvent _onTapStartGame;
        [SerializeField] private UnityEvent _onTapRepeatGame;
        
        public void ShowEndPanel()
        {
            _endPanel.DOLocalMoveX(0, 1f)
                .From(-_endPanel.rect.width)
                .OnStart(() =>
                {
                    _endPanel.gameObject.SetActive(true);
                });
        }
        
        public void ShowGameOverPanel()
        {
            _gameOverPanel.DOLocalMoveX(0, 1f)
                .From(-_gameOverPanel.rect.width)
                .OnStart(() =>
                {
                    _gameOverPanel.gameObject.SetActive(true);
                });
        }

        public void HideStartPanel()
        {
            _startPanel.DOLocalMoveX(-_startPanel.rect.width, 1f)
                .From(0)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    _startPanel.gameObject.SetActive(false);
                    _onTapStartGame.Invoke();
                });
        }

        public void OnTapRepeatGame()
        {
            _onTapRepeatGame.Invoke();
        }
    }
}