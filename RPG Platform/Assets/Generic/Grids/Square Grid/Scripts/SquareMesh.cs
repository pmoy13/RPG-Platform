/*
 * File:
 *   SquareMesh.cs
 * 
 * Description:
 *   TODO: Write description.
 */

using UnityEngine;
using System.Collections.Generic;

/*
 * Class:
 *   SquareMesh
 *   
 * Description:
 *   This class provides the mesh to render the Square
 *   grid, as well as methods to triangulate the cells.
 */
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SquareMesh : MonoBehaviour
{
    // The actual mesh that will be rendered.
    private Mesh _mesh;

    // Properties of the mesh to be rendered.
    private List<Vector3> _vertices;
    private List<int> _triangles;

    // We need to inherit the scale factor from the Square Grid.
    public SquareGrid SquareGrid;

    // Allows us to interact with the mesh via mouse.
    private MeshCollider _meshCollider;

    // Different colors that can be rendered.
    // TODO: Change these to textures!
    private List<Color> _colors;

    // TODO: Description.
    private void Awake()
    {
        // Get a reference to the Square Grid we need.
        SquareGrid = SquareGrid.GetComponent<SquareGrid>();

        // Get a reference to our mesh collider.
        _meshCollider = gameObject.AddComponent<MeshCollider>();

        // Allocate space for the mesh and its components.
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _mesh.name = "Square Mesh";
        _vertices = new List<Vector3>();
        _triangles = new List<int>();
        _colors = new List<Color>();
    }

    /*
     * Method:
     *   Triangulate
     *   
     * Description:
     *   Clears the old mesh data, then loops through
     *   each cell and triangulates the cell.
     */
    public void Triangulate(SquareCell[] cells)
    {
        // Clear the old data.
        _mesh.Clear();
        _vertices.Clear();
        _triangles.Clear();
        _colors.Clear();

        // Loop through and triangulate each cell.
        for (int index = 0; index < cells.Length; index++)
        {
            TriangulateCell(cells[index]);
        }

        // Assign the vertices, triangles and normals.
        // These have been set by calls to TriangulateCell.
        _mesh.vertices = _vertices.ToArray();
        _mesh.colors = _colors.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.RecalculateNormals();

        _meshCollider.sharedMesh = _mesh;
    }

    /*
     * Method:
     *   TriangulateCell
     *   
     * Description:
     *   Triangulates a single cell.
     */
    private void TriangulateCell(SquareCell squareCell)
    {
        // We will use the center of the square to calculate
        // each corner.
        Vector3 bottomLeft = squareCell.transform.localPosition;

        // Add two triangles, with the following vertices:
        //   1        4 * * 5
        //   * *        *   *
        //   *   *        * *
        //   0 * * 2        3
        // Add the first triangle (0 -> 1 -> 2)
        AddTriangle(
            bottomLeft,
            bottomLeft + Vector3.forward * SquareGrid.ScaleFactor,
            bottomLeft + Vector3.right * SquareGrid.ScaleFactor);
        AddTriangleColor(squareCell.Color);
        // Add the second triangle. (3 -> 4 -> 5)
        AddTriangle(
            bottomLeft + Vector3.right * SquareGrid.ScaleFactor,
            bottomLeft + Vector3.forward * SquareGrid.ScaleFactor,
            bottomLeft + (Vector3.forward + Vector3.right) * SquareGrid.ScaleFactor);
        AddTriangleColor(squareCell.Color);
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

    private void AddTriangleColor(Color color)
    {
        _colors.Add(color);
        _colors.Add(color);
        _colors.Add(color);
    }
}
