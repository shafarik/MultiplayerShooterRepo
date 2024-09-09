using Assets.Scripts.Fusion;
using Assets.Scripts.Player;
using Fusion;
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
            if (localPlayerObj != null)
            {
                if (!IsSpectatorMode)
                    transform.position = new Vector3(localPlayerObj.transform.position.x, localPlayerObj.transform.position.y, transform.position.z);
                else if (otherPlayerObj != null)
                    transform.position = new Vector3(otherPlayerObj.transform.position.x, otherPlayerObj.transform.position.y, transform.position.z);
            }

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

        public void ToSpectatorMode()
        {
            IsSpectatorMode = true;
        }
    }
}