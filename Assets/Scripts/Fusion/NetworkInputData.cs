using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Fusion
{
    public struct NetworkInputData : INetworkInput
    {
        public Vector3 direction;
        public Vector3 fireDirection;
    }
}