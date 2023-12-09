using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public AudioSource finishSound;
    public AudioSource endSound1;
    public AudioSource endSound2;
    public AudioSource BGM;
    public Image levelEndImage; // Reference to the Image component of the LevelEnd object

    public bool gameFinished = false;
    private float startTime;
    private float endTime;

    public TextMeshProUGUI timerText;
    public Camera playerCamera;

    private void Start()
    {
        startTime = Time.time;
        endTime = Time.time;
        levelEndImage.color = new Color(levelEndImage.color.r, levelEndImage.color.g, levelEndImage.color.b, 0f); // Start fully transparent
    }

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

            // Start the coroutine to show the LevelEnd image and then load the HubWorld scene
            SaveTimeForLevel(SceneManager.GetActiveScene().name, endTime);
            Data.S.lastLevelPlayed = SceneManager.GetActiveScene().name;

            StartCoroutine(CompleteLevelSequence());
        }
    }

    private void SaveTimeForLevel(string levelName, float time)
    {
        switch (levelName)
        {
            case "Level1":
                Data.S.level1Time = time;
                break;
            case "Level2":
                Data.S.level2Time = time;
                break;
            case "BossLevel":
                Data.S.bossTime = time;
                break;
        }
    }

    // New coroutine for the level completion sequence
    private IEnumerator CompleteLevelSequence()
    {
        // Wait for 1.5 seconds
        yield return new WaitForSeconds(1.5f);

        // White screen fade
        float fadeDuration = 0.5f;
        Color startColor = levelEndImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f); // Fully opaque
        for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
        {
            levelEndImage.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }
        levelEndImage.color = endColor; // Ensure it's fully opaque

        // Wait another 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Load the HubWorld scene
        SceneManager.LoadScene("HubWorld");
    }

    public void BossDefeated()
    {
        gameFinished = true;
        endTime = Time.time - startTime;
        timerText.color = Color.yellow;

        SaveTimeForLevel(SceneManager.GetActiveScene().name, endTime);

        // High score handling - do this after completing boss, tally up all three times from data
        if (IsHighScore(Data.S.level1Time + Data.S.level2Time + Data.S.bossTime) && Data.S.level1Time > 0 && Data.S.level2Time > 0 && Data.S.bossTime > 0)
        {
            PlayerPrefs.SetInt("ShowNameEntry", 1);
            PlayerPrefs.SetInt("ShowLeaderboard", 0);
        }
        else
        {
            PlayerPrefs.SetInt("ShowNameEntry", 0);
            PlayerPrefs.SetInt("ShowLeaderboard", 1);
        }

        // Save end time (based on scene name)
        PlayerPrefs.SetFloat("EndTime", Data.S.level1Time + Data.S.level2Time + Data.S.bossTime);

        StartCoroutine(BossDefeatSequence());
    }

    private IEnumerator BossDefeatSequence()
    {
        // Play finish sound
        endSound1.Play();
        endSound2.Play();
        BGM.Stop();

        // Slow down the timescale
        Time.timeScale = 0.4f;

        yield return new WaitForSecondsRealtime(0.8f);

        // White screen fade and sound volume decrease
        float fadeDuration = 5.0f;
        float startVolume = endSound1.volume;
        Color startColor = levelEndImage.color;
        Color endColor = Color.white;
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / fadeDuration)
        {
            levelEndImage.color = Color.Lerp(startColor, endColor, t);
            endSound1.volume = Mathf.Lerp(startVolume, 0, t); // Decrease volume
            yield return null;
        }
        levelEndImage.color = endColor; // Ensure it's fully opaque
        endSound1.volume = 0; // Ensure volume is set to 0

        // Wait another second using unscaled time
        yield return new WaitForSecondsRealtime(1f);

        // Restore the regular timescale
        Time.timeScale = 1.0f;

        // Load the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }
}
