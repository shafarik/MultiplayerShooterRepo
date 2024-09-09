using Assets.Scripts.Enemy;
using Fusion;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Components
{
    public class EnemyHeathComponent : HealthComponent
    {
        protected PlayerCharacter _player;

        public override void Death()
        {
            base.Death();

            if (_player != null)
                _player.AddKillToCount();

            GetComponent<EnemyCharacter>().Dead();
            GetComponent<EnemyCharacter>()._animator.SetBool("Dead", true);
        }

        public override void TakeDamage(int damage)
        {
            if (!GetComponent<EnemyCharacter>()._animator.GetBool("Dead"))
            {
                if (_player != null)
                    _player.AddDamageToCount(damage);

                base.TakeDamage(damage);

                if (Runner.IsServer)
                {
                    RpcSetTrigger();
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcSetTrigger()
        {
            GetComponent<EnemyCharacter>()._animator.SetTrigger("Hit");
        }
        public void SetPlayer(PlayerCharacter player)
        {
            if (player != null)
            {
                _player = player;

                Debug.Log("Player set to " + _player.name);

            }
            else
                Debug.Log("Player is null in enemyHealthComponent");
        }

    }
}