using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } private set { } }
    private int currentScore = 0;
    private int scoreTarget = 0;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject winPanel;

    private LevelGrid levelGrid;

    [SerializeField] private GameObject matcher;
    [SerializeField] private LevelDataSO[] levels;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadMenu;
    private int currentLevel;

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

        levelGrid = GetComponent<LevelGrid>();
        winPanel.SetActive(false);
        currentLevel = 0;
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
            matcher.SetActive(false);
        }
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        winPanel.SetActive(false);
        levelGrid.LoadLevelGrid(levels[currentLevel]);
        matcher.SetActive(true);
    }

    public void MainMenu()
    {
        matcher.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void NextLevel()
    {
        currentScore = 0;

        if (currentLevel < levels.Length)
        {
            currentLevel++;
            levelGrid.LoadLevelGrid(levels[currentLevel]);
        }
        else
        {
            levelGrid.GenerateRandomGrid(scoreTarget *= (int)(scoreTarget * 1.5f));
        }
        matcher.SetActive(true);

        loadMenu.SetActive(false);
        mainMenu.SetActive(false);
        winPanel.SetActive(false);
    }

    public void NextLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        if (currentLevel < levels.Length)
        {
            levelGrid.LoadLevelGrid(levels[currentLevel]);
        }
        else
        {
            levelGrid.GenerateRandomGrid(scoreTarget *= (int)(scoreTarget * 1.5f));
        }
        matcher.SetActive(true);

        loadMenu.SetActive(false);
        mainMenu.SetActive(false);
        winPanel.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
