using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public GameObject[,] levelGrid;
    public int width;
    public int height;
    public float tileSize;

    [SerializeField] TileDataSO[] tileDataArray;

    [SerializeField] private GameObject tilePrefab;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position;
        GenerateRandomGrid();
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
                tile.GetComponent<TileDataHolder>().SetTileData(tileDataArray[Random.Range(0, tileDataArray.Length)]);
                levelGrid[i, j] = tile;
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
            levelGrid[x,y].GetComponent<TileDataHolder>().SetTileData(_tileData);
        }
    }

    public void SetGridTile(Vector3 pos, TileDataSO _tileData)
    {
        int x, y;
        GetGridCoords(pos, out x,out y);
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
