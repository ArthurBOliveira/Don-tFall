using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    public void Online()
    {
        SceneManager.LoadScene("MainOnline");
    }

    public void SocketTest()
    {
        SceneManager.LoadScene("SocketTest");
    }

    public void Support()
    {
        SceneManager.LoadScene("Support");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Site()
    {
        Application.OpenURL("https://nivelhard.herokuapp.com/");
    }
}
