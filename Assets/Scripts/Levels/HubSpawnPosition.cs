using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSpawnPosition : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPointLevel1;
    public Transform spawnPointLevel2;

    void Start()
    {
        Transform spawnPoint;

        switch (Data.S.lastLevelPlayed)
        {
            case "Level1":
                player.transform.position = spawnPointLevel1.position;
                Data.S.lastLevelPlayed = "";
                break;
            case "Level2":
                player.transform.position = spawnPointLevel2.position;
                Data.S.lastLevelPlayed = "";
                break;
        }
    }
}
