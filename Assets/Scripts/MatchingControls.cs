using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingControls : MonoBehaviour
{
    [SerializeField] LevelGrid levelGrid;

    Stack<TileDataHolder> matchedTiles = new Stack<TileDataHolder>();
    TileDataHolder previousTile;

    int matchCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                TileDataHolder tile = levelGrid.GetTileDataHolder(touchPos);

                if (previousTile != null)
                {
                    if (!TileLinkChecker(previousTile.GetTileData(), tile.GetTileData()) && tile.GetTileData() != null)
                    {
                        return;
                    }

                    if (tile == previousTile && tile != matchedTiles.Peek())
                    {
                        previousTile.DisableConnection(DetermineConnection(previousTile, tile));
                        RemoveFromMatchedTiles();
                    }
                }

                if (previousTile)
                {
                    previousTile.EnableConnection(DetermineConnection(previousTile, tile));
                }

                AddToMatchedTiles(tile);
                return;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (matchCount >= 3)
                {
                    foreach (TileDataHolder tile in matchedTiles)
                    {
                        tile.DisableAllConnections();
                        ScoreManager.AddToScore(tile.GetTileData().scoreValue);
                        tile.DestroyTile();
                    }
                    matchCount = 0;
                    previousTile = null;
                    return;
                }

                matchCount = 0;
                previousTile = null;

                foreach (TileDataHolder tile in matchedTiles)
                {
                    tile.DisableAllConnections();
                }

                matchedTiles.Clear();
            }
        }
    }

    void RemoveFromMatchedTiles()
    {
        matchCount = matchCount > 0 ? matchCount-- : matchCount = 0;

        matchedTiles.Pop();
    }

    void AddToMatchedTiles(TileDataHolder tile)
    {
        matchCount++;
        previousTile = tile;

        matchedTiles.Push(tile);
    }

    bool TileLinkChecker(TileDataSO startTile, TileDataSO endTile)
    {
        return startTile.type == endTile.type;
    }

    ConnectionLines DetermineConnection(TileDataHolder firstTile, TileDataHolder secondTile)
    {
        if (secondTile.xPos == firstTile.xPos && secondTile.yPos > firstTile.yPos)
        {
            return ConnectionLines.Up;
        }

        if (secondTile.xPos > firstTile.xPos && secondTile.yPos > firstTile.yPos)
        {
            return ConnectionLines.UpRight;
        }

        if (secondTile.xPos > firstTile.xPos && secondTile.yPos == firstTile.yPos)
        {
            return ConnectionLines.Right;
        }

        if (secondTile.xPos > firstTile.xPos && secondTile.yPos < firstTile.yPos)
        {
            return ConnectionLines.DownRight;
        }

        if (secondTile.xPos == firstTile.xPos && secondTile.yPos < firstTile.yPos)
        {
            return ConnectionLines.Down;
        }

        if (secondTile.xPos < firstTile.xPos && secondTile.yPos < firstTile.yPos)
        {
            return ConnectionLines.DownLeft;
        }

        if (secondTile.xPos < firstTile.xPos && secondTile.yPos == firstTile.yPos)
        {
            return ConnectionLines.Left;
        }

        if (secondTile.xPos < firstTile.xPos && secondTile.yPos > firstTile.yPos)
        {
            return ConnectionLines.UpLeft;
        }

        return ConnectionLines.NULL;
    }
}
