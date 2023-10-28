using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

public class HealthManager : SingletonPun<HealthManager>
{
    public static byte HealthEventCode = 1;

    [SerializeField]
    private float maxHealth = 1000;
    private float health;

    //Functions registered in this event are called locally
    //public delegate void HealthListener(float health, float maxHealth);
    //public static event HealthListener OnHealthUpdate;
    
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            //OnHealthUpdate?.Invoke(health, maxHealth);

            //Make sure only the MasterClient is changing the health
            if (!PhotonNetwork.IsMasterClient)
                return;

            //Invoke or Raise event over the network
            object[] healthData = new object[] { health, maxHealth };
            RaiseEventOptions options = new RaiseEventOptions
            {
                Receivers = ReceiverGroup.All
            };
            PhotonNetwork.RaiseEvent(HealthEventCode, healthData, options, SendOptions.SendUnreliable);
        }
    }

    private void Start()
    {
        Health = maxHealth;
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        //Just refresh the call whenever a player enters the room
        Health = Health;
    }
}
