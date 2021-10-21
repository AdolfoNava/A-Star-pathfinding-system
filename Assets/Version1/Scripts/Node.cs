using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is for an individual grid spot that handles the value of the location in regards to the destination
public class Node 
{
	//Seperates from what is available for the capsule to
	//walk through in the case of the demo any red cube is unavailable
    public bool Walkable;

	//This is for the individual grid position that is relative to the scene instance
    public Vector3 worldPosition;
	public int gridX;
	public int gridY;
	//These costs are for the inherit value of the individual node it has for the shortest possible distance between the desired destination and moveable object in question
	public int gCost;
	public int hCost;

	public Node Parent;

	//The constructor that handles the main factors of the individula node
	public Node(bool walkable, Vector3 worldPos, int gridx, int gridy)
	{
		Walkable = walkable;
		worldPosition = worldPos;
		gridX = gridx;
		gridY = gridy;
	}
	//This is for the total value of the individual spot from the spot's diagonal and directional cost of traveling to a individual grid spot 
	public int fCost
	{
		get
		{
			return gCost + hCost;
		}
	}
}
