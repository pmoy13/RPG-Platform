using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SquareMoveRenderer : MonoBehaviour
{
    // The grid that we will draw on.
    public SquareGrid grid;

    // The LineRenderer to display the drawing.
    private LineRenderer _lineRenderer;

    // TODO: See if there's a more elegant way to pass in the test function.
    public DD5eSystem system;
    public Creature Creature;

	// Use this for initialization
	void Start ()
	{
	    system = new DD5eSystem();
	    _lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void DrawPossibleMoves(Creature creature)
    {
        // Calculate the distance to each cell of the grid.
        int[,] distances = SquareMovement.CalculateDistances(grid,
            creature, system.CalculateSquareGridMovementCost);


        // Find the first cell that the creature can reach.
        Pair<int, int> firstCoords = GetFirstLegalCell(distances, creature.MoveSpeed);
        int width = firstCoords.First;
        int height = firstCoords.Second;
        SquareDirection direction = SquareDirection.Invalid;

        if (firstCoords.First == -1 && firstCoords.Second == -1)
        {
            // There were no reachable cells to draw.
            return;
        }

        // Allocate space for the vertices of the LineRenderer.
        List<Vector3> positions = new List<Vector3>();

        // Draw the first two vertices that we know exist.
        BasicCell firstCell = grid.GetBasicCell(width, height);
        positions.Add(firstCell.transform.position);
        positions.Add(firstCell.transform.position + grid.ScaleFactor * Vector3.right);

        // Add vertices until we get to another cell.

        // Check the east neighbor.
        if ((width != distances.GetLength(0) - 1) &&
            (distances[width + 1, height] <= creature.MoveSpeed))
        {
            // East neighbor is reachable. Don't add any more vertices.
            width += 1;
            direction = SquareDirection.E;
        }
        // Check the north neighbor.
        else if ((height != distances.GetLength(1) - 1) &&
                 (distances[width, height + 1] <= creature.MoveSpeed))
        {
            // Draw the vertex corresponding to the east neighbor, then
            // stop.
            positions.Add(firstCell.transform.position +
                grid.ScaleFactor * (Vector3.right + Vector3.forward));
            height += 1;
            direction = SquareDirection.N;
        }
        // Check the west neighbor.
        else if ((width != 0) &&
                 (distances[width - 1, height] <= creature.MoveSpeed))
        {
            // Draw the vertex corresponding to the east neighbor and
            // north neighbors, then stop.
            positions.Add(firstCell.transform.position +
                grid.ScaleFactor * (Vector3.right + Vector3.forward));
            positions.Add(firstCell.transform.position +
                grid.ScaleFactor * Vector3.forward);
            width -= 1;
            direction = SquareDirection.W;
        }

        // Loop through the rest of the cells until we come back to the start.
        while ((width != firstCoords.First) || (height != firstCoords.Second))
        {
            BasicCell currCell = grid.GetBasicCell(width, height);

            switch (direction)
            {
                case SquareDirection.N:
                {
                    // Check the east neighbor.
                    if ((width != distances.GetLength(0) - 1) &&
                            (distances[width + 1, height] <= creature.MoveSpeed))
                    {
                        width += 1;
                        direction = SquareDirection.E;
                        continue;
                    }
                    // Add the east vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * (Vector3.right + Vector3.forward));

                    // Check the north neighbor.
                    if ((height != distances.GetLength(1) - 1) &&
                            (distances[width, height + 1] <= creature.MoveSpeed))
                    {
                        height += 1;
                        continue;
                    }
                    // Add the north vertex.
                    positions.Add(currCell.transform.position + 
                        grid.ScaleFactor * Vector3.forward);

                    // Check the west neighbor.
                    if ((width != 0) &&
                            (distances[width - 1, height] <= creature.MoveSpeed))
                    {
                        width -= 1;
                        direction = SquareDirection.W;
                        continue;
                    }
                    // Add the west vertex.
                    positions.Add(currCell.transform.position);

                    // The south neighbor must be reachable.
                    height -= 1;
                    direction = SquareDirection.S;
                    break;
                }
                case SquareDirection.E:
                {
                    // Check the south neighbor.
                    if ((height != 0) &&
                            (distances[width, height - 1] <= creature.MoveSpeed))
                    {
                        height -= 1;
                        direction = SquareDirection.S;
                        continue;
                    }
                    // Add the south vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * Vector3.right);

                    // Check the east neighbor.
                    if ((width != distances.GetLength(0) - 1) &&
                            (distances[width + 1, height] <= creature.MoveSpeed))
                    {
                        width += 1;
                        continue;
                    }
                    // Add the east vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * (Vector3.right + Vector3.forward));

                    // Check the north neighbor.
                    if ((height != distances.GetLength(1) - 1) &&
                            (distances[width, height + 1] <= creature.MoveSpeed))
                    {
                        height += 1;
                        direction = SquareDirection.N;
                        continue;
                    }
                    // Add the north vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * Vector3.forward);

                    // The west neighbor must be reachable.
                    width -= 1;
                    direction = SquareDirection.W;
                    break;
                }
                case SquareDirection.S:
                {
                    // Check the west neighbor.
                    if ((width != 0) &&
                            (distances[width - 1, height] <= creature.MoveSpeed))
                    {
                        width -= 1;
                        direction = SquareDirection.W;
                        continue;
                    }
                    // Add the west vertex.
                    positions.Add(currCell.transform.position);

                    // Check the south neighbor.
                    if ((height != 0) &&
                            (distances[width, height - 1] <= creature.MoveSpeed))
                    {
                        height -= 1;
                        continue;
                    }
                    // Add the south vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * Vector3.right);

                    // Check the east neighbor.
                    if ((width != distances.GetLength(0) - 1) &&
                            (distances[width + 1, height] <= creature.MoveSpeed))
                    {
                        width += 1;
                        direction = SquareDirection.E;
                        continue;
                    }
                    // Add the east vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * (Vector3.right + Vector3.forward));

                    // The north neighbor must be reachable.
                    height += 1;
                    direction = SquareDirection.N;
                    break;
                }
                case SquareDirection.W:
                {
                    // Check the north neighbor.
                    if ((height != distances.GetLength(1) - 1) &&
                            (distances[width, height + 1] <= creature.MoveSpeed))
                    {
                        height += 1;
                        direction = SquareDirection.N;
                        continue;
                    }
                    // Add the north vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * Vector3.forward);

                    // Check the west neighbor.
                    if ((width != 0) &&
                            (distances[width - 1, height] <= creature.MoveSpeed))
                    {
                        width -= 1;
                        direction = SquareDirection.W;
                        continue;
                    }
                    // Add the west vertex.
                    positions.Add(currCell.transform.position);

                    // Check the south neighbor.
                    if ((height != 0) &&
                            (distances[width, height - 1] <= creature.MoveSpeed))
                    {
                        height -= 1;
                        direction = SquareDirection.S;
                        continue;
                    }
                    // Add the south vertex.
                    positions.Add(currCell.transform.position +
                        grid.ScaleFactor * Vector3.right);

                    // The east neighbor must be reachable.
                    width += 1;
                    direction = SquareDirection.E;
                    break;
                }
            }
        }

        // Add the first vertex to the end of the list to complete the path.
        positions.Add(firstCell.transform.position);

        // Assign the values back to the LineRenderer.
        // Next, assign the vertices to the LineRenderer.
        _lineRenderer.numPositions = positions.Count;
        _lineRenderer.SetPositions(positions.ToArray());
    }

    private Pair<int, int> GetFirstLegalCell(int[,] distances, int speed)
    {
        for (int height = 0; height < distances.GetLength(1); height++)
        {
            for (int width = 0; width < distances.GetLength(0); width++)
            {
                if (distances[width, height] <= speed)
                {
                    return new Pair<int, int>(width, height);
                }
            }
        }

        // No cells were reachable.
        return new Pair<int, int>(-1, -1);
    }

    // TODO: Remove this test function.
    public void TestDraw()
    {
        DrawPossibleMoves(Creature);
    }
}
