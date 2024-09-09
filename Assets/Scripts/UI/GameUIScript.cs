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

        [Space]
        [Space]
        public TMP_Text TMPTimerText;
        public TMP_Text TMPHealthText;
        public TMP_Text TMPKillsText;
        public TMP_Text TMPAmmosText;
        public TMP_Text TMPDamageText;

        [Space]
        [Space]
        [SerializeField] private GameObject StartGameButton;

        private string _waveTimerText = "Wave time: ";
        private string _restTimerText = "Rest time: ";
        private string _timerText = "";

        private string _playerHeathText = "Health: ";
        private int _playerHealthAmount = -1;

        private string _playerKillsText = "Kills: ";
        private int _playerKillsAmount = -1;

        private string _playerAmmosText = "Ammo: ";
        private int _playerAmmosAmount = -1;

        private string _playerDamageText = "Damage: ";
        private int _playerDamageAmount = -1;

        private PlayerRef[] PlayerRefsArray;
        private int[] _healthValuesArray;
        private int[] _killValuesArray;
        private int[] _ammoValuesArray;
        private int[] _damageValuesArray;


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

        public void OnStartButtonClicked()
        {
            StartGameButton.SetActive(false);
        }

        public void CheckStartGameButton()
        {
            if (!Runner.IsServer)
            {
                StartGameButton.SetActive(false);
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

                    UpdatePlayerAmmo();

                    UpdatePlayerDamage();

                    RpcUpdateHealthKillValues(PlayerRefsArray, _healthValuesArray, _killValuesArray, _ammoValuesArray, _damageValuesArray);
                }
            }
        }

        private void UpdatePlayerHealth()
        {

            _healthValuesArray = _fusionManager.GetPlayersHealth();
        }
        private void UpdatePlayerAmmo()
        {

            _ammoValuesArray = _fusionManager.GetPlayersAmmo();
        }
        private void UpdatePlayerKills()
        {
            _killValuesArray = _fusionManager.GetPlayersKills();
        }

        private void UpdatePlayerDamage()
        {
            _damageValuesArray = _fusionManager.GetPlayersDamage();
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

            if (_playerAmmosAmount >= 0)
            {
                TMPAmmosText.text = _playerAmmosText + _playerAmmosAmount + "/40";
            }

            if (_playerDamageAmount >= 0)
            {
                TMPDamageText.text = _playerDamageText + _playerDamageAmount;
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
            if (Runner != null)
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
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcUpdateHealthKillValues(PlayerRef[] PlayerRefs, int[] HealthValues, int[] KillValues, int[] AmmoValues, int[] DamageValues)
        {
            for (int i = 0; i < PlayerRefs.Length; i++)
            {
                Debug.Log("UpdateHealthKill");

                if (Runner.LocalPlayer == PlayerRefs[i])
                {
                    _playerHealthAmount = HealthValues[i];
                    _playerKillsAmount = KillValues[i];
                    _playerAmmosAmount = AmmoValues[i];
                    _playerDamageAmount = DamageValues[i];
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
            CheckStartGameButton();

            ShowCanvas();
        }

    }
}