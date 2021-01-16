using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;
    public int rotationScale;
    public int minXRot;
    public int maxXRot;

    private void Awake()
    {
        this.transform.position = new Vector3(FindObjectOfType<WorldGrid>().gridSize.x / 2 - 0.5f, 5, FindObjectOfType<WorldGrid>().gridSize.z / 2 - 0.5f);
        this.transform.eulerAngles = new Vector3(Mathf.Clamp(this.transform.eulerAngles.x, minXRot, maxXRot), this.transform.eulerAngles.y, this.transform.eulerAngles.z);
    }

    private void Update()
    {
        if (Input.GetKey(up))
        {
            this.transform.eulerAngles = new Vector3(Mathf.Clamp(this.transform.eulerAngles.x + rotationScale * Time.deltaTime, minXRot, maxXRot), this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        }

        else if (Input.GetKey(down))
        {
            this.transform.eulerAngles = new Vector3(Mathf.Clamp(this.transform.eulerAngles.x - rotationScale * Time.deltaTime, minXRot, maxXRot), this.transform.eulerAngles.y, this.transform.eulerAngles.z);
        }

        if (Input.GetKey(right))
        {
            this.transform.eulerAngles += new Vector3(0, rotationScale * Time.deltaTime, 0);
        }

        else if (Input.GetKey(left))
        {
            this.transform.eulerAngles -= new Vector3(0, rotationScale * Time.deltaTime, 0);
        }
    }
}
