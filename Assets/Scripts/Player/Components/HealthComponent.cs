using Fusion;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player.Components
{
    public class HealthComponent : NetworkBehaviour
    {
        public int MaxHealth;
        public int CurrentHealth;
        public bool IsDead = false;



        private void Start()
        {
            CurrentHealth = MaxHealth;
        }
        public virtual void TakeDamage(int damage)
        {
            SetHealth(-damage);
        }

        public void SetHealth(int newHealth)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + newHealth, 0, MaxHealth);

            if (CurrentHealth == 0)
            {
                Death();
            }
        }


        public virtual void Death()
        {
            IsDead = true;
        }
    }
}