using System;
using UnityEngine;

public enum ConnectionLines
{
    Up,
    UpRight,
    Right,
    DownRight,
    Down,
    DownLeft,
    Left,
    UpLeft,
    NULL
}

public class TileDataHolder : MonoBehaviour
{
    private TileDataSO tileData = null;
    public bool isEmpty { get; private set; }

    public int xPos { get; private set; }
    public int yPos { get; private set; }

    [SerializeField] private SpriteRenderer tileRenderer;

    private LevelGrid levelGrid;

    /// <summary>
    /// 0 = Up
    /// 1 = Up Right
    /// 2 = Right
    /// 3 = Down Right
    /// 4 = Down
    /// 5 = Down Left
    /// 6 = Left
    /// 7 = Up Left
    /// </summary>
    [Tooltip("0 = Up, 1 = Up Right, 2 = Right, 3 = Down Right, 4 = Down, 5 = Down Left, 6 = Left, 7 = Up Left")]
    [SerializeField] private GameObject[] connectionLines;

    public void SetTileData(LevelGrid _levelGrid, TileDataSO _tileData, int x, int y)
    {
        isEmpty = false;
        xPos = x;
        yPos = y;

        levelGrid = _levelGrid;

        tileData = _tileData;

        tileRenderer.sprite = tileData.sprite;
    }

    public void SetTileData(TileDataSO _tileData)
    {
        isEmpty = false;
        tileData = _tileData;
        if (tileData != null)
        {
            tileRenderer.sprite = tileData.sprite;
        }
        else
        {
            tileRenderer.sprite = null;
        }
    }

    public TileDataSO GetTileData()
    {
        return tileData;
    }

    public void DestroyTile()
    {
        //Animate the tile going and add fx
        tileRenderer.sprite = null;
        SetTileEmpty();
    }

    public void SetTileEmpty()
    {
        SetTileData(null);
        isEmpty = true;
    }

    public void EnableConnection(ConnectionLines direction)
    {
        if (direction == ConnectionLines.NULL) return;
        connectionLines[(int)direction].SetActive(true);
    }

    public void DisableConnection(ConnectionLines direction)
    {
        if (direction == ConnectionLines.NULL) return;
        connectionLines[(int)direction].SetActive(false);
    }

    public void DisableAllConnections()
    {
        foreach (GameObject connection in connectionLines)
        {
            connection.SetActive(false);
        }
    }

    public ConnectionLines DetermineConnection(TileDataHolder otherTile)
    {
        if (otherTile.xPos == xPos && otherTile.yPos > yPos)
        {
            return ConnectionLines.Up;
        }

        if (otherTile.xPos > xPos && otherTile.yPos > yPos)
        {
            return ConnectionLines.UpRight;
        }

        if (otherTile.xPos > xPos && otherTile.yPos == yPos)
        {
            return ConnectionLines.Right;
        }

        if (otherTile.xPos > xPos && otherTile.yPos < yPos)
        {
            return ConnectionLines.DownRight;
        }

        if (otherTile.xPos == xPos && otherTile.yPos < yPos)
        {
            return ConnectionLines.Down;
        }

        if (otherTile.xPos < xPos && otherTile.yPos < yPos)
        {
            return ConnectionLines.DownLeft;
        }

        if (otherTile.xPos < xPos && otherTile.yPos == yPos)
        {
            return ConnectionLines.Left;
        }

        if (otherTile.xPos < xPos && otherTile.yPos > yPos)
        {
            return ConnectionLines.UpLeft;
        }

        return ConnectionLines.NULL;
    }

    private void Update()
    {
        if (yPos == 0) return;

        if (levelGrid.levelGrid[xPos, yPos - 1].GetComponent<TileDataHolder>().isEmpty)
        {
            levelGrid.levelGrid[xPos, yPos - 1].GetComponent<TileDataHolder>().SetTileData(tileData);
            SetTileEmpty();
            return;
        }
    }
}
