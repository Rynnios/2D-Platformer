using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public AudioSource finishSound;

    public bool gameFinished = false;
    private float startTime;
    private float endTime;

    public TextMeshProUGUI timerText;

    private void Update()
    {
        if (gameFinished) return;

        float t = Time.time - startTime;

        int minutes = (int)t / 60;
        int seconds = (int)t % 60;
        int milliseconds = (int)((t - Mathf.Floor(t)) * 100);

        timerText.text = $"TIME: {minutes}\'{seconds:00}\"{milliseconds:00}";
    }

    public bool IsHighScore(float playerTime)
    {
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey($"LeaderboardEntryTime{i}"))
            {
                float highScore = PlayerPrefs.GetFloat($"LeaderboardEntryTime{i}");
                if (playerTime < highScore)
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "MainPlayer" && !gameFinished)
        {
            gameFinished = true;
            endTime = Time.time - startTime;
            timerText.color = Color.yellow;
            finishSound.Play();

            // Check high score to determine what screen of menu player should be sent back to
            if (IsHighScore(endTime))
            {
                PlayerPrefs.SetInt("ShowNameEntry", 1);
                PlayerPrefs.SetInt("ShowLeaderboard", 0);
            }
            else
            {
                PlayerPrefs.SetInt("ShowNameEntry", 0);
                PlayerPrefs.SetInt("ShowLeaderboard", 1);
            }

            // Save end time
            PlayerPrefs.SetFloat("EndTime", endTime);

            Invoke(nameof(CompleteLevel), 2f);
        }
    }

    // Go to next scene when completing level - replace this with "next level" or "go to hub"
    private void CompleteLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
