using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridV2 : MonoBehaviour
{
    //This is where the program will start the pathfinding from.
    public Transform StartPosition;
    //This is the mask that the program will look for when trying to find obstructions to the path.
    public LayerMask WallMask;
    //A vector2 to store the width and height of the graph in world units
    //It's completely possible for a vector3 for grid size because y isn't used in this demo which could be 3d A* pathfinding
    public Vector2 vGridWorldSize;
    //This stores how big each square on the graph will be
    public float fNodeRadius;
    //The distance that the squares will spawn from eachother.
    public float fDistanceBetweenNodes;

    NodeV2[,] NodeArray;//The array of nodes that the A* algorithm uses.
    public List<NodeV2> FinalPath;//The completed path that the red line will be drawn along


    float fNodeDiameter;//Twice the amount of the radius (Set in the start function)
    int GridSizeX, GridSizeY;//Size of the Grid in Array units.


    private void Start()
    {
        //Double the radius to get diameter
        fNodeDiameter = fNodeRadius * 2;

        //Divide the grids world co-ordinates by the diameter to get the size of the graph in array units.
        GridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);
        GridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);
        CreateGrid();//Draw the grid
    }

    void CreateGrid()
    {
        NodeArray = new NodeV2[GridSizeX, GridSizeY];//Declare the array of nodes.
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;//Get the real world position of the bottom left of the grid.
        //This is a nested loop because of the multi array additional loops may be added for the extra dimension
        for (int x = 0; x < GridSizeX; x++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);//Get the world co ordinates of the bottom left of the graph
                bool Wall = true;//Make the node a wall

                //If the node is not being obstructed
                //Quick collision check against the current node and anything in the world at its position. If it is colliding with an object with a WallMask,
                //The if statement will return false.
                if (Physics.CheckSphere(worldPoint, fNodeRadius, WallMask))
                {
                    Wall = false;
                }
                //Create a new node in the array
                NodeArray[x, y] = new NodeV2(Wall, worldPoint, x, y);
            }
        }
    }

    //Function that gets the neighboring nodes of the given node
    public List<NodeV2> GetNeighboringNodes(NodeV2 a_NeighborNode)
    {
        //Make a new list of all available neighbors
        //that will be returned by the end of the method
        List<NodeV2> NeighborList = new List<NodeV2>();
        //Variable to check if the X and Y Position is within range of the node array to avoid out of range errors.
        int checkX;
        int checkY;

        //Check the right side of the current node.
        checkX = a_NeighborNode.GridX + 1;
        checkY = a_NeighborNode.GridY;
        if (checkX >= 0 && checkX < GridSizeX)//If the X Position is in range of the array
        {
            if (checkY >= 0 && checkY < GridSizeY)//If the Y Position is in range of the array
            {
                NeighborList.Add(NodeArray[checkX, checkY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        checkX = a_NeighborNode.GridX - 1;
        checkY = a_NeighborNode.GridY;
        if (checkX >= 0 && checkX < GridSizeX)
        {
            if (checkY >= 0 && checkY < GridSizeY)
            {
                NeighborList.Add(NodeArray[checkX, checkY]);
            }
        }
        //Check the Top side of the current node.
        checkX = a_NeighborNode.GridX;
        checkY = a_NeighborNode.GridY + 1;
        if (checkX >= 0 && checkX < GridSizeX)
        {
            if (checkY >= 0 && checkY < GridSizeY)
            {
                NeighborList.Add(NodeArray[checkX, checkY]);
            }
        }
        //Check the Bottom side of the current node.
        checkX = a_NeighborNode.GridX;
        checkY = a_NeighborNode.GridY - 1;
        if (checkX >= 0 && checkX < GridSizeX)
        {
            if (checkY >= 0 && checkY < GridSizeY)
            {
                NeighborList.Add(NodeArray[checkX, checkY]);
            }
        }

        return NeighborList;
    }

    //Gets the closest node to the given world position.
    public NodeV2 NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = ((a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x);
        float iyPos = ((a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y);

        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);

        int ix = Mathf.RoundToInt((GridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((GridSizeY - 1) * iyPos);

        return NodeArray[ix, iy];
    }


    //Function that draws the wireframe in the debugger
    private void OnDrawGizmos()
    {

        Gizmos.DrawWireCube(transform.position, new Vector3(vGridWorldSize.x, 1, vGridWorldSize.y));//Draw a wire cube with the given dimensions from the Unity inspector

        if (NodeArray != null)//checks for the grid instance
        {
            foreach (NodeV2 n in NodeArray)//Loop through every node in the grid
            {
                if (n.IsAWall)//If the current node is a wall node
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }


                if (FinalPath != null)
                {
                    if (FinalPath.Contains(n))//If the current node is in the final path
                    {
                        Gizmos.color = Color.red;
                    }

                }

                //Draw the node at the position of the node
                Gizmos.DrawCube(n.NodePosition, Vector3.one * (fNodeDiameter - fDistanceBetweenNodes));
            }
        }
    }
}
