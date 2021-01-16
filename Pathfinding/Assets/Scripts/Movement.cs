using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Functions functions;
    Placement placement;
    WorldGrid grid;

    public float initialTimer;
    float currentTimer;
    float timePercent;
    int index = 0;

    public Vector3 tempPos;

    private void Awake()
    {
        functions = GetComponent<Functions>();
        placement = GetComponent<Placement>();
        grid = GetComponent<WorldGrid>();
        currentTimer = initialTimer;
    }
    public void MoveCube()
    {
        placement.cube.transform.position = Vector3.Lerp(tempPos, grid.path[index].worldPos, Timer());

        if (placement.cube.transform.position == grid.path[index].worldPos)
        {
            placement.cube.transform.position = grid.path[index].worldPos;
            tempPos = placement.cube.transform.position;
            currentTimer = initialTimer;
            index++;
        }

        if (placement.cube.transform.position != grid.path[grid.path.Count - 1].worldPos)
        {
            Invoke("MoveCube", Time.deltaTime);
        }

        else
        {
            print("FINISHED");
        }
    }

    private float Timer()
    {
        currentTimer = Mathf.Clamp(currentTimer - Time.deltaTime, 0, initialTimer);
        timePercent = 1 - (currentTimer / initialTimer);

        return timePercent;

    }
}
