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
    public static AStarResults CalculatePath(int gridWidth,
        AdjacencyList graph, int source, int dest, Func<int, int, int, int> heuristic)
    {
        // The total number of vertices in the grid.
        int numVertices = graph.Nodes.Length;

        // Holds the weight of the path from the source to each vertex.
        float[] distancesFromSource = new float[numVertices];

        // Holds the previous vertex in the shortest path to a vertex.
        int[] predecessors = new int[numVertices];

        // Create the MinHeap to hold the edges (neighbors) of the grid.
        MinHeap minHeap = new MinHeap(numVertices);

        // Populate the MinHeap with each vertex.
        for (int currVertex = 0; currVertex < numVertices; currVertex++)
        {
            // Current path to this vertex has infinite weight.
            distancesFromSource[currVertex] = float.MaxValue;

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
            int minIndex = minHeap.ExtractMin();

            // Get the start of the linked list of neighbors.
            AdjacencyNode currAdjNode = graph.Nodes[minIndex];

            // Loop through each of the edges. See if we can find a path
            // through the current cell to a neighbor which is shorter than the
            // current path to that neighbor.
            while (currAdjNode != null)
            {
                // Get the vertex number of the neighboring cell.
                int neighbor = currAdjNode.Neighbor;

                // Is the distance from the neighbor through the current
                // node shorter than its current distance from the source?
                if (distancesFromSource[minIndex] != int.MaxValue && // Protects from overflow.
                    ((currAdjNode.EdgeWeight + distancesFromSource[minIndex])
                        < distancesFromSource[neighbor]))
                {
                    // Set the new shortest path value.
                    distancesFromSource[neighbor] =
                        distancesFromSource[minIndex] + currAdjNode.EdgeWeight;

                    // Create a new node in the MinHeap with the heuristic.
                    // Note that if a node already exists in the MinHeap, this
                    // will not add a new node, but rather it will exit early.
                    minHeap.InsertMinHeapNode(neighbor, distancesFromSource[neighbor] +
                        heuristic(gridWidth, neighbor, dest));

                    // Assign the current node's predecessor.
                    predecessors[neighbor] = minIndex;
                }

                // Go to the next edge in the linked list.
                currAdjNode = currAdjNode.NextNode;
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
            // Convert from a float to an int, to compensate for the
            // possibility of diagonals being counted for 1.5 then
            // rounded down.
            path.Reverse();
            return new AStarResults(path, (int)distancesFromSource[dest]);
        }
        else
        {
            // No valid path found.
            return null;
        }
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
