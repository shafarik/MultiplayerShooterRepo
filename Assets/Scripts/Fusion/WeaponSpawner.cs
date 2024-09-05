using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;


namespace Assets.Scripts.Fusion
{
    public class WeaponSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkPrefabRef _weaponPrefab;
        [SerializeField] private FusionManager _fusionManager;

        [SerializeField] private List<NetworkPrefabRef> _weaponPool = new List<NetworkPrefabRef>();

        private List<NetworkObject> _weaponList = new List<NetworkObject>();
        private List<NetworkObject> _playerList = new List<NetworkObject>();


        private void Start()
        {
            _fusionManager.OnPlayerSpawned += SpawnWeapon;
            _fusionManager.OnPlayerSpawned += CheckPlayerNum;
        }
        public void SpawnWeapon(NetworkObject player)
        {
            if (Runner.IsServer)
            {
                Debug.Log("Spawn Weapon");

                int RandomIndex = Random.Range(0, _weaponPool.Count);

                NetworkObject networkWeaponObject = Runner.Spawn(_weaponPool[RandomIndex], Vector3.zero, Quaternion.identity, null);

                _weaponList.Add(networkWeaponObject);
                _playerList.Add(player);

                _weaponPool.RemoveAt(RandomIndex);

                //networkWeaponObject.transform.SetParent(player.transform);
                //networkWeaponObject.transform.localPosition = new Vector3(0, -0.2f, 0);
                //networkWeaponObject.transform.localRotation = Quaternion.identity;

                RpcSetParent(networkWeaponObject, player);
            }
        }

        public void CheckPlayerNum(NetworkObject player)
        {
            if (_fusionManager.GetAllPlayerRefs().Length > 1)
            {
                if (Runner.IsServer)
                {
                    RpcSetParent(_weaponList[0], _playerList[0]);
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RpcSetParent(NetworkObject networkWeaponObject, NetworkObject player)
        {
            networkWeaponObject.transform.SetParent(player.transform);
            networkWeaponObject.transform.localPosition = new Vector3(0, -0.2f, 0);
            networkWeaponObject.transform.localRotation = Quaternion.identity;
        }

        private void OnDestroy()
        {
            _fusionManager.OnPlayerSpawned -= SpawnWeapon;
        }
    }
}