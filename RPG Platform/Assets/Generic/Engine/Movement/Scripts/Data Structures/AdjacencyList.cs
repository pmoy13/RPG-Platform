/*
 * File:
 *   AdjacencyList.cs
 * 
 * Description:
 *   This file contains the implementation of an adjacency list
 *   and its nodes. This is one way to represent a graph, and it
 *   is the chosen method for our movement algorithms.
 */

using System;

/*
 * Class:
 *   AdjacencyList
 * 
 * Description:
 *   This class is an implementation of the adjacency list
 *   representation of a graph. It is an array of vertices,
 *   with each cell of the array containing a linked list
 *   of nodes that represent edges from that vertex.
 */
public class AdjacencyList
{
    // The array of edge lists, indexed
    // by vertex number.
    public AdjacencyNode[] Nodes;

    /*
     * Method:
     *   AdjacencyList
     * 
     * Description:
     *   Creates a new instance of the adjacency
     *   list class, with no data.
     */
    public AdjacencyList(int numNodes)
    {
        Nodes = new AdjacencyNode[numNodes];
    }

    /*
     * Method:
     *   AdjacencyList
     * 
     * Description:
     *   Creates a new AdjacencyList object
     *   from the input graph, calculating
     *   all of the edge weights from the
     *   given distance function.
     */
    public AdjacencyList(BasicGrid grid,
        Func<BasicCell, int, float> calculateEdgeWeight)
    {
        // The total number of vertices in the grid.
        int numVertices = grid.GetNumVertices();

        // The maximum number of neighbors a grid cell can have.
        int maxNeighbors = grid.GetMaxNeighbors();

        // Allocate space for the graph.
        Nodes = new AdjacencyNode[numVertices];

        // Loop through each of the grid's vertices and construct
        // a list of edges from that vertex to any neighboring
        // vertices.
        for (int index = 0; index < numVertices; index++)
        {
            // The node which will point to the current node.
            // We construct the linked list of nodes in backwards
            // order, so we can ensure the final node doesn't
            // reference any other node.
            AdjacencyNode prevNode = null;

            // The current cell under consideration.
            BasicCell cell = grid.GetBasicCell(index);

            // If the current cell isn't traversible, don't add any edges.
            if (!cell.IsWalkable)
            {
                continue;
            }

            // Get all of the neighbors of the current cell.
            BasicCell[] neighbors = cell.GetNeighbors();

            // Loop through all possible neighbors of the cell. If the neighbor
            // in a direction exists, calculate its edge cost using the input function.
            for (int currNeighbor = 0; currNeighbor < maxNeighbors; currNeighbor++)
            {
                BasicCell neighbor = neighbors[currNeighbor];

                // Calculate the edge cost from the source to this current node.
                // If the edge exists, add an adjacency node for it.
                float edgeCost = calculateEdgeWeight(cell, currNeighbor);

                if (edgeCost != float.MaxValue)
                {
                    // Yes, so create an adjacency node for it.
                    AdjacencyNode currNode =
                        new AdjacencyNode(neighbor.GetIndexFromCoordinates(grid.Width),
                            edgeCost, prevNode);

                    // Update the previous node in the list.
                    prevNode = currNode;
                }
            }

            // All the edge weights for existing neighbors have been calculated,
            // so add the edge list to the graph.
            Nodes[index] = prevNode;
        }
    }
}

/*
 * Class:
 *   AdjacencyNode
 * 
 * Description:
 *   This class is the node for an adjacency
 *   list representation of a graph. It contains
 *   the data for a single edge of the graph: the
 *   neighboring vertex and the weight of the edge
 *   to the neighboring vertex. Also, it contains
 *   a reference to the next edge.
 */
public class AdjacencyNode
{
    // The neighboring vertex for the edge.
    public int Neighbor;

    // The weight of the edge to the neighbor.
    public float EdgeWeight;

    // The next node in the list of edges.
    public AdjacencyNode NextNode;

    /*
     * Method:
     *   AdjacencyNode
     * 
     * Description:
     *   Creates a new instance of the AdjacencyNode
     *   class from the input parameters.
     */
    public AdjacencyNode(int neighbor, float weight, AdjacencyNode next)
    {
        // Assign the internal fields.
        Neighbor = neighbor;
        EdgeWeight = weight;
        NextNode = next;
    }
}
