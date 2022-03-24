using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingControls : MonoBehaviour
{
    [SerializeField] LevelGrid levelGrid;

    LineRenderer line;
    int lineIndex = 0;
    Stack<TileDataHolder> matchedTiles = new Stack<TileDataHolder>();
    TileDataHolder previousTile;

    int matchCount = 0;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            TileDataHolder tile = levelGrid.GetTileDataHolder(touchPos);

            if (tile == previousTile && previousTile != null && tile == matchedTiles.Peek())
            {
                matchCount--;

                lineIndex--;
                line.positionCount--;
                matchedTiles.Pop();
                return;
            }

            if (matchedTiles.Count > 0 && TileLinkChecker(tile.GetTileData(), matchedTiles.Peek().GetTileData()))
            {
                lineIndex++;   
            }

            matchCount++;
            previousTile = tile;

            line.SetPosition(lineIndex, tile.transform.position);
            matchedTiles.Push(tile);
            return;
        }        
    }

    void AddToMatchedTiles(TileDataHolder tile)
    {

    }

    bool TileLinkChecker(TileDataSO startTile, TileDataSO endTile)
    {
        return startTile.type == endTile.type;
    }
}
