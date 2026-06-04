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

    public void PlayMatchAnimation()
    {
        StartCoroutine(AnimateMatchCoroutine());
    }

    IEnumerator AnimateMatchCoroutine()
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 0.8f;

        float elapsed = 0.0f;
        while (elapsed < 0.05f)
        {
            gameObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / 0.05f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;
        while (elapsed < 0.05f)
        {
            gameObject.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / 0.05f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localScale = originalScale;
    }

    public void PlayInvalidAnimation()
    {
        StartCoroutine(AnimateInvalidCoroutine());
    }

    IEnumerator AnimateInvalidCoroutine()
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = originalScale * 0.8f;

        float elapsed = 0.0f;
        while (elapsed < 0.05f)
        {
            gameObject.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / 0.05f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;
        while (elapsed < 0.05f)
        {
            gameObject.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / 0.05f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localScale = originalScale;
    }
}