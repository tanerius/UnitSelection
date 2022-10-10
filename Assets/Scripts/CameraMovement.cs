using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float speed;
    public Vector2 turn;
    public float sensibility = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(Input.GetKey("w"))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (Input.GetKey("e"))
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        if (Input.GetKey("q"))
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }

        if (Input.anyKey && !Input.GetMouseButton(0))
        {
            turn.x += Input.GetAxis("Mouse X") * sensibility;
            turn.y += Input.GetAxis("Mouse Y") * sensibility;
            transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
        }
    }
}