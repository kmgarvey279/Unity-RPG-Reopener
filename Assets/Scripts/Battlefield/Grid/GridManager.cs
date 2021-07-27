using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [Header("Grid Parameters")]
    [SerializeField] private int xCount;
    [SerializeField] private int yCount;
    [SerializeField] private Transform startWorld;
    private float tileSize = 0.75f;
    
    [Header("Array of tiles")]
    private GameObject[,] tileArray;

    [Header("Currently Displayed Tiles")]
    private List<Tile> displayedTiles = new List<Tile>();
    private List<Tile> aoeTiles = new List<Tile>();
    
    [Header("Misc")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject canvas;
    [SerializeField] private BattleManager battleManager;

    private void Start()
    {
        tileArray = new GameObject[xCount, yCount];
        GenerateGrid(); 
    }

    public List<Combatant> GetTargetsInAOE()
    {
        List<Combatant> targets = new List<Combatant>();
        foreach(Tile tile in aoeTiles)
        {
            if(tile.occupier != null && tile.occupier.CompareTag("Combatant"))
            {
                targets.Add(tile.occupier.GetComponent<Combatant>());
            }
        }
        return targets;
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

    public void DisplayTilesInRange(Tile start, int range, int aoe, bool isMoveRange = false)
    {
        int loops = 0;
        List<Tile> checkedTiles = new List<Tile>();
        List<Tile> uncheckedTiles = new List<Tile>();
        
        uncheckedTiles.Add(start);

        while(uncheckedTiles.Count > 0)
        {
            Tile tile = uncheckedTiles[0];
            
            if(isMoveRange && (tile.occupier == null || tile == start) || !isMoveRange)
            {
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
                    tile.Display(moveCost);
                }
                else
                {
                    tile.Display();
                }
            }
            
            uncheckedTiles.Remove(tile);
            checkedTiles.Add(tile);

            loops++;
            if(loops > 200)
            {
                return;
            }
        }
        displayedTiles = checkedTiles;

        start.Select();
    }

    public void DisplayAOE(Tile start, int range)
    {
        if(aoeTiles.Count > 0)
        {
            ClearAOE();
        }

        int loops = 0;
        List<Tile> checkedTiles = new List<Tile>();
        List<Tile> uncheckedTiles = new List<Tile>();
        uncheckedTiles.Add(tileArray[start.x, start.y].GetComponent<Tile>());

        while(uncheckedTiles.Count > 0)
        {
            Tile tile = uncheckedTiles[0];
            
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
            tile.ToggleAOE(true);
            
            uncheckedTiles.Remove(tile);
            checkedTiles.Add(tile);

            loops++;
            if(loops > 200)
            {
                return;
            }
        }
        aoeTiles = checkedTiles;
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
            tile.ToggleAOE(false);
        }
        aoeTiles.Clear();
    }

    public Tile GetClosestTileInRange(Tile start, Tile end, int moveRange)
    {
        List<Tile> path = GetPath(end, start);
        path.Remove(end);
        for(int i = 0; i < path.Count; i++)
        {
            if(GetMoveCost(path[i], start) <= moveRange)
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
