using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    [SerializeField]
    Canvas boardCanvas = null;
    public Canvas BoardCanvas
    {
        get => boardCanvas;
        private set => boardCanvas = value;
    }

    [SerializeField]
    GameObject jellybeanPrefab = null;
    public GameObject JellybeanPrefab
    {
        get => jellybeanPrefab;
        private set => jellybeanPrefab = value;
    }

    [SerializeField]
    GameObject matchEffectPrefab = null;
    public GameObject MatchEffectPrefab
    {
        get => matchEffectPrefab;
        private set => matchEffectPrefab = value;
    }

    int currentScore = 0;
    float timeRemaining = 0.0f;
    bool isGameActive = false;

    void Awake()
    {
        // If multiple copies are generated, discard them.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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