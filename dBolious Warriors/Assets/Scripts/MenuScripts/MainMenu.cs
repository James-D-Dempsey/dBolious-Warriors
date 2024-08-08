using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    public void playPVE() //hardcoded to go to start match
    {
        SceneManager.LoadScene(1);
    }

    public void playPVP() //hardcoded to go to start match
    {
        SceneManager.LoadScene(2);
    }

    public void quitGame() //exits application
    {
        Application.Quit();
    }
}
