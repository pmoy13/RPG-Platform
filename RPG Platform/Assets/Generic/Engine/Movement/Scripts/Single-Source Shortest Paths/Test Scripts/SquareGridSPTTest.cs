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
	    grid.Cells[0, 1].IsWalkable = false;
	    grid.Cells[1, 1].IsWalkable = false;
	    grid.Cells[2, 1].IsWalkable = false;
        RpgSystem = new DD5eSystem();
		distances = DijkstraSPT.CalculateDistances(
            SquareMovement.GetAdjacencyListFromGrid(grid, DD5eSystem.SquareLargeSize,
                RpgSystem.CalculateSquareGridMovementCost), 12);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
