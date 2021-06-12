using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridDisplay
{
    public List<Tile> displayedTiles = new List<Tile>();
    public List<Tile> aoeTiles = new List<Tile>();
    public int aoe;
    public Tile selectedTile;

    public GridDisplay(List<Tile> displayedTiles, int aoe)
    {
        this.displayedTiles = displayedTiles;
        this.aoe = aoe;
    }

    public List<Combatant> GetTargetsInAOE()
    {
        List<Combatant> targets = new List<Combatant>();
        foreach (Tile tile in aoeTiles)
        {
            if(tile.occupier != null && tile.occupier.CompareTag("Combatant"))
            {
                targets.Add(tile.occupier.GetComponent<Combatant>());
            }
        }
        return targets;
    }
}

public class GridManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform startPoint;
    
    [SerializeField] private int xCount;
    [SerializeField] private int yCount;
    private Vector3 tileSize = new Vector3(0.5f, 0.25f);

    public GameObject[,] tileArray;

    public GameObject tilePrefab;
    public GameObject canvas;

    public GridDisplay gridDisplay;

    private int previousMoveX;
    private int previousMoveY;

    private void Start()
    {
        tileArray = new GameObject[xCount, yCount];
        GenerateGrid(); 
    }

    private void Update()
    {
        if(gridDisplay.selectedTile != null)
        {
            int moveX = (int)(Input.GetAxisRaw("Horizontal"));
            int moveY = (int)(Input.GetAxisRaw("Vertical"));
            int[] isoDirection = ConvertToIsoDirection(moveX, moveY);

            if(previousMoveX != isoDirection[0] || previousMoveY != isoDirection[1])
            {
                previousMoveX = isoDirection[0];
                previousMoveY = isoDirection[1];
                
                int newX = gridDisplay.selectedTile.x + isoDirection[0];
                int newY = gridDisplay.selectedTile.y + isoDirection[1];
                
                ChangeSelectedTile(newX, newY);
            }
        } 
    }

    private void ChangeSelectedTile(int newX, int newY)
    {
        if(newX >= 0 && newX < xCount && newY >= 0 && newY < yCount)
        {
            gridDisplay.selectedTile = tileArray[newX, newY].GetComponent<Tile>();
            gridDisplay.selectedTile.Select();
        }
    }

    private int[] ConvertToIsoDirection(int moveX, int moveY)
    {
        int[] originalDirection = new int[]{moveX, moveY};

        int[][] directions = new int[][]
        {
           new int[] {0, 1},
           new int[] {1, 1},
           new int[] {1, 0},
           new int[] {1, -1},
           new int[] {0, -1},
           new int[] {-1, -1},
           new int[] {-1, 0},
           new int[] {-1, 1}
        };

        int index = System.Array.IndexOf(directions, originalDirection);
        if(index != null)
        {
            int next = index + 1;
            if(next == 8)
            {
                next = 0; 
            }
            return directions[next];
        }
        return originalDirection;
    }

    public void OnSelectedTileChange()
    {

    }

    public void DisplayAOE()
    {

    }

    void GenerateGrid()
    {
        for(int x = 0; x < xCount; x++)
        {  
            for(int y = 0; y < yCount; y++)
            {
                Vector3 spawnPos = new Vector3(tileSize.x * ((float)x - (float)y), (tileSize.y * ((float)x + (float)y)));
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

    Vector3Int GetCell(int x, int y)
    {
        return tilemap.WorldToCell(new Vector3(x, y));
    }

    public void DisplayTilesInRange(Tile start, int range, int aoe)
    {
        int loops = 0;
        List<Tile> checkedTiles = new List<Tile>();
        List<Tile> uncheckedTiles = new List<Tile>();
        uncheckedTiles.Add(start);

        while(uncheckedTiles.Count > 0)
        {
            Tile tile = uncheckedTiles[0];
            
            if(tile.occupier == null || tile == start)
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
                tile.Display();
            }
            
            uncheckedTiles.Remove(tile);
            checkedTiles.Add(tile);

            loops++;
            if(loops > 200)
            {
                return;
            }
        }
        gridDisplay = new GridDisplay(checkedTiles, aoe);

        gridDisplay.selectedTile = start;
        start.Select();

        if(aoe >= 0)
            DisplayAOE(start, aoe);
    }

    public void DisplayAOE(Tile start, int range)
    {
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
        gridDisplay.aoeTiles = checkedTiles;
    }

    public void HideTiles()
    {
        if(gridDisplay.displayedTiles.Count > 0)
        {
            foreach (Tile tile in gridDisplay.displayedTiles)
            {
                tile.Hide();
            }
        }
        gridDisplay = null;
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
        if(tile.x - 1 > 0)
        {
            adjacentTiles.Add(tileArray[tile.x - 1, tile.y].GetComponent<Tile>());
        }
        if(tile.y + 1 < yCount)
        {
            adjacentTiles.Add(tileArray[tile.x, tile.y + 1].GetComponent<Tile>());
        }
        if(tile.y - 1 > 0)
        {
            adjacentTiles.Add(tileArray[tile.x, tile.y - 1].GetComponent<Tile>());
        }
        return adjacentTiles;
    }
}
