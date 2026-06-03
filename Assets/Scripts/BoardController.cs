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
    Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };

    [Header("References")]
    [SerializeField]
    Transform boardParent = null;

    [SerializeField]
    GameObject jellybeanPrefab = null;

    Jellybean[,] board = null;

    public event Action<int> OnMatchScoreAwarded = null;
    public event Action<Jellybean> OnInvalidMatchClicked = null;

    public void InitializeNewBoard()
    {
        ClearBoard();
        board = new Jellybean[rows, columns];

        // The loop continues until a board state that is internally valid is created.
        int[,] tempGrid = new int[rows, columns];
        do
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    tempGrid[r, c] = UnityEngine.Random.Range(0, colors.Length);
                }
            }
        } while (!HasValidMatchOnGrid(tempGrid));

        // Create Board data.
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                GameObject go = Instantiate(jellybeanPrefab, boardParent);
                Jellybean bean = go.GetComponent<Jellybean>();
                bean.Setup(r, c, tempGrid[r, c], colors[tempGrid[r, c]]);
                bean.OnClicked += HandleJellybeanClick;
                board[r, c] = bean;
            }
        }
    }

    void HandleJellybeanClick(Jellybean clickedBean)
    {
        List<Jellybean> group = FindConnectedGroup(clickedBean);
        if (group.Count >= 3)
        {
            OnMatchScoreAwarded?.Invoke(group.Count);

            // Refill only the relevant cells.
            foreach (Jellybean bean in group)
            {
                int newColorIdx = UnityEngine.Random.Range(0, colors.Length);
                bean.SetColor(newColorIdx, colors[newColorIdx]);
            }

            // 盤面全体の有効性チェック
            while (!HasValidMatchOnBoard())
            {
                RegenerateAllColorsData();
            }
        }
        else
        {
            OnInvalidMatchClicked?.Invoke(clickedBean);
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
                int nextR = current.Row + dir.x;
                int nextC = current.Column + dir.y;
                if (nextR >= 0 && nextR < rows && nextC >= 0 && nextC < columns)
                {
                    Jellybean neighbor = board[nextR, nextC];
                    if (!visited[nextR, nextC] && neighbor.ColorIndex == targetColor)
                    {
                        visited[nextR, nextC] = true;
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
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                if (!checkedCells[r, c])
                {
                    List<Jellybean> group = FindConnectedGroup(board[r, c]);
                    if (group.Count >= 3)
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
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                int newColorIdx = UnityEngine.Random.Range(0, colors.Length);
                board[r, c].SetColor(newColorIdx, colors[newColorIdx]);
            }
        }
    }

    // Validation logic for the temporary grid used for initial generation.
    bool HasValidMatchOnGrid(int[,] grid)
    {
        int rCount = grid.GetLength(0);
        int cCount = grid.GetLength(1);
        bool[,] visited = new bool[rCount, cCount];

        for (int r = 0; r < rCount; r++)
        {
            for (int c = 0; c < cCount; c++)
            {
                if (!visited[r, c])
                {
                    // 簡易的な一時探索
                    int count = 0;
                    int targetColor = grid[r, c];
                    Queue<Vector2Int> q = new Queue<Vector2Int>();
                    List<Vector2Int> currentGroup = new List<Vector2Int>();

                    q.Enqueue(new Vector2Int(r, c));
                    visited[r, c] = true;

                    while (q.Count > 0)
                    {
                        Vector2Int curr = q.Dequeue();
                        currentGroup.Add(curr);
                        count++;

                        Vector2Int[] dirs = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
                        foreach (var dir in dirs)
                        {
                            int nR = curr.x + dir.x;
                            int nC = curr.y + dir.y;
                            if (nR >= 0 && nR < rCount && nC >= 0 && nC < cCount)
                            {
                                if (!visited[nR, nC] && grid[nR, nC] == targetColor)
                                {
                                    visited[nR, nC] = true;
                                    q.Enqueue(new Vector2Int(nR, nC));
                                }
                            }
                        }
                    }

                    if (count >= 3)
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