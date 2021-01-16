using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject cubeRef;
    public GameObject cube;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            LayerMask gameLayers = GetComponent<Functions>().gameLayers;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, GetComponent<Translate>().maxDistance, gameLayers))
            {
                Vector3Int position = Vector3Int.RoundToInt(GetComponent<Functions>().Vector3ToGrid(raycastHit));

                if (GetComponent<Functions>().BoundaryTest(GetComponent<WorldGrid>().gridSize, position))
                {
                    cube = Instantiate(cubeRef, position, Quaternion.identity);
                    cube.transform.SetParent(GetComponent<Functions>().cubesParent.transform);
                    GetComponent<WorldGrid>().grid[position.x, position.y, position.z].walkable = false;
                    GetComponent<Translate>().startNode = GetComponent<WorldGrid>().grid[position.x, position.y, position.z];
                }
            }
        }
    }
}
