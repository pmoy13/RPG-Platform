/*
 * File:
 *   AStar.cs
 * 
 * Description:
 *   This file contains a static class used to
 *   run the A* pathfinding algorithm, and a results
 *   class to return both the weight of the path and
 *   the path itself from the algorithm. This algorithm
 *   should be used to calculate individual moves: given
 *   a source and destination on the grid, this algorithm
 *   will find the shortest path between them faster than
 *   Dijkstra's algorithm.
 */

using System;
using System.Collections.Generic;

/*
 * Class:
 *   AStar
 * 
 * Description:
 *   This is a static class with methods that will return
 *   the path and its weight representing a move between
 *   two cells in a grid. The algorithm itself it inaccessible
 *   from outside accesses, but we provide wrappers to be used
 *   with both square and hexagonal grids. Also, different
 *   heuristics are contained in the class, which improve the
 *   performance of the algorithm.
 */
public static class AStar
{
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
    public static AStarResults CalculateSquareGridPath(
        SquareGrid grid, int source, int dest, int totalMoves)
    {
        // Calculate the shortest path using the A* algorithm.
        AStarResults results = CalculatePath(grid, source, dest, SquareGridHeuristic);

        // Does the path exist and cost less than the allowable cost?
        if (results.path != null && results.pathWeight <= totalMoves)
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
     *   CalculatePath
     * 
     * Description:
     *   Uses the A* algorithm and the specified heuristic to
     *   calculate the shortest path between the given source
     *   and destination cells in a grid, either hexagonal or square.
     *   If a valid path exists, then this function returns the 
     *   shortest valid path with its corresponding weight. Otherwise,
     *   this function returns null to denote the destination is
     *   unreachable from the source.
     */
    private static AStarResults CalculatePath(
        BasicGrid grid, int source, int dest, Func<BasicCell, BasicCell, int> heuristic)
    {
        // The total number of vertices in the grid.
        int numVertices = grid.GetNumVertices();

        // The maximum number of neighbors a grid cell can have.
        int maxNeighbors = grid.GetMaxNeighbors();

        // Holds the weight of the path from the source to each vertex.
        int[] distancesFromSource = new int[numVertices];

        // Holds the previous vertex in the shortest path to a vertex.
        int[] predecessors = new int[numVertices];

        // Holds the movement cost to traverse a cell. We think of the
        // grid as a undirected graph with weighted edges.
        int[] edgeWeights = new int[numVertices];

        // Create the MinHeap to hold the edges (neighbors) of the grid.
        MinHeap minHeap = new MinHeap(numVertices);

        // Populate the MinHeap with each vertex.
        for (int currVertex = 0; currVertex < numVertices; currVertex++)
        {
            // Current path to this vertex has infinite weight.
            distancesFromSource[currVertex] = int.MaxValue;

            // Assign the edge weights.
            edgeWeights[currVertex] = CalculateEdgeWeight(grid, currVertex);

            // There are no predecessors at the start of the algorithm.
            predecessors[currVertex] = int.MaxValue;
        }

        // Initialize the source vertex.
        distancesFromSource[source] = 0; // Path to source has no weight.
        minHeap.InsertMinHeapNode(source, 0); // Add the source node to the heap.

        // Calculate the shortest path to each vertex, one at a time. Go
        // until either the heap containing nodes which have not had their
        // paths finalized has been exhausted, or until the destination node
        // has had its path finalized.
        while (!minHeap.IsEmpty())
        {
            // Extract the vertex closest to the source. This vertex's distance
            // has been finalized.
            MinHeapNode currNode = minHeap.ExtractMin();

            // Get the index into the grid's Cells array.
            int minIndex = currNode.vertexNum;

            // Is this the destination? If so, then we can end the loop.
            if (minIndex == dest)
            {
                // The shortest path has already been calculated. Exit.
                break;
            }

            // Get all of the neighbors of the current cell.
            BasicCell[] neighbors = grid.GetBasicCell(minIndex).GetNeighbors();

            // Loop through each of the neighbors. See if we can find a path
            // through the current cell to a neighbor which is shorter than the
            // current path to that neighbor.
            for (int currNeighbor = 0; currNeighbor < maxNeighbors; currNeighbor++)
            {
                BasicCell neighbor = neighbors[currNeighbor];

                // Is there a neighbor at this index? If so, is it traversible?
                // Not every cell has the maximum number of neighbors.
                if (neighbor != null && neighbor.IsWalkable)
                {
                    // Yes, so extract its index.
                    int currIndex = neighbor.GetIndexFromCoordinates(grid.Width);

                    // Is the distance from the neighbor through the current
                    // node shorter than its current distance from the source?
                    if (distancesFromSource[minIndex] != int.MaxValue && // Protects from overflow.
                        ((edgeWeights[currIndex] + distancesFromSource[minIndex])
                            < distancesFromSource[currIndex]))
                    {
                        // Set the new shortest path value.
                        distancesFromSource[currIndex] =
                            distancesFromSource[minIndex] + edgeWeights[currIndex];

                        // Create a new node in the MinHeap with the heuristic.
                        // Note that if a node already exists in the MinHeap, this
                        // will not add a new node, but rather it will exit early.
                        minHeap.InsertMinHeapNode(currIndex, distancesFromSource[currIndex] +
                            heuristic(grid.GetBasicCell(currIndex), grid.GetBasicCell(dest)));

                        // Assign the current node's predecessor.
                        predecessors[currIndex] = minIndex;
                    }
                }
            }
        }

        // Check to see if we've found a path to the destination.
        if (predecessors[dest] != int.MaxValue)
        {
            // We have a valid path, so reconstruct it.
            List<int> path = new List<int>();
            int currIndex = dest;

            // Go through the predecessors until we reach the
            // starting node.
            while (currIndex != source)
            {
                // Add the node.
                path.Add(currIndex);

                // Find the predecessor to that node in the path.
                currIndex = predecessors[currIndex];
            }

            // The path has been calculated, but in reverse order.
            // Return the correct ordering, as well as the path weight.
            path.Reverse();
            return new AStarResults(path, distancesFromSource[dest]);
        }
        else
        {
            // No valid path found.
            return null;
        }
    }

    /*
     * Method:
     *   CalculateEdgeWeight
     * 
     * Description:
     *   Takes an index into the vertex array for a grid, and
     *   calculates the movement cost to move into that
     *   cell of the grid. This represents the weight of
     *   the edge between this cell and any of its neighbors.
     */
    private static int CalculateEdgeWeight(BasicGrid grid, int vertexNum)
    {
        // The movement cost associated with this cell.
        int movementCost = 0;

        // Calculate the movement cost.
        if (grid.GetBasicCell(vertexNum).IsDangerousTerrain)
        {
            // Dangerous terrain costs double normal terrain to traverse.
            movementCost = 2;
        }
        else
        {
            movementCost = 1;
        }

        // Return the calculated cost.
        return movementCost;
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
    private static int SquareGridHeuristic(BasicCell node, BasicCell dest)
    {
        // For square grids, the smallest possible distance from two cells
        // would be moving diagonally, where in one move you can cover both
        // a step in the X direction and a step in the Z direction. Thus,
        // to make sure we do not overestimate the distance between the cells
        // we just take the maximum of the differences between the source and
        // destination cells' X and Z values.
        return Math.Max(Math.Abs(node.X() - dest.X()),
                         Math.Abs(node.Z() - dest.Z()));
    }
}

/*
 * Class:
 *   AStarResults
 * 
 * Description:
 *   This is a simple results class to hold the
 *   results of the A* algorithm, namely the path and
 *   the weight of the path.
 */
public class AStarResults
{
    // The path from source to destination.
    public List<int> path;

    // The weight of the path.
    public int pathWeight;

    /*
     * Method:
     *   AStarResults
     * 
     * Description:
     *   Creates an instance of AStarResults with the
     *   specified members. This class should only ever
     *   be instantiated with the return value from a
     *   wrapper function for the A* algorithm.
     */
    public AStarResults(List<int> path, int pathWeight)
    {
        // Assign the members.
        this.path = path;
        this.pathWeight = pathWeight;
    }
}
