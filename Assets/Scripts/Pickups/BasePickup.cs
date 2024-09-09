using Assets.Scripts.Player;
using Assets.Scripts.Player.Components;
using Fusion;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class BasePickup : NetworkBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player" && !collision.GetComponent<PlayerHealthComponent>().IsDead)
            {
                GivePickupTo(collision);

                Destroy(this.gameObject);
            }
        }

        public virtual bool GivePickupTo(Collider2D collision)
        {
            return false;
        }
    }
}