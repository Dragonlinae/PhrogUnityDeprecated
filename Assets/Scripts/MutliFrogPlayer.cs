using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace MultiFrog
{
    public class MultiFrogPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> netPosition = new NetworkVariable<Vector3>();
        public NetworkVariable<Vector3> netRotation = new NetworkVariable<Vector3>();
        public NetworkVariable<bool> netSpawned = new NetworkVariable<bool>(false);
        public NetworkAnimator netAniamtor;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                foreach (var player in FindObjectsOfType<SingleFrogController>())
                {
                    player.gameObject.SetActive(false);
                }
                gameObject.transform.position = new Vector3(Random.Range(-5, 5), 3, Random.Range(-5, 5));
                Move();
                gameObject.GetComponentInChildren<Camera>().enabled = true;
                SpawnedInServerRpc();
            }
            else
            {
                Destroy(gameObject.GetComponentInChildren<Camera>().gameObject);
                Destroy(gameObject.GetComponent<MultiFrogController>());
            }
        }

        public void Move()
        {
            SubmitPositionChangeServerRpc(gameObject.transform.position, gameObject.transform.rotation.eulerAngles, gameObject.GetComponent<Animator>().GetBool("Jump"), gameObject.GetComponent<Animator>().GetBool("Roll"));
        }

        [ServerRpc]
        void SubmitPositionChangeServerRpc(Vector3 position, Vector3 rotation, bool jump, bool roll)
        {
            netPosition.Value = position;
            netRotation.Value = rotation;
        }

        [ServerRpc]
        void SpawnedInServerRpc()
        {
            netSpawned.Value = true;
        }

        void Update()
        {
            if (IsOwner)
            {
                Move();
            }
            else if (netSpawned.Value)
            {
                gameObject.transform.position = netPosition.Value;
                gameObject.transform.rotation = Quaternion.Euler(netRotation.Value);
            }
        }
    }
}