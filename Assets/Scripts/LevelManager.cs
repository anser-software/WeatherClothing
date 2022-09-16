using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    
    public static LevelManager instance { get; private set; }

    public int currentScene => PlayerPrefs.GetInt("CurrentScene", 1);

    public int currentLevel => PlayerPrefs.GetInt("CurrentLevel", 1);

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartNextLevel();
    }

    public void WinLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);

        SetNextScene();
    }

    private void SetNextScene()
    {
        var nextScene = currentScene + 1;

        if (nextScene >= SceneManager.sceneCountInBuildSettings)
            nextScene = 1;

        PlayerPrefs.SetInt("CurrentScene", nextScene);
    }
    
    public void StartNextLevel()
    {
        SceneManager.LoadScene(currentScene);
    }

}
