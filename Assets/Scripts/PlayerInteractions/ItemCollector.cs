using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    static public ItemCollector instance;
    public GameObject Finish;


    // Count cherries to know if you completed the level
    private int targetCherries = 5; // Maybe this can be set with a param for each level later
    private int currentCherries = 0;
    
    // Item sounds
    public AudioSource completedCherryReq;

    // Text fields
    [SerializeField] private TMP_Text cherriesText;
    [SerializeField] private TMP_Text skillPointsText;

    private void Start()
    {
        // Initialize text fields
        cherriesText.text = ": " + currentCherries + "/" + targetCherries;

        // Makes it impossible to win until all cherries are collected by disabling collider of trophy until conditions are met
        Finish = GameObject.FindGameObjectWithTag("FinishTrophy");
        if (Finish != null) // We need to include this for the hub world which doesn't have a trophy
        {
            Finish.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Item collection handler
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collect cherry
        if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            currentCherries++;
            cherriesText.text = ": " + currentCherries + "/" + targetCherries;

            AllCherriesCollected();
        }

        // Collect skill point (pineapple)
        else if(collision.gameObject.CompareTag("SkillPoint"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Skill Point Collected");
            Data.S.skillPoints++;
        }
    }

    public void AllCherriesCollected()
    {
        if (currentCherries == targetCherries)
        {
            completedCherryReq.Play();
            Finish.GetComponent<BoxCollider2D>().enabled = true;
        } 
    }
}
