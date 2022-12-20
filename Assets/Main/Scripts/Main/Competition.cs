using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Competition : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinOrCreateRoom("CompetitionRoom", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom() {
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }
}