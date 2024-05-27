using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InterventionPointsDisplay : MonoBehaviour
{
    [SerializeField] private AnimatedBar interventionBar;
    [SerializeField] private OutlinedText valueText;
    [SerializeField] private List<InterventionNode> nodes = new List<InterventionNode>();
    [SerializeField] private GameObject disabledOverlay;

    [SerializeField] private OutlinedText interventionPrompt;
    [SerializeField] private Color interventionUsableColor;
    [SerializeField] private Color interventionUnusableColor;

    private static int nodeCost = 25;
    private static int barMax = 50;

    //public void SetInitialValues(int points)
    //{
    //    interventionBar.SetInitialValue(barMax, points);
    //    valueText.SetText(points.ToString());

    //    int nodesToFill = points / nodeCost;
    //    for (int i = 0; i < nodes.Count; i++)
    //    {
    //        if (i < nodesToFill)
    //        {
    //            nodes[i].ToggleReady(true);
    //        }
    //        else
    //        {
    //            nodes[i].ToggleReady(false);
    //        }
    //    }
    //}

    public void UpdatePoints(int points, bool interventionQueued, bool interventionSpent)
    {
        interventionBar.DisplayChange(points);
        interventionBar.ResolveChange(points);

        //valueText.SetText(points.ToString());

        int nodesToFill = points / nodeCost;
        for (int i = 0; i < nodes.Count; i++)
        {
            if (i < nodesToFill && !interventionSpent)
            {
                nodes[i].ToggleReady(true);
                interventionPrompt.SetTextColor(interventionUsableColor);
            }
            else
            {
                nodes[i].ToggleReady(false);
                interventionPrompt.SetTextColor(interventionUnusableColor);
            }
        }

        if (interventionQueued)
        {
            nodes[0].ToggleQueued(true);
        }
        else
        {
            nodes[0].ToggleQueued(false);
        }
    }

    public void UpdateQueuedPoints(int queuedPoints)
    {
        int queuedNodes = queuedPoints / nodeCost;

        for (int i = 0; i < nodes.Count; i++)
        {
            if (i < queuedNodes)
            {
                nodes[i].ToggleQueued(true);
            }
            else
            {
                nodes[i].ToggleQueued(false);
            }
        }

    }

    public void ToggleCanQueueIcon(bool canQueue)
    {
        disabledOverlay.SetActive(!canQueue);
    }
}
