using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchingControls : MonoBehaviour
{
    [SerializeField] LevelGrid levelGrid;

    Stack<TileDataHolder> matchedTiles = new Stack<TileDataHolder>();
    TileDataHolder previousTile;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            TileDataHolder tile = levelGrid.GetTileDataHolder(mousePos);

            if (previousTile == null && tile != null)
            {
                AddToMatchedTiles(tile);
            }
            else 
            {
                if (tile == null) return;

                if (tile.GetTileData() == null) return;

                if (tile == previousTile)
                {
                    RemoveFromMatchedTiles(matchedTiles.Peek());
                }
                
                if (TileLinkChecker(previousTile.GetTileData(), tile.GetTileData()))
                {
                    AddToMatchedTiles(tile);
                    return;
                }
                else
                {
                    return;
                }
            }            
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            TileDataHolder tile = levelGrid.GetTileDataHolder(mousePos);

            if (tile == null)
            {
                return;
            }

            if (matchedTiles.Count >= 3)
            {
                for (int i = matchedTiles.Count; i > 0; i--)
                {
                    TileDataSO scoreData = matchedTiles.Peek().GetTileData();
                    if (scoreData == null) return;
                    GameManager.Instance.AddToScore(scoreData.scoreValue);
                    matchedTiles.Peek().DisableAllConnections();
                    matchedTiles.Peek().DestroyTile();
                    matchedTiles.Pop();
                }
                previousTile = null;
                matchedTiles.Clear();
                return;
            }
            else
            {
                foreach (TileDataHolder tileHolder in matchedTiles)
                {
                    tileHolder.DisableAllConnections();
                }

                previousTile = null;
                matchedTiles.Clear();
            }
        }


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = new Vector3(touch.position.x, touch.position.y, Camera.main.transform.position.z);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(pos);

            TileDataHolder tile = levelGrid.GetTileDataHolder(touchPos);

            if (tile == null)
            {
                return;
            }

            if (tile.isEmpty)
            {
                return;
            }

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved && previousTile == null)
            {
                AddToMatchedTiles(tile);
            }

            if (previousTile == null)
            {
                return;
            }

            if (touch.phase == TouchPhase.Moved && tile != previousTile)
            {
                if (TileLinkChecker(previousTile.GetTileData(), tile.GetTileData()))
                {
                    AddToMatchedTiles(tile);
                }
            }
            else if (touch.phase == TouchPhase.Moved && matchedTiles.Contains(tile))
            {
                while (matchedTiles.Peek() != tile)
                {
                    RemoveFromMatchedTiles(matchedTiles.Peek());
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (matchedTiles.Count >= 3)
                {
                    for (int i = matchedTiles.Count; i > 0; i--)
                    {
                        TileDataSO scoreData = matchedTiles.Peek().GetTileData();
                        GameManager.Instance.AddToScore(scoreData.scoreValue);
                        matchedTiles.Peek().DisableAllConnections();
                        matchedTiles.Peek().DestroyTile();
                        matchedTiles.Pop();
                    }
                    previousTile = null;
                    matchedTiles.Clear();
                    return;
                }
                else
                {
                    foreach (TileDataHolder tileHolder in matchedTiles)
                    {
                        tileHolder.DisableAllConnections();
                    }

                    previousTile = null;
                    matchedTiles.Clear();
                }
            }
        }
    }

    void RemoveFromMatchedTiles(TileDataHolder tile)
    {
        tile.DisableAllConnections();
        matchedTiles.Pop();
    }

    void AddToMatchedTiles(TileDataHolder tile)
    {
        if (previousTile != null)
        {
            previousTile.EnableConnection(previousTile.DetermineConnection(tile));
        }

        previousTile = tile;

        matchedTiles.Push(tile);
    }

    bool TileLinkChecker(TileDataSO startTile, TileDataSO endTile)
    {
        if (startTile == null || endTile == null)
        {
            return false;
        }

        return startTile.type == endTile.type;
    }
}
