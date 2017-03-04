/*
 * File:
 *   HexGrid.cs
 * 
 * Description:
 *   This file contains a class used to instantiate a
 *   hexagonal grid object.
 */

using UnityEngine;

public class HexGrid : BasicGrid
{
    // Prefab of the hexagon cells used
    // to create the grid.
    public HexCell CellPrefab;

    // References to the HexCell objects that make up
    // the grid.
    private HexCell[] _cells;
    
    // Reference to the mesh used to render the grid.
    private HexMesh _hexMesh;

    public override int GetMaxNeighbors()
    {
        // Each hex cell can have up to six neighbors.
        return HexMetrics.NeighborsPerHexagon;
    }

    public override BasicCell GetBasicCell(int index)
    {
        return _cells[index];
    }

    public override BasicCell GetBasicCell(int width, int height)
    {
        return _cells[width + height * Width];
    }

    /*
     * Method:
     *   Awake
     *   
     * Description:
     *   Instantiates HexCell prefabs which are
     *   stored in the cells array.
     */
    private void Awake()
    {
        // Get the Hex Mesh from a child component.
        _hexMesh = GetComponentInChildren<HexMesh>();

        // Allocate space for each HexCell.
        _cells = new HexCell[Height * Width];

        // Instantiate the HexCell objects.
        int index = 0; // Used to index into the cells array.
        for (int height = 0; height < Height; height++)
        {
            for (int width = 0; width < Width; width++)
            {
                CreateCell(width, height, index++);
            }
        }
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
        _hexMesh.Triangulate(_cells);
    }

    /*
     * Method:
     *   CreateCell
     *   
     * Description:
     *   Instantiates a single HexCell prefab at
     *   the input location, then adds the prefab to
     *   the cells array.
     */
    private void CreateCell(int x, int z, int index)
    {
        // Determine the coordinates of the new HexCell.
        Vector3 position;
        // In the same row, adjacent hexagons are not at the same
        // height. They alternate lower and higher, so we must account
        // for that.
        int verticalRowOffset = x / 2;
        position.x = ScaleFactor * (x * (HexMetrics.OuterUnitRadius * 1.5f));
        position.y = 0f;
        position.z = ScaleFactor * ((z + x * 0.5f - verticalRowOffset)
                       * (HexMetrics.InnerUnitRadius * 2f));

        // Create (instantiate) the new HexCell.
        HexCell newHexCell = Instantiate<HexCell>(CellPrefab);
        _cells[index] = newHexCell;
        newHexCell.transform.SetParent(transform, false);
        newHexCell.transform.localPosition = position;

        // Save the coordinates of the new HexCell.
        newHexCell.Coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
    }
}
