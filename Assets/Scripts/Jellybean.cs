using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Jellybean : MonoBehaviour
{
    [SerializeField]
    Image image = null;

    [SerializeField]
    Button button = null;
    public Button Button
    {
        get => button;
        private set => button = value;
    }

    public int Row { get; private set; }
    public int Column { get; private set; }
    public int ColorIndex { get; private set; }

    // Click Event.
    public event Action<Jellybean> OnClicked = null;

    void Awake()
    {
        button.onClick.AddListener(() => OnClicked?.Invoke(this));
    }

    public void Setup(int row, int column, int colorIndex, Color color)
    {
        Row = row;
        Column = column;
        SetColor(colorIndex, color);
    }

    public void SetColor(int colorIndex, Color color)
    {
        ColorIndex = colorIndex;
        image.color = color;
    }

    public void PlayMatchAnimation(int colorIndex, Color color)
    {
        GameObject effect = Instantiate(GameManager.Instance.MatchEffectPrefab, GameManager.Instance.BoardCanvas.gameObject.transform);
        effect.transform.position = transform.position;

        StartCoroutine(AnimateMatchCoroutine(colorIndex, color));
    }

    IEnumerator AnimateMatchCoroutine(int colorIndex, Color color)
    {
        Vector3 targetScale = Vector3.one * 0.0f;

        float elapsed = 0.0f;
        while (elapsed < 0.35f)
        {
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, targetScale, elapsed / 0.35f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetColor(colorIndex, color);

        elapsed = 0.0f;
        while (elapsed < 0.35f)
        {
            gameObject.transform.localScale = Vector3.Lerp(targetScale, Vector3.one, elapsed / 0.35f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localScale = Vector3.one;
    }

    public void PlayInvalidAnimation()
    {
        StartCoroutine(AnimateInvalidCoroutine());
    }

    IEnumerator AnimateInvalidCoroutine()
    {
        Vector3 targetScale = Vector3.one * 0.5f;

        float elapsed = 0.0f;
        while (elapsed < 0.2f)
        {
            gameObject.transform.localScale = Vector3.Lerp(Vector3.one, targetScale, elapsed / 0.2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;
        while (elapsed < 0.2f)
        {
            gameObject.transform.localScale = Vector3.Lerp(targetScale, Vector3.one, elapsed / 0.2f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localScale = Vector3.one;
    }
}