using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    private Boundary boundary;
    [SerializeField]
    //What is the id of the prefab we want to spawn from the pool
    private string enemyId;
    [SerializeField]
    private float minSpawnInterval = 2.0f;
    [SerializeField]
    private float maxSpawnInterval = 5.0f;

    private float spawnInterval;
    private bool isSpawning;

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            CalculateScreenRestrictions();
        }
    }

    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        CalculateScreenRestrictions();
    }

    private void CalculateScreenRestrictions()
    {
        boundary = new Boundary();
        boundary.CalculateScreenRestrictions();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (!isSpawning)
            StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        //Generate a random waiting interval
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        isSpawning = true;
        //Wait for the spawnInterval
        yield return new WaitForSeconds(spawnInterval);
        //SpawnEnemy();
        SpawnOverNetwork();
        isSpawning = false;
    }

    private void SpawnOverNetwork()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject(enemyId, GetSpawnPosition(), Quaternion.identity);
        }
    }
    private void SpawnEnemy()
    {
        //Get an object from the pool
        GameObject enemy = ObjectPoolManager.Instance.GetPooledObject(enemyId);
        //Did we get an object from the pool?
        if(enemy != null)
        {
            //Position the enemy 
            enemy.transform.position = GetSpawnPosition();
            enemy.SetActive(true);
        }
    }

    private Vector2 GetSpawnPosition()
    {
        //Get a random vector based on the four edges of our screen
        int areaToSpawn = Random.Range(0, 4);
        switch (areaToSpawn)
        {
            //upper part - random x, fixed max y
            case 0:
                return new Vector2(Random.Range(-boundary.Bounds.x, boundary.Bounds.x),
                    boundary.Bounds.y);
            //lower part - random x, fixed min y
            case 1:
                return new Vector2(Random.Range(-boundary.Bounds.x, boundary.Bounds.x),
                   -boundary.Bounds.y);
            //right part - fixed max x, random y
            case 2:
                return new Vector2(boundary.Bounds.x, Random.Range(-boundary.Bounds.y,
                     boundary.Bounds.y));
            //left part - fixed min x, random y
            case 3:
                return new Vector2(-boundary.Bounds.x, Random.Range(-boundary.Bounds.y,
                     boundary.Bounds.y));
        }
        return Vector2.zero;
    }
}
