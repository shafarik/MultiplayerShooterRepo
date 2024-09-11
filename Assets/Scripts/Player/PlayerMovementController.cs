using Fusion;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovementController : NetworkBehaviour
    {


        public void MovePlayer(Vector3 direction, float moveSpeed)
        {
            if (Runner.IsServer)
                transform.position += direction * moveSpeed * Runner.DeltaTime;
        }
    }
}