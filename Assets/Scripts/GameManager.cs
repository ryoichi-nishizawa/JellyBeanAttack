using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Core Settings")]
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
        boardController.OnInvalidMatchClicked += PlayInvalidAnimation;
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

        currentScore += count * 10;
    }

    void PlayInvalidAnimation(Jellybean bean)
    {
        StartCoroutine(AnimateInvalidClick(bean.gameObject));
    }

    void EndGame()
    {
        isGameActive = false;
        boardController.SetInputActive(false);
        uiManager.ShowGameOver(currentScore);
    }

    IEnumerator AnimateInvalidClick(GameObject target)
    {
        if (target == null)
        {
            yield break;
        }

        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 0.8f;

        float elapsed = 0.0f;
        while (elapsed < 0.05f)
        {
            target.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / 0.05f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;
        while (elapsed < 0.05f)
        {
            target.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / 0.05f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = originalScale;
    }
}