using Assets.Scripts.Player;
using Assets.Scripts.Player.Components;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public class Bullet : NetworkBehaviour
    {
        private Vector3 _moveDirection;
        private PlayerCharacter _player;
        private WeaponStats _stats;
        public string TargetTagName = "Enemy";

        public void Init(PlayerCharacter player, WeaponStats stats, Vector3 direction)
        {
            _player = player;
            _stats = stats;
            _moveDirection = direction;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Вычисляем угол в градусах

            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

            Destroy(gameObject, 2.0f);
        }

        public override void FixedUpdateNetwork()
        {
            transform.position += _moveDirection.normalized * _stats.BulletSpeed * Runner.DeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Triggered");

            if (collision.gameObject.tag == TargetTagName)
            {
                if (_player != null)
                    collision.gameObject.GetComponent<EnemyHeathComponent>().SetPlayer(_player);

                collision.gameObject.GetComponent<EnemyHeathComponent>().TakeDamage(_stats.Damage);
            }
        }
    }
}
