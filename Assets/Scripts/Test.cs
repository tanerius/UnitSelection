using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3 rotPos;
    public bool rotationDone = false;

    // Start is called before the first frame update
    void Start()
    {
        rotPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < rotPos.y + 1.45f && !rotationDone)
        {
            transform.RotateAround(rotPos, new Vector3(0, 0, 1), 200 * Time.deltaTime);
        }
        else
        {
            rotationDone = true;
            transform.position = Vector3.Lerp(transform.position, new Vector3(rotPos.x -10, rotPos.y - 2, rotPos.z), 4 * Time.deltaTime);

            if (transform.position.x < rotPos.x - 2)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y + 270, transform.rotation.z), 7 * Time.deltaTime);
            }
        }
    }
}