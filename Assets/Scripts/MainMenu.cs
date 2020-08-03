using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadLevel(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Share()
    {
        Application.OpenURL("izyplay.com.br/site/pt/");
    }

}
