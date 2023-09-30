using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkManager : MonoBehaviour
{
    private const string PlayerPrefabName = "Player";
    private void Start()
    {
        //Make sure whether connect to Photon or not
        if (!PhotonNetwork.IsConnected)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }
        //If we are connected, we instantiate the player
        PhotonNetwork.Instantiate(PlayerPrefabName, Vector3.zero, Quaternion.identity);
    }
}
