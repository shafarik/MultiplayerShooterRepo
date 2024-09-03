using Assets.Scripts.Enemy;
using Assets.Scripts.Player;
using Assets.Scripts.Player.Components;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class BombPickup : BasePickup
    {
        public float BombRadius;
        public override bool GivePickupTo(Collider2D collision)
        {

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, BombRadius);

            foreach (var collider in colliders)
            {
                if (collider.gameObject.tag == "Enemy")
                {
                    collider.GetComponent<EnemyHeathComponent>().TakeDamage(2000);
                }
            }
            return true;
        }
    }
}