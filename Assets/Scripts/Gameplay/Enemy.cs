using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private int scorePoints = 100;

    [SerializeField]
    private Image healthbar;

    private float currentHealth;
    private Transform target;
    private bool isDestroyed;

    private void OnEnable()
    {
        LookAtTarget();
        currentHealth = maxHealth;
    }

    private void UpdateHealthbar()
    {
        healthbar.fillAmount = currentHealth / maxHealth;
    }

    private void LookAtTarget()
    {
        Quaternion newRotation;
        //Make the enemy face the target
        //If we do not have a specific target, make the object look at the center
        if(target == null)
        {
            newRotation = Quaternion.LookRotation(transform.position, Vector3.forward);
        }
        else
        {
            newRotation = Quaternion.LookRotation(transform.position - target.transform.position, Vector3.forward);
        }

        //Since rotation is only based on the z-axis
        newRotation.x = 0;
        newRotation.y = 0;
        transform.rotation = newRotation;
    }

    private void Move()
    {
        //Since the object has been rotated, just make it move 
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }

    public void NetworkDestroy()
    {
        if (PhotonNetwork.IsMasterClient)
            DestroyGlobally();
        else
            DestroyLocally();
    }

    private void DestroyLocally()
    {
        isDestroyed = true;
    }

    private void DestroyGlobally()
    {
        PhotonNetwork.Destroy(this.gameObject);
        isDestroyed = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Bullet>(out Bullet bullet))
        {
            TakeDamage(bullet.Damage, bullet.Owner);
        }
    }

    public void TakeDamage(float damage, Player from)
    {
        currentHealth -= damage;
        UpdateHealthbar();

        if(currentHealth <= 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                foreach(Player p in PhotonNetwork.PlayerList)
                {
                    if(p.ActorNumber == from.ActorNumber)
                    {
                        p.AddScore(scorePoints);
                    }
                }
            }
            NetworkDestroy();
        }
    }
}
