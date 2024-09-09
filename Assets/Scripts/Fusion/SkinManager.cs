using Assets.Scripts.Player;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Fusion
{
    public class SkinManager : NetworkBehaviour
    {
        [SerializeField] private FusionManager _fusionManager;

        public List<RuntimeAnimatorController> Animators = new List<RuntimeAnimatorController>();

        public int SkinIndex = 0;

        // Use this for initialization
        void Start()
        {
            _fusionManager.OnPlayerSpawned += SetSkinToPlayer;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetSkinIndex(int index)
        {
            SkinIndex = index;
        }

        private void OnDestroy()
        {
            _fusionManager.OnPlayerSpawned -= SetSkinToPlayer;
        }

        public void SetSkinToPlayer(NetworkObject player)
        {
            if (Runner.IsServer)
            {
                NetworkObject[] netObjArray = _fusionManager.GetAllPlayers();

                List<NetworkId> netIdList = new List<NetworkId>();

                for (int i = 0; i < netObjArray.Length; i++)
                {
                    netIdList.Add(netObjArray[i].Id);
                }

                RpcUpdateSkin(_fusionManager.GetAllPlayerRefs(), netIdList.ToArray());

                RpcUpdateOtherPlayerSkin(_fusionManager.GetAllPlayerRefs(), netIdList.ToArray(), SkinIndex);
            }
            //player.GetComponent<PlayerCharacter>().SetPlayerSkin(Animators[SkinIndex]);
        }


        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcUpdateSkin(PlayerRef[] playerRefsArray, NetworkId[] netObjIdArray)
        {
            for (int i = 0; i < playerRefsArray.Length; i++)
            {
                if (Runner.LocalPlayer == playerRefsArray[i])
                {
                    NetworkObject playerObj = Runner.FindObject(netObjIdArray[i]);

                    playerObj.GetComponent<PlayerCharacter>().SetPlayerSkin(Animators[SkinIndex], this, playerObj);
                }
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RpcUpdateOtherPlayerSkin(PlayerRef[] playerRefsArray, NetworkId[] netObjIdArray, int Index)
        {
            for (int i = 0; i < playerRefsArray.Length; i++)
            {
                if (Runner.LocalPlayer != playerRefsArray[i])
                {
                    NetworkObject playerObj = Runner.FindObject(netObjIdArray[i]);

                    if (playerObj != null)
                        playerObj.GetComponent<PlayerCharacter>().SetPlayerSkin(Animators[Index], this, playerObj);
                }
            }
        }

    }
}