using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VectorSelector : MonoBehaviour
{
    public Camera mainCamera;

    public GameObject bgX;
    public GameObject bgY;
    public GameObject bgZ;

    public Vector3 cameraVector;
    public Vector3[] bgVectors;
    public float[] bgAngles;

    public Vector3 activeVector;
    public float nearestAngle;

    public ThrusterController thrConScript;

    // Start is called before the first frame update
    void Start()
    {
        thrConScript = gameObject.GetComponent<ThrusterController>();

        cameraVector = mainCamera.transform.forward;

        bgVectors = new Vector3[3];
        bgAngles = new float[3];
        bgVectors[0] = new Vector3(-1, 0, 0);
        bgVectors[1] = new Vector3(0, -1, 0);
        bgVectors[2] = new Vector3(0, 0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        cameraVector = mainCamera.transform.forward;
        
        FindActiveVector();
        if (activeVector == bgVectors[0])
        {
            thrConScript.x = true;
            thrConScript.y = false;
            thrConScript.z = false;
        } 
        else if (activeVector == bgVectors[1])
        {
            thrConScript.y = true;
            thrConScript.x = false;
            thrConScript.z = false;
        }
        else if (activeVector == bgVectors[2])
        {
            thrConScript.z = true;
            thrConScript.x = false;
            thrConScript.y = false;
        }
    }

    private void FindActiveVector()
    {
        activeVector = bgVectors[0];

        for (int i = 0; i < 3; i++)
        {
            bgAngles[i] = Vector3.Angle(cameraVector, bgVectors[i]);
        }

        GetSmallestAngle();

        if(nearestAngle == bgAngles[0])
        {
            activeVector = bgVectors[0];
        } 
        else if (nearestAngle == bgAngles[1])
        {
            activeVector = bgVectors[1];
        }
        else if (nearestAngle == bgAngles[2])
        {
            activeVector = bgVectors[2];
        }
    }

    public void GetSmallestAngle()
    {
        var differance = Mathf.Infinity;

        foreach (float curAngle in bgAngles)
        {
            if(Mathf.Abs(180 - curAngle) < differance)
            {
                differance = Mathf.Abs(180 - curAngle);
                nearestAngle = curAngle;
            }
            if (Mathf.Abs(360 - curAngle) < differance)
            {
                differance = Mathf.Abs(360 - curAngle);
                nearestAngle = curAngle;
            }
            if (Mathf.Abs(curAngle) < differance)
            {
                differance = Mathf.Abs(curAngle);
                nearestAngle = curAngle;
            }
        }
    }
}