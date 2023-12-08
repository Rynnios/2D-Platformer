using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    static public ItemCollector instance;
    private EnterLevel enterLevelScript;

    public GameObject Finish;

    // Count cherries to know if you completed the level
    private int targetCherries; // Maybe this can be set with a param for each level later
    private int currentCherries = 0;

    // Item sounds
    public AudioSource getCherry;
    public AudioSource getSkillPoint;
    public AudioSource completedCherryReq;

    // Text fields
    [SerializeField] private TMP_Text cherriesText;
    [SerializeField] private TMP_Text skillPointsText;
    [SerializeField] private GameObject cherriesContainer;

    private void Start()
    {
        // Count the number of cherry objects in the scene
        targetCherries = GameObject.FindGameObjectsWithTag("Cherry").Length;

        // Initialize text fields
        if (cherriesText != null)
            cherriesText.text = ": " + currentCherries + "/" + targetCherries;
        skillPointsText.text = ": " + Data.S.skillPoints;

        // Makes it impossible to win until all cherries are collected by disabling collider of trophy until conditions are met
        Finish = GameObject.FindGameObjectWithTag("FinishTrophy");
        if (Finish != null) // We need to include this for the hub world which doesn't have a trophy
        {
            Finish.GetComponent<BoxCollider2D>().enabled = false;
        }

        // Enter level script
        enterLevelScript = FindObjectOfType<EnterLevel>();
    }

    // Item collection handler
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collect cherry
        if (collision.transform.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            getCherry.Play();

            // Recalculate the number of cherries left
            if (cherriesContainer != null)
            {
                // Count only the cherries that are children of the cherriesContainer
                currentCherries = targetCherries - (cherriesContainer.transform.childCount - 1);
            }
            else
            {
                // Count all cherries in the scene
                currentCherries = GameObject.FindGameObjectsWithTag("Cherry").Length;
            }

            cherriesText.text = ": " + currentCherries + "/" + targetCherries;

            AllCherriesCollected();
        }


        // Collect skill point (pineapple)
        else if (collision.gameObject.CompareTag("SkillPoint"))
        {
            string pineappleID = SceneManager.GetActiveScene().name + "_" + collision.gameObject.name;

            Destroy(collision.gameObject);
            getSkillPoint.Play();

            // Check if this pineapple has already been collected
            if (!Data.S.collectedPineapples[pineappleID])
            {
                Data.S.skillPoints++;
                Data.S.collectedPineapples[pineappleID] = true;
                skillPointsText.text = ": " + Data.S.skillPoints;

                // Update HubWorldInfo display if in HubWorld
                if (SceneManager.GetActiveScene().name == "HubWorld")
                {
                    UpdateHubWorldPineappleDisplay(collision.gameObject.name);
                }
            }
        }
    }

    // Updates pineapples live in hub
    private void UpdateHubWorldPineappleDisplay(string pineappleName)
    {
        GameObject levelInfoObject = GameObject.Find("HubWorldInfo");
        if (levelInfoObject != null)
        {
            GameObject pineappleSprite = levelInfoObject.transform.Find(pineappleName).gameObject;
            if (pineappleSprite != null)
            {
                SpriteRenderer sr = pineappleSprite.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = new Color(1f, 1f, 1f, 1f); // Full opacity for collected pineapple
                }
            }

            // Call UpdateTrophyDisplay method if enterLevelScript is not null
            if (enterLevelScript != null)
            {
                enterLevelScript.UpdateTrophyDisplay("HubWorld", levelInfoObject);
            }
        }
    }

    public void AllCherriesCollected()
    {
        if (currentCherries == targetCherries)
        {
            completedCherryReq.Play();
            Finish.GetComponent<BoxCollider2D>().enabled = true;
            Finish.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); 
        } 
    }
}
