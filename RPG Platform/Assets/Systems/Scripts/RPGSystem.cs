using System.Collections;
using System.Collections.Generic;

public abstract class RPGSystem
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
}
