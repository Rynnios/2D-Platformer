using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveScores : MonoBehaviour
{
    public TMP_InputField inputField;
    public TMP_Text timerText;
    public TMP_Text finalTimeText;
    float timer;
    float finalScore;

    private void Start()
    {
        finalScore = PlayerPrefs.GetFloat("CurrentScore", 0);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");

        timerText.text = $"{minutes}:{seconds}";

        if (finalScore > 0)
        {
            FinalTimeScore();
        }
    }

    public void FinalTimeScore()
    {
        finalScore = PlayerPrefs.GetFloat("CurrentScore");
        string fminutes = Mathf.Floor(finalScore / 60).ToString("00");
        string fseconds = (finalScore % 60).ToString("00");

        finalTimeText.text = $"{fminutes}:{fseconds}";
    }

    public void SetScores()
    {
        List<float> scores = new List<float>();
        List<string> names = new List<string>();

        scores.Add(PlayerPrefs.GetFloat("Score1"));
        scores.Add(PlayerPrefs.GetFloat("Score2"));
        scores.Add(PlayerPrefs.GetFloat("Score3"));

        names.Add(PlayerPrefs.GetString("Name1"));
        names.Add(PlayerPrefs.GetString("Name2"));
        names.Add(PlayerPrefs.GetString("Name3"));

        for (int i = 0; i < 3; i++)
        {
            if (scores[i] <= 0)
            {
                scores[i] = 1000f;
            }

            if (finalScore < scores[i])
            {
                scores.Insert(i, finalScore);
                names.Insert(i, inputField.text);
                break;
            }
        }

        PlayerPrefs.SetFloat("Score1", scores[0]);
        PlayerPrefs.SetFloat("Score2", scores[1]);
        PlayerPrefs.SetFloat("Score3", scores[2]);

        PlayerPrefs.SetString("Name1", names[0]);
        PlayerPrefs.SetString("Name2", names[1]);
        PlayerPrefs.SetString("Name3", names[2]);

        SceneManager.LoadScene(3);

    }
}
