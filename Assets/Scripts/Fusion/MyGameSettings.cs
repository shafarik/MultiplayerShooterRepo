using Assets.Scripts.Fusion.Data;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Fusion
{
    public class MyGameSettings : NetworkBehaviour
    {
        public float WaveTimer;
        public float RestTimer;

        private bool _isWaveNow = false;
        private bool _isRestNow = false;

        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private PickupSpawner _pickupSpawner;
        [SerializeField] private List<WaveData> _waveDataList = new List<WaveData>();

        public delegate void WaveRestSwapAction(int index);

        public event WaveRestSwapAction OnWaveRestSwap;

        private int _waveNum = 0;

        public void StartWave()
        {
            _waveNum++;

            WaveTimer = _waveDataList[_waveNum - 1].WaveDuration;

            _isWaveNow = true;
            OnWaveRestSwap?.Invoke(0);

            SpawnPickup();
            SpawnEnemy();
        }

        public void StartRest()
        {
            _enemySpawner.DespawnAllEnemies();

            RestTimer = _waveDataList[_waveNum - 1].RestDuration;

            _isRestNow = true;
            OnWaveRestSwap?.Invoke(1);
        }

        private void Update()
        {
            RestTimer -= Time.deltaTime;
            WaveTimer -= Time.deltaTime;

            if (_isWaveNow && WaveTimer < 0)
            {
                _isWaveNow = false;
                StartRest();
            }

            if (_isRestNow && RestTimer < 0)
            {
                _isRestNow = false;
                StartWave();
            }
        }

        public void SpawnPickup()
        {
            switch (_waveNum)
            {
                case 1:
                    _pickupSpawner.SpawnPickups(_waveDataList[0].SuppliesPrefabPool, _waveDataList[0].SuppliesCount);
                    break;

                case 2:
                    _pickupSpawner.SpawnPickups(_waveDataList[1].SuppliesPrefabPool, _waveDataList[1].SuppliesCount);
                    break;

                case 3:
                    _pickupSpawner.SpawnPickups(_waveDataList[2].SuppliesPrefabPool, _waveDataList[2].SuppliesCount);
                    break;

                default:
                    break;
            }
        }

        public void SpawnEnemy()
        {
            switch (_waveNum)
            {
                case 1:
                    _enemySpawner.SpawnEnemies(_waveDataList[0].EnemiesPrefabPool, _waveDataList[0].EnemiesCount);
                    break;

                case 2:
                    _enemySpawner.SpawnEnemies(_waveDataList[1].EnemiesPrefabPool, _waveDataList[1].EnemiesCount);
                    break;

                case 3:
                    _enemySpawner.SpawnEnemies(_waveDataList[2].EnemiesPrefabPool, _waveDataList[2].EnemiesCount);
                    break;

                default:
                    break;
            }
        }
    }
}