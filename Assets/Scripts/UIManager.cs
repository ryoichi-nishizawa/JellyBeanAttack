using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("TextMeshPro References")]
    [SerializeField]
    TextMeshProUGUI scoreText = null;

    [SerializeField]
    TextMeshProUGUI timerText = null;

    [SerializeField]
    TextMeshProUGUI finalScoreText = null;

    [Header("Panels & Buttons")]
    [SerializeField]
    GameObject gameOverPanel = null;

    [SerializeField]
    Button restartButton = null;
    public Button RestartButton
    {
        get => restartButton;
        private set => restartButton = value;
    }

    [Header("Module References")]
    [SerializeField]
    GameManager gameManager = null;

    readonly WaitForSeconds delay100msec = new WaitForSeconds(0.1f);

    public void UpdateGameplayUI(int score, float timeRemaining)
    {
        scoreText.text = $"{score}";
        timerText.text = $"{Mathf.CeilToInt(timeRemaining)}s";
    }

    public void ShowGameOver(in int finalScore)
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(AnimateScoreCoroutine(finalScore));
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }

    IEnumerator AnimateScoreCoroutine(int finalScore)
    {
        RestartButton.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);

        yield return delay100msec;
        finalScoreText.gameObject.SetActive(true);

        for (int score = 0 ; score < finalScore ; score += gameManager.AdditionalScore)
        {
            finalScoreText.text = $"{score}";
            yield return null;
        }

        finalScoreText.text = $"{finalScore}";
        yield return delay100msec;

        RestartButton.gameObject.SetActive(true);
    }
}