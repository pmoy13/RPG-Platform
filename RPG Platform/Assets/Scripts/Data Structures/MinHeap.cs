/*
 * File:
 *   MinHeap.cs
 * 
 * Description:
 *   This file contains a class which implements
 *   a min heap data structure.
 */

/*
 * Class:
 *   MinHeap
 *   
 * Description:
 *   This class implements a min heap
 *   data structure. Min heaps are complete
 *   binary trees that satisfy the min heap
 *   property: each parent node has a key
 *   less than or equal to each of its children.
 */

using System;

public class MinHeap
{
    // Current number of nodes in the min heap.
    // The arrays which store the min heap nodes
    // can be larger than this size, say after a
    // node has been extracted from the min heap,
    // so this variable tells us which of the indices
    // in the arrays contain valid data.
    private int _currSize;

    // Nodes which make up the min heap.
    private MinHeapNode[] _nodes;

    // The min heap is manipulated with only vertex
    // numbers, which the min heap has to translate into
    // min heap nodes. This array maps these vertex numbers
    // into indices into the _nodes array so we can get the
    // corresponding min heap nodes.
    private int[] _positions;

    // This is the value we will return if the min heap
    // extract min is invoked with invalid parameters.
    public static int InvalidExtract = int.MaxValue;

    /*
     * Method:
     *   MinHeap
     * 
     * Description:
     *   Creates a new, empty MinHeap object with the given
     *   maximum size.
     */
    public MinHeap(int capacity)
    {
        // Allocate space for the MinHeap and its members.
        _nodes = new MinHeapNode[capacity];
        _positions = new int[capacity];

        // Initially has no nodes.
        _currSize = 0;

        // Initialize each vertex as not containing valid data.
        for (int index = 0; index < capacity; index++)
        {
            _positions[index] = capacity;
        }
    }

    /*
     * Method:
     *   InsertMinHeapNode
     * 
     * Description:
     *   Inserts a new MinHeapNode into the MinHeap.
     *   The new node is first inserted at the bottom
     *   of the heap, then it is swapped up into its
     *   correct spot in the heap. Note that the node
     *   to be added should have a key of MAX_INT.
     */
    public void InsertMinHeapNode(int index, float key)
    {
        // Check to ensure this index doesn't already
        // have an associated node. If it does, exit.
        // Since we don't save the capacity, we have
        // to compute it indirectly as the length of
        // the positions array.
        if (_positions[index] != _positions.Length)
        {
            // The node already exists in the heap.
            return;
        }

        // Update the size of the heap.
        _currSize += 1;

        // Add the new node to the end of the heap. We initially give
        // the node a key of max value, so that when we decrease the key
        // we know that the operation will complete.
        _nodes[_currSize - 1] = new MinHeapNode(index, float.MaxValue);
        _positions[index] = _currSize - 1;

        // Restore the heap property, adding in the correct key value.
        this.DecreaseKey(index, key);
    }

    /*
     * Method:
     *   SwapMinHeadNodes
     * 
     * Description:
     *   Swaps the MinHeapNodes in the MinHeap
     *   at the input indices.
     */
    private void SwapMinHeapNodes(int a, int b)
    {
        // Get the nodes to be swapped.
        MinHeapNode aNode = _nodes[a];
        MinHeapNode bNode = _nodes[b];

        // Update their indices in the MinHeap.
        int tempPosition = _positions[aNode.index];
        _positions[aNode.index] = _positions[bNode.index];
        _positions[bNode.index] = tempPosition;

        // Swap the nodes themselves in the MinHeap.
        _nodes[a] = bNode;
        _nodes[b] = aNode;
    }

    /*
     * Method:
     *   MinHeapify
     * 
     * Description:
     *   Restores the min heap property in
     *   the internal array at the given index.
     *   Assumes that the trees at the left and
     *   right children already satisfy the min
     *   heap property.
     */
    public void MinHeapify(int index)
    {
        // Keep track of the index of the smallest key we've seen.
        // This will either be the node itself, in which case the
        // min heap property is satisfied, or one of the children,
        // in which case we need to swap the smallest child with the
        // parent node.
        int smallest = index;

        // Left and right children of the node.
        int left = 2 * index + 1;
        int right = 2 * index + 2;

        // Is the left child smaller than the current node?
        if ((left < _currSize) && // If the node has a left child...
            (_nodes[left].key <
               _nodes[smallest].key)) // Is the left child smaller?
        {
            // The left child is the smallest node we've found.
            smallest = left;
        }
        // Is the right child smaller than the current node?
        if ((right < _currSize) && // If the node has a right child...
            (_nodes[right].key <
               _nodes[smallest].key)) // Is the right child smaller?
        {
            // The right child is the smallest node.
            smallest = right;
        }

        // Has the smallest node been modified? If so, one or both children was
        // smaller than the parent. We need to switch the parent with the smallest
        // child.
        if (smallest != index)
        {
            // Swap the nodes in the heap.
            SwapMinHeapNodes(smallest, index);

            // Make sure the heap properties are satisfied at the new smallest node.
            // In this way, we protect against the min heap property of the subtree
            // of the child that we swapped being broken by the replacement of the
            // child with the parent.
            MinHeapify(smallest);
        }
    }

