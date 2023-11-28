using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public AudioSource finishSound;

    private bool levelCompleted = false;
    float timer;

    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "MainPlayer" && !levelCompleted)
        {
            finishSound.Play();
            Invoke(nameof(CompleteLevel), 2f);
            levelCompleted = true;

            PlayerPrefs.GetFloat("CurrentScore");
            PlayerPrefs.SetFloat("CurrentScore", timer);
            Debug.Log(timer);
            Debug.Log(PlayerPrefs.GetFloat("CurrentScore"));

        }
    }

    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
