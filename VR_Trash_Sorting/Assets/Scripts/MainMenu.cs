using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSceneFromId(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
