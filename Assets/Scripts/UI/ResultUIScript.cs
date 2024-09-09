using Assets.Scripts.Fusion;
using Assets.Scripts.Player;
using Assets.Scripts.Player.Components;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Assets.Scripts.UI
{
    public class ResultUIScript : BasicUIScript
    {
        [SerializeField] private FusionManager _fusionManager;
        [SerializeField] private MyGameSettings _gameSetting;


        public GameObject ResultElement;
        public Transform ResultboardContent;

        public List<Sprite> SkinSpriteList = new List<Sprite>();

        private NetworkObject[] _playersArray;
        private PlayerRef[] _playerRefsArray;
        public List<string> _playerNames = new List<string>();
        private int[] _killValuesArray;
        private int[] _damageValuesArray;
        private void Start()
        {
            _gameSetting.OnWaveRestSwap += CheckFinish;
        }
        public override void ShowCanvas()
        {
            base.ShowCanvas();

        }

        public void CheckFinish(int WaveRestIndex)
        {
            if (WaveRestIndex == 1)
            {
                if (_gameSetting.GetWaveNum() == 3)
                {

                    GetArrays();

                    if (Runner.IsServer)
                    {
                        RpcShowResults(_playersArray, _playerNames.ToArray(), _killValuesArray, _damageValuesArray);
                    }

                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcShowResults(NetworkObject[] playersArray, string[] playerNames, int[] killValuesArray, int[] damageValuesArray)
        {
            ShowCanvas();


            Resulboard(playersArray, playerNames, killValuesArray, damageValuesArray);
        }

        public void Resulboard(NetworkObject[] playersArray, string[] playerNames, int[] killValuesArray, int[] damageValuesArray)
        {
            foreach (Transform child in ResultboardContent.transform)
            {
                Destroy(child.gameObject);
            }



            for (int i = 0; i < playersArray.Length; i++)
            {
                GameObject ResultboardElement = Instantiate(ResultElement, ResultboardContent);
                ResultboardElement.GetComponent<ResultsElementScript>().NewResultElement(
                    playerNames[i],
                    damageValuesArray[i],
                    killValuesArray[i],
                    playersArray[i].GetComponent<PlayerCharacter>().PlayerIsDead() ? "Dead" : "Alive",
                    SkinSpriteList[playersArray[i].GetComponent<PlayerCharacter>().SkinIndex]
                    );
            }
        }

        public void GetArrays()
        {
            _playersArray = _fusionManager.GetAllPlayers();
            _playerRefsArray = _fusionManager.GetAllPlayerRefs();
            _killValuesArray = _fusionManager.GetPlayersKills();
            _damageValuesArray = _fusionManager.GetPlayersDamage();

            if (_playerRefsArray == null)
            {
                Debug.Log("_playerRefsArray = null");
            }
            else
            {
                foreach (var playerRef in _playerRefsArray)
                {
                    _playerNames.Add(playerRef.ToString());
                }
            }
        }

    }
}