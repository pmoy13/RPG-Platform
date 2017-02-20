/*
 * File:
 *   BasicGrid.cs
 * 
 * Description:
 *   This file contains the implementation of a basic
 *   grid. Basic grids provide the base class for both hex
 *   and square grids.
 */

using UnityEngine;

/*
 * Class:
 *   BasicGrid
 * 
 * Description:
 *   This is an abstract class that contains methods and
 *   members common to both square and hex grids.
 */
public abstract class BasicGrid : MonoBehaviour
{
    // The dimensions of the grid.
    public int Width = 6;
    public int Height = 6;

    // Many of the grid computations are done assuming a unit
    // sized grid. They then use this scale factor to scale
    // the results to the actual grid's size.
    public float ScaleFactor = 10f;

    /*
     * Method:
     *   GetNumVertices
     * 
     * Description:
     *   Returns the total number of vertices in the grid.
     *   Each vertex represents a single cell, either hex or square.
     */
    public int GetNumVertices()
    {
        return Width * Height;
    }

    /*
     * Method:
     *   GetMaxNeighbors
     * 
     * Description:
     *   Returns the maximum number of neighbors that a cell
     *   in this grid can have. For square cells, this is 8.
     *   For hex cells, it is 6.
     */
    public abstract int GetMaxNeighbors();

    /*
     * Method:
     *   GetBasicCell
     * 
     * Description:
     *   Returns the cell at the given index of the grid.
     *   Both hex and square grids have a private array of
     *   cells, which we can access through this method.
     */
    public abstract BasicCell GetBasicCell(int index);
}
