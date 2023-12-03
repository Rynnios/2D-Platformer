using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public RectTransform titleScreen;
    public RectTransform leaderboardScreen;
    public RectTransform nameEntryScreen;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI warningMessage;
    public LeaderboardManager leaderboardManager;
    public float transitionSpeed = 0.3f;

    private Vector3 aboveScreenPosition = new Vector3(0, 800, 0);
    private Vector3 belowScreenPosition = new Vector3(0, -800, 0);

    private Vector3 titleScreenTarget;
    private Vector3 leaderboardTarget;
    private Vector3 nameEntryTarget;
    private Vector3 titleScreenVelocity = Vector3.zero;
    private Vector3 leaderboardVelocity = Vector3.zero;
    private Vector3 nameEntryVelocity = Vector3.zero;

    private bool canShowWarningMessage = true;

    void Start()
    {
        // Check if the leaderboard should be shown
        if (PlayerPrefs.GetInt("ShowLeaderboard", 0) == 1)
        {
            titleScreen.localPosition = aboveScreenPosition;
            leaderboardScreen.localPosition = aboveScreenPosition;
            nameEntryScreen.localPosition = aboveScreenPosition;

            titleScreenTarget = aboveScreenPosition;
            leaderboardTarget = Vector3.zero;
            nameEntryTarget = aboveScreenPosition;

            PlayerPrefs.SetInt("ShowLeaderboard", 0); // Reset the flag
        }
        // Check if the name entry screen should be shown
        else if (PlayerPrefs.GetInt("ShowNameEntry", 0) == 1)
        {
            titleScreen.localPosition = aboveScreenPosition;
            leaderboardScreen.localPosition = aboveScreenPosition;
            nameEntryScreen.localPosition = aboveScreenPosition;

            titleScreenTarget = aboveScreenPosition;
            leaderboardTarget = aboveScreenPosition;
            nameEntryTarget = Vector3.zero;

            PlayerPrefs.SetInt("ShowNameEntry", 0); // Reset the flag
        }
        else
        {
            // Normal initialization
            titleScreenTarget = titleScreen.localPosition;
            leaderboardTarget = leaderboardScreen.localPosition;
            nameEntryTarget = nameEntryScreen.localPosition;
        }
    }

    void Update()
    {
        titleScreen.localPosition = Vector3.SmoothDamp(titleScreen.localPosition, titleScreenTarget, ref titleScreenVelocity, transitionSpeed);
        leaderboardScreen.localPosition = Vector3.SmoothDamp(leaderboardScreen.localPosition, leaderboardTarget, ref leaderboardVelocity, transitionSpeed);
        nameEntryScreen.localPosition = Vector3.SmoothDamp(nameEntryScreen.localPosition, nameEntryTarget, ref nameEntryVelocity, transitionSpeed);
    }

    public void ShowLeaderboard()
    {
        titleScreenTarget = aboveScreenPosition;
        leaderboardTarget = Vector3.zero;
    }

    public void ShowTitleScreen()
    {
        titleScreenTarget = Vector3.zero;
        leaderboardTarget = belowScreenPosition;
    }

    // Method to start the game
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Method to exit the game
    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Quit if in editor
#endif
    }

    // Submit name to leaderboard
    public void SubmitName()
    {
        string playerName = nameInputField.text.Trim().ToUpper();

        // Warn player if name length is less than 3
        if (playerName.Length < 3)
        {
            if (canShowWarningMessage)
            {
                StartCoroutine(ShowWarningMessage());
            }
            return;
        }

        float playerTime = PlayerPrefs.GetFloat("EndTime", 0);

        // Continue with the leaderboard update
        leaderboardManager.InsertPlayerScore(playerName, playerTime);
        leaderboardManager.SortAndPopulateLeaderboard();

        // Animate the UI to show the leaderboard and hide the name entry
        nameEntryTarget = belowScreenPosition;
        leaderboardTarget = Vector3.zero;

        // Clear input for next time
        nameInputField.text = string.Empty;
    }

    IEnumerator ShowWarningMessage()
    {
        canShowWarningMessage = false;
        warningMessage.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        // Hide warning message
        canShowWarningMessage = true;
        warningMessage.gameObject.SetActive(false);
    }
}