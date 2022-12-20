using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayer : MonoBehaviourPunCallbacks
{

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine) {
            float x = Input.GetAxis("Horizontal") * 0.2f;
            float y = Input.GetAxis("Vertical") * 0.2f;

            transform.position += new Vector3(x, y, 0);
        }
    }
}
