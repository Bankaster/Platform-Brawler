using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void ExitButton()
    {
        //Close the Application
        Application.Quit();
    }

    public void MainMenuButton()
    {
        //Load the Main Menu Scene
        SceneManager.LoadScene(0);
    }
}
