using Assets.Scripts.Player;
using Assets.Scripts.Player.Components;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class HealthPickup : BasePickup
    {
        public int HealAmount = 50;
        public override bool GivePickupTo(Collider2D collision)
        {
            if (collision.GetComponent<PlayerCharacter>() != null)
            {
                collision.GetComponent<PlayerHealthComponent>().SetHealth(HealAmount);
                return true;
            }

            return false;
        }
    }
}