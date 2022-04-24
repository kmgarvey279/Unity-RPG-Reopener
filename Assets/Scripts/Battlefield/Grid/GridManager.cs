using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public enum TargetType
{
    TargetPlayer,
    TargetEnemy
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

    private void Start()
    {
        playerTileArray = new Tile[xCount, yCount];
        enemyTileArray = new Tile[xCount, yCount];
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        int x = 0;
        int y = 0;

        foreach(Tile tile in playerTiles)
        {
            tile.x = x;
            tile.y = y;
            playerTileArray[x, y] = tile;

            y++;
            if(y >= yCount)
            {
                y = 0;
                x++;
            }
        }

        x = 0;
        y = 0;
        foreach(Tile tile in enemyTiles)
        {
            tile.x = x;
            tile.y = y;
            enemyTileArray[x, y] = tile;

            y++;
            if(y >= yCount)
            {
                y = 0;
                x++;
            }
        }
    }

    public void DisplaySelectableTargets(int userRow, TargetType targetType, bool isMelee)
    {
        List<int> meleeRows = new List<int> {2, 1, 0};
        bool selected = false;

        if(targetType == TargetType.TargetEnemy)
        {
            foreach (Tile tile in playerTileArray)
            {
                tile.Display(false);
            }
            foreach(Tile tile in enemyTileArray)
            {   
                tile.Display(false);
                if(tile.occupier)
                {
                    List<int> rowsInRange = meleeRows;
                    for(int i = rowsInRange.Count - 1; i >= 0; i--)
                    {
                        if(i > userRow)
                        {
                            rowsInRange.RemoveAt(i);
                        }
                    }

                    if(!isMelee || rowsInRange.Contains(tile.x))
                    {   
                        tile.Display(true);
                        if(!selected)
                        {
                            tile.Select();
                            selected = true;
                        }
                    }
                } 
            }
        } 
        else if(targetType == TargetType.TargetPlayer)
        {
            foreach (Tile tile in enemyTileArray)
            {
                tile.Display(false);
            }
            foreach(Tile tile in playerTileArray)
            {   
                if(tile.occupier)
                {
                    tile.Display(true);
                    if(!selected)
                    {
                        tile.Select();
                        selected = true;
                    }
                } 
            }
        }
    }

    public List<Combatant> DisplayFixedAOE(Tile start, Action action, TargetType targetType, bool flip)
    {
        foreach(Tile tile in enemyTileArray)
        {
            tile.Display(false);
        }
        foreach(Tile tile in playerTileArray)
        {
            tile.Display(false);
        }
        return DisplayAOE(start, action, targetType, flip);
    }

    public void DisplaySelectableTiles(TargetType targetType)
    {
        bool selected = false;
        if(targetType == TargetType.TargetPlayer)
        {
            foreach (Tile tile in enemyTileArray)
            {
                tile.Display(false);
            }
            foreach(Tile tile in playerTileArray)
            {
                tile.Display(true);
                if(tile.occupier && !selected)
                {
                    tile.Select();
                    selected = true;
                }
            }
        }
        else if(targetType == TargetType.TargetEnemy)
        {
            foreach(Tile tile in playerTileArray)
            {
                tile.Display(false);
            }
            Tile tileToSelect = null;
            int lowestMoveCost = 99;
            foreach(Tile tile in enemyTileArray)
            {
                tile.Display(true);
                if(tile.occupier)
                {
                    int thisMoveCost = GetMoveCost(tile, enemyTileArray[2,0]);
                    if(thisMoveCost < lowestMoveCost)
                    {
                        tileToSelect = tile;
                        lowestMoveCost = thisMoveCost;
                    }
                }
            }
            if(tileToSelect != null)
            {
                tileToSelect.Select();
            }
        }
    }

    public Tile[,] GetTileArray(TargetType targetType)
    {
        if(targetType == TargetType.TargetPlayer)
        {
            return playerTileArray;
        }
        else
        {
            return enemyTileArray;
        }
    }

    public List<Combatant> DisplayAOE(Tile start, Action action, TargetType targetType, bool flip)
    {
        List<Combatant> targets = new List<Combatant>();
        ClearAOE();

        List<Tile> aoeTiles = new List<Tile>(){};
        foreach(AOE aoe in action.aoes)
        {
            List<Tile> newTiles = new List<Tile>();
            Tile aoeStart = start;

            if(action.isFixedAOE)
            {
                aoeStart = GetTileArray(targetType)[aoe.fixedStartPosition.x, aoe.fixedStartPosition.y];
            }
            newTiles.Add(aoeStart);

            if(aoe.aoeType == AOEType.Cross)
            {
                newTiles.AddRange(GetRow(aoeStart, targetType, 1));
                newTiles.AddRange(GetColumn(aoeStart, targetType, 1));
            }
            else if(aoe.aoeType == AOEType.X)
            {
                newTiles.AddRange(GetDiagonal(aoeStart, targetType, 1, true));
                newTiles.AddRange(GetDiagonal(aoeStart, targetType, 1, false));
            }
            else if(aoe.aoeType == AOEType.Row)
            {    
                newTiles.AddRange(GetRow(aoeStart, targetType, 2));
            }
            else if(aoe.aoeType == AOEType.Column)
            {
                newTiles.AddRange(GetColumn(aoeStart, targetType, 2));
            }
            else if(aoe.aoeType == AOEType.Diagonal)
            {
                newTiles.AddRange(GetDiagonal(aoeStart, targetType, 2, flip));
            }
            else if(aoe.aoeType == AOEType.All)
            {
                newTiles.AddRange(GetAll(targetType));
            }

            foreach(Tile tile in newTiles)
            {
                if(!aoeTiles.Contains(tile))
                {
                    aoeTiles.Add(tile);
                }
            }
        }

        foreach (Tile tile in aoeTiles)
        {
            tile.DisplayAOE(); 

            if(tile.occupier != null && (tile.occupier is PlayableCombatant && targetType == TargetType.TargetPlayer || tile.occupier is EnemyCombatant && targetType == TargetType.TargetEnemy))
            {
                tile.occupier.Select();
                targets.Add(tile.occupier);  
            }
        }
        return targets;
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
            if(tile.occupier != null)
            {
                tile.occupier.Deselect();
            }
            tile.HideAOE();
        }
        foreach(Tile tile in enemyTileArray)
        {
            if(tile.occupier != null)
            {
                tile.occupier.Deselect();
            }
            tile.HideAOE();
        }
    }

    private void ClearPathNodes()
    {
        foreach (Tile tile in playerTileArray)
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

    public List<Tile> GetRow(Tile start, TargetType targetType, int range)
    {
        Tile[,] tileArray = GetTileArray(targetType);
        List<Tile> row = new List<Tile>(); 

        int currentX = start.x;
        for (int i = 0; i < range; i++)
        {
            if (currentX + 1 < xCount)
            {
                currentX += 1;
                row.Add(tileArray[currentX, start.y]);
            }
        }
        currentX = start.x;
        for (int i = 0; i < range; i++)
        {
            if (currentX - 1 >= 0)
            {
                currentX -= 1;
                row.Add(tileArray[currentX, start.y]);
            }
        }
        return row;
    }

    public List<Tile> GetColumn(Tile start, TargetType targetType, int range)
    {
        Tile[,] tileArray = GetTileArray(targetType);
        List<Tile> column = new List<Tile>();

        int currentY = start.y;
        for (int i = 0; i < range; i++)
        {
            if (currentY + 1 < yCount)
            {
                currentY += 1;
                column.Add(tileArray[start.x, currentY]);
            }
        }
        currentY = start.y;
        for (int i = 0; i < range; i++)
        {
            if (currentY - 1 >= 0)
            {
                currentY -= 1;
                column.Add(tileArray[start.x, currentY]);
            }
        }
        return column;
    }

    public List<Tile> GetDiagonal(Tile start, TargetType targetType, int range, bool flip)
    {
        Tile[,] tileArray = GetTileArray(targetType);
        List<Tile> diagonal = new List<Tile>();

        int currentX = start.x;
        int currentY = start.y;
        if(flip)
        {
            for (int i = 0; i < range; i++)
            {
                if(currentX - 1 >= 0 && currentY + 1 < yCount)
                {
                    currentX -= 1;
                    currentY += 1;
                    diagonal.Add(tileArray[currentX, currentY]);
                }
            }
            currentX = start.x;
            currentY = start.y;
            for (int i = 0; i < range; i++)
            {
                if(currentX + 1 < xCount && currentY - 1 >= 0)
                {
                    currentX += 1;
                    currentY -= 1;
                    diagonal.Add(tileArray[currentX, currentY]);
                }
            }
        }
        else
        {
            for (int i = 0; i < range; i++)
            {
                if(currentX + 1 < xCount && currentY + 1 < yCount)
                {
                    currentX += 1;
                    currentY += 1;
                    diagonal.Add(tileArray[currentX, currentY]);
                }
            }
            currentX = start.x;
            currentY = start.y;
            for (int i = 0; i < range; i++)
            {
                if(currentX - 1 >= 0 && currentY - 1 >= 0)
                {
                    currentX -= 1;
                    currentY -= 1;
                    diagonal.Add(tileArray[currentX, currentY]);
                }
            }
        }
        return diagonal;
    }

    public List<Tile> GetAll(TargetType targetType)
    {
        List<Tile> all = new List<Tile>();
        Tile[,] tileArray = GetTileArray(targetType);
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

    public List<Tile> GetPath(Tile start, Tile end, TargetType targetType)
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
            adjacentTiles.AddRange(GetRow(currentNode.tile, targetType, 1));
            adjacentTiles.AddRange(GetColumn(currentNode.tile, targetType, 1));
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
                    // if(childNode.tile.occupier == null)
                        openList.Add(childNode);
                }
            }
            loops++;
        }
        return path;
    }

    public void DisplayPath(List<Tile> path)
    {
        ClearPathNodes();
        if(path.Count > 1)
        {
            for(int i = 0; i < path.Count; i++)
            {
                if(i == 0)
                {
                    path[i].DisplayPathNode(PathType.Start, GetDirection(path[i], path[i + 1]), new Vector2(0,0));
                }
                else if(i == path.Count - 1)
                {
                    path[i].DisplayPathNode(PathType.End, GetDirection(path[i - 1], path[i]), new Vector2(0, 0));
                }
                else 
                {
                    Vector2 direction1 = GetDirection(path[i - 1], path[i]);
                    Vector2 direction2 = GetDirection(path[i], path[i + 1]);
                    if(direction1 != direction2)
                    {
                        path[i].DisplayPathNode(PathType.Turn, direction1, direction2);
                    }
                    else
                    {
                        path[i].DisplayPathNode(PathType.Straight, direction1, new Vector2(0, 0));
                    }
                }
            }
        }
    }

    public List<Tile> GetAdjacentTiles(Tile tile, TargetType targetType)
    {
        List<Tile> adjacentTiles = new List<Tile>();

        Tile[,] tileArray;
        if(targetType == TargetType.TargetPlayer)
        {
            tileArray = playerTileArray;
        }
        else
        {   
            tileArray = enemyTileArray;
        }

        // if(getDiagonals)
        // {
        //     if(tile.x + 1 < xCount && tile.y + 1 < yCount)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x + 1, tile.y + 1]);
        //     }
        //     if(tile.x - 1 >= 0 && tile.y - 1 >= 0)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x - 1, tile.y - 1]);
        //     }
        //     if(tile.x - 1 >= 0 && tile.y + 1 < yCount)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x - 1, tile.y + 1]);
        //     }
        //     if(tile.x + 1 < xCount && tile.y - 1 >= 0)
        //     {
        //         adjacentTiles.Add(tileArray[tile.x + 1, tile.y - 1]);
        //     }
        // }
        // else
        // {
            if(tile.x + 1 < xCount)
            {
                adjacentTiles.Add(tileArray[tile.x + 1, tile.y]);
            }
            if(tile.x - 1 >= 0)
            {
                adjacentTiles.Add(tileArray[tile.x - 1, tile.y]);
            }
            if(tile.y + 1 < yCount)
            {
                adjacentTiles.Add(tileArray[tile.x, tile.y + 1]);
            }
            if(tile.y - 1 >= 0)
            {
                adjacentTiles.Add(tileArray[tile.x, tile.y - 1]);
            }
        // }
        return adjacentTiles;
    }
}
