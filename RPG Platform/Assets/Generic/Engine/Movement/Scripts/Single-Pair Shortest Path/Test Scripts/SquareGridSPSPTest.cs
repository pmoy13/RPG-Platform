using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridSPSPTest : MonoBehaviour {

    public SquareGrid grid;
    public AStarResults results;
    public List<int> path;
    public int weight;
    public DD5eSystem RpgSystem;

    // Use this for initialization
    void Start()
    {
        grid.Cells[0, 1].IsWalkable = false;
        grid.Cells[1, 1].IsWalkable = false;
        grid.Cells[2, 1].IsWalkable = false;
        RpgSystem = new DD5eSystem();
        results = SquareMovement.CalculateSquareGridPath(grid, RpgSystem.CalculateMovementCost,
                                                    12, 0, int.MaxValue);

        path = new List<int>();
        if (results != null)
        {
            path = results.path;
            weight = results.pathWeight;
        }
        else
        {
            Debug.Log("No valid path found!");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
