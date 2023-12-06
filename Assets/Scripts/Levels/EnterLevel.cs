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
}
