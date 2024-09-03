using Assets.Scripts.Weapon;
using Fusion;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemySkeletonCharacter : EnemyCharacter
    {
        [SerializeField] private NetworkPrefabRef _projectilePrefab;
        [SerializeField] private WeaponStats _skeletonProjectileStat;
        public override void Attack()
        {
            Runner.Spawn(_projectilePrefab, transform.position, Quaternion.identity, null, (runner, o) =>
            {
                o.GetComponent<Bullet>().Init(null, _skeletonProjectileStat, target - transform.position);

            });
        }
    }
}