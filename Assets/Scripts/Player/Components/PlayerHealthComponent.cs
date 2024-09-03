using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.Components
{
    public class PlayerHealthComponent : HealthComponent
    {

        public override void Death()
        {
            base.Death();

            GetComponent<PlayerCharacter>().CharacterIsDead();
        }
    }
}