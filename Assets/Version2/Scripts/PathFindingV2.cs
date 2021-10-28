using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingV2 : MonoBehaviour
{
    GridV2 Grid;//For referencing the grid class
    public Transform StartPosition;
    public Transform TargetPosition;

    private void Awake()
    {
        Grid = GetComponent<GridV2>();
    }

    private void FixedUpdate()
    {
        FindPath(StartPosition.position, TargetPosition.position);
    }
    //Find the cheapest fcost between two different node objects on a grid
    void FindPath(Vector3 StartPos, Vector3 TargetPos)
    {   //Gets the node closest to the target and starting position
        NodeV2 StartNode = Grid.NodeFromWorldPoint(StartPos);
        NodeV2 TargetNode = Grid.NodeFromWorldPoint(TargetPos);
        //List of nodes for the open and closed list
        List<NodeV2> OpenList = new List<NodeV2>();
        //closed list is for any node that isn't being looked at because it costs more to travel for the pathing
        HashSet<NodeV2> ClosedList = new HashSet<NodeV2>();
        
        //Add the starting node to the open list to begin the program
        OpenList.Add(StartNode);
        //When there is something in the open list
        while (OpenList.Count > 0)
        {   //Create a node and set it to the first item in the open list
            NodeV2 CurrentNode = OpenList[0];
            //Loop through the open list starting from the second object
            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            //Move the current node around the system for memory sake
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (NodeV2 NeighborNode in Grid.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.IsAWall || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.gCost + GetDistance(CurrentNode, NeighborNode);//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.gCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.hCost = GetDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.Parent = CurrentNode;//Set the parent of the node for retracing steps

                    if (!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    }
                }
            }

        }
    }



    void GetFinalPath(NodeV2 StartingNode, NodeV2 EndNode)
    {
        List<NodeV2> FinalPath = new List<NodeV2>();//List to hold the path sequentially 
        NodeV2 CurrentNode = EndNode;//Node to store the current node being checked
            
        //While loop to work through each node going through the parents to the beginning of the path
        while (CurrentNode != StartingNode)
        {
            //Add that node to the final path
            FinalPath.Add(CurrentNode);
            //Move onto its parent node
            CurrentNode = CurrentNode.Parent;
        }
        //Reverse the path to get the correct order
        FinalPath.Reverse();
        //Set the final path
        Grid.FinalPath = FinalPath;

    }
    //Returns distance between two node points
    int GetDistance(NodeV2 nodeA, NodeV2 nodeB)
    {
        int x = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int y = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        return x + y;//Return the sum
    }
}
