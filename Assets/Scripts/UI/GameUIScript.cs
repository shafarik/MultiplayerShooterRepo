using Assets.Scripts.Fusion;
using Assets.Scripts.Player;
using Assets.Scripts.Player.Components;
using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class GameUIScript : NetworkBehaviour
    {

        [SerializeField] private MyGameSettings _gameSettings;
        [SerializeField] private FusionManager _fusionManager;

        public TMP_Text TMPTimerText;
        public TMP_Text TMPHealthText;

        private string _waveTimerText = "Wave time: ";
        private string _restTimerText = "Rest time: ";
        private string _timerText = "";

        private string _playerHeathText = "Health: ";
        private int _playerHealthAmount = -1;


        private bool _isWaveNow;

        // Use this for initialization
        void Start()
        {
            _gameSettings.OnWaveRestSwap += UpdateTimerUI;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateTimerText();

            UpdatePlayerHealth();

        }

        private void UpdatePlayerHealth()
        {
            if (Runner != null && Runner.LocalPlayer != null)
            {
                _playerHealthAmount = _fusionManager.GetPlayerNetworkObject(Runner.LocalPlayer).GetComponent<PlayerHealthComponent>().CurrentHealth;
                if (_playerHealthAmount >= 0)
                {
                    TMPHealthText.text = _playerHeathText + _playerHealthAmount;
                }
            }
        }

        private void UpdateTimerText()
        {
            if (_timerText == "")
            {
                TMPTimerText.text = "Wave was not started";
            }
            else
            {
                if (_isWaveNow)
                    TMPTimerText.text = _timerText + _gameSettings.WaveTimer;
                else
                    TMPTimerText.text = _timerText + _gameSettings.RestTimer;
            }
        }

        public void UpdateTimerUI(int index)
        {
            switch (index)
            {
                case 0:
                    _timerText = _waveTimerText;
                    _isWaveNow = true;
                    break;
                case 1:
                    _timerText = _restTimerText;
                    _isWaveNow = false;
                    break;
                default:
                    break;
            }
        }
    }
}