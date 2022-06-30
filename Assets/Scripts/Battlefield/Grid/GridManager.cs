using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public enum CombatantType
{
    Player,
    Enemy
}

public class GridManager : MonoBehaviour
{
    // public Tilemap tilemap;
    [Header("Grid Parameters")]
    [SerializeField] private int xCount;
    [SerializeField] private int yCount;
    
    [Header("Array of tiles")]
    public List<Tile> playerTiles = new List<Tile>();
    public Tile[,] playerTileArray;
    public List<Tile> enemyTiles = new List<Tile>();
    public Tile[,] enemyTileArray;
    [Header("Center Positions")]
    public Dictionary<int, Dictionary<CombatantType, Transform>> centerPositions = new Dictionary<int, Dictionary<CombatantType, Transform>>();
    [SerializeField] private Transform c0Player;
    [SerializeField] private Transform c0Enemy;
    [SerializeField] private Transform c1Player;
    [SerializeField] private Transform c1Enemy;
    [SerializeField] private Transform c2Player;
    [SerializeField] private Transform c2Enemy;

    private void Start()
    {
        GenerateGrid();
        centerPositions.Add(0, new Dictionary<CombatantType, Transform>(){{CombatantType.Player, c0Player}, {CombatantType.Enemy, c0Enemy}});
        centerPositions.Add(1, new Dictionary<CombatantType, Transform>(){{CombatantType.Player, c1Player}, {CombatantType.Enemy, c1Enemy}});
        centerPositions.Add(2, new Dictionary<CombatantType, Transform>(){{CombatantType.Player, c2Player}, {CombatantType.Enemy, c2Enemy}});    
    }

    private void GenerateGrid()
    {
        playerTileArray = new Tile[xCount, yCount];
        foreach(Tile tile in playerTiles)
        {
            playerTileArray[tile.x, tile.y] = tile;
            Debug.Log("Player Tile: " + tile.x + "/" + tile.y);
        }
        enemyTileArray = new Tile[xCount, yCount];
        foreach(Tile tile in enemyTiles)
        {
            enemyTileArray[tile.x, tile.y] = tile;
            Debug.Log("Enemy Tile: " + tile.x + "/" + tile.y);
        }
    }

