using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void returnToHome() 
    {
        SceneManager.LoadScene(0); //hardcoded return to main menu
    }
}
