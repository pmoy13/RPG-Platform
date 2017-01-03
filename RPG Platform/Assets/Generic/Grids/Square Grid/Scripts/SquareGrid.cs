using UnityEngine;

public class SquareGrid : MonoBehaviour
{
    // Dimensions of the grid.
    public int Width = 6;
    public int Height = 6;

    // The scale factor allows us to dynamically
    // adjust the size of the grid.
    public float ScaleFactor = 10f;

    // Prefab of the square cells used to
    // create the grid.
    public SquareCell CellPrefab;

    // References to the SquareCell objects that make
    // up the grid.
    private SquareCell[] _cells;

    // Reference to the mesh used to render the grid.
    private SquareMesh _squareMesh;

    // Colors to let us know if we've touched a cell.
    public Color DefaultColor = Color.white;

    public SquareCell[] Cells
    {
        get { return _cells; }
    }

    /*
     * Method:
     *   Awake
     *   
     * Description:
     *   Instantiates SquareCell prefabs which are
     *   stored in the cells array.
     */

    private void Awake()
    {
        // Get the Square Mesh from a child component.
        _squareMesh = GetComponentInChildren<SquareMesh>();

        // Allocate space for each SquareCell.
        _cells = new SquareCell[Height * Width];

        // Instantiate the SquareCell objects.
        int index = 0; // Used to index into the cells array.
        for (int height = 0; height < Height; height++)
        {
            for (int width = 0; width < Width; width++)
            {
                CreateCell(width, height, index++);
            }
        }

        // Assign the neighbors for each cell.
        SetNeighbors();
    }

    /*
     * Method:
     *   Start
     *   
     * Description:
     *   TODO: Description.
     */
    private void Start()
    {
        _squareMesh.Triangulate(_cells);
    }

    /*
     * Method:
     *   CreateCell
     *   
     * Description:
     *   Instantiates a single SquareCell prefab at
     *   the input location, then adds the prefab to
     *   the cells array.
     */
    private void CreateCell(int x, int z, int index)
    {
        // Determine the coordinates of the new SquareCell.
        Vector3 position;
        position.x = ScaleFactor * x;
        position.y = 0f;
        position.z = ScaleFactor * z;

        // Create (instantiate) the new SquareCell.
        SquareCell newSquareCell = Instantiate<SquareCell>(CellPrefab);
        _cells[index] = newSquareCell;
        newSquareCell.transform.SetParent(transform, false);
        newSquareCell.transform.localPosition = position;
        newSquareCell.Color = DefaultColor;

        // Save the coordinates of the new SquareCell.
        newSquareCell.Coordinates = new SquareCoordinates(x, z);
    }

    private void SetNeighbors()
    {
        int numCells = _cells.Length;
        for (int index = 0; index < numCells; index++)
        {
            if (index == 0)
            {
                // Bottom left corner.
                // Assign N, NE, E
                _cells[index].SetNeighbor(SquareDirection.N, _cells[index + Width]);
                _cells[index].SetNeighbor(SquareDirection.NE, _cells[index + Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.E, _cells[index + 1]);
            }
            else if (index == (Width - 1))
            {
                // Bottom right corner.
                // Assign W, NW, N
                _cells[index].SetNeighbor(SquareDirection.W, _cells[index - 1]);
                _cells[index].SetNeighbor(SquareDirection.NW, _cells[index + Width - 1]);
                _cells[index].SetNeighbor(SquareDirection.N, _cells[index + Width]);
            }
            else if (index == (numCells - Width))
            {
                // Top left corner.
                // Assign E, SE, S
                _cells[index].SetNeighbor(SquareDirection.E, _cells[index + 1]);
                _cells[index].SetNeighbor(SquareDirection.SE, _cells[index - Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.S, _cells[index - Width]);
            }
            else if (index == (numCells - 1))
            {
                // Top right corner.
                // Assign W, SW, S
                _cells[index].SetNeighbor(SquareDirection.W, _cells[index - 1]);
                _cells[index].SetNeighbor(SquareDirection.SW, _cells[index - Width - 1]);
                _cells[index].SetNeighbor(SquareDirection.S, _cells[index - Width]);
            }
            else if ((index / Width) == 0)
            {
                // Bottom row (not a corner).
                // Assign W, NW, N, NE, E
                _cells[index].SetNeighbor(SquareDirection.W, _cells[index - 1]);
                _cells[index].SetNeighbor(SquareDirection.NW, _cells[index + Width - 1]);
                _cells[index].SetNeighbor(SquareDirection.N, _cells[index + Width]);
                _cells[index].SetNeighbor(SquareDirection.NE, _cells[index + Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.E, _cells[index + 1]);
            }
            else if (index % Width == 0)
            {
                // Left column (not a corner).
                // Assign N, NE, E, SE, S
                _cells[index].SetNeighbor(SquareDirection.N, _cells[index + Width]);
                _cells[index].SetNeighbor(SquareDirection.NE, _cells[index + Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.E, _cells[index + 1]);
                _cells[index].SetNeighbor(SquareDirection.SE, _cells[index - Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.S, _cells[index - Width]);
            }
            else if ((numCells - index) < Width)
            {
                // Bottom row (not a corner).
                // Assign W, SW, S, SE, E
                _cells[index].SetNeighbor(SquareDirection.W, _cells[index - 1]);
                _cells[index].SetNeighbor(SquareDirection.SW, _cells[index - Width - 1]);
                _cells[index].SetNeighbor(SquareDirection.S, _cells[index - Width]);
                _cells[index].SetNeighbor(SquareDirection.SE, _cells[index - Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.E, _cells[index + 1]);
            }
            else if ((index % Width) == (Width - 1))
            {
                // Right column (not a corner).
                // Assign N, NW, W, SW, S
                _cells[index].SetNeighbor(SquareDirection.N, _cells[index + Width]);
                _cells[index].SetNeighbor(SquareDirection.NW, _cells[index + Width - 1]);
                _cells[index].SetNeighbor(SquareDirection.W, _cells[index - 1]);
                _cells[index].SetNeighbor(SquareDirection.SW, _cells[index - Width - 1]);
                _cells[index].SetNeighbor(SquareDirection.S, _cells[index - Width]);
            }
            else
            {
                // Middle cell.
                // Assign all neighbors.
                _cells[index].SetNeighbor(SquareDirection.N, _cells[index + Width]);
                _cells[index].SetNeighbor(SquareDirection.NE, _cells[index + Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.E, _cells[index + 1]);
                _cells[index].SetNeighbor(SquareDirection.SE, _cells[index - Width + 1]);
                _cells[index].SetNeighbor(SquareDirection.S, _cells[index - Width]);
                _cells[index].SetNeighbor(SquareDirection.SW, _cells[index - Width - 1]);
                _cells[index].SetNeighbor(SquareDirection.W, _cells[index - 1]);
                _cells[index].SetNeighbor(SquareDirection.NW, _cells[index + Width - 1]);
            }
        }
    }

    public void TouchCell(Vector3 position, Color color)
    {
        // Determine which cell was touched.
        position = transform.InverseTransformPoint(position);
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position, ScaleFactor);
        int index = coordinates.X + coordinates.Z * Width;
        SquareCell cell = _cells[index];

        // Edit the cell's color and triangulate again.
        cell.Color = color;
        // TODO: See if we can optimize and only triangule the changed cells.
        _squareMesh.Triangulate(_cells);
    }
}
