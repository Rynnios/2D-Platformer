using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardTable : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;

    private void Awake()
    {
        //entryContainer = transform.Find("");
        //entryTemplate = entryContainer.Find("");

        //entryTemplate.gameObject.SetActive(false);

        //float templateHeight = 30f;
        //for (int i = 0; i < 10; i++)
        //{
        //    Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        //    RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

        //    entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight * i);
        //    entryTransform.gameObject.SetActive(true);

            //int rank = i + 1;
            //string rankString;
            //switch (rank)
            //{
            //    default:
            //        rankString = rank + "TH"; break;

            //    case 1: rankString = "1st"; break;
            //    case 2: rankString = "2nd"; break;
            //    case 3: rankString = "3rd"; break;
            //}

            //entryTransform.Find("posText").GetComponent<Text>().text = rankString;

            //int score = Random.Range(0, 100);
            //entryTransform.Find("timeText").GetComponent<Text>().text = score.ToString();

            //string name = "AAA";
            //entryTransform.Find("nameText").GetComponent<Text>().text = name;



        //}
    }
}
