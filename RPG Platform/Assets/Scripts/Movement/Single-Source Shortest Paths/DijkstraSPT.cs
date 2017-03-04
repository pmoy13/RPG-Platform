/*
 * File:
 *   DijkstraSPT.cs
 *   
 * Description:
 *   This file contains a single static class
 *   used to calculate the shortest path from a 
 *   single grid vertex to every other vertex. It
 *   uses Dijkstra's algorithm with a min heap
 *   data structure. This algorithm should be used to
 *   calculate all of the possible moves for a creature
 *   during structured time. Individual moves should
 *   be done with A* instead.
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
     *   CalculateDistances
     *   
     * Description:
     *   A static method which takes a grid object and
     *   a source vertex, then uses Dijkstra's algorithm
     *   to calculate the shortest path distances from
     *   the source vertex to each vertex of the grid.
     *   Used to find the possible moves for a creature
     *   during structured movement time, like combat.
     */
    public static float[] CalculateDistances(AdjacencyList graph, int source)
    {
        // The total number of vertices in the graph.
        int numVertices = graph.Nodes.Length;

        // Holds the weight of the path from the source to each vertex.
        float[] distancesFromSource = new float[numVertices];

        // Create the MinHeap to hold the edges (neighbors) of the graph.
        MinHeap minHeap = new MinHeap(numVertices);

        // Populate the MinHeap with each vertex.
        for (int currVertex = 0; currVertex < numVertices; currVertex++)
        {
            // Current path to this vertex has infinite weight.
            distancesFromSource[currVertex] = float.MaxValue;

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

                // Has the neighbor's shortest path to the source been finalized?
                // If not, is the distance from the neighbor through the current
                // node shorter than its current distance from the source?
                if (minHeap.Contains(neighbor) && // Hasn't had its path finalized.
                    distancesFromSource[minIndex] != float.MaxValue && // Protects from overflow.
                    ((currAdjNode.EdgeWeight + distancesFromSource[minIndex])
                        < distancesFromSource[neighbor]))
                {
                    // Set the new shortest path value.
                    distancesFromSource[neighbor] =
                        distancesFromSource[minIndex] + currAdjNode.EdgeWeight;

                    // Update the key in the heap to reflect the new value.
                    minHeap.DecreaseKey(neighbor, distancesFromSource[neighbor]);
                }

                // Go to the next edge in the linked list.
                currAdjNode = currAdjNode.NextNode;
            }
        }

        // All shortest paths have been calculated.
        return distancesFromSource;
    }
}
