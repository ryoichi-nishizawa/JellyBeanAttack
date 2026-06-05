using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    int rows = 6;

    [SerializeField]
    int columns = 6;

    [SerializeField]
    int matchJellybeans = 3;

    [SerializeField]
    Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };

    [Header("References")]
    [SerializeField]
    Transform boardParent = null;

    readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    bool[,] visitedGrid = null;
    int[,] colorGrid = null;
    Jellybean[,] board = null;

    public event Action<int> OnMatchScoreAwarded = null;

    /// <summary>
    /// Initializing a new board.
    /// </summary>
    public void InitializeNewBoard()
    {
        ClearBoard();

        visitedGrid = new bool[rows, columns];
        colorGrid = new int[rows, columns];
        board = new Jellybean[rows, columns];

        GenerateValidGridData();

        // Create Jelly bean.
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                GameObject go = Instantiate(GameManager.Instance.JellybeanPrefab, boardParent);
                Jellybean bean = go.GetComponent<Jellybean>();

                bean.Setup(row, column, colorGrid[row, column], colors[colorGrid[row, column]]);
                bean.OnClicked += HandleJellybeanClick;
                board[row, column] = bean;
            }
        }
    }

    /// <summary>
    /// Click on the jelly bean.
    /// </summary>
    void HandleJellybeanClick(Jellybean clickedBean)
    {
        var startPos = new Vector2Int(clickedBean.Row, clickedBean.Column);
        List<Vector2Int> group = FindConnectedGroup(startPos);

        if (group.Count >= matchJellybeans)
        {
            OnMatchScoreAwarded?.Invoke(group.Count);

            foreach (Vector2Int pos in group)
            {
                colorGrid[pos.x, pos.y] = UnityEngine.Random.Range(0, colors.Length);
                board[pos.x, pos.y].PlayMatchAnimation(colorGrid[pos.x, pos.y], colors[colorGrid[pos.x, pos.y]]);
            }

            // When there are no matching items.
            while (!HasValidMatch())
            {
                GenerateValidGridData();
                SyncAllViews();
            }
        }
        else
        {
            clickedBean.PlayInvalidAnimation();
        }
    }

    /// <summary>
    /// Randomly generate grid data until a valid match is found.
    /// </summary>
    void GenerateValidGridData()
    {
        int safety = 0;
        do
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    colorGrid[row, column] = UnityEngine.Random.Range(0, colors.Length);
                }
            }

            safety++;
        } while (!HasValidMatch() && safety < 100);
    }

    /// <summary>
    /// Reflect all current data.
    /// </summary>
    void SyncAllViews()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                int colorIdx = colorGrid[row, column];
                board[row, column].PlayMatchAnimation(colorIdx, colors[colorIdx]);
            }
        }
    }

    /// <summary>
    /// Determine if there are any valid matches on the grid.
    /// </summary>
    bool HasValidMatch()
    {
        Array.Clear(visitedGrid, 0, visitedGrid.Length);
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (!visitedGrid[row, column])
                {
                    List<Vector2Int> group = FindConnectedGroup(new Vector2Int(row, column));
                    if (group.Count >= matchJellybeans)
                    {
                        return true;
                    }

                    foreach (Vector2Int pos in group)
                    {
                        visitedGrid[pos.x, pos.y] = true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Returns groups of the same color connected to the specified coordinates.
    /// </summary>
    List<Vector2Int> FindConnectedGroup(Vector2Int startPos)
    {
        List<Vector2Int> group = new List<Vector2Int>();
        int targetColor = colorGrid[startPos.x, startPos.y];

        Array.Clear(visitedGrid, 0, visitedGrid.Length);
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(startPos);
        visitedGrid[startPos.x, startPos.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            group.Add(current);

            foreach (Vector2Int dir in directions)
            {
                int nextRow = current.x + dir.x;
                int nextColumn = current.y + dir.y;
                if (nextRow >= 0 && nextRow < rows && nextColumn >= 0 && nextColumn < columns)
                {
                    if (!visitedGrid[nextRow, nextColumn] && colorGrid[nextRow, nextColumn] == targetColor)
                    {
                        visitedGrid[nextRow, nextColumn] = true;
                        queue.Enqueue(new Vector2Int(nextRow, nextColumn));
                    }
                }
            }
        }

        return group;
    }

    public void SetInputActive(bool active)
    {
        if (board == null)
        {
            return;
        }

        foreach (Jellybean bean in board)
        {
            if (bean != null)
            {
                bean.Button.interactable = active;
            }
        }
    }

    void ClearBoard()
    {
        if (board == null)
        {
            return;
        }

        foreach (Transform child in boardParent)
        {
            Destroy(child.gameObject);
        }

        board = null;
    }
}