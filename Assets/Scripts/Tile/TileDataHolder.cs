using System.Collections;
using System.Collections.Generic;
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
    private TileDataSO tileData;
    private bool isEmpty = false;

    public int xPos { get; private set; }
    public int yPos { get; private set; }

    [SerializeField] private SpriteRenderer tileRenderer;

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

    public void SetTileData(TileDataSO _tileData, int x, int y)
    {
        xPos = x;
        yPos = y;

        tileData = _tileData;
        isEmpty = false;

        tileRenderer.sprite = tileData.sprite;
    }

    public TileDataSO GetTileData()
    {
        return tileData;
    }

    public void DestroyTile()
    {
        //Animate the tile going and add fx
        tileRenderer.sprite = null;
        tileData = null;
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
}
