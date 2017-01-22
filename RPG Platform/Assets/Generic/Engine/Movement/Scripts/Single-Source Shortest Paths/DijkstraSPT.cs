/*
 * File:
 *   DijkstraSPT.cs
 *   
 * Description:
 *   This file contains a single static class
 *   used to calculate the shortest path from a 
 *   single grid vertex to every other vertex. It
 *   uses Dijkstra's algorithm with a min heap
 *   data structure.
 */

 /*
  * Class:
  *   DijkstraSPT
  *   
  * Description:
  *   This is a static class with a static method
  *   which takes a grid vertex as input and returns
  *   the shortest path distance from that vertex
  *   to each vertex in the grid. It uses Dijkstra's
  *   algorithm. It also contains helper methods
  *   to facilitate the running of the algorithm.
  */
public static class DijkstraSPT
{
    /*
     * Method:
     *   CalculatePaths
     *   
     * Description:
     *   A static method which takes a grid object and
     *   a source vertex, then uses Dijkstra's algorithm
     *   to calculate the shortest path distances from
     *   the source vertex to each vertex of the grid.
     *   Used to find the possible moves for a creature
     *   during structured movement time, like combat.
     */
    public static int[] CalculatePaths(BasicGrid grid, int source)
    {
        // The total number of vertices in the grid.
        int numVertices = grid.GetNumVertices();

        // The maximum number of neighbors a grid cell can have.
        int maxNeighbors = grid.GetMaxNeighbors();

        // Holds the weight of the path from the source to each vertex.
        int[] distancesFromSource = new int[numVertices];

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

            // Create a new MinHeapNode and add it to the MinHeap.
            minHeap.InsertMinHeapNode(currVertex, int.MaxValue);
        }

        // Initialize the source vertex.
        distancesFromSource[source] = 0; // Path to source has no weight.
        minHeap.DecreaseKey(source, 0);

        // Calculate the shortest path to each vertex, one at a time. The
        // MinHeap only holds those vertices whose shortest paths have not
        // yet been finalized. When it's empty, we're done.
        while (!minHeap.IsEmpty())
        {
            // Extract the vertex closest to the source. This vertex's distance
            // has been finalized.
            MinHeapNode currNode = minHeap.ExtractMin();

            // Get the index into the grid's Cells array.
            int minIndex = currNode.vertexNum;

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

                    // Has the neighbor's shortest path to the source been finalized?
                    // If not, is the distance from the neighbor through the current
                    // node shorter than its current distance from the source?
                    if (minHeap.Contains(currIndex) && // Hasn't had its path finalized.
                        distancesFromSource[minIndex] != int.MaxValue && // Protects from overflow.
                        ((edgeWeights[currIndex] + distancesFromSource[minIndex])
                            < distancesFromSource[currIndex]))
                    {
                        // Set the new shortest path value.
                        distancesFromSource[currIndex] =
                            distancesFromSource[minIndex] + edgeWeights[currIndex];

                        // Update the key in the heap to reflect the new value.
                        minHeap.DecreaseKey(currIndex, distancesFromSource[currIndex]);
                    }
                }
            }
        }

        // All shortest paths have been calculated.
        return distancesFromSource;
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
}
