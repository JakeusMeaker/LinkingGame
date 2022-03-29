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
    public event EventHandler<OnTileDestroyedEventArgs> OnTileDestroyed;

    public class OnTileDestroyedEventArgs : EventArgs
    {
        public TileDataHolder tileDataHolder { get; set; }
    }

    private TileDataSO tileData = null;
    public bool isEmpty { get; private set; }

    public int xPos { get; private set; }
    public int yPos { get; private set; }

    [SerializeField] private SpriteRenderer tileRenderer;
    private Vector3 defaultTileRendererScale;
    [SerializeField] private Vector3 tileExpansionScale;
    [SerializeField] private float tileExpansionSpeed;

    [SerializeField] private ParticleSystem particals;

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

    private void Start()
    {
        defaultTileRendererScale = tileRenderer.transform.localScale;
    }

    public void CheckTileBelow()
    {
        if (yPos == 0) return;

        if (levelGrid.levelGrid[xPos, yPos - 1].GetComponent<TileDataHolder>().isEmpty)
        {
            levelGrid.levelGrid[xPos, yPos - 1].GetComponent<TileDataHolder>().SetTileData(tileData);
            SetTileEmpty();
            return;
        }
    }

    public void SetTileData(LevelGrid _levelGrid, TileDataSO _tileData, int x, int y)
    {
        xPos = x;
        yPos = y;

        if (levelGrid == null)
        {
            levelGrid = _levelGrid;
        }

        tileData = _tileData;
        isEmpty = false;

        tileRenderer.sprite = tileData.sprite;
    }

    public void SetTileData(TileDataSO _tileData)
    {
        tileRenderer.transform.localScale = defaultTileRendererScale;
        tileData = _tileData;
        if (tileData != null)
        {
            isEmpty = false;
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
        while (tileRenderer.transform.localScale.x < tileExpansionScale.x)
        {
            tileRenderer.transform.localScale += Vector3.Lerp(tileRenderer.transform.localScale, tileExpansionScale, tileExpansionSpeed * Time.deltaTime);
        }
        particals.Emit(20);
        tileRenderer.sprite = null;
        SetTileEmpty();
        OnTileDestroyed?.Invoke(this, new OnTileDestroyedEventArgs { tileDataHolder = this });
    }

    public void SetTileEmpty()
    {
        isEmpty = true;
        SetTileData(null);
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

    // Determines the direction of the given tile to this tile
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
}
