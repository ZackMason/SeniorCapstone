using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMainMenu : MonoBehaviour
{
    public string gameStartScene;

    public void StartGame(){
        SceneManager.LoadScene(gameStartScene);
    }
}


