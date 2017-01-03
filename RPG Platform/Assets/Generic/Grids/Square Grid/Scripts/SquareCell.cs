using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareCell : MonoBehaviour
{
    public SquareCoordinates Coordinates;

    public Color Color;

    [SerializeField] private SquareCell[] _neighbors;

    void Awake()
    {
        _neighbors = new SquareCell[8];
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
}
