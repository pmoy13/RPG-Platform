/*
 * File:
 *   HexMesh.cs
 * 
 * Description:
 *   TODO: Write description.
 */

using UnityEngine;
using System.Collections.Generic;

/*
 * Class:
 *   HexMesh
 *   
 * Description:
 *   This class provides the mesh to render the hexagonal
 *   grid, as well as methods to triangulate the cells.
 */
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    // The actual mesh that will be rendered.
    private Mesh _mesh;

    // Properties of the mesh to be rendered.
    private List<Vector3> _vertices;
    private List<int> _triangles;

    // We need to inherit the scale factor from the Hex Grid.
    public HexGrid HexGrid;

    // TODO: Description.
    private void Awake()
    {
        // Get a reference to the Hex Grid we need.
        HexGrid = HexGrid.GetComponent<HexGrid>();

        // Allocate space for the mesh and its components.
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _mesh.name = "Hex Mesh";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
    }

    /*
     * Method:
     *   Triangulate
     *   
     * Description:
     *   Clears the old mesh data, then loops through
     *   each cell and triangulates the cell.
     */
    public void Triangulate(HexCell[] cells)
    {
        // Clear the old data.
        _mesh.Clear();
        _vertices.Clear();
        _triangles.Clear();

        // Loop through and triangulate each cell.
        for (int index = 0; index < cells.Length; index++)
        {
            TriangulateCell(cells[index]);
        }

        // Assign the vertices, triangles and normals.
        // These have been set by calls to TriangulateCell.
        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();
    }

    /*
     * Method:
     *   TriangulateCell
     *   
     * Description:
     *   Triangulates a single cell.
     */
    private void TriangulateCell(HexCell hexCell)
    {
        // Each triangle's first vertex is the center of the
        // hexagon.
        Vector3 center = hexCell.transform.localPosition;

        // Add six triangles, calculating the other two vertices
        // of each triangle from the center of the hexagon.
        for (int numTris = 0; numTris < HexMetrics.TrianglesPerHexagon; numTris++)
        {
            AddTriangle(
                center,
                center + HexMetrics.CornerVector3S[numTris] * HexGrid.ScaleFactor,
                center + HexMetrics.CornerVector3S[numTris + 1] * HexGrid.ScaleFactor);
        }
    }

    /*
     * Method:
     *   AddTriangle
     *   
     * Description:
     *   Adds a single triangle to the mesh.
     */
    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // The index of the first vertex is the length of
        // the vertex list before adding any new vertices.
        int vertexIndex = _vertices.Count;

        // Add the three vectors to the vertices.
        _vertices.Add(v1);
        _vertices.Add(v2);
        _vertices.Add(v3);

        // Add the triangle formed by the three vertices.
        _triangles.Add(vertexIndex);
        _triangles.Add(vertexIndex + 1);
        _triangles.Add(vertexIndex + 2);
    }
}
