using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionSetup : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TextMeshProUGUI logger;

    private int maxPlayers = 4;
    public override void OnConnectedToMaster()
    {
        Log("Connected to the Master Server.", "green");
        Log("Trying to join a random room..");
        //When connected to the master server, attempt to join a random room
        PhotonNetwork.JoinRandomRoom();
    }

    //When you joined a room
    public override void OnJoinedRoom()
    {
        Log("Successfully joined a room", "green");
        DisplayRoomInformation();
    }

    //When others join the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Log($"{newPlayer.NickName} has joined the room", "green");
        DisplayRoomInformation();
    }

    //When other left the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Log($"{otherPlayer.NickName} has left the room", "orange");
        DisplayRoomInformation();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Log($"Failed to join random room: {message}", "red");
        //If no rooms availabe (all rooms at max capacity or no roooms created), create a new room
        string roomName = $"{PhotonNetwork.NickName}'s Room";
        PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions
        { MaxPlayers = maxPlayers });
    }

    public void Connect()
    {
        //Check first if we are already connected to the PhotonNetwork
        if (PhotonNetwork.IsConnected)
        {
            Log("Already connected");
        }
        else
        {
            //Attempt to connect to the Photon Network
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void DisplayRoomInformation()
    {
        Log($"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers} " +
            $"Players in {PhotonNetwork.CurrentRoom.Name}", "orange");
    }
    public void Log(string message, string colorCode = null)
    {
        if(logger == null)
        {
            Debug.Log(message);
            return;
        }

        logger.text += (string.IsNullOrEmpty(colorCode) ? "" : "<color=" + colorCode + ">") +
            message + (string.IsNullOrEmpty(colorCode) ? "" : "</color>") 
            + System.Environment.NewLine;
    }
}
