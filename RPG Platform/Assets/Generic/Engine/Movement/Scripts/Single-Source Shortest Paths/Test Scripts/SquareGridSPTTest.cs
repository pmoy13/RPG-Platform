using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inteded for a 10x10 square grid.
public class SquareGridSPTTest : MonoBehaviour
{
    public SquareGrid grid;
    public float[] distances;
    public DD5eSystem RpgSystem;

	// Use this for initialization
	void Start ()
	{
	    grid.Cells[5].IsWalkable = false;
	    grid.Cells[6].IsWalkable = false;
	    grid.Cells[7].IsWalkable = false;
        RpgSystem = new DD5eSystem();
		distances = DijkstraSPT.CalculatePaths(
            new AdjacencyList(grid, RpgSystem.CalculateMovementCost), 12);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
