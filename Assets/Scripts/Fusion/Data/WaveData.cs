using Fusion;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Fusion.Data
{
    [CreateAssetMenu(fileName = "NewWaveData", menuName = "WaveData/Data")]

    public class WaveData : ScriptableObject
    {
        public int WaveDuration;
        public int RestDuration;
        public List<NetworkPrefabRef> EnemiesPrefabPool = new List<NetworkPrefabRef>();
        public int EnemiesCount;

        public List<NetworkPrefabRef> SuppliesPrefabPool = new List<NetworkPrefabRef>();
        public int SuppliesCount;
    }
}