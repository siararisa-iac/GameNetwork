using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

public class WorldHealth : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private TextMeshProUGUI healthText;

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        //We are only interested in the health event
        if(eventCode == HealthManager.HealthEventCode)
        {
            //get the data from the parameters passed by the RaiseEvent
            object[] data = (object[])photonEvent.CustomData;
            float health = (float)data[0];
            float maxHealth = (float)data[1];
            UpdateHealthBar(health, maxHealth);
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        //HealthManager.OnHealthUpdate += UpdateHealthBar;
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        //HealthManager.OnHealthUpdate -= UpdateHealthBar;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.NetworkDestroy();
            HealthManager.Instance.Health -= enemy.Damage;
        }
    }

    void UpdateHealthBar(float health, float max)
    {
        healthBar.fillAmount = health / max;
        healthText.text = $"{health}/{max}";
    }

    
}
