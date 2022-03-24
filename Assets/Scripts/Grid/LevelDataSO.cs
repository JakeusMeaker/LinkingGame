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

    public GameObject[,] levelGrid;
}