    /*
     * Method:
     *   IsEmpty
     * 
     * Description:
     *   Checks to see if the min heap is empty.
     */
    public bool IsEmpty()
    {
        return (_currSize == 0);
    }

    /*
     * Method:
     *   ExtractMin
     * 
     * Description:
     *   Returns the index of the MinHeapNode with the smallest
     *   key in the MinHeap, then ensures that the resulting
     *   tree still satisfies the min heap property. The
     *   returned node is removed from the heap.
     */
    public int ExtractMin()
    {
        // If the heap is empty, there's nothing to extract.
        if (IsEmpty())
        {
            return InvalidExtract;
        }

        // Save the root. (Always the minimum!)
        MinHeapNode root = _nodes[0];

        // Replace the root with the last node.
        _nodes[0] = _nodes[_currSize - 1];
        _positions[_nodes[0].index] = 0; // Last node is now the first node.

        // Update the position of the min node getting extracted.
        // This is necessary because it allows us to determine
        // wether a given vertex number corresponds to a node
        // in the heap, or if that vertex's node has been extracted
        // already. When used in Dijkstra's shortest path algorithm,
        // vertices whose nodes have been extracted have had their
        // shortest paths finalized.
        _positions[root.index] = _currSize - 1;

        // Account for the new size after the root is removed.
        _currSize -= 1;

        // Heapify from the new root of the tree to restore the min heap
        // property.
        MinHeapify(0);

        // Return the extracted node.
        return root.index;
    }

    /*
     * Method:
     *   DecreaseKey
     * 
     * Description:
     *   Takes a node in the min heap and decreases that node's key,
     *   ensuring that the min heap property remains satisfied. The
     *   vertexNum argument is the index into the graph (grid), not
     *   the index into the min heap itself.
     */
    public void DecreaseKey(int vertexNum, float newDistance)
    {
        // Get the vertex's index in the heap array.
        int heapIndex = _positions[vertexNum];

        // Check to see if the new distance is greater than or
        // equal to the current distance. If so, then there's no
        // need to do anything - the input parameters were invalid.
        if (_nodes[heapIndex].key <= newDistance)
        {
            // Invalid input parameters.
            return;
        }

        // Update the node's distance value.
        _nodes[heapIndex].key = newDistance;

        // Travel up and heapify the MinHeap.
        while ((heapIndex > 0) && // We haven't exhausted the entire heap.
               (_nodes[heapIndex].key <
                  _nodes[(heapIndex + 1)/2 - 1].key))
        {
            // Parent is greater than child - swap them.
            SwapMinHeapNodes(heapIndex, (heapIndex + 1)/2 - 1);

            // Move to the parent's index.
            heapIndex = (heapIndex + 1)/2 - 1;
        }
    }

    /*
     * Method:
     *   Contains
     * 
     * Description:
     *   Takes an index into the graph (grid), and checks
     *   to see if the corresponding node is in the MinHeap.
     *   This is used to check if a node's shortest path has
     *   been finalized - nodes reminaing in the heap haven't
     *   had their shortest paths finalized, while nodes that
     *   have been extracted and are no longer in the heap have
     *   had their distances finalized.
     */
    public bool Contains(int vertexNum)
    {
        // Check to make sure this vertex number is within
        // the bounds of the arrays used. If not, then the
        // given index is not in the heap, and in fact the heap
        // not be associated with the graph this vertex came
        // from.
        if (vertexNum >= _positions.Length)
        {
            return false;
        }

        // The corresponding node could still be in the array,
        // but it may be in a portion of the array that doesn't
        // contain valid data.
        return (_positions[vertexNum] < _currSize);
    }
}

/*
 * Class:
 *   MinHeapNode
 * 
 * Description:
 *   Each instance of this class represents a node in the
 *   MinHeap.
 */
public class MinHeapNode
{
    // The index into a grid's cells array.
    public int index;

    // Current distance from the start node. This is the key
    // of the MinHeap - we call it distance, since we only use
    // the heap for calculating shortest paths.
    public float key;

    /*
     * Method:
     *   MinHeapNode
     * 
     * Description:
     *   Creates a new instance of the class.
     */
    public MinHeapNode(int index, float key)
    {
        // Assign the fields.
        this.index = index;
        this.key = key;
    }
}
