using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inteded for a 10x10 square grid.
public class SquareGridSPTTest : MonoBehaviour
{
    public SquareGrid grid;
    public float[] distances;
    public GameSystem gameSystem;
    private RPGSystem _system;

	// Use this for initialization
	void Start ()
	{
	    _system = gameSystem.GetCurrentSystem();

	    grid.Cells[0, 1].IsWalkable = false;
	    grid.Cells[1, 1].IsWalkable = false;
	    grid.Cells[2, 1].IsWalkable = false;

		distances = DijkstraSPT.CalculateDistances(
            SquareMovement.GetAdjacencyListFromGrid(grid, 1,
                _system.CalculateSquareGridMovementCost), 12);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
