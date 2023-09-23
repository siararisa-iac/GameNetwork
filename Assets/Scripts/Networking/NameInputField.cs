using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

[RequireComponent(typeof(TMP_InputField))]
public class NameInputField : MonoBehaviour
{
    private TMP_InputField input;
    [SerializeField]
    private Button connectButton;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
        connectButton.interactable = false;
    }

    //Normally, we would sign in and check our database, but for now we register a nickname for the player
    public void SetPlayerNickName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            connectButton.interactable = false;
            return;
        }
        //Set the nickname of the player
        PhotonNetwork.NickName = value;
        connectButton.interactable = true;
    }
}
