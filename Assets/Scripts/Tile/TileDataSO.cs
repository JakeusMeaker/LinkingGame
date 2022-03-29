using UnityEngine;

public enum TileType
{
    Red,
    Blue,
    Green,
    Yellow,
    Orange
}

[CreateAssetMenu(fileName = "NewTile", menuName = "Tiles/NewTile")]
public class TileDataSO : ScriptableObject
{
    public TileType type;
    public Sprite sprite;
    public int scoreValue;
}
