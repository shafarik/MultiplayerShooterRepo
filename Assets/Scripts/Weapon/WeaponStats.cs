using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(fileName = "NewWeaponStats", menuName = "Weapon/Stats")]
    public class WeaponStats : ScriptableObject
    {
        public int Damage;
        public int AmmoAmount;
        public int ProjectileAmount;
        public float Distance;
        public float BulletSpeed;
        public float AttackTime;
        public float Spread;
    }
}
