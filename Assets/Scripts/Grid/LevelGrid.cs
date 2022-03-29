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
    }

    public void LoadLevelGrid(LevelDataSO levelData)
    {
        ClearGrid();

        levelGrid = new GameObject[levelData.width, levelData.height];

        width = levelData.width;
        height = levelData.height;
        tileSize = levelData.tileSize;

        int listIndexRef = 0;
        
        for (int i = 0; i < levelData.width; i++)
        {
            for (int j = 0; j < levelData.height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j) * levelData.tileSize + levelData.offset, Quaternion.identity, transform);
                tile.transform.localScale = tile.transform.localScale * levelData.tileSize;
                TileDataHolder spawnedTile = tile.GetComponent<TileDataHolder>();
                spawnedTile.SetTileData(this, levelData.levelGrid[listIndexRef], i, j);
                spawnedTile.OnTileDestroyed += TileDataHolder_OnTileDestroyed;
                levelGrid[i, j] = tile;
                if(listIndexRef < levelData.levelGrid.Count) listIndexRef++;
            }
        }
        GameManager.Instance.SetScoreTarget(levelData.levelTargetScore);
    }

    public void GenerateRandomGrid(int levelScore)
    {
        ClearGrid();

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
        GameManager.Instance.SetScoreTarget(levelScore);
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

    public void ClearGrid()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
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
