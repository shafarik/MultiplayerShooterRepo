using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using Assets.Scripts.Player;
using System.Linq;
using Assets.Scripts.Player.Components;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Fusion
{
    public class FusionManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        private NetworkRunner _runner;

        [SerializeField] private FixedJoystick _movementJoystick;
        [SerializeField] private FixedJoystick _fireJoystick;

        [SerializeField] private NetworkPrefabRef _playerPrefab;
        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        public string SessionNameString = "TestRoom";

        #region Delegates
        public delegate void WeeaponSpawnAction(NetworkObject player);

        public event WeeaponSpawnAction OnPlayerSpawned;

        public delegate void BulletSpawnAction(NetworkObject player, Vector3 direction);

        public event BulletSpawnAction OnNeedToSpawnBullet;

        #endregion

        #region BasicFunctions
        //  public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        //   public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
        // public void OnInput(NetworkRunner runner, NetworkInput input) { }
        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        //  public void OnConnectedToServer(NetworkRunner runner) { }
        //  public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        //  public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        //   public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        //   public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }
        #endregion


        async void StartGame(GameMode mode)
        {
            // Create the Fusion runner and let it know that we will be providing user input
            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = SessionNameString,
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
        //private void Update()
        //{
        //    PrintSpawnedCharacters();
        //}
        public void PrintSpawnedCharacters()
        {
            if (_spawnedCharacters.Count > 0)
            {
                foreach (var kvp in _spawnedCharacters)
                {
                    Debug.Log($"PlayerRef: {kvp.Key}, NetworkObject: {kvp.Value}");
                }

            }
            else
            {
                Debug.Log("_spawnedCharacters is empty");
            }
        }
        public void TriggerPlayerSpawnedAction(NetworkObject player)
        {
            OnPlayerSpawned?.Invoke(player);
        }

        public void TriggerNeedToSpawnBulletAction(NetworkObject player, Vector3 direction)
        {
            OnNeedToSpawnBullet?.Invoke(player, direction);
        }
        public void HostButton(TMP_InputField inputField)
        {
            if (inputField != null)
            {
                if (inputField.text != "")
                {
                    SessionNameString = inputField.text;
                }
            }

            StartGame(GameMode.Host);
        }

        public void ClientButton(TMP_InputField inputField)
        {
            if (inputField != null)
            {
                if (inputField.text != "")
                {
                    SessionNameString = inputField.text;
                }
            }

            StartGame(GameMode.Client);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                // Create a unique position for the player
                Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
                NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                // Keep track of the player avatars for easy access
                _spawnedCharacters.Add(player, networkPlayerObject);

                TriggerPlayerSpawnedAction(networkPlayerObject);
            }

            //if (runner.IsClient)
            //{
            //    Debug.Log("OnPlayerSpawned in Client " + player);

            //    TriggerPlayerSpawnedAction(_spawnedCharacters[player]);
            //}
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }

        public NetworkObject[] GetAllPlayers()
        {
            NetworkObject[] networkObjectsArray = _spawnedCharacters.Values.ToArray();
            return networkObjectsArray;
        }

        public PlayerRef[] GetAllPlayerRefs()
        {
            PlayerRef[] PlayerRefsArray = _spawnedCharacters.Keys.ToArray();
            return PlayerRefsArray;
        }

        public NetworkObject GetPlayerNetworkObject(PlayerRef playerRef)
        {
            return _spawnedCharacters[playerRef];
        }

        public int[] GetPlayersHealth()
        {
            List<int> HealthList = new List<int>();
            foreach (var player in GetAllPlayers())
            {
                HealthList.Add(player.GetComponent<PlayerHealthComponent>().CurrentHealth);
            }
            return HealthList.ToArray();
        }

        public int[] GetPlayersKills()
        {
            List<int> KillsList = new List<int>();
            foreach (var player in GetAllPlayers())
            {
                KillsList.Add(player.GetComponent<PlayerCharacter>().KillsCount);
            }
            return KillsList.ToArray();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();

            data.direction = new Vector3(_movementJoystick.Horizontal, _movementJoystick.Vertical, 0);



            if (_fireJoystick.Horizontal != 0 || _fireJoystick.Vertical != 0)
            {

                Vector3 BulletDirection = new Vector3(_fireJoystick.Horizontal, _fireJoystick.Vertical, 0);

                data.fireDirection = BulletDirection;

                //if (_spawnedCharacters.TryGetValue(runner.LocalPlayer, out NetworkObject playerObject))
                //{
                //    Debug.Log("Fire");
                //    if (playerObject.GetComponent<PlayerCharacter>().CanMoveOrShoot())
                //    {
                //        TriggerNeedToSpawnBulletAction(playerObject, BulletDirection);

                //    }
                //}
            }
            input.Set(data);
        }
    }
}