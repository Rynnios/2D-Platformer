using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using static Cinemachine.DocumentationSortingAttribute;

public class EnterLevel : MonoBehaviour
{
    private string sceneToLoad;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "HubWorld")
        {
            UpdateLevelInfo();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Level1")
        {
            sceneToLoad = "Level1";
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (collision.gameObject.name == "Level2")
        {
            sceneToLoad = "Level2";
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (collision.gameObject.name == "Level3")
        {
            sceneToLoad = "Boss Level";
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void UpdateLevelInfo()
    {
        // Define level names and their corresponding GameObjects
        string[] levels = { "HubWorld", "Level1", "Level2", "BossLevel" };
        foreach (string level in levels)
        {
            GameObject levelInfoObject = GameObject.Find(level + "Info");
            if (levelInfoObject != null)
            {
                // Update time info for levels other than HubWorld
                if (level != "HubWorld")
                {
                    UpdateLevelTime(level, levelInfoObject);
                }

                // Update pineapple info
                UpdatePineappleInfo(level, levelInfoObject);

                // Update trophy display
                UpdateTrophyDisplay(level, levelInfoObject);
            }
        }
    }

    private void UpdateLevelTime(string level, GameObject levelInfoObject)
    {
        TMP_Text timeText = levelInfoObject.transform.Find("TimeText").GetComponent<TMP_Text>();
        if (timeText != null)
        {
            float time = 0;
            switch (level)
            {
                case "Level1":
                    time = Data.S.level1Time;
                    break;
                case "Level2":
                    time = Data.S.level2Time;
                    break;
                case "Boss Level":
                    time = Data.S.bossTime;
                    break;
            }

            if (time > 0)
            {
                int minutes = (int)time / 60;
                int seconds = (int)time % 60;
                int milliseconds = (int)((time - (int)time) * 100);
                timeText.text = $"{minutes}\'{seconds:00}\"{milliseconds:00}";
            }
            else
            {
                timeText.text = "-\'--\"--";
            }
        }
    }

    private void UpdatePineappleInfo(string level, GameObject levelInfoObject)
    {
        int totalPineapples = level == "BossLevel" ? 1 : 3;
        for (int i = 1; i <= totalPineapples; i++)
        {
            string key = level + "_Pineapple" + i;
            GameObject pineappleSprite = levelInfoObject.transform.Find("Pineapple" + i).gameObject;
            if (pineappleSprite != null)
            {
                SpriteRenderer sr = pineappleSprite.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (Data.S.collectedPineapples.ContainsKey(key) && Data.S.collectedPineapples[key])
                    {
                        sr.color = new Color(1f, 1f, 1f, 1f); // Full opacity for collected pineapples
                    }
                    else
                    {
                        sr.color = new Color(0f, 0f, 0f, 1f); // Fully transparent for uncollected pineapples
                    }
                }
            }
        }
    }

    public void UpdateTrophyDisplay(string level, GameObject levelInfoObject)
    {
        GameObject trophyImage = levelInfoObject.transform.Find("WinImage").gameObject;
        if (trophyImage != null)
        {
            bool levelCompleted = false;

            // Check if the level is completed based on the criteria
            if (level == "HubWorld")
            {
                // Check if all pineapples in HubWorld are collected
                levelCompleted = Data.S.collectedPineapples["HubWorld_Pineapple1"] &&
                                 Data.S.collectedPineapples["HubWorld_Pineapple2"] &&
                                 Data.S.collectedPineapples["HubWorld_Pineapple3"];
            }
            else
            {
                // For other levels, check if the level time is greater than zero
                switch (level)
                {
                    case "Level1":
                        levelCompleted = Data.S.level1Time > 0;
                        break;
                    case "Level2":
                        levelCompleted = Data.S.level2Time > 0;
                        break;
                    case "BossLevel":
                        levelCompleted = Data.S.bossTime > 0;
                        break;
                }
            }

            trophyImage.SetActive(levelCompleted); // Enable trophy if level is completed
        }
    }
}
