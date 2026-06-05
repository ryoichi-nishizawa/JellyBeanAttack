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

    Jellybean[,] board = null;

    public event Action<int> OnMatchScoreAwarded = null;

    public void InitializeNewBoard()
    {
        ClearBoard();
        board = new Jellybean[rows, columns];

        // The loop continues until a board state that is internally valid is created.
        int[,] tempGrid = new int[rows, columns];
        do
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    tempGrid[row, column] = UnityEngine.Random.Range(0, colors.Length);
                }
            }
        } while (!HasValidMatchOnGrid(tempGrid));

        // Create Board data.
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                GameObject go = Instantiate(GameManager.Instance.JellybeanPrefab, boardParent);
                Jellybean bean = go.GetComponent<Jellybean>();
                bean.Setup(row, column, tempGrid[row, column], colors[tempGrid[row, column]]);
                bean.OnClicked += HandleJellybeanClick;
                board[row, column] = bean;
            }
        }
    }

    void HandleJellybeanClick(Jellybean clickedBean)
    {
        List<Jellybean> group = FindConnectedGroup(clickedBean);
        if (group.Count >= matchJellybeans)
        {
            OnMatchScoreAwarded?.Invoke(group.Count);

            // Refill only the relevant cells.
            foreach (Jellybean bean in group)
            {
                int newColorIndex = UnityEngine.Random.Range(0, colors.Length);
                bean.PlayMatchAnimation(newColorIndex, colors[newColorIndex]);
            }

            // Check the effectiveness of the entire board.
            while (!HasValidMatchOnBoard())
            {
                RegenerateAllColorsData();
            }
        }
        else
        {
            clickedBean.PlayInvalidAnimation();
        }
    }

    // BFS search on current board data.
    // https://en.wikipedia.org/wiki/Breadth-first_search
    List<Jellybean> FindConnectedGroup(Jellybean startBean)
    {
        List<Jellybean> group = new List<Jellybean>();
        int targetColor = startBean.ColorIndex;

        bool[,] visited = new bool[rows, columns];
        Queue<Jellybean> queue = new Queue<Jellybean>();

        queue.Enqueue(startBean);
        visited[startBean.Row, startBean.Column] = true;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        while (queue.Count > 0)
        {
            Jellybean current = queue.Dequeue();
            group.Add(current);

            foreach (Vector2Int dir in directions)
            {
                int nextRow = current.Row + dir.x;
                int nextColumn = current.Column + dir.y;
                if (nextRow >= 0 && nextRow < rows && nextColumn >= 0 && nextColumn < columns)
                {
                    Jellybean neighbor = board[nextRow, nextColumn];
                    if (!visited[nextRow, nextColumn] && neighbor.ColorIndex == targetColor)
                    {
                        visited[nextRow, nextColumn] = true;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }

        return group;
    }

    bool HasValidMatchOnBoard()
    {
        bool[,] checkedCells = new bool[rows, columns];
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (!checkedCells[row, column])
                {
                    List<Jellybean> group = FindConnectedGroup(board[row, column]);
                    if (group.Count >= matchJellybeans)
                    {
                        return true;
                    }

                    foreach (Jellybean b in group)
                    {
                        checkedCells[b.Row, b.Column] = true;
                    }
                }
            }
        }

        return false;
    }

    void RegenerateAllColorsData()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                int newColorIndex = UnityEngine.Random.Range(0, colors.Length);
                board[row, column].SetColor(newColorIndex, colors[newColorIndex]);
            }
        }
    }

    // Validation logic for the temporary grid used for initial generation.
    bool HasValidMatchOnGrid(int[,] grid)
    {
        int rowCount = grid.GetLength(0);
        int columnCount = grid.GetLength(1);
        bool[,] visited = new bool[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int column = 0; column < columnCount; column++)
            {
                if (!visited[row, column])
                {
                    // Simple temporary search.
                    int count = 0;
                    int targetColor = grid[row, column];
                    Queue<Vector2Int> q = new Queue<Vector2Int>();
                    List<Vector2Int> currentGroup = new List<Vector2Int>();

                    q.Enqueue(new Vector2Int(row, column));
                    visited[row, column] = true;

                    while (q.Count > 0)
                    {
                        Vector2Int curr = q.Dequeue();
                        currentGroup.Add(curr);
                        count++;

                        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                        foreach (var dir in dirs)
                        {
                            int nextRow = curr.x + dir.x;
                            int nextColumn = curr.y + dir.y;
                            if (nextRow >= 0 && nextRow < rowCount && nextColumn >= 0 && nextColumn < columnCount)
                            {
                                if (!visited[nextRow, nextColumn] && grid[nextRow, nextColumn] == targetColor)
                                {
                                    visited[nextRow, nextColumn] = true;
                                    q.Enqueue(new Vector2Int(nextRow, nextColumn));
                                }
                            }
                        }
                    }

                    if (count >= matchJellybeans)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
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