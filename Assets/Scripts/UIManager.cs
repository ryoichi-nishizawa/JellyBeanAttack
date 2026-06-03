using UnityEngine;
using UnityEngine.UI;
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

    public Button RestartButton = null;

    public void UpdateGameplayUI(int score, float timeRemaining)
    {
        scoreText.text = $"Score : {score}";
        timerText.text = $"Time : {Mathf.CeilToInt(timeRemaining)}s";
    }

    public void ShowGameOver(int finalScore)
    {
        finalScoreText.text = $"Score : {finalScore}";
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
}