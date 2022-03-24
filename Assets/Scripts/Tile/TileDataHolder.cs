using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDataHolder : MonoBehaviour
{
    private TileDataSO tileData;
    private bool isEmpty = false;

    [SerializeField]private SpriteRenderer tileRenderer;

    public void SetTileData(TileDataSO _tileData)
    {
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
        tileData = null;
        isEmpty = true;
    }

}
