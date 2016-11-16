using System;
using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour
{

    public Transform EmptySprite;
    public int m_Height = 30;
    public int m_Width = 10;
    public int m_Header = 10;

    private Transform[,] _grid;

    void Awake()
    {
        _grid = new Transform[m_Width,m_Height];
    }

// Use this for initialization
    void Start()
    {
        DrawEmptyCells();
    }

    // Update is called once per frame
    void Update()
    {

    }

    bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < m_Width && y >= 0);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            var pos = Vectorf.Round(child.position);
            if (!IsWithinBoard((int) pos.x, (int) pos.y))
            {
                return false;
            }

            if (IsOccupied((int) pos.x, (int) pos.y, shape))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsOccupied(int x, int y, Shape shape)
    {
        return (_grid[x, y] != null && _grid[x, y].parent != shape.transform);
    }

    public void StoreShape(Shape shape)
    {
        if (shape == null)
            return;

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            _grid[(int) pos.x, (int) pos.y] = child;
        }
    }

    void DrawEmptyCells()
    {
        if (EmptySprite != null)
        {
            for (var y = 0; y < m_Height - m_Header; y++)
            {
                for (var x = 0; x < m_Width; x++)
                {
                    var clone = Instantiate(EmptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    if (clone == null) continue;

                    clone.name = string.Format("Board Space ( x = {0} , y = {1}", x, y);
                    clone.transform.parent = transform;
                }
            }
        }
        else
        {
            Debug.Log("Warning no sprite!");
        }
    }

    bool IsComplete(int y)
    {
        for (int x = 0; x < m_Width; x++)
        {
            if (_grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void ClearRow(int y)
    {
        for (int x = 0; x < m_Width; x++)
        {
            if (_grid[x, y] != null)
            {
                Destroy(_grid[x, y].gameObject);
            }

            _grid[x, y] = null;
        }
    }

    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < m_Width; x++)
        {
            if (_grid[x, y] != null)
            {
                _grid[x, y-1] = _grid[x, y];
                _grid[x, y] = null;
                _grid[x, y-1].position += new Vector3(0, -1, 0);
            }
        }
    }

    void ShiftRowsDown(int startY)
    {
        for (int i = startY; i < m_Height; ++i)
        {
            ShiftOneRowDown(i);
        }

    }

    public void ClearAllRows()
    {
        for (int y = 0; y < m_Height; y++)
        {
            if (IsComplete(y))
            {
                ClearRow(y);

                ShiftRowsDown(y+1);
                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (m_Height - m_Header - 1))
            {
                return true;
            }
        }

        return false;
    }

}
