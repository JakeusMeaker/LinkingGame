using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } private set { } }
    private int currentScore = 0;
    private int scoreTarget = 0;

    private TMP_Text scoreText;
    private GameObject winPanel;

    private void Awake()
    {
        #region Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        #endregion
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TMP_Text>();
        winPanel = GameObject.FindGameObjectWithTag("WinPanel");
        winPanel.SetActive(false);
    }

    public void SetScoreTarget(int score)
    {
        scoreTarget = score;
        UpdateScoreText();
    }

    public void AddToScore(int score)
    {
        currentScore += score;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{currentScore} \n /{scoreTarget}";

        if (currentScore >= scoreTarget)
        {
            winPanel.SetActive(true);
        }
    }
}
