using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Fusion;
using static Unity.Collections.Unicode;

namespace Assets.Scripts.Player
{
    public class PlayerCharacter : NetworkBehaviour
    {
        public float MoveSpeed = 1.0f;


        public int IsFasingRight = 1;

        public int KillsCount = 0;

        public int SkinIndex = 0;

        private Vector3 _moveDirection;
        private Vector3 _fireDirection;

        private PlayerStateMachine _stateMachine;

        public delegate void BulletFireAction(PlayerCharacter player, Vector3 direction);

        public event BulletFireAction OnBulletFire;

        public delegate void PlayerIsSpawned(PlayerCharacter player);

        public event PlayerIsSpawned OnPlayerSpawned;


        public override void Spawned()
        {
            base.Spawned();

            _stateMachine = new PlayerStateMachine();
            _stateMachine.Init(this);

            OnPlayerSpawned?.Invoke(this);
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

                    if (_stateMachine.CurrentState != _stateMachine.IdleState && data.direction == Vector3.zero)
                    {
                        _stateMachine.ChangeState(_stateMachine.IdleState);
                    }
                    transform.position += data.direction * MoveSpeed * Runner.DeltaTime;

                    if (_fireDirection != Vector3.zero)
                    {
                        Flip();

                        BulletFire(this, _fireDirection);
                    }


                    Flip();
                }
            }
        }

        public void SetFireDirection(Vector3 newDirection)
        {
            _fireDirection = newDirection;
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

        public void AddKillToCount()
        {
            KillsCount++;
        }

        public void BulletFire(PlayerCharacter player, Vector3 direction)
        {
            if (player != null && direction != null)
                OnBulletFire?.Invoke(player, direction);
        }

        public void SetPlayerSkin(RuntimeAnimatorController SkinController)
        {
            GetComponentInChildren<Animator>().runtimeAnimatorController = SkinController;

        }

        public void SetSkinIndex(int index)
        {
            SkinIndex = index;
        }
    }
}
