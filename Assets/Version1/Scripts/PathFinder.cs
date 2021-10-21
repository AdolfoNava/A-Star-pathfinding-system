using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
	//Seeker is the individual that seeks the target grid spot
	public Transform seeker, target;
	Grid grid;

	void Awake()
	{
		grid = GetComponent<Grid>();
	}

	void Update()
	{
		FindPath(seeker.position, target.position);
	}
	//The main function of the system that determines the best path to travel towards
	void FindPath(Vector3 startPos, Vector3 targetPos)
	{	//The position of the seeker
		Node startNode = grid.NodeFromWorldPoint(startPos);
		//The seeker is the destination gameobject in question in the scene
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		//Open set is for the nodes that have either yet to be considered the correct path to take or is in consideration
		List<Node> openSet = new List<Node>();
		//This is for grid spots that have have been determined to have an total node cost too significant to be considered the fastest node to travel enroute to the destination
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
				{
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{	//This will end the pathfinding of the game instance
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node))
			{
				//This will check if the path is available to be walked over
				if (!neighbour.Walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
				{
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.Parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}
	//This is for the visualization of the correct path
	void RetracePath(Node startNode, Node endNode)
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		path.Reverse();

		grid.Path = path;

	}
	//The value of the distance between two individual node points
	int GetDistance(Node nodeA, Node nodeB)
	{
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
