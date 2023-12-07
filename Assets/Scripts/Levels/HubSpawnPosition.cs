using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubSpawnPosition : MonoBehaviour
{
    public GameObject player;
    private SpriteRenderer spriteRend;

    public Transform spawnPointLevel1;
    public Transform spawnPointLevel2;
    public Transform spawnPointBossLevel;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        switch (Data.S.lastLevelPlayed)
        {
            case "Level1":
                player.transform.position = spawnPointLevel1.position;
                Data.S.lastLevelPlayed = "";
                break;
            case "Level2":
                player.transform.position = spawnPointLevel2.position;
                spriteRend.flipX = true;
                Data.S.lastLevelPlayed = "";
                break;
            case "BossLevel":
                player.transform.position = spawnPointBossLevel.position;
                spriteRend.flipX = true;
                Data.S.lastLevelPlayed = "";
                break;
        }
    }
}
