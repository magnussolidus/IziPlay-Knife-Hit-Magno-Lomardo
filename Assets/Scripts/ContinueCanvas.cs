using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueCanvas : MonoBehaviour
{

    public GameplayController gc;
    void Start()
    {
        if (!gc)
            gc = GameplayController.instance;
        
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void CallRestart()
    {
        gc.StartGame();
        this.GetComponent<Canvas>().enabled = false;
    }

}
