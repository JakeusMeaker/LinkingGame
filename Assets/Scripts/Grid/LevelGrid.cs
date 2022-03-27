using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGrid : MonoBehaviour
{
    public GameObject[,] levelGrid;
    public int width;
    public int height;
    public float tileSize;

    [SerializeField] TileDataSO[] tileDataArray;

    [SerializeField] private GameObject tilePrefab;
    Vector3 offset;

    [SerializeField] private LevelDataSO[] levels;

    private void Start()
    {
        offset = transform.position;
        if (levels.Length != 0)
        {
            LoadLevelGrid(0);
        }
        else
        {
            GenerateRandomGrid();
        }
    }

    private void LoadLevelGrid(int level)
    {
        for (int i = 0; i < levels[level].levelGrid.GetLength(0); i++)
        {
            for (int j = 0; j < levels[level].levelGrid.GetLength(1); j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j) * levels[level].tileSize + levels[level].offset, Quaternion.identity, transform);
                tile.transform.localScale = tile.transform.localScale * levels[level].tileSize;
                TileDataHolder spawnedTile = tile.GetComponent<TileDataHolder>();
                spawnedTile.SetTileData(this, levels[level].levelGrid[i, j], i, j);
                spawnedTile.OnTileDestroyed += TileDataHolder_OnTileDestroyed;
                levelGrid[i, j] = tile;
            }
        }

        ScoreManager.Instance.SetScoreTarget(levels[0].levelTargetScore);

    }

    // Start is called before the first frame update
    void GenerateRandomGrid()
    {
        levelGrid = new GameObject[width, height];

        for (int i = 0; i < levelGrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelGrid.GetLength(1); j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j) * tileSize + offset, Quaternion.identity, transform);
                tile.transform.localScale = tile.transform.localScale * tileSize;
                TileDataHolder spawnedTile = tile.GetComponent<TileDataHolder>();
                spawnedTile.SetTileData(this, tileDataArray[Random.Range(0, tileDataArray.Length)], i, j);
                spawnedTile.OnTileDestroyed += TileDataHolder_OnTileDestroyed;
                levelGrid[i, j] = tile;
            }
        }

        ScoreManager.Instance.SetScoreTarget(5000);
    }

    private void TileDataHolder_OnTileDestroyed(object sender, TileDataHolder.OnTileDestroyedEventArgs e)
    {
        for (int x = 0; x < levelGrid.GetLength(0); x++)
        {
            for (int y = 0; y < levelGrid.GetLength(1); y++)
            {
                if (y == height -1)
                {
                    if (levelGrid[x, y].GetComponent<TileDataHolder>().isEmpty)
                    {
                        levelGrid[x, y].GetComponent<TileDataHolder>().
                                                                    SetTileData(this, tileDataArray[Random.Range(0, tileDataArray.Length)], x, y);
                    }
                }

                levelGrid[x, y].GetComponent<TileDataHolder>().CheckTileBelow();
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * tileSize + offset;
    }

    public void GetGridCoords(Vector3 pos, out int x, out int y)
    {
        x = Mathf.FloorToInt((pos - offset).x / tileSize);
        y = Mathf.FloorToInt((pos - offset).y / tileSize);
    }

    public void SetGridTile(int x, int y, TileDataSO _tileData)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            levelGrid[x, y].GetComponent<TileDataHolder>().SetTileData(this, _tileData, x, y);
        }
    }

    public void SetGridTile(Vector3 pos, TileDataSO _tileData)
    {
        int x, y;
        GetGridCoords(pos, out x, out y);
        SetGridTile(x, y, _tileData);
    }

    public TileDataSO GetTileData(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return levelGrid[x, y].GetComponent<TileDataHolder>().GetTileData();
        }
        else return null;
    }

    public TileDataSO GetTileData(Vector3 pos)
    {
        int x, y;
        GetGridCoords(pos, out x, out y);
        return GetTileData(x, y);
    }

    public TileDataHolder GetTileDataHolder(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return levelGrid[x, y].GetComponent<TileDataHolder>();
        }
        else return null;
    }

    public TileDataHolder GetTileDataHolder(Vector3 pos)
    {
        int x, y;
        GetGridCoords(pos, out x, out y);
        return GetTileDataHolder(x, y);
    }

}
