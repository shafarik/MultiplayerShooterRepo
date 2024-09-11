using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Weapon
{
    public class WeaponScript : MonoBehaviour
    {
        public NetworkPrefabRef BulletPrefab;
        public WeaponStats WeaponStat;

        public int CurrentAmmoAmount;

        public GameObject WeaponObject;

        private void Start()
        {
            CurrentAmmoAmount = WeaponStat.AmmoAmount;
        }

        public void DecreaseAmmo()
        {
            CurrentAmmoAmount--;
        }

        public bool CanFire()
        {
            return CurrentAmmoAmount == 0 ? false : true;
        }

        public void AddAmmo(int newAmmo)
        {
            CurrentAmmoAmount = Mathf.Clamp(CurrentAmmoAmount + newAmmo, 0, WeaponStat.AmmoAmount);
        }

        public void HideWeapon()
        {
            WeaponObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }
}