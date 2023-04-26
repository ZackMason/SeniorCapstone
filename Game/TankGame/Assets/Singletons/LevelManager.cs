using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    private float _progressTarget;
    
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
        }
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update() {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _progressTarget, 3f * Time.deltaTime);
    }

    public async void LoadScene(string sceneName) {
        _progressBar.fillAmount = 0f;
        _progressTarget = 0f;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        _loaderCanvas.SetActive(true);

        do {
            await Task.Delay(100);
            _progressTarget = scene.progress;
        } while(scene.progress < 0.9f);
        await Task.Delay(100);
        scene.allowSceneActivation = true;
        await Task.Delay(900);

        _loaderCanvas.SetActive(false);
    }
}
