using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Text         menuButton;
    public GameObject   credits;

    void Start ()
    {
        menuButton.enabled = false;
        StartCoroutine ("ButtonDelay");
    }

    public void Menu ()
    {
        SceneManager.LoadScene("Menu");
    }



    IEnumerator ButtonDelay ()
    {
        yield return new WaitForSeconds (15f);
        menuButton.enabled = true;
        credits.SetActive(false);

    }
}
