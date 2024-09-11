using Assets.Scripts.Fusion;
using Assets.Scripts.Player.Components;
using Fusion;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Enemy
{
    public class EnemyCharacter : NetworkBehaviour
    {
        [SerializeField] public Animator _animator;
        [SerializeField] protected NavMeshAgent _enemyAgent;
        [SerializeField] private GameObject _body;

        public NetworkObject[] PlayersArray;
        public int Damage = 10;
        public float AttackTime = 0.5f;


        private FusionManager _fusionManager;
        private bool _isDead = false;
        protected Vector3 target;
        private int _isFacingRight = 1;
        private NetworkObject _targetPlayer;

        private float AttackTimer;


        public void Init(FusionManager fusionManager)
        {

            _fusionManager = fusionManager;

            _enemyAgent.updateRotation = false;
            _enemyAgent.updateUpAxis = false;

            PlayersArray = _fusionManager.GetAllPlayers();
            if (PlayersArray.Length > 0)
            {
                target = PlayersArray[0].transform.position;
                _targetPlayer = PlayersArray[0];
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (Runner != null)
            {
                if (Runner.IsServer)
                {
                    AttackTimer -= Time.deltaTime;

                    SetTarget();
                    SetAgentPosition();

                    Flip();

                    CheckAttackDistance();

                }
            }
        }

        public void CheckAttackDistance()
        {
            if (!_isDead)
            {
                if (_enemyAgent.remainingDistance <= _enemyAgent.stoppingDistance)
                {
                    ProvideAttack();
                }

            }
        }

        public void ProvideAttack()
        {
            if (_targetPlayer != null)
            {
                if (Vector3.Distance(_targetPlayer.transform.position, transform.position) < _enemyAgent.stoppingDistance * 2)
                {
                    if (_targetPlayer != null && AttackTimer < 0)
                    {
                        Attack();
                        AttackTimer = AttackTime;
                    }

                }

            }

        }

        public virtual void Attack()
        {
            _targetPlayer.GetComponent<PlayerHealthComponent>().TakeDamage(Damage);
        }

        public void SetTarget()
        {
            if (Runner.IsServer)
            {
                if (PlayersArray.Length > 0)
                {
                    if (PlayersArray.Length > 1)
                    {
                        for (int i = 0; i < PlayersArray.Length - 1; i++)
                        {
                            if (Vector3.Distance(transform.position, PlayersArray[i].transform.position) <
                                Vector3.Distance(transform.position, PlayersArray[i + 1].transform.position))
                            {
                                target = PlayersArray[i].transform.position;
                                _targetPlayer = PlayersArray[i];

                            }
                            else
                            {
                                target = PlayersArray[i + 1].transform.position;
                                _targetPlayer = PlayersArray[i + 1];

                            }
                        }
                    }
                    else
                    {
                        target = PlayersArray[0].transform.position;
                        _targetPlayer = PlayersArray[0];

                    }
                }

            }
        }

        public void SetAgentPosition()
        {
            if (Runner.IsServer)
            {
                if (target != null)
                    _enemyAgent.SetDestination(new Vector3(target.x, target.y, transform.position.z));

            }
        }

        public void Dead()
        {
            _isDead = true;
            _enemyAgent.isStopped = true;
        }

        public void Flip()
        {
            if (Runner.IsServer)
            {
                if (target != null)
                {

                    if (_isFacingRight == 1 && target.x < transform.position.x)
                    {
                        _body.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);

                        Debug.Log("SetRotation");
                        _isFacingRight = -1;
                    }
                    else if (_isFacingRight == -1 && target.x > transform.position.x)
                    {
                        _body.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

                        Debug.Log("SetRotation");
                        _isFacingRight = 1;
                    }
                }

            }
        }
    }
}