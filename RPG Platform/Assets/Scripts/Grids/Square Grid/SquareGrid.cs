using UnityEngine;

public class SquareGrid : BasicGrid
{
    // Prefab of the square cells used to
    // create the grid.
    public SquareCell CellPrefab;

    // References to the SquareCell objects that make
    // up the grid.
    private SquareCell[,] _cells;

    // Reference to the mesh used to render the grid.
    private SquareMesh _squareMesh;

    // Colors to let us know if we've touched a cell.
    public Color DefaultColor = Color.white;

    public SquareCell[,] Cells
    {
        get { return _cells; }
    }

    public override int GetMaxNeighbors()
    {
        // Each square cell can have up to eight neighbors.
        return 8;
    }

    public override BasicCell GetBasicCell(int index)
    {
        return _cells[index % Width, index / Width];
    }

    public override BasicCell GetBasicCell(int width, int height)
    {
        return _cells[width, height];
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
        _cells = new SquareCell[Height, Width];

        // Instantiate the SquareCell objects.
        for (int height = 0; height < Height; height++)
        {
            for (int width = 0; width < Width; width++)
            {
                CreateCell(width, height);
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
    private void CreateCell(int x, int z)
    {
        // Determine the coordinates of the new SquareCell.
        Vector3 position;
        position.x = ScaleFactor * x;
        position.y = 0f;
        position.z = ScaleFactor * z;

        // Create (instantiate) the new SquareCell.
        SquareCell newSquareCell = Instantiate<SquareCell>(CellPrefab);
        _cells[x, z] = newSquareCell;
        newSquareCell.transform.SetParent(transform, false);
        newSquareCell.transform.localPosition = position;
        newSquareCell.Color = DefaultColor;

        // Save the coordinates of the new SquareCell.
        newSquareCell.Coordinates = new SquareCoordinates(x, z);
    }

    private void SetNeighbors()
    {
        for (int width = 0; width < _cells.GetLength(0); width++)
        {
            for (int height = 0; height < _cells.GetLength(1); height++)
            {
                if (width == 0 && height == 0)
                {
                    // Bottom left corner.
                    // Assign N, NE, E
                    _cells[width, height].SetNeighbor(SquareDirection.N, _cells[width, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.NE, _cells[width + 1, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.E, _cells[width + 1, height]);
                }
                else if (width == (Width - 1) && height == 0)
                {
                    // Bottom right corner.
                    // Assign W, NW, N
                    _cells[width, height].SetNeighbor(SquareDirection.W, _cells[width - 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.NW, _cells[width - 1, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.N, _cells[width, height + 1]);
                }
                else if (width == 0 && height == (Height - 1))
                {
                    // Top left corner.
                    // Assign E, SE, S
                    _cells[width, height].SetNeighbor(SquareDirection.E, _cells[width + 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.SE, _cells[width + 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.S, _cells[width, height - 1]);
                }
                else if (width == (Width - 1) && height == (Height - 1))
                {
                    // Top right corner.
                    // Assign W, SW, S
                    _cells[width, height].SetNeighbor(SquareDirection.W, _cells[width - 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.SW, _cells[width - 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.S, _cells[width, height - 1]);
                }
                else if (height == 0)
                {
                    // Bottom row (not a corner).
                    // Assign W, NW, N, NE, E
                    _cells[width, height].SetNeighbor(SquareDirection.W, _cells[width - 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.NW, _cells[width - 1, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.N, _cells[width, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.NE, _cells[width + 1, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.E, _cells[width + 1, height]);
                }
                else if (width == 0)
                {
                    // Left column (not a corner).
                    // Assign N, NE, E, SE, S
                    _cells[width, height].SetNeighbor(SquareDirection.N, _cells[width, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.NE, _cells[width + 1, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.E, _cells[width + 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.SE, _cells[width + 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.S, _cells[width, height - 1]);
                }
                else if (height == (Height - 1))
                {
                    // Top row (not a corner).
                    // Assign W, SW, S, SE, E
                    _cells[width, height].SetNeighbor(SquareDirection.W, _cells[width - 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.SW, _cells[width - 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.S, _cells[width, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.SE, _cells[width + 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.E, _cells[width + 1, height]);
                }
                else if (width == (Width - 1))
                {
                    // Right column (not a corner).
                    // Assign N, NW, W, SW, S
                    _cells[width, height].SetNeighbor(SquareDirection.N, _cells[width, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.NW, _cells[width - 1, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.W, _cells[width - 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.SW, _cells[width - 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.S, _cells[width, height - 1]);
                }
                else
                {
                    // Middle cell.
                    // Assign all neighbors.
                    _cells[width, height].SetNeighbor(SquareDirection.N, _cells[width, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.NE, _cells[width + 1, height + 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.E, _cells[width + 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.SE, _cells[width + 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.S, _cells[width, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.SW, _cells[width - 1, height - 1]);
                    _cells[width, height].SetNeighbor(SquareDirection.W, _cells[width - 1, height]);
                    _cells[width, height].SetNeighbor(SquareDirection.NW, _cells[width - 1, height + 1]);
                }
            }
        }
    }

    public int TouchCell(Vector3 position)
    {
        // Determine which cell was touched.
        position = transform.InverseTransformPoint(position);
        SquareCoordinates coordinates = SquareCoordinates.FromPosition(position, ScaleFactor);
        
        // Return the coordinates of the touched cell.
        return (coordinates.X + Width * coordinates.Z);
    }
}
