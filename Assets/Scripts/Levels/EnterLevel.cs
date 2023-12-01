using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class EnterLevel : MonoBehaviour
{
    private bool enterAllowed;
    private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Level1>())
        {
            sceneToLoad = "Level 1";
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (collision.GetComponent<Level2>())
        {
            sceneToLoad = "Level 2";
            SceneManager.LoadScene(sceneToLoad);

        }
        else if (collision.GetComponent<Level3>())
        {
            sceneToLoad = "Boss Level";

            SceneManager.LoadScene(sceneToLoad);

        }
    }
}
