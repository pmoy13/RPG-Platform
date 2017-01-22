using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGridSPSPTest : MonoBehaviour {

    public SquareGrid grid;
    public AStarResults results;
    public List<int> path;
    public int weight;

    // Use this for initialization
    void Start()
    {
        grid.Cells[5].IsWalkable = false;
        grid.Cells[6].IsWalkable = false;
        grid.Cells[7].IsWalkable = false;
        results = AStar.CalculateSquareGridPath(grid, 12, 0, int.MaxValue);

        path = new List<int>();
        path = results.path;
        weight = results.pathWeight;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
