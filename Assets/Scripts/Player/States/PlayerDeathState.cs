﻿using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerDeathState : PlayerState
    {
        public PlayerDeathState(PlayerCharacter player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}