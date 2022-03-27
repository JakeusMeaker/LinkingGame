using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewLevel", menuName = "Level/NewLevel")]
public class LevelDataSO : ScriptableObject
{
    public int width;
    public int height;
    public float tileSize;
    public Vector3 offset;

    public int levelTargetScore;

    public TileDataSO[,] levelGrid;

    public void SetUpGrid()
    {
        levelGrid = new TileDataSO[width, height];
    }

    public void AddToGrid(int x, int y, TileDataSO tile)
    {
        levelGrid[x, y] = tile;
    }
}
