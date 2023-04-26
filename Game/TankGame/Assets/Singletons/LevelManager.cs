using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [SerializeField] private GameObject _optionsCanvas;
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    private float _progressTarget;

    private void _showLoader(bool on) => _loaderCanvas.SetActive(on);
    public void ShowOptions(bool on) => _optionsCanvas.SetActive(on);
    public bool IsShowingOptions() => _optionsCanvas.activeSelf;
    
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

    void Update() =>
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _progressTarget, 3f * Time.deltaTime);
    
    public void LoadScene(string sceneName) =>
        StartCoroutine(_loadScene(sceneName));

    IEnumerator _loadScene(string sceneName) {
        _progressBar.fillAmount = 0f;
        _progressTarget = 0f;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        _showLoader(true);

        do {
            yield return null;
            _progressTarget = scene.progress;
        } while(scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        _showLoader(false);
    }
}
