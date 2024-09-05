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

        private NetworkObject _player;

        private void Start()
        {
            _fusionManager.OnPlayerSpawned += SetPlayer;
            // _fusionManager.OnNeedToSpawnBullet += SpawnBullet;
        }
        private void OnDestroy()
        {
            _fusionManager.OnPlayerSpawned -= SetPlayer;

            if (_player != null)
                _player.GetComponent<PlayerCharacter>().OnBulletFire -= SpawnBullet;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
        }

        public void SetPlayer(NetworkObject player)
        {
            _player = player;

            _player.GetComponent<PlayerCharacter>().OnBulletFire += SpawnBullet;
        }
        public void SpawnBullet(PlayerCharacter player, Vector3 direction)
        {
            if (Runner.IsServer)
            {
                var PlayerWeaponScript = player.GetComponentInChildren<WeaponScript>();
                if (_timer < 0 && PlayerWeaponScript.CanFire())
                {
                    _timer = PlayerWeaponScript.WeaponStat.AttackTime;

                    int projamount = PlayerWeaponScript.WeaponStat.ProjectileAmount;

                    if (projamount > 1)
                    {
                        float newDirectionY = direction.y - (projamount / 2) * PlayerWeaponScript.WeaponStat.Spread;

                        for (int i = 0; i < projamount; i++)
                        {
                            direction = new Vector3(direction.x, newDirectionY, direction.z);

                            Vector3 SpawnPosition = player.transform.position +
                            (new Vector3(1 * player.IsFasingRight, -0.1f, 0));

                            Runner.Spawn(PlayerWeaponScript.BulletPrefab,
                                SpawnPosition,
                                Quaternion.identity,
                                null,
                                (runner, o) =>
                                {
                                    o.GetComponent<Bullet>().Init(
                                        player,
                                        PlayerWeaponScript.WeaponStat,
                                        direction);
                                });

                            PlayerWeaponScript.DecreaseAmmo();

                            newDirectionY += PlayerWeaponScript.WeaponStat.Spread;
                        }

                    }
                    else
                    {
                        Vector3 SpawnPosition = player.transform.position +
                            (new Vector3(1 * player.IsFasingRight, -0.1f, 0));

                        Runner.Spawn(PlayerWeaponScript.BulletPrefab,
                            SpawnPosition,
                            Quaternion.identity,
                            null,
                            (runner, o) =>
                            {
                                o.GetComponent<Bullet>().Init(
                                    player,
                                    PlayerWeaponScript.WeaponStat,
                                    direction);
                            });

                        PlayerWeaponScript.DecreaseAmmo();
                    }
                }

            }
        }
    }
}