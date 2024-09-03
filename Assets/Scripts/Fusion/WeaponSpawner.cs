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

        private void Start()
        {
            _fusionManager.OnPlayerSpawned += SpawnWeapon;
        }
        public void SpawnWeapon(NetworkObject player)
        {
            Debug.Log("Spawn Weapon");

            int RandomIndex = Random.Range(0, _weaponPool.Count);

            NetworkObject networkWeaponObject = Runner.Spawn(_weaponPool[RandomIndex], Vector3.zero, Quaternion.identity, null);

            _weaponPool.RemoveAt(RandomIndex);

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