using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<Tile> pathNodes = new List<Tile>();
    
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

    public void DisplayTilesInRange(Tile start, int range, bool stopAtOccupiedTile = false, bool excludeStartingTile = false)
    {
        List<Tile> tilesInRange = GetTilesInRange(start, range, stopAtOccupiedTile, excludeStartingTile);
        foreach (Tile tile in tilesInRange)
        {
            tile.Display();
            displayedTiles.Add(tile);
        }
    }

    public void DisplayAOE(Tile start, int range, bool targetPlayer, bool targetEnemy)
    {
        if(aoeTiles.Count > 0)
        {
            ClearAOE();
        }
        List<Tile> tilesInRange = GetTilesInRange(start, range);
        foreach (Tile tile in tilesInRange)
        {
            tile.DisplayAOE(targetPlayer, targetEnemy); 
            aoeTiles.Add(tile);
        }
    }

    public void DisplayLineAOE(Tile start, Vector2 direction, int range, bool targetPlayer, bool targetEnemy, bool stopAtOccupiedTile)
    {
        if(aoeTiles.Count > 0)
        {
            ClearAOE();
        }
        List<Tile> tilesInRange = GetRow(start, direction, range, stopAtOccupiedTile);
        foreach (Tile tile in tilesInRange)
        {
            tile.DisplayAOE(targetPlayer, targetEnemy); 
        }
        aoeTiles = tilesInRange;
    }

    public List<Combatant> GetTargetsInRange(Tile start, int range, bool targetPlayer, bool targetEnemy)
    {
        List<Combatant> targets = new List<Combatant>();
        List<Tile> tilesInRange = GetTilesInRange(start, range);
        foreach(Tile tile in tilesInRange)
        {
            if(tile.occupier != null)
            {
                if(tile.occupier is PlayableCombatant && targetPlayer || tile.occupier is EnemyCombatant && targetEnemy)
                {
                    targets.Add(tile.occupier);
                }
            }
        }
        return targets;
    }

    public List<Tile> GetTilesInRange(Tile start, int range, bool stopAtOccupiedTile = false, bool excludeStartingTile = false)
    {
        //endless loop safeguard
        int loops = 0;
        //lists to check/return
        List<Tile> closedList = new List<Tile>();
        List<Tile> openList = new List<Tile>();
        openList.Add(start);
        while(openList.Count > 0 && loops < 200)
        {
            Tile currentTile = openList[0];
            int currentTileMoveCost = GetMoveCost(openList[0], start);
            closedList.Add(currentTile);
            List<Tile> adjacentTiles =  GetAdjacentTiles(currentTile);
            if(adjacentTiles.Count > 0)  
            {
                foreach (Tile adjacentTile in adjacentTiles)
                {
                    bool inOpenList = false;
                    bool inClosedList = false;
                    if((stopAtOccupiedTile && adjacentTile.occupier == null || !stopAtOccupiedTile) && currentTileMoveCost + 1 <= range)
                    {
                        if(!closedList.Contains(adjacentTile) && !openList.Contains(adjacentTile))
                        {
                            openList.Add(adjacentTile);
                        }
                    }    
                } 
            }
            // }
            // if(isMoveRange)
            // {   
            //     int moveCost = Mathf.Abs(tile.x - start.x) + Mathf.Abs(tile.y - start.y);
            // }
        // }
            openList.Remove(currentTile);
            //endless loop safeguard
            loops++;
        }
        if(excludeStartingTile)
            closedList.Remove(start);
        return closedList;
    }

    public void HideTiles()
    {
        ClearAOE();
        ClearPathNodes();
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
            tile.HideAOE();
        }
        aoeTiles.Clear();
    }

    private void ClearPathNodes()
    {
        foreach (Tile tile in pathNodes)
        {
            tile.HidePathNode();
        }
        pathNodes.Clear();
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

    public List<Tile> GetRow(Tile start, Vector2 direction, int range, bool stopAtOccupiedTile)
    {
       List<Tile> row = new List<Tile>(){start}; 
       int currentX = start.x;
       int currentY = start.y;
       for(int i = 0; i < range; i++)
       {
           currentX = currentX + (int)direction.x;
           currentY = currentY + (int)direction.y;
           //check if next tile is included in grid
           if(currentX < xCount && currentX >= 0 && currentY < yCount && currentY >= 0)
           {
                Tile thisTile = tileArray[currentX, currentY].GetComponent<Tile>();
                if(stopAtOccupiedTile && thisTile.occupier != null)
                {
                    return row;
                }
                row.Add(thisTile);
           }
           else
           {
               break;
           }
       }
       return row;
    }

    private class Node
    {
        public Tile tile;
        public Node parent;
        public int g;
        public int h;
        public int f;
        public Node(Tile tile, Node parent, int g, int h)
        {
            this.tile = tile;
            this.parent = parent;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }
    }

    public List<Tile> GetPath(Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        List<Node> closedList = new List<Node>();
        List<Node> openList = new List<Node>();

        openList.Add(new Node(start, null, 0, GetMoveCost(start, end)));
        
        int loops = 0;
        while(openList.Count > 0 && loops < 100)
        {
            openList.Sort((nodeA, nodeB) => nodeA.f.CompareTo(nodeB.f));
            Node currentNode = openList[0];
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            if(currentNode.tile == end)
            {
                Debug.Log("Found the end");
                Node node = currentNode;
                int loopCount = 0;
                for(int i = 0; i < currentNode.g + 1; i++)
                {
                    path.Insert(0, node.tile);
                    node = node.parent;
                    loopCount++;
                }
                return path;
            }
            List<Tile> adjacentTiles = GetAdjacentTiles(currentNode.tile);
            // int lowestMoveCost = GetMoveCost(adjacentTiles[0], currentTile);   
            if(adjacentTiles.Count > 1)
            {
                for(int i = 0; i < adjacentTiles.Count; i++)
                {
                    for(int j = 0; j < closedList.Count; j++)
                    {
                        if(closedList[j].tile == adjacentTiles[i])
                        {
                            continue;
                        }
                    } 
                    int g = currentNode.g + 1;
                    int h = GetMoveCost(adjacentTiles[i], end);
                    Node childNode = new Node(adjacentTiles[i], currentNode, g, h);
                    for(int j = 0; j < openList.Count; j++)
                    {
                        if(openList[j].tile == childNode.tile)
                        {
                            if(childNode.g > openList[i].g)
                            {
                                continue;
                            }
                        }
                    } 
                    if(childNode.tile.occupier == null)
                        openList.Add(childNode);
                }
            }
            loops++;
        }
        return path;
    }

    public void DisplayPath(List<Tile> path)
    {
        if(pathNodes.Count > 0)
        {
            ClearPathNodes();
        }
        foreach (Tile tile in path)
        {
            tile.DisplayPathNode(); 
            pathNodes.Add(tile);
        }
    }

    public List<Tile> GetAdjacentTiles(Tile tile)
    {
        List<Tile> adjacentTiles = new List<Tile>();

        // if(getDiagonal)
        // {
        //     if(tile.x + 1 < xCount && tile.y + 1 < yCount)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x + 1, tile.y + 1].GetComponent<Tile>());
        //     }
        //     if(tile.x + 1 < xCount && tile.y - 1 > 0)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x + 1, tile.y - 1].GetComponent<Tile>());
        //     }
        //     if(tile.x - 1 > 0 && tile.y + 1 < yCount)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x - 1, tile.y + 1].GetComponent<Tile>());
        //     }
        //     if(tile.x - 1 > 0 && tile.y - 1 > 0)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x - 1, tile.y - 1].GetComponent<Tile>());
        //     }  
        // }

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
