using Assets.Scripts.Enemy;
using Assets.Scripts.Fusion.Data;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Fusion
{
    public class EnemySpawner : NetworkBehaviour
    {

        [SerializeField] private NetworkPrefabRef _enemyPrefab;
        [SerializeField] private FusionManager _fusionManager;

        [SerializeField] private List<NetworkObject> _enemyPool = new List<NetworkObject>();
        [SerializeField] private List<NetworkPrefabRef> _enemiesPrefabPool = new List<NetworkPrefabRef>();

        public int enemiesCount;

        public float SpawnEnemyPositionRange = 30.0f;


        public void SpawnEnemies(List<NetworkPrefabRef> enemiesPrefabPool, int enemiesCount)
        {
            for (int i = 0; i < enemiesCount; i++)
            {
                int randomIndex = Random.Range(0, enemiesPrefabPool.Count);

                float randomX = 0;
                float randomY = 0;


                do
                {
                    randomX = Random.Range(-SpawnEnemyPositionRange, SpawnEnemyPositionRange + 1);
                    randomY = Random.Range(-SpawnEnemyPositionRange, SpawnEnemyPositionRange + 1);

                    Debug.Log("Randomize Iteration");
                }
                while (randomX == randomY);

                Vector3 randomVector = new Vector3(randomX, randomY, 0f);
                Debug.Log("Random vector is " + randomVector);

                NetworkObject SpawnedEnemy = Runner.Spawn(enemiesPrefabPool[randomIndex], randomVector, Quaternion.identity, null, (runner, o) =>
                {
                    o.GetComponent<EnemyCharacter>().Init(_fusionManager);
                });

                _enemyPool.Add(SpawnedEnemy);

            }
        }
        public void SpawnWave()
        {
            SpawnEnemies(_enemiesPrefabPool, enemiesCount);

        }
        public void DespawnAllEnemies()
        {
            foreach (var enemy in _enemyPool)
            {
                Runner.Despawn(enemy);
            }

            _enemyPool.Clear();
        }
    }
}