using System;
using UnityEngine;
using UnityEngine.UI;

public class Jellybean : MonoBehaviour
{
    [SerializeField]
    Image image = null;

    [SerializeField]
    Button button = null;
    public Button Button => button;

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
}