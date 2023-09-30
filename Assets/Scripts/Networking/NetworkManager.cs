using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class NetworkManager : SingletonPUN<NetworkManager>
{
    private const string PlayerPrefabName = "Player";
    [SerializeField]
    private Sprite[] playerSprites;

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

    public Sprite GetPlayerSprite(int index)
    {
        return playerSprites[index];
    }
}
