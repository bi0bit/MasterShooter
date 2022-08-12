using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Character.CharacterPlayer _characterPlayer;
        [SerializeField] private Character.Girl _girl;
        [SerializeField] private WayMove _wayMove;

        [SerializeField] private UIManager _uiManager;
        
        private void Awake()
        {
            _wayMove.SetTarget(_characterPlayer);
            _wayMove.OnReachedFinish += EndGame;
            _characterPlayer.OnStartShooting += StartShooting;
            _characterPlayer.OnEndShooting += EndShooting;
            _characterPlayer.OnDie += GameOver;
        }

        private void EndGame()
        {
            _characterPlayer.Dance();
            _girl.Dance();
            _uiManager.ShowEndPanel();
        }

        private void GameOver()
        {
            _characterPlayer.EndShooting();
            EndShooting();
            _uiManager.ShowGameOverPanel();
        }

        private void EndShooting()
        {
            Time.timeScale = 1f;
        }

        private void StartShooting()
        {
            Time.timeScale = .45f;
        }

        public void RepeatGame()
        {
            SceneManager.LoadScene("SampleScene");
        }

        public void StartWay()
        {
            _wayMove.StartWay();
        }
    }
}