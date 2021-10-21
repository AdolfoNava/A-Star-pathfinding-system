using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //The special layermask is for whatever gameobjects would be considered a barrier for movement
    public LayerMask unwalkableLayerMask;
    //The size of the total grid size
    public Vector2 gridWorldSize;
    //The size of the individual node that will be consistent throughout the entire grid of the instance of the game environment
    public float nodeRadius;
    //The nodes will be setup as a grid for the size of the nodes 
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    //A collection of nodes that is the path of the two points of interest
    public List<Node> Path;
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSizeX / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSizeY / nodeDiameter);
        CreateGrid();
    }
    //Forms a grid system based on the max values of the grid size and length of the individual grid size
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        for(int x = 0; x<gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +Vector3.forward*(y*nodeDiameter+nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius,unwalkableLayerMask));
                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }
    //Determines the neighboring squares of the called node in question
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
    //WARNING THIS ONE IN PARTICULAR DOESN'T FUNCTION PROPERLY
    //Determines the node location in the scene
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    
    //Visualizes the grid in the debugger
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

        if (grid != null) { 
            foreach(Node n in grid)
            {
                Gizmos.color = (n.Walkable) ? Color.white : Color.red;
                if (Path != null)
                    if (Path.Contains(n))
                        Gizmos.color = Color.black;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
