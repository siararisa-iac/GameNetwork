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
    private TextMeshProUGUI playerScore;
    [SerializeField]
    private Image playerIcon;
    [SerializeField]
    private GameObject waiting, connected;

    private Photon.Realtime.Player player;

    private void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += UpdateUI;
    }

    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdateUI;
    }

    private void Update()
    {
        if(player == null)
        {
            return;
        }

        playerScore.text = player.GetScore().ToString();
    }

    void UpdateUI()
    {
        //Check if there is an available player
        if(PhotonNetwork.PlayerList.Length - 1 >= playerNumber)
        {
            waiting.SetActive(false);
            connected.SetActive(true);
            //Update the info
            player = PhotonNetwork.PlayerList[playerNumber];
            playerName.text = player.NickName;
            playerIcon.sprite = NetworkManager.Instance.GetPlayerSprite(playerNumber);
            playerScore.text = player.GetScore().ToString();
        }
        else
        {
            waiting.SetActive(true);
            connected.SetActive(false);
        }
    }
}
