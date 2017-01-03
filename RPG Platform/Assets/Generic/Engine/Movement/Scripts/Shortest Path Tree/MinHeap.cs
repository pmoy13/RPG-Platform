using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap
{
    // Current number of nodes.
    private int _currSize;

    // Maximum number of nodes.
    private int _capacity;

    // Storage for the data.
    [SerializeField] MinHeapNode[] _nodes;
    [SerializeField] int[] _positions;

    public MinHeap(int capacity)
    {
        // Allocate space for 
        _capacity = capacity;
        _nodes = new MinHeapNode[capacity];
        _positions = new int[capacity];

        // Initially has no nodes.
        _currSize = 0;
    }

    public void AddMinHeapNode(MinHeapNode node, int index)
    {
        _nodes[index] = node;
        _positions[index] = index;
    }

    // Helper for MinHeapify
    private void SwapMinHeapNode(int a, int b)
    {
        // Get the nodes to be swapped.
        MinHeapNode aNode = _nodes[a];
        MinHeapNode bNode = _nodes[b];

        // Swap their positions in the MinHeap.
        int tempPosition = _positions[aNode.vertexNum];
        _positions[a] = _positions[bNode.vertexNum];
        _positions[b] = tempPosition;

        // Swap the nodes themselves in the array.
        _nodes[a] = bNode;
        _nodes[b] = aNode;
    }

    public void MinHeapify(int index)
    {
        // Smallest value we've encountered.
        int smallest = index;
        // Left and right children of the node.
        int left = 2 * index + 1;
        int right = 2 * index + 2;

        // Is the left child smaller than the current node?
        if ((left < _currSize) && // If the node has a left child...
            (_nodes[left].distanceFromStart < _nodes[smallest].distanceFromStart)) // Is the left child smaller?
        {
            // The left child should be the smallest node.
            smallest = left;
        }
        // Is the right child smaller than the current node?
        if ((right < _currSize) && // If the node has a right child...
            (_nodes[right].distanceFromStart < _nodes[smallest].distanceFromStart)) // Is the right child smaller?
        {
            // The right child should be the smallest node.
            smallest = right;
        }

        // Has the smallest node been modified? If so, make the change.
        if (smallest != index)
        {
            // Swap the nodes in the heap.
            SwapMinHeapNode(smallest, index);

            // Make sure the heap properties are satisfied at the new smallest node.
            MinHeapify(smallest);
        }
    }

    private bool IsEmpty()
    {
        return (_currSize == 0);
    }

    private MinHeapNode ExtractMin()
    {
        // If the heap is empty, there's nothing to extract.
        if (IsEmpty())
        {
            return null;
        }

        // Save the root. (Always the minimum!)
        MinHeapNode root = _nodes[0];

        // Replace the root with the last node, then heapify.
        _nodes[0] = _nodes[_currSize - 1];
        _positions[root.vertexNum] = _currSize - 1;
        _currSize -= 1; // Account for the removed node.
        _positions[_nodes[0].vertexNum] = 0; // Last node is now the first node.

        // Heapify from the root of the heap.
        MinHeapify(0);

        // Return the extracted node.
        return root;
    }

    private void DecreaseKey(int vertexNum, int newDistance)
    {
        // Get the vertex's index in the heap array.
        int heapIndex = _positions[vertexNum];

        // Update the node's distance value.
        _nodes[heapIndex].distanceFromStart = newDistance;

        // Travel up and heapify the MinHeap.
        while ((heapIndex >= 0) && // We haven't exhausted the entire heap.
               (_nodes[heapIndex].distanceFromStart < _nodes[(heapIndex - 1)/2].distanceFromStart))
        {
            // Parent is greater than child - swap them.
            SwapMinHeapNode(heapIndex, (heapIndex - 1)/2);

            // Move to the parent's index.
            heapIndex = (heapIndex - 1)/2;
        }
    }

    private bool IsInMinHeap(int vertexNum)
    {
        return (_positions[vertexNum] < _currSize);
    }
}

public class MinHeapNode
{
    // The index into a grid's cells array.
    public int vertexNum;

    // Current distance from the start node.
    public int distanceFromStart;

    public MinHeapNode(int vertexNum, int distanceFromStart)
    {
        this.vertexNum = vertexNum;
        this.distanceFromStart = distanceFromStart;
    }
}
