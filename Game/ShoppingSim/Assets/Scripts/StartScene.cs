using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string SelectScene;

    public void SceneOpen(){
        SceneManager.LoadScene(SelectScene);
    }
}


