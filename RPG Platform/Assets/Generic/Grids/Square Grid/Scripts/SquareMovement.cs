
using System;

public static class SquareMovement
{
    /*
     * Method:
     *   FromSquareGrid
     * 
     * Description:
     *   Creates a new AdjacencyList object from a square
     *   grid with the given vertex size. As adjacency lists
     *   are used to calculate movement costs, the size input
     *   represents the size of the creature moving: for example,
     *   a Huge creature in Dungeons and Dragons is 3x3 square cells.
     */

    public static AdjacencyList GetAdjacencyListFromGrid(SquareGrid grid, int creatureSize,
        Func<SquareCell, int, SquareDirection, float> calculateEdgeCost)
    {
        // Calculate the number of vertices the graph will have.
        int graphWidth = grid.Width - (creatureSize - 1);
        int graphHeight = grid.Height - (creatureSize - 1);

        // Create the new adjacency list to return.
        AdjacencyList graph = new AdjacencyList(graphWidth * graphHeight);

        // Because we are skipping some of the cells in the grid, we need
        // to keep a counter of the current index in our graph. Otherwise,
        // we'll end up with some wasted space.
        int graphIndex = 0;

        // Loop through each of the graph's vertices and construct
        // a list of edges from that vertex to any neighboring
        // vertices.
        for (int heightIndex = 0; heightIndex < graphHeight; heightIndex++)
        {
            for (int widthIndex = 0; widthIndex < graphWidth; widthIndex++)
            {
                // The node which will point to the current node.
                // We construct the linked list of nodes in backwards
                // order, so we can ensure the final node doesn't
                // reference any other node.
                AdjacencyNode prevNode = null;

                // Loop through the neighbor in each direction. Here, neighbor
                // means the neighboring graph vertex, which is not necessarily the
                // neighboring grid cell.
                for (SquareDirection direction = SquareDirection.N;
                    direction <= SquareDirection.NW;
                    direction++)
                {
                    // Calculate the edge cost from the source to this current node.
                    // If the edge exists, add an adjacency node for it.
                    float edgeCost = calculateEdgeCost(
                        (SquareCell)grid.GetBasicCell(widthIndex + grid.Width * heightIndex),
                        creatureSize, direction);

                    if (edgeCost != float.MaxValue)
                    {
                        // Yes, so create an adjacency node for it.
                        AdjacencyNode currNode =
                            new AdjacencyNode(CalculateNeighborVertex(
                                direction, graphIndex, graphWidth), edgeCost,
                                prevNode);

                        // Update the previous node in the list.
                        prevNode = currNode;
                    }
                }

                // Assign the linked list to the vertex array.
                graph.Nodes[graphIndex] = prevNode;

                // Increment the graph index.
                graphIndex++;
            }
        }

        // Return the newly populated graph.
        return graph;
    }

    /*
     * Method:
     *   CalculateSquareGridPath
     * 
     * Description:
     *   Calculates the shortest path between the given source
     *   and destination cells in a square grid, then checks to
     *   see if that path exists, and if the path exists it checks
     *   to see whether or not the weight of the shortest path is
     *   less than or equal to the movement cost passed in. If it
     *   is, then the path and its cost are returned, otherwise
     *   it returns null to denote the creature cannot make the
     *   specified move with the given maximum distance.
     */
    public static AStarResults CalculateSquareGridPath(SquareGrid grid,
        Func<BasicCell, int, float> edgeFunc, int source, int dest, int totalMoves)
    {
        // Calculate the shortest path using the A* algorithm.
        AStarResults results = AStar.CalculatePath(grid.Width, new AdjacencyList(grid, edgeFunc),
                                   source, dest, SquareGridHeuristic);

        // Does the path exist and cost less than the allowable cost?
        if (results != null && results.pathWeight <= totalMoves)
        {
            // Yes, so return it.
            return results;
        }
        // No, so the path is invalid.
        else
        {
            return null;
        }
    }

    /*
     * Method:
     *   CalculateNeighborVertex
     * 
     * Description:
     *   Calculates the correct index for the neighbor
     *   vertex in a given direction. Note that the
     *   neighbor in question is the neighbor as seen]
     *   by the graph, not the grid.
     */

    private static int CalculateNeighborVertex(SquareDirection direction, int source,
        int graphWidth)
    {
        switch (direction)
        {
            case SquareDirection.N:
                return (source + graphWidth);
            case SquareDirection.NE:
                return (source + graphWidth + 1);
            case SquareDirection.E:
                return (source + 1);
            case SquareDirection.SE:
                return (source - graphWidth + 1);
            case SquareDirection.S:
                return (source - graphWidth);
            case SquareDirection.SW:
                return (source - graphWidth - 1);
            case SquareDirection.W:
                return (source - 1);
            case SquareDirection.NW:
                return (source + graphWidth - 1);
            default:
                // This case should never be reached, as the above cases
                // are exhaustive.
                return int.MaxValue;
        }
    }

    /*
     * Method:
     *   SquareGridHeuristic
     * 
     * Description:
     *   This is the heuristic used when running the A*
     *   algorithm on square grid objects. It is an
     *   admissible heuristic, so it will never overestimate
     *   the true cost of moving between the two cells.
     */
    private static int SquareGridHeuristic(int gridWidth, int source, int dest)
    {
        // For square grids, the smallest possible distance from two cells
        // would be moving diagonally, where in one move you can cover both
        // a step in the X direction and a step in the Z direction. Thus,
        // to make sure we do not overestimate the distance between the cells
        // we just take the maximum of the differences between the source and
        // destination cells' X and Z values. The coordinates can be calculated
        // with the width of the grid.
        return Math.Max(Math.Abs(source / gridWidth - dest / gridWidth), // Difference in Z.
                         Math.Abs(source % gridWidth - dest % gridWidth)); // Difference in X.
    }
}
