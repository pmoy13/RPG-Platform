using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderSystem : RPGSystem
{
    // Movement costs for normal and dangerous terrain. Character
    // movement speed is given in feet, which we convert to squares.
    // In Dungeons and Dragons, each grid square is 5 feet. These
    // values should only be used for square grids.
    private static float _feetPerSquare = 5;
    private static float _normalTerrainCost = 1;
    private static float _diagonalTerrainCost = 1.5f;
    private static float _dangerousTerrainModifier = 2;

    // Sizes of various creatures in a square grid.
    public static int SquareMediumSize = 1;
    public static int SquareLargeSize = 2;
    public static int SquareHugeSize = 3;

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
        // Z coordinate.
        if (cell.X() != neighborCell.X() && cell.Z() != neighborCell.Z())
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

            // No hard corners - this is a legal move.
            if (neighborCell.IsDangerousTerrain)
            {
                return _diagonalTerrainCost * _dangerousTerrainModifier;
            }
            else
            {
                return _diagonalTerrainCost;
            }
        }

        // If the cell is dangerous terrain, it costs 10 feet to pass. Otherwise,
        // it costs 5 feet to pass.
        if (neighborCell.IsDangerousTerrain)
        {
            return _normalTerrainCost * _dangerousTerrainModifier;
        }
        else
        {
            return _normalTerrainCost;
        }
    }

    /*
     * Method:
     *   CalculateSquareGridMovementCost
     * 
     * Description:
     *   Calculates the movement cost for a creature of a given size
     *   in the given direction. Only works for a square grid.
     */
    public override float CalculateSquareGridMovementCost(SquareCell baseCell, int size,
        SquareDirection direction)
    {
        // Keep track of the maximum movement cost we've seen. This
        // is the movement cost between two adjacent cells, and the
        // cost of the actual move will be the maximum of all of the
        // individual costs to enter the new cells that the creature
        // will enter.
        float maxMoveCost = 0;

        // Determine the row and / or column to check, and the starting cells.
        SquareCell currentCell = baseCell;
        SquareDirection checkDir = SquareDirection.Invalid;
        SquareCell altCell = baseCell;
        SquareDirection altDir = SquareDirection.Invalid;

        int startLoop = size - 1;

        switch (direction)
        {
            case SquareDirection.N:
                {
                    while (startLoop > 0)
                    {
                        currentCell = currentCell.GetNeighbor(SquareDirection.N);
                        startLoop--;
                    }
                    checkDir = SquareDirection.E;
                    break;
                }
            case SquareDirection.NE:
                {
                    while (startLoop > 0)
                    {
                        currentCell = currentCell.GetNeighbor(SquareDirection.N);
                        altCell = altCell.GetNeighbor(SquareDirection.E);
                        startLoop--;
                    }
                    checkDir = SquareDirection.E;
                    altDir = SquareDirection.N;
                    break;
                }
            case SquareDirection.E:
                {
                    while (startLoop > 0)
                    {
                        currentCell = currentCell.GetNeighbor(SquareDirection.E);
                        startLoop--;
                    }
                    checkDir = SquareDirection.N;
                    break;
                }
            case SquareDirection.SE:
                {
                    while (startLoop > 0)
                    {
                        currentCell = currentCell.GetNeighbor(SquareDirection.E);
                        startLoop--;
                    }
                    checkDir = SquareDirection.N;
                    altDir = SquareDirection.E;
                    break;
                }
            case SquareDirection.S:
                {
                    checkDir = SquareDirection.E;
                    break;
                }
            case SquareDirection.SW:
                {
                    checkDir = SquareDirection.E;
                    altDir = SquareDirection.N;
                    break;
                }
            case SquareDirection.W:
                {
                    checkDir = SquareDirection.N;
                    break;
                }
            case SquareDirection.NW:
                {
                    while (startLoop > 0)
                    {
                        currentCell = currentCell.GetNeighbor(SquareDirection.N);
                        startLoop--;
                    }
                    checkDir = SquareDirection.E;
                    altDir = SquareDirection.N;
                    break;
                }
        }

        // Loop through each cell on the row and / or column we need to check.
        // Compare their movement costs to the highest-so-far movement cost, and
        // find the maximum.
        for (int sizeCount = 0; sizeCount < size; sizeCount++)
        {
            float currentMoveCost = CalculateMovementCost(currentCell, (int)direction);

            currentCell = currentCell.GetNeighbor(checkDir);

            if (currentMoveCost > maxMoveCost)
            {
                maxMoveCost = currentMoveCost;
            }

            if (altDir != SquareDirection.Invalid)
            {
                float altMoveCost = CalculateMovementCost(altCell, (int)direction);

                altCell = altCell.GetNeighbor(altDir);

                if (altMoveCost > maxMoveCost)
                {
                    maxMoveCost = altMoveCost;
                }
            }
        }

        // Return the max move cost.
        return maxMoveCost;
    }
}
