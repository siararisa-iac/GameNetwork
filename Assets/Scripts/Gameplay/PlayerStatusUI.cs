using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField]
    private int playerNumber;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private Image playerIcon;
    [SerializeField]
    private GameObject waiting, connected;

    private void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += UpdateUI;
    }

    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdateUI;
    }

    void UpdateUI()
    {
        //Check if there is an available player
        if(PhotonNetwork.PlayerList.Length - 1 >= playerNumber)
        {
            waiting.SetActive(false);
            connected.SetActive(true);
            //Update the info
            Photon.Realtime.Player player = PhotonNetwork.PlayerList[playerNumber];
            playerName.text = player.NickName;
            playerIcon.sprite = NetworkManager.Instance.GetPlayerSprite(playerNumber);
        }
        else
        {
            waiting.SetActive(true);
            connected.SetActive(false);
        }
    }
}
