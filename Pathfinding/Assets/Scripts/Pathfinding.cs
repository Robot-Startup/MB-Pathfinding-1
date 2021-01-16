using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    WorldGrid grid;
    Functions functions;
    private void Awake()
    {
        grid = GetComponent<WorldGrid>();
        functions = GetComponent<Functions>();
    }

    public void FindPath(Node startNode, Vector3 destination)
    {
        Node destinationNode = GetComponent<WorldGrid>().grid[Convert.ToInt32(destination.x), Convert.ToInt32(destination.y), Convert.ToInt32(destination.z)];
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == destinationNode)
            {
                functions.RetracePath(startNode, destinationNode);
                GetComponent<Movement>().tempPos = GetComponent<Placement>().cube.transform.position;
                GetComponent<Movement>().MoveCube();
                return;
            }

            foreach (Node neighbor in functions.FindNeighbors(currentNode, grid.grid, grid.gridSize))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int movementCost = currentNode.gCost + functions.CalculateDistance(currentNode, neighbor);
                
                if (movementCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = movementCost;
                    neighbor.hCost = functions.CalculateDistance(destinationNode, neighbor);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }
}
