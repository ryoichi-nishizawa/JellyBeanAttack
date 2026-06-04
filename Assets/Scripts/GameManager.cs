using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Core Settings")]
    [SerializeField]
    int additionalScore = 10;
    public int AdditionalScore
    {
        get => additionalScore;
        private set => additionalScore = value;
    }

    [SerializeField]
    float gameDuration = 60.0f;

    [Header("Module References")]
    [SerializeField]
    BoardController boardController = null;

    [SerializeField]
    UIManager uiManager = null;

    int currentScore = 0;
    float timeRemaining = 0.0f;
    bool isGameActive = false;

    void Start()
    {
        boardController.OnMatchScoreAwarded += AddScore;
        uiManager.RestartButton.onClick.AddListener(StartGame);
        StartGame();
    }

    void Update()
    {
        if (!isGameActive)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.0f)
        {
            timeRemaining = 0.0f;
            EndGame();
        }

        uiManager.UpdateGameplayUI(currentScore, timeRemaining);
    }

    void StartGame()
    {
        currentScore = 0;
        timeRemaining = gameDuration;

        uiManager.HideGameOver();
        uiManager.UpdateGameplayUI(currentScore, timeRemaining);

        boardController.InitializeNewBoard();
        boardController.SetInputActive(true);

        isGameActive = true;
    }

    void AddScore(int count)
    {
        if (!isGameActive)
        {
            return;
        }

        currentScore += count * additionalScore;
    }

    void EndGame()
    {
        isGameActive = false;
        boardController.SetInputActive(false);
        uiManager.ShowGameOver(currentScore);
    }
}