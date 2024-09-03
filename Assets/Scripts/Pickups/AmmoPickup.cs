using Assets.Scripts.Player;
using Assets.Scripts.Weapon;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Pickups
{
    public class AmmoPickup : BasePickup
    {
        public int AmmoAmount = 40;
        public override bool GivePickupTo(Collider2D collision)
        {
            if (collision != null)
            {
                collision.GetComponentInChildren<WeaponScript>().AddAmmo(AmmoAmount);
                return true;
            }

            return false;
        }
    }
}