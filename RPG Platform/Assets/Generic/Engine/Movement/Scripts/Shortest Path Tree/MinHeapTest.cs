using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeapTest : MonoBehaviour
{
    // Only for testing purposes.
    [SerializeField] MinHeap testHeap;

	// Use this for initialization
	void Start () {
        // Create the MinHeap to test.
        testHeap = new MinHeap(20);

        // Populate the MinHeap with random values.
	    for (int i = 0; i < 20; i++)
	    {
	        testHeap.AddMinHeapNode(new MinHeapNode(i, Random.Range(0, 50)), i);
	    }
        testHeap.MinHeapify(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
