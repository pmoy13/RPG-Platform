using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    // How far the creature can move in a single action.
    public int MoveSpeed;

    // The index of the cell that the creature curently
    // occupies. In the case of a creature occupying
    // multiple squares, this represents the bottom
    // left square occupied by the creature.
    public int Position;

    // The size of the creature, measured in squares.
    // For example, a creature occupying a 2x2 section
    // of four squares will have SquareSize = 2.
    public int SquareSize;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
