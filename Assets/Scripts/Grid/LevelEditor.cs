using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class LevelEditor : MonoBehaviour
{
    [SerializeField] private LevelDataSO levelToEdit;
    [SerializeField] private GameObject backgroundTile;

    
    public void GenerateEmptyLevel()
    {
        levelToEdit.levelGrid = new GameObject[levelToEdit.width, levelToEdit.height];

        for (int i = 0; i < levelToEdit.levelGrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelToEdit.levelGrid.GetLength(1); j++)
            {
                GameObject tile = Instantiate(backgroundTile, new Vector3(i, j) * levelToEdit.tileSize + levelToEdit.offset, Quaternion.identity, transform);
                tile.transform.localScale = tile.transform.localScale * levelToEdit.tileSize;
                levelToEdit.levelGrid[i, j] = tile;
            }
        }
    }

    public void ClearGrid()
    {
        if (levelToEdit.levelGrid.Length == 0) return;

        for (int i = 0; i < levelToEdit.levelGrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelToEdit.levelGrid.GetLength(1); j++)
            {
                Destroy(levelToEdit.levelGrid[i, j]);
            }
        }
    }

    void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            levelToEdit.offset = transform.position;
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
    }
}
