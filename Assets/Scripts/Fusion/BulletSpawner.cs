using Assets.Scripts.Player;
using Assets.Scripts.Weapon;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Fusion
{
    public class BulletSpawner : NetworkBehaviour
    {

        [SerializeField] private FusionManager _fusionManager;

        private float _timer = 0.0f;

        private void Start()
        {
            _fusionManager.OnNeedToSpawnBullet += SpawnBullet;
        }
        private void OnDestroy()
        {
            _fusionManager.OnNeedToSpawnBullet -= SpawnBullet;
        }
        private void Update()
        {
            _timer -= Time.deltaTime;
        }

        public void SpawnBullet(NetworkObject player, Vector3 direction)
        {
            if (_timer < 0 && player.GetComponentInChildren<WeaponScript>().CanFire())
            {
                _timer = player.GetComponentInChildren<WeaponScript>().WeaponStat.AttackTime;

                int projamount = player.GetComponentInChildren<WeaponScript>().WeaponStat.ProjectileAmount;

                if (projamount > 1)
                {
                    float newDirectionY = direction.y - (projamount / 2) * player.GetComponentInChildren<WeaponScript>().WeaponStat.Spread;

                    for (int i = 0; i < projamount; i++)
                    {
                        direction = new Vector3(direction.x, newDirectionY, direction.z);

                        Vector3 SpawnPosition = player.transform.position +
                        (new Vector3(1 * player.GetComponent<PlayerCharacter>().IsFasingRight, -0.1f, 0));

                        Runner.Spawn(player.GetComponentInChildren<WeaponScript>().BulletPrefab,
                            SpawnPosition,
                            Quaternion.identity,
                            null,
                            (runner, o) =>
                            {
                                o.GetComponent<Bullet>().Init(
                                    player,
                                    player.GetComponentInChildren<WeaponScript>().WeaponStat,
                                    direction);
                            });

                        player.GetComponentInChildren<WeaponScript>().DecreaseAmmo();

                        newDirectionY += player.GetComponentInChildren<WeaponScript>().WeaponStat.Spread;
                    }

                }
                else
                {
                    Vector3 SpawnPosition = player.transform.position +
                        (new Vector3(1 * player.GetComponent<PlayerCharacter>().IsFasingRight, -0.1f, 0));

                    Runner.Spawn(player.GetComponentInChildren<WeaponScript>().BulletPrefab,
                        SpawnPosition,
                        Quaternion.identity,
                        null,
                        (runner, o) =>
                        {
                            o.GetComponent<Bullet>().Init(
                                player,
                                player.GetComponentInChildren<WeaponScript>().WeaponStat,
                                direction);
                        });

                    player.GetComponentInChildren<WeaponScript>().DecreaseAmmo();
                }
            }
        }
    }
}