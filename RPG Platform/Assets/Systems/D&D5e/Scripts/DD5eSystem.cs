using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DD5eSystem : RPGSystem
{
    // Movement costs for normal and dangerous terrain. Character
    // movement speed is given in feet, which we convert to squares.
    // In Dungeons and Dragons, each grid square is 5 feet. These
    // values should only be used for square grids.
    private static float _feetPerSquare = 5;
    private static float _normalTerrainCost = 1;
    private static float _dangerousTerrainCost = 2 * _normalTerrainCost;

    /*
     * Method:
     *   CalculateMovementCost
     * 
     * Description:
     *   Calculates the movement cost between
     *   two adjacent cells, using the specific
     *   movement rules from the system. Should
     *   only be used with square grids, as the
     *   calculation for diagonal movement doesn't
     *   make sense for hexagonal grids.
     */
    public override float CalculateMovementCost(BasicCell cell, int neighbor)
    {
        // Get the neighboring cell (the destination of the edge).
        BasicCell neighborCell = cell.GetNeighbors()[neighbor];

        // Does the neighbor exist? If so, is it traversible? If not,
        // this edge cannot exist.
        if (neighborCell == null || !neighborCell.IsWalkable)
        {
            // Return an illegal value for an edge weight.
            return float.MaxValue;
        }

        // The neighbor exists. Is it diagonal from the source? Note
        // that a diagonal cell will have both a different X and a different
        // Z coordinate, so it's enough to check if the X coordinates differ.
        if (cell.X() != neighborCell.X())
        {
            // Yes - now we have to check if either cell next to the diagonal
            // path is impassable. If it is, then this edge is invalid. The
            // two cells which neighbor both of these diagonally adjacent cells
            // have the X coordinate from one cell and the Z coordinate from the
            // other cell. Since there's no convenient way to get a cell from
            // its coordinates, we will just loop through each of the neighbors
            // and check their coordinates.
            for (int index = 0; index < cell.GetNeighbors().Length; index++)
            {
                BasicCell cornerCell = cell.GetNeighbors()[index];

                // Does this cell exist? If not, continue searching.
                if (cornerCell == null)
                {
                    continue;
                }

                if (cornerCell.X() == cell.X() && cornerCell.Z() == neighborCell.Z())
                {
                    // Found one of the corner cells. Check it.
                    if (!cornerCell.IsWalkable)
                    {
                        // Hard corner - the edge cannot exist.
                        return float.MaxValue;
                    }
                }
                else if (cornerCell.Z() == cell.Z() && cornerCell.X() == neighborCell.X())
                {
                    // Found the other corner cells. Check it.
                    if (!cornerCell.IsWalkable)
                    {
                        // Hard corner - the edge cannot exist.
                        return float.MaxValue;
                    }
                }
            }
        }

        // If the cell is dangerous terrain, it costs 10 feet to pass. Otherwise,
        // it costs 5 feet to pass.
        if (neighborCell.IsDangerousTerrain)
        {
            return _dangerousTerrainCost;
        }
        else
        {
            return _normalTerrainCost;
        }
    }
}
