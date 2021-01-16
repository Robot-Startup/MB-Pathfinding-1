using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Functions : MonoBehaviour
{
    public GameObject platformParent;
    public GameObject cubesParent;
    public LayerMask gameLayers;

    public void CreatePlatform(GameObject platformRef, Vector3 gridSize, Vector3 origin)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.z; z++)
            {
                GameObject obj = Instantiate(platformRef, origin + new Vector3(x, -1, z), Quaternion.identity);
                obj.transform.SetParent(platformParent.transform);
            }
        }
    }

    public void CreateGrid(Vector3 gridSize, Node[,,] grid)
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    grid[x, y, z] = new Node(true, new Vector3(x, y, z));
                }
            }
        }
    }
    
    // This place should have all of the rulesets to check if location is valid to be considered a neighbor (is touching something, etc)
    public List<Node> FindNeighbors(Node node, Node[,,] grid, Vector3 gridSize)
    {
        List<Node> neighbors = new List<Node>();
        Vector3 origin = node.worldPos;
        Vector3 originModified;

        if (!Physics.Raycast(origin, Vector3.up, 1f, gameLayers))
        {
            originModified = origin + Vector3.up;

            if (BoundaryTest(gridSize, originModified))
            {
                if (EmptyCheckY(true, originModified))
                {
                    neighbors.Add(grid[Convert.ToInt32(originModified.x), Convert.ToInt32(originModified.y), Convert.ToInt32(originModified.z)]);
                }
            }
        }

        if (!Physics.Raycast(origin, Vector3.down, 1f, gameLayers))
        {
            originModified = origin + Vector3.down;

            if (BoundaryTest(gridSize, originModified))
            {
                if (EmptyCheckY(false, originModified))
                {
                    neighbors.Add(grid[Convert.ToInt32(originModified.x), Convert.ToInt32(originModified.y), Convert.ToInt32(originModified.z)]);
                }
            }
        }

        if (!Physics.Raycast(origin, Vector3.left, 1f, gameLayers))
        {
            originModified = origin + Vector3.left;

            if (BoundaryTest(gridSize, originModified))
            {
                if (EmptyCheckX(false, originModified))
                {
                    neighbors.Add(grid[Convert.ToInt32(originModified.x), Convert.ToInt32(originModified.y), Convert.ToInt32(originModified.z)]);
                }
            }
        }

        if (!Physics.Raycast(origin, Vector3.right, 1f, gameLayers))
        {
            originModified = origin + Vector3.right;

            if (BoundaryTest(gridSize, originModified))
            {
                if (EmptyCheckX(true, originModified))
                {
                    neighbors.Add(grid[Convert.ToInt32(originModified.x), Convert.ToInt32(originModified.y), Convert.ToInt32(originModified.z)]);
                }
            }
        }

        if (!Physics.Raycast(origin, Vector3.forward, 1f, gameLayers))
        {
            originModified = origin + Vector3.forward;

            if (BoundaryTest(gridSize, originModified))
            {
                if (EmptyCheckZ(true, originModified))
                {
                    neighbors.Add(grid[Convert.ToInt32(originModified.x), Convert.ToInt32(originModified.y), Convert.ToInt32(originModified.z)]);
                }
            }
        }

        if (!Physics.Raycast(origin, Vector3.back, 1f, gameLayers))
        {
            originModified = origin + Vector3.back;

            if (BoundaryTest(gridSize, originModified))
            {
                if (EmptyCheckZ(false, originModified))
                {
                    neighbors.Add(grid[Convert.ToInt32(originModified.x), Convert.ToInt32(originModified.y), Convert.ToInt32(originModified.z)]);
                }
            }
        }

        return neighbors;
    }

    public bool BoundaryTest(Vector3 gridSize, Vector3 location)
    {
        bool boolean = true;

        if (gridSize.x <= location.x || location.x < 0)
        {
            boolean = false;
        }

        else if (gridSize.y <= location.y || location.y < 0)
        {
            boolean = false;
        }

        else if (gridSize.z <= location.z || location.z < 0)
        {
            boolean = false;
        }

        return boolean;
    }

    public int CalculateDistance(Node nodeA, Node nodeB)
    {
        Vector3 positionA = nodeA.worldPos;
        Vector3 positionB = nodeB.worldPos;

        int distX = Mathf.RoundToInt(Mathf.Abs(positionA.x - positionB.x));
        int distY = Mathf.RoundToInt(Mathf.Abs(positionA.y - positionB.y));
        int distZ = Mathf.RoundToInt(Mathf.Abs(positionA.z - positionB.z));

        return distX + distY + distZ;
    }

    public void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        GetComponent<WorldGrid>().path = path;
    }

    public Vector3 Vector3ToGrid(RaycastHit raycastHit)
    {
        Vector3 rawPosition = raycastHit.point - raycastHit.transform.position;
        Vector3 truePosition = new Vector3(0, 0, 0);

        if (rawPosition.x > 0)
        {
            truePosition.x = Mathf.Floor(rawPosition.x + 0.5f);
        }

        else if (rawPosition.x < 0)
        {
            truePosition.x = Mathf.Floor(rawPosition.x * -1 + 0.5f) * -1;
        }

        if (rawPosition.y > 0)
        {
            truePosition.y = Mathf.Floor(rawPosition.y + 0.5f);
        }

        else if (rawPosition.y < 0)
        {
            truePosition.y = Mathf.Floor(rawPosition.y * -1 + 0.5f) * -1;
        }

        if (rawPosition.z > 0)
        {
            truePosition.z = Mathf.Floor(rawPosition.z + 0.5f);
        }

        else if (rawPosition.z < 0)
        {
            truePosition.z = Mathf.Floor(rawPosition.z * -1 + 0.5f) * -1;
        }

        truePosition += raycastHit.transform.position;
        return truePosition;
    }

    public bool EmptyCheckY(bool up, Vector3 position)
    {
        if (up)
        {
            if (Physics.Raycast(position, Vector3.left, 1f))
            {
                if (Physics.Raycast(position + Vector3.left, Vector3.down, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.right, 1f))
            {
                if (Physics.Raycast(position + Vector3.right, Vector3.down, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.forward, 1f))
            {
                if (Physics.Raycast(position + Vector3.forward, Vector3.down, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.back, 1f))
            {
                if (Physics.Raycast(position + Vector3.back, Vector3.down, 1f))
                {
                    return true;
                }
            }

            return false;
        }

        else
        {
            if (Physics.Raycast(position, Vector3.left, 1f))
            {
                if (Physics.Raycast(position + Vector3.left, Vector3.up, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.right, 1f))
            {
                if (Physics.Raycast(position + Vector3.right, Vector3.up, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.forward, 1f))
            {
                if (Physics.Raycast(position + Vector3.forward, Vector3.up, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.back, 1f))
            {
                if (Physics.Raycast(position + Vector3.back, Vector3.up, 1f))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public bool EmptyCheckX(bool right, Vector3 position)
    {
        if (right)
        {
            if (Physics.Raycast(position, Vector3.down, 1f))
            {
                if (Physics.Raycast(position + Vector3.down, Vector3.left, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.up, 1f))
            {
                if (Physics.Raycast(position + Vector3.up, Vector3.left, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.forward, 1f))
            {
                if (Physics.Raycast(position + Vector3.forward, Vector3.left, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.back, 1f))
            {
                if (Physics.Raycast(position + Vector3.back, Vector3.left, 1f))
                {
                    return true;
                }
            }

            return false;
        }

        else
        {
            if (Physics.Raycast(position, Vector3.down, 1f))
            {
                if (Physics.Raycast(position + Vector3.down, Vector3.right, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.up, 1f))
            {
                if (Physics.Raycast(position + Vector3.up, Vector3.right, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.forward, 1f))
            {
                if (Physics.Raycast(position + Vector3.forward, Vector3.right, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.back, 1f))
            {
                if (Physics.Raycast(position + Vector3.back, Vector3.right, 1f))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public bool EmptyCheckZ(bool forward, Vector3 position)
    {
        if (forward)
        {
            if (Physics.Raycast(position, Vector3.down, 1f))
            {
                if (Physics.Raycast(position + Vector3.down, Vector3.back, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.left, 1f))
            {
                if (Physics.Raycast(position + Vector3.left, Vector3.back, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.right, 1f))
            {
                if (Physics.Raycast(position + Vector3.right, Vector3.back, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.up, 1f))
            {
                if (Physics.Raycast(position + Vector3.up, Vector3.back, 1f))
                {
                    return true;
                }
            }

            return false;
        }

        else
        {
            if (Physics.Raycast(position, Vector3.down, 1f))
            {
                if (Physics.Raycast(position + Vector3.down, Vector3.forward, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.left, 1f))
            {
                if (Physics.Raycast(position + Vector3.left, Vector3.forward, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.right, 1f))
            {
                if (Physics.Raycast(position + Vector3.right, Vector3.forward, 1f))
                {
                    return true;
                }
            }

            if (Physics.Raycast(position, Vector3.up, 1f))
            {
                if (Physics.Raycast(position + Vector3.up, Vector3.forward, 1f))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
