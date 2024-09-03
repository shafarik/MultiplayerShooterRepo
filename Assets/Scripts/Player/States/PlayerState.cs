using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerState
    {
        protected PlayerStateMachine _stateMachine;
        protected PlayerCharacter _player;
        protected Animator _animator;

        protected string _animBoolName;

        public PlayerState(PlayerCharacter player, PlayerStateMachine stateMachine, string animBoolName)
        {
            _stateMachine = stateMachine;
            _player = player;
            _animBoolName = animBoolName;
            _animator = _player.GetComponent<Animator>();
        }

        public virtual void Enter()
        {
            _animator.SetBool(_animBoolName, true);
        }

        public virtual void Update()
        {

        }

        public virtual void Exit()
        {
            _animator.SetBool(_animBoolName, false);

        }
    }
}