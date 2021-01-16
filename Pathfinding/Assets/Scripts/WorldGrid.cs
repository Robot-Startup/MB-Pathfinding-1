using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldGrid : MonoBehaviour
{
    public GameObject platform;
    public GameObject cube;
    public Vector3 origin;
    public Vector3 gridSize;
    public Node[,,] grid;
    public List<Node> path;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3 (gridSize.x / 2 - 0.5f, gridSize.y / 2 - 0.5f, gridSize.z / 2 - 0.5f), gridSize);

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }
            }
        }    
    }

    private void Awake()
    {
        grid = new Node[Convert.ToInt32(gridSize.x), Convert.ToInt32(gridSize.y), Convert.ToInt32(gridSize.z)];
        Functions functions = GetComponent<Functions>();
        functions.CreatePlatform(platform, gridSize, origin);
        functions.CreateGrid(gridSize, grid);
    }
}
