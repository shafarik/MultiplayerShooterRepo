using Assets.Scripts.Fusion;
using Assets.Scripts.Player;
using Assets.Scripts.Player.Components;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class GameUIScript : BasicUIScript
    {

        [SerializeField] private MyGameSettings _gameSettings;
        [SerializeField] private FusionManager _fusionManager;

        public TMP_Text TMPTimerText;
        public TMP_Text TMPHealthText;
        public TMP_Text TMPKillsText;

        private string _waveTimerText = "Wave time: ";
        private string _restTimerText = "Rest time: ";
        private string _timerText = "";

        private string _playerHeathText = "Health: ";
        private int _playerHealthAmount = -1;

        private string _playerKillsText = "Kills: ";
        private int _playerKillsAmount = -1;

        private PlayerRef[] PlayerRefsArray;
        private int[] _healthValuesArray;
        private int[] _killValuesArray;


        private bool _isWaveNow;

        // Use this for initialization
        void Start()
        {
            _gameSettings.OnWaveRestSwap += UpdateTimerUI;

            _fusionManager.OnPlayerSpawned += ShowCanvasOnConnetct;

            _fusionManager.OnPlayerSpawned += SetPlayerRefsArray;
        }

        private void OnDestroy()
        {
            _gameSettings.OnWaveRestSwap -= UpdateTimerUI;

            _fusionManager.OnPlayerSpawned -= ShowCanvasOnConnetct;

            _fusionManager.OnPlayerSpawned -= SetPlayerRefsArray;
        }

        // Update is called once per frame
        void Update()
        {
            if (Runner != null)
            {
                if (Runner.IsServer)
                {
                    UpdateTimerText();

                    UpdateValues();
                }
                UpdatePlayerStatText();
            }

        }

        private void ShowCanvasOnConnetct(NetworkObject player)
        {
            RpcShowCanvas();
        }

        public void SetPlayerRefsArray(NetworkObject player)
        {
            PlayerRefsArray = _fusionManager.GetAllPlayerRefs();
        }

        private void UpdateValues()
        {
            if (Runner != null)
            {

                if (Runner.IsServer)
                {
                    UpdatePlayerHealth();

                    UpdatePlayerKills();

                    RpcUpdateHealthKillValues(PlayerRefsArray, _healthValuesArray, _killValuesArray);
                }
            }
        }

        private void UpdatePlayerHealth()
        {

            _healthValuesArray = _fusionManager.GetPlayersHealth();
        }
        private void UpdatePlayerKills()
        {
            _killValuesArray = _fusionManager.GetPlayersKills();
        }

        private void UpdatePlayerStatText()
        {

            if (_playerHealthAmount >= 0)
            {
                TMPHealthText.text = _playerHeathText + _playerHealthAmount;
            }

            if (_playerKillsAmount >= 0)
            {
                TMPKillsText.text = _playerKillsText + _playerKillsAmount;
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
                if (Runner.IsServer && _isWaveNow)
                {
                    RpcUpdateTimer(_timerText, _gameSettings.WaveTimer);

                    TMPTimerText.text = _timerText + _gameSettings.WaveTimer;
                }
                else
                {
                    RpcUpdateTimer(_timerText, _gameSettings.RestTimer);

                    TMPTimerText.text = _timerText + _gameSettings.RestTimer;
                }
            }
        }

        public void UpdateTimerUI(int index)
        {
            if (Runner.IsServer)
            {

                switch (index)
                {
                    case 0:
                        RpcUpdateText(_waveTimerText);
                        _timerText = _waveTimerText;
                        _isWaveNow = true;
                        break;
                    case 1:
                        RpcUpdateText(_restTimerText);
                        _timerText = _restTimerText;
                        _isWaveNow = false;
                        break;
                    default:
                        break;
                }
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcUpdateHealthKillValues(PlayerRef[] PlayerRefs, int[] HealthValues, int[] KillValues)
        {
            for (int i = 0; i < PlayerRefs.Length; i++)
            {
                Debug.Log("UpdateHealthKill");

                if (Runner.LocalPlayer == PlayerRefs[i])
                {
                    _playerHealthAmount = HealthValues[i];
                    _playerKillsAmount = KillValues[i];
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcUpdateText(string text)
        {
            _timerText = text;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcUpdateTimer(string text, float value)
        {
            TMPTimerText.text = text + value;
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcShowCanvas()
        {
            ShowCanvas();
        }

    }
}