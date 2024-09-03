using Assets.Scripts.Player.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerStateMachine
    {

        public PlayerState CurrentState { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerRunState RunState { get; private set; }
        public PlayerDeathState DeathState { get; private set; }
        public void Initiaize(PlayerState _newState)
        {
            CurrentState = _newState;
            CurrentState.Enter();
        }

        public void Init(PlayerCharacter player)
        {
            IdleState = new PlayerIdleState(player, this, "Idle");
            RunState = new PlayerRunState(player, this, "Run");
            DeathState = new PlayerDeathState(player, this, "Death");


            Initiaize(IdleState);
        }

        public void ChangeState(PlayerState _newState)
        {
            CurrentState.Exit();
            CurrentState = _newState;
            CurrentState.Enter();
        }
    }
}
