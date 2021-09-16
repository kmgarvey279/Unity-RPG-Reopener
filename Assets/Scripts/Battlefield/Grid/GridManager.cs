using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    [Header("Grid Parameters")]
    [SerializeField] private int xCount;
    [SerializeField] private int yCount;
    [SerializeField] private Transform startWorld;
    private float tileSize = 1f;
    
    [Header("Array of tiles")]
    public GameObject[,] tileArray;

    [Header("Currently Displayed Tiles")]
    private List<Tile> displayedTiles = new List<Tile>();
    private List<Tile> aoeTiles = new List<Tile>();
    
    [Header("Misc")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject canvas;
    [SerializeField] private BattleManager battleManager;

    private void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        tileArray = new GameObject[xCount, yCount];
        GenerateGrid(); 
    }

    void GenerateGrid()
    {
        for(int x = 0; x < xCount; x++)
        {  
            for(int y = 0; y < yCount; y++)
            {
                float xPos = tileSize * (float)x + startWorld.position.x;
                float yPos = tileSize * (float)y + startWorld.position.y;
                Vector3 spawnPos = new Vector3(xPos, yPos);
                SpawnTile(x, y, spawnPos);
            }
        }
    }

    void SpawnTile(int x, int y, Vector3 spawnPos)
    {
        GameObject tile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
        tile.transform.SetParent(canvas.transform);
        tile.GetComponent<Tile>().x = x;
        tile.GetComponent<Tile>().y = y;
        //store gameobject in grid array 
        tileArray[x, y] = tile;
    }

    public void DisplayTilesInRange(Tile start, int range, bool isMoveRange = false)
    {
        List<Tile> tilesInRange = GetTilesInRange(start, range, isMoveRange);
        foreach (Tile tile in tilesInRange)
        {
            if(isMoveRange)
            {
                tile.Display(GetMoveCost(tile, start));
            }
            else 
            {
                tile.Display();
            }   
        }
        displayedTiles = tilesInRange;
        start.Select();
    }

    public void DisplayAOE(Tile start, int range, bool targetPlayer, bool targetEnemy)
    {
        if(aoeTiles.Count > 0)
        {
            ClearAOE();
        }

        List<Tile> tilesInRange = GetTilesInRange(start, range, false);
        foreach (Tile tile in tilesInRange)
        {
            tile.DisplayAOE(targetPlayer, targetEnemy); 
        }
        aoeTiles = tilesInRange;
        start.Select();
    }

    public List<Combatant> GetTargetsInRange(Tile start, int range, bool targetPlayer, bool targetEnemy)
    {
        List<Combatant> targets = new List<Combatant>();
        List<Tile> tilesInRange = GetTilesInRange(start, range, false);
        foreach(Tile tile in tilesInRange)
        {
            if(tile.occupier != null)
            {
                if(tile.occupier is AllyCombatant && targetPlayer || tile.occupier is EnemyCombatant && targetEnemy)
                {
                    targets.Add(tile.occupier);
                }
            }
        }
        return targets;
    }

    public List<Tile> GetTilesInRange(Tile start, int range, bool isMoveRange = false)
    {
        //endless loop safeguard
        int loops = 0;
        //
        List<Tile> checkedTiles = new List<Tile>();
        List<Tile> uncheckedTiles = new List<Tile>();
        
        uncheckedTiles.Add(start);

        while(uncheckedTiles.Count > 0)
        {
            Tile tile = uncheckedTiles[0];
            
            if(isMoveRange && (tile.occupier == null || tile == start) || !isMoveRange)
            {
                checkedTiles.Add(tile);
                List<Tile> adjacentTiles =  GetAdjacentTiles(tile, false);
                if(adjacentTiles.Count > 0)  
                {
                    foreach (Tile adjacentTile in adjacentTiles)
                    {
                        int moveCost = Mathf.Abs(adjacentTile.x - start.x) + Mathf.Abs(adjacentTile.y - start.y); 
                        if(!checkedTiles.Contains(adjacentTile) && !uncheckedTiles.Contains(adjacentTile) && moveCost <= range)
                        {
                            uncheckedTiles.Add(adjacentTile);
                        }    
                    } 
                }
                if(isMoveRange)
                {   
                    int moveCost = Mathf.Abs(tile.x - start.x) + Mathf.Abs(tile.y - start.y);
                }
            }
            uncheckedTiles.Remove(tile);
            //endless loop safeguard
            loops++;
            if(loops > 200)
            {
                break;
            }
            //
        }
        return checkedTiles;
    }

    public void HideTiles()
    {
        ClearAOE();
        if(displayedTiles.Count > 0)
        {
            foreach (Tile tile in displayedTiles)
            {
                tile.Hide();
            }
        }
        displayedTiles.Clear();
    }

    private void ClearAOE()
    {
        foreach (Tile tile in aoeTiles)
        {
            tile.ClearAOE();
        }
        aoeTiles.Clear();
    }

    public Tile GetClosestTileInRange(Tile start, Tile end, int range)
    {
        List<Tile> path = GetPath(end, start);
        path.Remove(end);
        for(int i = 0; i < path.Count; i++)
        {
            if(GetMoveCost(path[i], start) <= range)
            {
                return path[i];
            }
        }
        return start;
    }

    public int GetMoveCost(Tile end, Tile start)
    {
        return Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
    }

    public List<Tile> GetPath(Tile start, Tile end)
    {
        int loops = 0;
        List<Tile> path = new List<Tile>();
        path.Add(end);
        while(path[0] != start)
        {
            Tile currentTile = path[0];
            int currentMoveCost = GetMoveCost(currentTile, start); 
            List<Tile> adjacentTiles =  GetAdjacentTiles(currentTile, true);  
            for(int i = 0; i < adjacentTiles.Count; i++)
            {
                int nextMoveCost = GetMoveCost(adjacentTiles[i], start); 
                if(nextMoveCost < currentMoveCost)
                {
                    path.Insert(0, adjacentTiles[i]);
                    break;
                }
            }
            loops++;
            if(loops > 100)
            {
                return path;
            }
        }
        return path;
    }

    public List<Tile> GetAdjacentTiles(Tile tile, bool getDiagonal)
    {
        List<Tile> adjacentTiles = new List<Tile>();

        if(getDiagonal)
        {
            if(tile.x + 1 < xCount && tile.y + 1 < yCount)
            {
                adjacentTiles.Add(tileArray[tile.x + 1, tile.y + 1].GetComponent<Tile>());
            }
            if(tile.x + 1 < xCount && tile.y - 1 > 0)
            {
                adjacentTiles.Add(tileArray[tile.x + 1, tile.y - 1].GetComponent<Tile>());
            }
            if(tile.x - 1 > 0 && tile.y + 1 < yCount)
            {
                adjacentTiles.Add(tileArray[tile.x - 1, tile.y + 1].GetComponent<Tile>());
            }
            if(tile.x - 1 > 0 && tile.y - 1 > 0)
            {
                adjacentTiles.Add(tileArray[tile.x - 1, tile.y - 1].GetComponent<Tile>());
            }  
        }

        if(tile.x + 1 < xCount)
        {
            adjacentTiles.Add(tileArray[tile.x + 1, tile.y].GetComponent<Tile>());
        }
        if(tile.x - 1 >= 0)
        {
            adjacentTiles.Add(tileArray[tile.x - 1, tile.y].GetComponent<Tile>());
        }
        if(tile.y + 1 < yCount)
        {
            adjacentTiles.Add(tileArray[tile.x, tile.y + 1].GetComponent<Tile>());
        }
        if(tile.y - 1 >= 0)
        {
            adjacentTiles.Add(tileArray[tile.x, tile.y - 1].GetComponent<Tile>());
        }
        return adjacentTiles;
    }
}
