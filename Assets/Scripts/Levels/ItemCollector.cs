using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    static public ItemCollector instance;
    public GameObject Finish;


    private void Start()
    {
        //Makes it impossible to win until all cherries are collected by disabling collider of trophy until conditions are met
        Finish = GameObject.FindGameObjectWithTag("FinishTrophy");
        Finish.GetComponent<BoxCollider2D>().enabled = false;

    }

    //Count cherries to know if you completed the level
    private int cherries = 5;
    public AudioSource completedCherryReq;


    [SerializeField] private TMP_Text cherriesText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //The tag connected to game object Cherry, which is "Cherry" in this case
        if (collision.gameObject.CompareTag("Cherry"))
        {
            Destroy(collision.gameObject);
            cherries--;
            Debug.Log("Cherries: " + cherries);
            cherriesText.text = "Cherries: " + cherries;

            AllCherriesCollected();

        }
    }

    public void AllCherriesCollected()
    {
        if (cherries == 0)
        {
            completedCherryReq.Play();
            Finish.GetComponent<BoxCollider2D>().enabled = true;
        } 
    }
}
