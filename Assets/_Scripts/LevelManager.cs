using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonClass<LevelManager>
{

    private int _requiredPoints;
    public GameObject _gameOverPanel;
    public GameObject _levelComplete;
    
    public int _pointsToWin
    {
        get { return _requiredPoints; }
        set
        {
            _requiredPoints=value;
            CheckStatus();
        }
    }

    public override void Awake()
    {
        Time.timeScale = 1;
        base.Awake();
    }

    public void CheckStatus()
    {
        if (_pointsToWin <= 0)
        {
          LevelComplete();  
        }
    }

    public void LevelComplete()
    {
        Time.timeScale = 0;
        _levelComplete.SetActive(true);
    }

    public void GameOver()
    {
        _gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
