using UnityEngine;
using System.Collections;

public class HexCell : BasicCell
{
    // Cube coordinates (X, Y, Z)
    public HexCoordinates Coordinates;

    public HexCell[] _neighbors;

	// Use this for initialization
	private void Start () {
	
	}
	
	// Update is called once per frame
	private void Update () {
	
	}

    // Getters for the coordinates of the cell.
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
        return Coordinates.X + gridWidth * Coordinates.Z;
    }

    public override BasicCell[] GetNeighbors()
    {
        return _neighbors;
    }

    public override void Highlight(Color color)
    {
        throw new System.NotImplementedException();
    }
}
