using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCell : BasicCell
{
    public SquareCoordinates Coordinates;

    public Color Color;

    [SerializeField] private SquareCell[] _neighbors;

    void Awake()
    {
        _neighbors = new SquareCell[8];

        // Temp... maybe this shouldn't be here.
        IsWalkable = true;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public SquareCell GetNeighbor(SquareDirection direction)
    {
        return _neighbors[(int)direction];
    }

    public void SetNeighbor(SquareDirection direction, SquareCell cell)
    {
        _neighbors[(int)direction] = cell;
    }

    public override int X()
    {
        return Coordinates.X;
    }

    public override int Z()
    {
        return Coordinates.Z;
    }

    public override int GetIndexFromCoordinates(int gridWidth)
    {
        return (Coordinates.X + gridWidth * Coordinates.Z);
    }

    public override BasicCell[] GetNeighbors()
    {
        return _neighbors;
    }

    public override void Highlight(int[,] distances, float scale, int moveSpeed)
    {
        // If this cell isn't reachable, don't draw anything.
        if (distances[X(), Z()] > moveSpeed)
        {
            return;
        }

        // Since this cell is reachable, highlight the cell to demonstrate.
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        List<Vector3> positions = new List<Vector3>();

        // Every move adds a single vertex, but we need to have a starting
        // vertex to tie the others together. This variable lets us know whether
        // or not we need to add this first vertex.
        bool moveFound = false;

        /*
         * Check each of the neighbors in a cardinal direction.
         * If they're in range, don't draw a line along that border.
         * Else, draw the line. If the neighbor doesn't exist, draw
         * the border as well.
         */

        // First, find the indices for the LineRenderer.

        // Check the north neighbor.
        if ((Z() == distances.GetLength(1) - 1) || (distances[X(), Z() + 1] > moveSpeed))
        {
            if (!moveFound)
            {
                // Add the first vertex (top right).
                positions.Add(transform.position + (Vector3.forward + Vector3.right) * scale);

                // Now we have a first vertex.
                moveFound = true;
            }

            // Add the top left index.
            positions.Add(transform.position + Vector3.forward * scale);
        }
        // Check the east neighbor.
        if ((X() == 0) || (distances[X() - 1, Z()] > moveSpeed))
        {
            if (!moveFound)
            {
                // Add the first vertex (top left).
                positions.Add(transform.position + Vector3.forward * scale);

                // Now we have a first vertex.
                moveFound = true;
            }

            // Add the bottom left index.
            positions.Add(transform.position);
        }
        // Check the south neighbor.
        if ((Z() == 0) || (distances[X(), Z() - 1] > moveSpeed))
        {
            if (!moveFound)
            {
                // Add the first vertex (bottom left).
                positions.Add(transform.position);

                // Now we have a first vertex.
                moveFound = true;
            }

            // Add the bottom right index.
            positions.Add(transform.position + Vector3.right * scale);
        }
        // Check the west neighbor.
        if ((X() == distances.GetLength(0) - 1) || (distances[X() + 1, Z()] > moveSpeed))
        {
            if (!moveFound)
            {
                // Add the first vertex (bottom right).
                positions.Add(transform.position + Vector3.right * scale);
            }

            // Add the top right index.
            positions.Add(transform.position + (Vector3.forward + Vector3.right) * scale);
        }

        // Next, assign the vertices to the LineRenderer.
        lineRenderer.numPositions = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
