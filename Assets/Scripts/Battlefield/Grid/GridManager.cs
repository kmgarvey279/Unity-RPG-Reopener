using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class GridManager : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    private Canvas canvas;
    [field: Header("Array of tiles")]
    [field: SerializeField] public List<Tile> PlayerTiles { get; private set; } = new List<Tile>();
    private Tile[,] playerTileArray;
    [field: SerializeField] public List<Tile> EnemyTiles { get; private set; } = new List<Tile>();
    private Tile[,] enemyTileArray;
    [Header("Center Positions")]
    [SerializeField] private List<Transform> playerCenterTiles = new List<Transform>();
    [SerializeField] private List<Transform> enemyCenterTiles = new List<Transform>();
    [field: SerializeField] public List<Tile> SwapTiles = new List<Tile>();
    public Dictionary<CombatantType, List<Transform>> CenterTiles { get; private set; } = new Dictionary<CombatantType, List<Transform>>();

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        Camera camera = FindFirstObjectByType<Camera>();
        if (camera)
        {
            canvas.worldCamera = camera;
        }
        else
        {
            Debug.LogError("Unable to find camera for grid");
        }
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        playerTileArray = new Tile[1, 3];
        foreach(Tile tile in PlayerTiles)
        {
            playerTileArray[tile.X, tile.Y] = tile;
        }

        enemyTileArray = new Tile[3, 3];
        foreach(Tile tile in EnemyTiles)
        {
            enemyTileArray[tile.X, tile.Y] = tile;
        }
        CenterTiles.Add(CombatantType.Player, playerCenterTiles);
        CenterTiles.Add(CombatantType.Enemy, enemyCenterTiles);
    }

    public List<Combatant> GetSelectableTargets(TargetingType targetingType, Combatant actor, bool isMelee, bool isBackAttack)
    {
        Debug.Log("displaying selectable targets");
        List<Combatant> targetableCombatants = new List<Combatant>();
        switch (targetingType)
        {
            case TargetingType.TargetSelf:
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.All))
                {
                    combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                }
                actor.ChangeSelectState(CombatantTargetState.Targetable);
                targetableCombatants.Add(actor);
                //actor.Select();
                break;
            case TargetingType.TargetAllies:
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Enemy))
                {
                    combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                }
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Player))
                {
                    if (combatant == actor)
                    {
                        combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                    }
                    else
                    {
                        targetableCombatants.Add(combatant);
                        combatant.ChangeSelectState(CombatantTargetState.Targetable);
                    }
                }
                //if (targetableCombatants.Count > 0)
                //{
                //    targetableCombatants[0].Select();
                //}
                break;
            case TargetingType.TargetFriendly:
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Enemy))
                {
                    combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                }
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Player))
                {
                    targetableCombatants.Add(combatant);
                    combatant.ChangeSelectState(CombatantTargetState.Targetable);
                }
                //if (targetableCombatants.Count > 0)
                //{
                //    targetableCombatants[0].Select();
                //}
                break;
            case TargetingType.TargetKOAllies:
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Enemy))
                {
                    combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                }
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Player))
                {
                    combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                }
                foreach (Combatant combatant in battleManager.GetKOedCombatants())
                {
                    targetableCombatants.Add(combatant);
                    combatant.ChangeSelectState(CombatantTargetState.Targetable);
                }
                //if (targetableCombatants.Count > 0)
                //{
                //    targetableCombatants[0].Select();
                //}
                break;
            case TargetingType.TargetHostile:
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Player))
                {
                    combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                }
                foreach (Combatant combatant in battleManager.GetCombatants(CombatantType.Enemy))
                {
                    bool isBlocked = false;
                    if (isMelee)
                    {
                        if (isBackAttack)
                        {
                            for (int i = combatant.Tile.X - 1; i >= 0; i--)
                            {
                                if (enemyTileArray[i, combatant.Tile.Y].Occupier != null)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = combatant.Tile.X + 1; i < 3; i++)
                            {
                                if (enemyTileArray[i, combatant.Tile.Y].Occupier != null)
                                {
                                    isBlocked = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!isBlocked)
                    {
                        targetableCombatants.Add(combatant);
                        combatant.ChangeSelectState(CombatantTargetState.Targetable);
                    }
                    else
                    {
                        combatant.ChangeSelectState(CombatantTargetState.Untargetable);
                    }
                }
                //if (targetableCombatants.Count > 0)
                //{
                //    targetableCombatants[0].Select();
                //}
                break;
            default:
                break;
        }
        return targetableCombatants;
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

    public List<Combatant> GetEnemyTargets(Tile start, AOEType aoeType, bool isMelee)
    {
        List<Combatant> targets = new List<Combatant>();
        List<Tile> tiles = new List<Tile>();

        tiles.Add(start);
        if (aoeType == AOEType.Row)
        {
            tiles.AddRange(GetRow(start));
        }
        else if (aoeType == AOEType.All || aoeType == AOEType.Random)
        {
            tiles.AddRange(GetAll(isMelee));
        }

        foreach (Tile tile in tiles)
        {
            if (tile.Occupier && !targets.Contains(tile.Occupier))
            {
                targets.Add(tile.Occupier);
            }
        }
        return targets;
    }

    public List<Tile> GetRow(Tile start)
    {
        List<Tile> row = new List<Tile>();

        for (int i = 0; i < 3; i++)
        {
            row.Add(GetTileArray(CombatantType.Enemy)[i, start.Y]);
        }
        return row;
    }

    //public void GetRowAttackPosition(List<ActionSubevent> actionSubevents)
    //{
    //    if (actionSubevents.Count > 0)
    //}

    public List<Tile> GetAll(bool isMelee)
    {
        List<Tile> all = new List<Tile>();
        Tile[,] tileArray = GetTileArray(CombatantType.Enemy);
        foreach(Tile tile in tileArray)
        {
            if (tile.Occupier)
            {
                bool isBlocked = false;
                if (isMelee && tile.X != 2)
                {
                    for (int i = tile.X + 1; i < 3; i++)
                    {
                        if (enemyTileArray[i, tile.Y].Occupier != null)
                        {
                            isBlocked = true;
                        }
                    }
                }
                if (!isBlocked)
                {
                    all.Add(tile);
                }
            }
        }
        return all;
    }

    //private class Node
    //{
    //    public Tile tile;
    //    public Node parent;
    //    public int g;
    //    public int h;
    //    public int f;
    //    public Node(Tile tile, Node parent, int g, int h)
    //    {
    //        this.tile = tile;
    //        this.parent = parent;
    //        this.g = g;
    //        this.h = h;
    //        this.f = g + h;
    //    }
    //}

    //public List<Tile> GetPath(Tile start, Tile end, CombatantType combatantType)
    //{
    //    List<Tile> path = new List<Tile>();
    //    List<Node> closedList = new List<Node>();
    //    List<Node> openList = new List<Node>();

    //    openList.Add(new Node(start, null, 0, GetMoveCost(start, end)));
        
    //    int loops = 0;
    //    while(openList.Count > 0 && loops < 100)
    //    {
    //        openList.Sort((nodeA, nodeB) => nodeA.f.CompareTo(nodeB.f));
    //        Node currentNode = openList[0];
    //        openList.Remove(currentNode);
    //        closedList.Add(currentNode);
    //        if(currentNode.tile == end)
    //        {
    //            Debug.Log("Found the end");
    //            Node node = currentNode;
    //            for(int i = 0; i < currentNode.g + 1; i++)
    //            {
    //                path.Insert(0, node.tile);
    //                node = node.parent;
    //            }
    //            return path;
    //        }
    //        List<Tile> adjacentTiles = new List<Tile>();
    //        adjacentTiles.AddRange(GetRow(currentNode.tile, combatantType));
    //        adjacentTiles.AddRange(GetColumn(currentNode.tile, combatantType, 1));
    //        // int lowestMoveCost = GetMoveCost(adjacentTiles[0], currentTile);   
    //        if(adjacentTiles.Count > 1)
    //        {
    //            for(int i = 0; i < adjacentTiles.Count; i++)
    //            {
    //                for(int j = 0; j < closedList.Count; j++)
    //                {
    //                    if(closedList[j].tile == adjacentTiles[i])
    //                    {
    //                        continue;
    //                    }
    //                } 
    //                int g = currentNode.g + 1;
    //                int h = GetMoveCost(adjacentTiles[i], end);
    //                Node childNode = new Node(adjacentTiles[i], currentNode, g, h);
    //                for(int j = 0; j < openList.Count; j++)
    //                {
    //                    if(openList[j].tile == childNode.tile)
    //                    {
    //                        if(childNode.g > openList[i].g)
    //                        {
    //                            continue;
    //                        }
    //                    }
    //                } 
    //                openList.Add(childNode);
    //            }
    //        }
    //        loops++;
    //    }
    //    return path;
    //}
}
