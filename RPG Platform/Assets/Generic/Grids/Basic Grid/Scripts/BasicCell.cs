/*
 * File:
 *   BasicCell.cs
 * 
 * Description:
 *   This file contains the implementation of a basic
 *   cell, which is the component of a basic grid.
 *   Basic cells provide the base class for both hex
 *   and square cells.
 */

using UnityEngine;

/*
 * Class:
 *   BasicCell
 * 
 * Description:
 *   This is an abstract class that contains methods and
 *   members common to both square and hex cells.
 */
public abstract class BasicCell : MonoBehaviour
{
    // Dangerous terrain costs double to move over.
    public bool IsDangerousTerrain = false;

    // Some terrain cannot be traversed.
    public bool IsWalkable = true;

    /*
     * Method:
     *   X
     * 
     * Description:
     *   A getter for the private member x, which is the
     *   x coordinate of the cell.
     */
    public abstract int X();

    /*
     * Method:
     *   Z
     * 
     * Description:
     *   A getter for the private member z, which is the
     *   z coordinate of the cell.
     */
    public abstract int Z();

    /*
     * Method:
     *   GetIndexFromCoordinates
     * 
     * Description:
     *   Returns the index of this cell within the grid surrounding it.
     */
    public abstract int GetIndexFromCoordinates(int gridWidth);

    /*
     * Method:
     *   GetNeighbors
     * 
     * Description:
     *   Returns all neighbors of the cell. Note that some
     *   entries in the returned array may be null, if the cell
     *   has no neighbor in that direction.
     */
    public abstract BasicCell[] GetNeighbors();
}
