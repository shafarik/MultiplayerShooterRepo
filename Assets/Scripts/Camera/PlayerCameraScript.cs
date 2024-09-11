using Assets.Scripts.Fusion;
using Assets.Scripts.Player;
using Fusion;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Camera
{
    public class PlayerCameraScript : NetworkBehaviour
    {
        [SerializeField] private FusionManager _fusionManager;

        private PlayerRef[] _playerRefs;
        private NetworkObject[] _playerObj;
        private List<NetworkId> netIdList = new List<NetworkId>();

        private NetworkObject localPlayerObj;
        private NetworkObject otherPlayerObj;

        public bool IsSpectatorMode = false;
        // Use this for initialization
        void Start()
        {
            _fusionManager.OnPlayerSpawned += PlayerSpawned;
        }

        private void PlayerSpawned(NetworkObject player)
        {

            _playerObj = _fusionManager.GetAllPlayers();
            _playerRefs = _fusionManager.GetAllPlayerRefs();

            //      GetComponent<CinemachineVirtualCamera>().Follow = player.gameObject.transform;

            if (Runner.IsServer)
            {
                for (int i = 0; i < _playerObj.Length; i++)
                {
                    if (Runner.LocalPlayer == _playerRefs[i])
                    {
                        GetComponent<CinemachineVirtualCamera>().Follow = _playerObj[i].gameObject.transform;
                    }
                    else
                        RpcSetVirtualCameraTarget(_playerObj[i].Id, _playerRefs[i]);
                }
            }
        }

        public void SetPlayerPos()
        {

            if (Runner.IsServer)
            {
                _playerRefs = _fusionManager.GetAllPlayerRefs();
                _playerObj = _fusionManager.GetAllPlayers();

                netIdList.Clear();

                for (int i = 0; i < _playerObj.Length; i++)
                {
                    netIdList.Add(_playerObj[i].Id);
                }

                if (_playerRefs != null && netIdList != null)
                {

                    RpcSetCameraPosition(_playerRefs, netIdList.ToArray());

                }
            }
        }

        void Update()
        {

        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcSetCameraPosition(PlayerRef[] playerRefs, NetworkId[] playerId)
        {
            for (int i = 0; i < playerRefs.Length; i++)
            {
                if (Runner.LocalPlayer == playerRefs[i])
                {
                    localPlayerObj = Runner.FindObject(playerId[i]);

                    localPlayerObj.GetComponent<PlayerCharacter>().OnPlayerDead += ToSpectatorMode;
                }
                else if (Runner.LocalPlayer != playerRefs[i])
                {
                    otherPlayerObj = Runner.FindObject(playerId[i]);
                }
            }
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcSetVirtualCameraTarget(NetworkId PlayerId, PlayerRef _playerRef)
        {
            if (Runner.LocalPlayer == _playerRef)
                GetComponent<CinemachineVirtualCamera>().Follow = Runner.FindObject(PlayerId).transform;
        }

        public void ToSpectatorMode()
        {
            IsSpectatorMode = true;
        }
    }
}