using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Fusion;


namespace Assets.Scripts.Player
{
    public class PlayerCharacter : NetworkBehaviour
    {
        public float MoveSpeed = 1.0f;


        public int IsFasingRight = 1;
        private Vector3 _moveDirection;
        private Vector3 _fireDirection;

        private PlayerStateMachine _stateMachine;

        public override void Spawned()
        {
            base.Spawned();

            _stateMachine = new PlayerStateMachine();
            _stateMachine.Init(this);
        }
        public override void FixedUpdateNetwork()
        {
            if (CanMoveOrShoot())
            {
                if (GetInput(out NetworkInputData data))
                {
                    data.direction.Normalize();
                    _moveDirection = data.direction;
                    _fireDirection = data.fireDirection;

                    if (_stateMachine.CurrentState != _stateMachine.RunState && data.direction != Vector3.zero)
                    {
                        _stateMachine.ChangeState(_stateMachine.RunState);
                    }

                    transform.position += data.direction * MoveSpeed * Runner.DeltaTime;

                    Flip();
                }
            }
        }

        public void Flip()
        {
            if (_moveDirection != Vector3.zero)
            {
                if (IsFasingRight == 1 && _moveDirection.x < 0)
                {
                    transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                    IsFasingRight = -1;
                }
                else if (IsFasingRight == -1 && _moveDirection.x > 0)
                {
                    transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    IsFasingRight = 1;
                }
            }
            else
            {
                if (IsFasingRight == 1 && _fireDirection.x < 0)
                {
                    transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                    IsFasingRight = -1;
                }
                else if (IsFasingRight == -1 && _fireDirection.x > 0)
                {
                    transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                    IsFasingRight = 1;
                }
            }
        }

        public void CharacterIsDead()
        {
            _stateMachine.ChangeState(_stateMachine.DeathState);
        }

        public bool CanMoveOrShoot()
        {
            if (_stateMachine.CurrentState == _stateMachine.DeathState)
                return false;

            return true;
        }
    }
}
