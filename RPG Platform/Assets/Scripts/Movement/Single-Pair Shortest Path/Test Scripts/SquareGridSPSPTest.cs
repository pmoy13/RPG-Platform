using UnityEngine;
using UnityEngine.EventSystems;

public class SquareGridSPSPTest : MonoBehaviour {

    public SquareGrid grid;
    public SquareMesh squareMesh;
    public CreatureSquareMove creature;

    // Use this for initialization
    void Start()
    {
        grid.Cells[0, 1].IsWalkable = false;
        grid.Cells[1, 1].IsWalkable = false;
        grid.Cells[2, 1].IsWalkable = false;
    }

    // TODO: Description.
    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Reset the grid to white so we can see the new path.
            TestResetGrid();

            // Draw the new path.
            TestHighlightPath();

            // Show our changes!
            squareMesh.Triangulate(grid.Cells);
        }
    }

    private void TestHighlightPath()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            creature.HighlightMove(grid.TouchCell(hit.point));
        }
    }

    private void TestResetGrid()
    {
        for (int width = 0; width < grid.Width; width++)
        {
            for (int height = 0; height < grid.Height; height++)
            {
                grid.Cells[width, height].Color = Color.white;
            }
        }
    }
}
