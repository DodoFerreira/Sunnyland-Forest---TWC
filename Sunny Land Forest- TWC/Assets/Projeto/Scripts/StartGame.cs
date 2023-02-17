using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PressEnter();
        }
        
    }

    public void PressEnter ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void Quit()
    {
        Application.Quit();
    }

}
