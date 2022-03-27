using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

[ExecuteAlways]
public class LevelEditor : MonoBehaviour
{
    [SerializeField] private LevelDataSO levelToEdit;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private bool isNewGrid;
    TileDataSO selectedTile = null;

    private GameObject[,] gridHolder;

    private void Start()
    {
        if (isNewGrid)
        {
            ClearGrid();
            GenerateEmptyLevel();
        }
        else
        {
            LoadLevel();
        }
    }

    public void GenerateEmptyLevel()
    {
        ClearGrid();

        gridHolder = new GameObject[levelToEdit.width, levelToEdit.height];
        levelToEdit.SetUpGrid();
        
        for (int i = 0; i < levelToEdit.levelGrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelToEdit.levelGrid.GetLength(1); j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j) * levelToEdit.tileSize + levelToEdit.offset, Quaternion.identity, transform);
                gridHolder[i, j] = tile;
                tile.transform.localScale = tile.transform.localScale * levelToEdit.tileSize;
                levelToEdit.levelGrid[i, j] = null;
            }
        }
    }

    public void LoadLevel()
    {
        for (int i = 0; i < levelToEdit.width; i++)
        {
            for (int j = 0; j < levelToEdit.height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j) * levelToEdit.tileSize + levelToEdit.offset, Quaternion.identity, transform);
                tile.transform.localScale = tile.transform.localScale * levelToEdit.tileSize;
                tile.GetComponent<TileDataHolder>().SetTileData(levelToEdit.levelGrid[i, j]);
            }
        }
    }

    public void GetGridCoords(Vector3 pos, out int x, out int y)
    {
        x = Mathf.FloorToInt((pos - levelToEdit.offset).x / levelToEdit.tileSize);
        y = Mathf.FloorToInt((pos - levelToEdit.offset).y / levelToEdit.tileSize);
    }

    public void SetGridTile(int x, int y, TileDataSO _tileData)
    {
        if (x >= 0 && y >= 0 && x < levelToEdit.width && y < levelToEdit.height)
        {
            levelToEdit.AddToGrid(x, y, _tileData);
            gridHolder[x, y].GetComponent<TileDataHolder>().SetTileData(_tileData);
        }
    }

    public void ClearGrid()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void SelectTileType(TileDataSO tileData)
    {
        selectedTile = tileData;
    }

    void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            levelToEdit.offset = transform.position;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x, y;
            GetGridCoords(mousePos, out x, out y);
            SetGridTile(x, y, selectedTile);
        }
    }
}

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LevelEditor myScript = (LevelEditor)target;

        if (GUILayout.Button("ClearGrid"))
        {
            myScript.ClearGrid();
        }

        if (GUILayout.Button("GenerateGrid"))
        {
            myScript.GenerateEmptyLevel();
        }
        
        if (GUILayout.Button("LoadLevel"))
        {
            myScript.LoadLevel();
        }
    }
}
