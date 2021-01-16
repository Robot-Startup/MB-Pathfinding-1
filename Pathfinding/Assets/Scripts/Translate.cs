using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Translate : MonoBehaviour
{
    public KeyCode translateKey;
    public float maxDistance;
    RaycastHit raycastHit;
    public Node startNode;
    Pathfinding pathfinding;

    private void Awake()
    {
        pathfinding = GetComponent<Pathfinding>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(translateKey))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, maxDistance, GetComponent<Functions>().gameLayers))
            {
                Vector3Int position = Vector3Int.RoundToInt(GetComponent<Functions>().Vector3ToGrid(raycastHit));
                if (GetComponent<Functions>().BoundaryTest(GetComponent<WorldGrid>().gridSize, position))
                {
                    if (startNode != null)
                    {
                        print("started pathfinding");
                        pathfinding.FindPath(startNode, position);
                    }
                }
            }
        }
    }
}
