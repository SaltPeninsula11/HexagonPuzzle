using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class SampleScene : MonoBehaviourPunCallbacks
{
    public GameObject player;
    //https://www.youtube.com/watch?v=S20gzSh14CM 11:20

    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        PhotonNetwork.Instantiate(player.name, v, Quaternion.identity);
    }
}
