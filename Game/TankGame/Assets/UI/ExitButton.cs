using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void OnExit() => Application.Quit();
    public void QuitToMenu() => SceneManager.LoadScene("MainMenu");
}
