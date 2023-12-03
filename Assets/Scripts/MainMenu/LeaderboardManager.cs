using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public float playerTime;
    }

    public List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    public Transform leaderboardContent;
    public int maxEntries = 10;

    private void Awake()
    {
        // Create sample leaderboard data if none exists
        if (!PlayerPrefs.HasKey("LeaderboardInitialized"))
        {
            InitializeLeaderboardWithSampleData();
            SaveLeaderboard();
            PlayerPrefs.SetInt("LeaderboardInitialized", 1);
        }

        // InitializeLeaderboardWithSampleData();
        // SaveLeaderboard();

        LoadLeaderboard();
    }

    private void InitializeLeaderboardWithSampleData()
    {
        leaderboardEntries = new List<LeaderboardEntry>
        {
            new LeaderboardEntry { playerName = "CAT", playerTime = 270.0f },
            new LeaderboardEntry { playerName = "DOG", playerTime = 300.0f },
            new LeaderboardEntry { playerName = "ZAP", playerTime = 330.0f },
            new LeaderboardEntry { playerName = "BOO", playerTime = 360.0f },
            new LeaderboardEntry { playerName = "RUN", playerTime = 390.0f },
            new LeaderboardEntry { playerName = "SKY", playerTime = 420.0f },
            new LeaderboardEntry { playerName = "GEM", playerTime = 450.0f },
            new LeaderboardEntry { playerName = "PIE", playerTime = 480.0f },
            new LeaderboardEntry { playerName = "SUN", playerTime = 510.0f },
            new LeaderboardEntry { playerName = "OWL", playerTime = 540.0f }
        };

        leaderboardEntries = leaderboardEntries.OrderBy(entry => entry.playerTime).ToList();
    }

    public void InsertPlayerScore(string playerName, float playerTime)
    {
        leaderboardEntries.Add(new LeaderboardEntry { playerName = playerName, playerTime = playerTime });
        leaderboardEntries = leaderboardEntries.OrderBy(entry => entry.playerTime).ToList();
        if (leaderboardEntries.Count > maxEntries)
        {
            leaderboardEntries.RemoveAt(leaderboardEntries.Count - 1);
        }
        SaveLeaderboard();
        PopulateLeaderboard();
    }

    public void PopulateLeaderboard()
    {
        for (int i = 0; i < leaderboardContent.childCount; i++)
        {
            if (i < leaderboardEntries.Count)
            {
                Transform entryTransform = leaderboardContent.GetChild(i);

                // Update name
                TextMeshProUGUI nameText = entryTransform.Find("Name/NameText").GetComponent<TextMeshProUGUI>();
                nameText.text = leaderboardEntries[i].playerName;

                // Update time
                TextMeshProUGUI timeText = entryTransform.Find("Time/TimeText").GetComponent<TextMeshProUGUI>();
                timeText.text = FormatTime(leaderboardEntries[i].playerTime);

                // Activate slot
                entryTransform.gameObject.SetActive(true);
            }
            else
            {
                // If there are no more entries to display, deactivate remaining slots
                leaderboardContent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void SortAndPopulateLeaderboard()
    {
        // Sort leaderboard entries by time
        leaderboardEntries.Sort((entry1, entry2) => entry1.playerTime.CompareTo(entry2.playerTime));

        // If list is too long, remove last entry
        if (leaderboardEntries.Count > maxEntries)
        {
            leaderboardEntries.RemoveAt(leaderboardEntries.Count - 1);
        }

        PopulateLeaderboard();
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)timeInSeconds / 60;
        int seconds = (int)timeInSeconds % 60;
        int milliseconds = (int)((timeInSeconds - Mathf.Floor(timeInSeconds)) * 100);
        return $"{minutes}\'{seconds:00}\"{milliseconds:00}";
    }

    private void SaveLeaderboard()
    {
        for (int i = 0; i < maxEntries; i++)
        {
            if (i < leaderboardEntries.Count)
            {
                PlayerPrefs.SetString($"LeaderboardEntryName{i}", leaderboardEntries[i].playerName);
                PlayerPrefs.SetFloat($"LeaderboardEntryTime{i}", leaderboardEntries[i].playerTime);
            }
            else
            {
                PlayerPrefs.DeleteKey($"LeaderboardEntryName{i}");
                PlayerPrefs.DeleteKey($"LeaderboardEntryTime{i}");
            }
        }
        PlayerPrefs.Save();
    }

    private void LoadLeaderboard()
    {
        leaderboardEntries.Clear();
        for (int i = 0; i < maxEntries; i++)
        {
            if (PlayerPrefs.HasKey($"LeaderboardEntryName{i}"))
            {
                string name = PlayerPrefs.GetString($"LeaderboardEntryName{i}");
                float time = PlayerPrefs.GetFloat($"LeaderboardEntryTime{i}");
                leaderboardEntries.Add(new LeaderboardEntry { playerName = name, playerTime = time });
            }
        }

        // Data is assumed to already be sorted as it was saved in order
        PopulateLeaderboard();
    }
}