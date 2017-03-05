/*
 * File:
 *   SquareMovement
 * 
 * Description:
 *   This file contains a static class which calculates
 *   creature movement over a square grid. The methods
 *   are system-agnostic, and take as an argument the
 *   relevant system-specific functions.
 */

using System;

/*
 * Class:
 *   SquareMovement
 * 
 * Description:
 *   This is a static class which contains static methods
 *   used to calculate movement over a square grid. These
 *   should be used during structured time, like combat.
 */
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
                        (SquareCell)grid.GetBasicCell(widthIndex, heightIndex),
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
    public static AStarResults CalculateSquareGridPath(SquareGrid grid, CreatureSquareMove creature,
        Func<SquareCell, int, SquareDirection, float> edgeFunc, int source, int dest)
    {
        // Calculate the shortest path using the A* algorithm.
        AStarResults results = AStar.CalculatePath(grid.Width,
            GetAdjacencyListFromGrid(grid, creature.SquareSize, edgeFunc),
                                   source, dest, SquareGridHeuristic);

        // Does the path exist and cost less than the allowable cost?
        if (results != null && results.PathWeight <= creature.MoveSpeed)
        {
            // Yes, so we need to convert the path from a graph path to a grid path.
            for (int graphIndex = 0; graphIndex < results.Path.Count; graphIndex++)
            {
                // Get the Z-coordinate of the index with respect to the graph.
                int graphHeight =
                    results.Path[graphIndex]/(grid.Width - (creature.SquareSize - 1));

                // For each row of height, we need to add the size of the creature - 1
                // to account for the difference in sizes between the graph and the grid.
                results.Path[graphIndex] += graphHeight * (creature.SquareSize - 1);
            }

            // Return the grid indices.
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
     *   CalculateDistances
     * 
     * Description:
     *   Takes a square grid and calculates the distance to
     *   each of the other cells in the grid. This function
     *   can tolerate multiple size creatures.
     */
    public static int[,] CalculateDistances(SquareGrid grid, CreatureSquareMove creature,
        Func<SquareCell, int, SquareDirection, float> calculateEdgeCost)
    {
        // Calculate the distances from the point of view of a graph.
        float[] graphDistances = DijkstraSPT.CalculateDistances(
            GetAdjacencyListFromGrid(grid, creature.SquareSize, calculateEdgeCost),
                creature.Position);

        // Allocate space to store the distances to each cell of the grid.
        int[,] gridDistances = new int[grid.Width, grid.Height];

        // At the beginning, each distance should be the maximum possible.
        for (int gridWidth = 0; gridWidth < grid.Width; gridWidth++)
        {
            for (int gridHeight = 0; gridHeight < grid.Height; gridHeight++)
            {
                gridDistances[gridWidth, gridHeight] = int.MaxValue;
            }
        }

        // Convert from graph to grid vertices. The distance to the grid vertex
        // will be the minimum of the distance to each graph vertex which overlaps
        // with the grid vertex.
        for (int graphIndex = 0; graphIndex < graphDistances.Length; graphIndex++)
        {
            /*
             * Each loop of this outer loop handles all of the grid vertices
             * encompassed by a single graph vertex.
             */

            // Since the conversion from float to int is strange, if there's no
            // valid path just skip the index.
            if (graphDistances[graphIndex] == float.MaxValue)
            {
                continue;
            }

            int gridWidthIndex = graphIndex % (grid.Width - (creature.SquareSize - 1));
            int gridHeightIndex = graphIndex / (grid.Width - (creature.SquareSize - 1));

            // Iterate along the length of a graph vertex.
            for (int height = 0; height < creature.SquareSize; height++)
            {
                // Iterate along the height of a graph vertex.
                for (int width = 0; width < creature.SquareSize; width++)
                {
                    if (graphDistances[graphIndex] <
                        (int)gridDistances[gridWidthIndex + width, gridHeightIndex + height])
                    {
                        gridDistances[gridWidthIndex + width, gridHeightIndex + height] =
                            (int)graphDistances[graphIndex];
                    }
                }
            }
        }

        return gridDistances;
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