    public void DisplaySelectableTargets(Action action, CombatantType combatantType, Combatant actor)
    {
        if(combatantType == CombatantType.Enemy)
        {
            Combatant firstSelectedCombatant = null;
            int highestValue = 0;
            
            foreach(Tile tile in enemyTileArray)
            {   
                tile.Display(false, false);
                foreach(Combatant combatant in tile.occupiers)
                {
                    //check if tile is blocked
                    if(action.isMelee && tile.x == 0 && enemyTileArray[1, tile.y].occupiers.Count > 0
                        || action.isMelee && actor.tile.x == 0)
                    {
                        break;
                    }
                    else
                    {
                        combatant.ChangeSelectState(true);
                        int thisValue = tile.x + tile.y;
                        if(thisValue >= highestValue)
                        {
                            firstSelectedCombatant = tile.occupiers[0];
                            highestValue = thisValue;
                        }
                    }
                    tile.Display(true, false);
                }
            }
            if(firstSelectedCombatant != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstSelectedCombatant.targetSelect.gameObject);
            }
        } 
        else if(combatantType == CombatantType.Player)
        {
            foreach(Tile tile in playerTileArray)
            {   
                // tile.Display(false);
                foreach(Combatant combatant in tile.occupiers)
                {
                    if((action.targetingType == TargetingType.TargetSelf && combatant == actor) || action.targetingType != TargetingType.TargetSelf)
                    {
                        // tile.Display(true);
                        combatant.ChangeSelectState(true);
                    }
                } 
            }
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(actor.targetSelect.gameObject);
        }
    }

    public void DisplaySelectableTiles(CombatantType combatantType, Tile start)
    {
        if(combatantType == CombatantType.Player)
        {
            foreach(Tile tile in playerTileArray)
            {
                tile.Display(true, true);
            }
            start.Select();
        }
        else if(combatantType == CombatantType.Enemy)
        {
            Tile tileToSelect = null;
            int highestValue = 0;
            foreach(Tile tile in enemyTileArray)
            {
                tile.Display(true, true);
                if(tile.occupiers.Count > 0)
                {
                    int thisValue = tile.x + tile.y;
                    if(thisValue >= highestValue)
                    {
                        tileToSelect = tile;
                        highestValue = thisValue;
                    }
                }
            }
            if(tileToSelect != null)
            {
                tileToSelect.Select();
            }
        }
    }

    public Tile[,] GetTileArray(CombatantType combatantType)
    {
        if(combatantType == CombatantType.Player)
        {
            return playerTileArray;
        }
        else
        {
            return enemyTileArray;
        }
    }

    public List<Tile> GetAOETiles(Tile start, Action action, CombatantType combatantType)
    {
        List<Tile> aoeTiles = new List<Tile>();
        List<Tile> newTiles = new List<Tile>();
        Tile aoeStart = start;

        if(action.aoeType == AOEType.Tile)
        {
            newTiles.Add(aoeStart);
        }
        if(action.aoeType == AOEType.Cross)
        {
            newTiles.Add(aoeStart);
            newTiles.AddRange(GetRow(aoeStart, combatantType, 1));
            newTiles.AddRange(GetColumn(aoeStart, combatantType, 1));
        }
        else if(action.aoeType == AOEType.Row)
        {    
            newTiles.Add(aoeStart);
            newTiles.AddRange(GetRow(aoeStart, combatantType, 1));
        }
        else if(action.aoeType == AOEType.Column)
        {
            newTiles.Add(aoeStart);
            newTiles.AddRange(GetColumn(aoeStart, combatantType, 1));
        }
        else if(action.aoeType == AOEType.All)
        {
            newTiles.AddRange(GetAll(combatantType));
        }

        foreach(Tile tile in newTiles)
        {
            Tile tileToAdd = tile;
            if(!aoeTiles.Contains(tileToAdd))
            {
                aoeTiles.Add(tileToAdd);
            }
        }
        return aoeTiles;
    }

    public List<Combatant> GetTargetsInAOE(List<Tile> aoeTiles, CombatantType combatantType)
    {
        List<Combatant> targets = new List<Combatant>();
        foreach(Tile tile in aoeTiles)
        {
            if(tile.occupiers.Count > 0 && (tile.occupiers[0].combatantType == combatantType))
            {
                foreach(Combatant occupier in tile.occupiers)
                {
                    targets.Add(occupier); 
                } 
            }
        }
        return targets;
    }

    public void DisplayAOE(List<Tile> aoeTiles, CombatantType combatantType)
    {
        ClearAOE();
        foreach(Tile tile in aoeTiles)
        {
            tile.DisplayAOE(); 
            // if(tile.occupiers.Count > 0 && (tile.occupiers[0].combatantType == combatantType))
            // {
            //     foreach(Combatant occupier in tile.occupiers)
            //     {
            //         occupier.Select();
            //     }
            // }
        }
    }

    public void HideTiles()
    {
        ClearAOE();
        ClearPathNodes();
        foreach (Tile tile in playerTileArray)
        {
            tile.Hide();
        }
        foreach (Tile tile in enemyTileArray)
        {
            tile.Hide();
        }
    }

    private void ClearAOE()
    {
        foreach(Tile tile in playerTileArray)
        {
            // foreach(Combatant occupier in tile.occupiers)
            // {
            //     occupier.Deselect();
            // }
            tile.HideAOE();
        }
        foreach(Tile tile in enemyTileArray)
        {
            // foreach(Combatant occupier in tile.occupiers)
            // {
            //     occupier.Deselect();
            // }
            tile.HideAOE();
        }
    }

    private void ClearPathNodes()
    {
        foreach (Tile tile in playerTileArray)
        {
            tile.HidePathNode();
        }
        foreach (Tile tile in enemyTileArray)
        {
            tile.HidePathNode();
        }
    }

    public int GetMoveCost(Tile end, Tile start)
    {
        return Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y);
    }

    public Vector2 GetDirection(Tile tile1, Tile tile2)
    {
        return new Vector2(tile2.x - tile1.x, tile2.y - tile1.y);
    }

    public List<Tile> GetRow(Tile start, CombatantType combatantType, int range)
    {
        Tile[,] tileArray = GetTileArray(combatantType);

        List<Tile> row = new List<Tile>(); 

        int currentX = start.x;
        for(int i = 0; i < range; i++)
        {
            if(currentX + 1 < xCount)
            {
                currentX += 1;
                row.Add(tileArray[currentX, start.y]);
            }
        }
        currentX = start.x;
        for(int i = 0; i < range; i++)
        {
            if(currentX - 1 >= 0)
            {
                currentX -= 1;
                row.Add(tileArray[currentX, start.y]);
            }
        }
        return row;
    }

    public List<Tile> GetColumn(Tile start, CombatantType combatantType, int range)
    {
        Tile[,] tileArray = GetTileArray(combatantType);
        List<Tile> column = new List<Tile>();

        int currentY = start.y;
        for(int i = 0; i < range; i++)
        {
            if(currentY + 1 < yCount)
            {
                currentY += 1;
                column.Add(tileArray[start.x, currentY]);
            }
        }
        currentY = start.y;
        for(int i = 0; i < range; i++)
        {
            if(currentY - 1 >= 0)
            {
                currentY -= 1;
                column.Add(tileArray[start.x, currentY]);
            }
        }
        return column;
    }

    // public List<Tile> GetDiagonal(Tile start, CombatantType combatantType, int range, bool flip)
    // {
    //     Tile[,] tileArray = GetTileArray(combatantType);
    //     List<Tile> diagonal = new List<Tile>();

    //     int currentX = start.x;
    //     int currentY = start.y;
    //     if(flip)
    //     {
    //         for (int i = 0; i < range; i++)
    //         {
    //             if(currentX - 1 >= 0 && currentY + 1 < yCount)
    //             {
    //                 currentX -= 1;
    //                 currentY += 1;
    //                 diagonal.Add(tileArray[currentX, currentY]);
    //             }
    //         }
        //     currentX = start.x;
        //     currentY = start.y;
        //     for (int i = 0; i < range; i++)
        //     {
        //         if(currentX + 1 < xCount && currentY - 1 >= 0)
        //         {
        //             currentX += 1;
        //             currentY -= 1;
        //             diagonal.Add(tileArray[currentX, currentY]);
        //         }
        //     }
        // }
        // else
        // {
        //     for (int i = 0; i < range; i++)
        //     {
        //         if(currentX + 1 < xCount && currentY + 1 < yCount)
        //         {
        //             currentX += 1;
        //             currentY += 1;
        //             diagonal.Add(tileArray[currentX, currentY]);
        //         }
        //     }
        //     currentX = start.x;
        //     currentY = start.y;
        //     for (int i = 0; i < range; i++)
        //     {
    //             if(currentX - 1 >= 0 && currentY - 1 >= 0)
    //             {
    //                 currentX -= 1;
    //                 currentY -= 1;
    //                 diagonal.Add(tileArray[currentX, currentY]);
    //             }
    //         }
    //     }
    //     return diagonal;
    // }

    public List<Tile> GetAll(CombatantType combatantType)
    {
        List<Tile> all = new List<Tile>();
        Tile[,] tileArray = GetTileArray(combatantType);
        foreach(Tile tile in tileArray)
        {
            all.Add(tile); 
        }
        return all;
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

    public List<Tile> GetPath(Tile start, Tile end, CombatantType combatantType)
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
                for(int i = 0; i < currentNode.g + 1; i++)
                {
                    path.Insert(0, node.tile);
                    node = node.parent;
                }
                return path;
            }
            List<Tile> adjacentTiles = new List<Tile>();
            adjacentTiles.AddRange(GetRow(currentNode.tile, combatantType, 1));
            adjacentTiles.AddRange(GetColumn(currentNode.tile, combatantType, 1));
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
                    openList.Add(childNode);
                }
            }
            loops++;
        }
        return path;
    }

    public void DisplayPaths(List<List<Tile>> paths, CombatantType combatantType)
    {
        ClearPathNodes();
        foreach(List<Tile> path in paths)
        {
            if(path.Count > 1)
            {
                for(int i = 0; i < path.Count; i++)
                {
                    if(i == path.Count - 1)
                    {
                        path[i].DisplayPathNode(PathType.End, GetDirection(path[i - 1], path[i]), new Vector2(0, 0), combatantType);
                    }
                    else if(i == 0)
                    {
                        // if(swap)
                        // {
                        //     path[i].DisplayPathNode(PathType.End, GetDirection(path[i + 1], path[i]), new Vector2(0,0), combatantType);
                        // }
                        // else
                        // {
                            path[i].DisplayPathNode(PathType.Start, GetDirection(path[i], path[i + 1]), new Vector2(0,0), combatantType);
                        // }
                    }
                    else 
                    {
                        Vector2 direction1 = GetDirection(path[i - 1], path[i]);
                        Vector2 direction2 = GetDirection(path[i], path[i + 1]);
                        if(direction1 != direction2)
                        {
                            path[i].DisplayPathNode(PathType.Turn, direction1, direction2, combatantType);
                        }
                        else
                        {
                            path[i].DisplayPathNode(PathType.Straight, direction1, new Vector2(0, 0), combatantType);
                        }
                    }
                }
            }
        }
    }
}
