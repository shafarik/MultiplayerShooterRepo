using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;

namespace Assets.Scripts.Fusion
{
    public class PickupSpawner : NetworkBehaviour
    {

        [SerializeField] private FusionManager _fusionManager;

        public float SpawnPickupPositionRange = 15.0f;


        public void SpawnPickups(List<NetworkPrefabRef> pickupsPrefabPool, int pickupsCount)
        {
            if (Runner != null)
            {
            if (Runner.IsServer)
            {
                for (int i = 0; i < pickupsCount; i++)
                {
                    int randomIndex = Random.Range(0, pickupsPrefabPool.Count);

                    float randomX = 0;
                    float randomY = 0;


                    do
                    {
                        randomX = Random.Range(-SpawnPickupPositionRange, SpawnPickupPositionRange + 1);
                        randomY = Random.Range(-SpawnPickupPositionRange + 5, SpawnPickupPositionRange - 5);

                    }
                    while (randomX == randomY);

                    Vector3 randomVector = new Vector3(randomX, randomY, 0f);

                    Runner.Spawn(pickupsPrefabPool[randomIndex], randomVector, Quaternion.identity, null);



                }

            }

            }
        }
    }
}