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

    public override int GetIndexFromCoordinates(int gridWidth)
    {
        return (Coordinates.X + gridWidth * Coordinates.Z);
    }

    public override BasicCell[] GetNeighbors()
    {
        return _neighbors;
    }
}
