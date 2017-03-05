using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RPGSystem : MonoBehaviour
{
    /*
     * Method:
     *   CalculateMovementCost
     * 
     * Description:
     *   Calculates the movement cost between
     *   two adjacent cells, using the specific
     *   movement rules from the system.
     */
    public abstract float CalculateMovementCost(BasicCell cell, int neighbor);

    // TODO: Description.
    public abstract float CalculateSquareGridMovementCost(SquareCell baseCell, int size, SquareDirection direction);
}
