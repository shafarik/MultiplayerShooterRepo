using Assets.Scripts.Enemy;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Components
{
    public class EnemyHeathComponent : HealthComponent
    {

        public override void Death()
        {
            base.Death();

            GetComponent<EnemyCharacter>().Dead();
            GetComponent<EnemyCharacter>()._animator.SetBool("Dead", true);
        }

        public override void TakeDamage(int damage)
        {
            if (!GetComponent<EnemyCharacter>()._animator.GetBool("Dead"))
            {
                base.TakeDamage(damage);

                GetComponent<EnemyCharacter>()._animator.SetTrigger("Hit");
            }
        }
    }
}