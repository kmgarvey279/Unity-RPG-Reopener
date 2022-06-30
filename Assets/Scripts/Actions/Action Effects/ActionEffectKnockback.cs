using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionEffectKnockback : ActionEffect
{
    [Range(-1, 1)] public int moveX;
    [Range(-1, 1)] public int moveY;

    public override void ApplyEffect(ActionEvent actionEvent)
    {
        actionEvent.targets[0].Knockback(GetKnockbackDestination(actionEvent.targets[0].tile));
    }

    public Vector2Int GetKnockbackDestination(Tile start)
    {
        Vector2Int newCoordinates = new Vector2Int(start.x, start.y);
        if(moveX != 0 && moveY != 0)
        {
            return newCoordinates;
        }
        int newX = newCoordinates.x + moveX;
        if(newX >= 0 && newX < 3)
        {
            newCoordinates.x = newCoordinates.x + moveX;
        }
        int newY = newCoordinates.y + moveY;
        if(newY >= 0 && newY < 3)
        {
            newCoordinates.y = newCoordinates.y + moveY;   
        }
        return newCoordinates;
    }
}
