using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterController : MonoBehaviour
{
    public Camera mainCamera;

    public Selected[] cubesScripts;
    public Thruster[] thrusterScripts;

    public bool x;
    public bool y;
    public bool z;

    public GameObject bgX;
    public GameObject bgY;
    public GameObject bgZ;

    public float[] distances;

    public float min = 1000;

    public GameObject nearestObject;

    public GameObject[] cubes;
    public GameObject[] activeCubes;

    // Start is called before the first frame update
    void Start()
    {
        cubes = GameObject.FindGameObjectsWithTag("Selectable");
        if (cubes != null)
        {
            cubesScripts = new Selected[cubes.Length];
            thrusterScripts = new Thruster[cubes.Length];
            distances = new float[cubes.Length];
            activeCubes = new GameObject[cubes.Length];
            for (int i = 0; i < cubes.Length; i++)
            {
                cubesScripts[i] = cubes[i].GetComponent<Selected>();
                thrusterScripts[i] = cubes[i].GetComponent<Thruster>();
                thrusterScripts[i].nearestObject = nearestObject;
            }

            FindNearestObject();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (x)
        {
            for (int i = 0; i < cubesScripts.Length; i++)
            {
                thrusterScripts[i].xView = true;
                thrusterScripts[i].yView = false;
                thrusterScripts[i].zView = false;
            }

            bgX.SetActive(true);
            bgY.SetActive(false);
            bgZ.SetActive(false);
        }
        else if (y)
        {
            for (int i = 0; i < cubesScripts.Length; i++)
            {
                thrusterScripts[i].yView = true;
                thrusterScripts[i].xView = false;
                thrusterScripts[i].zView = false;
            }

            bgY.SetActive(true);
            bgX.SetActive(false);
            bgZ.SetActive(false);
        }
        else if (z)
        {
            for (int i = 0; i < cubesScripts.Length; i++)
            {
                thrusterScripts[i].zView = true;
                thrusterScripts[i].xView = false;
                thrusterScripts[i].yView = false;
            }

            bgZ.SetActive(true);
            bgX.SetActive(false);
            bgY.SetActive(false);
        }

        FindNearestObject();

        for (int i = 0; i < cubesScripts.Length; i++)
        {
            thrusterScripts[i].nearestObject = nearestObject;
            if (cubesScripts[i].isSelected)
            {
                activeCubes[i] = cubes[i];
            }
            else
            {
                activeCubes[i] = null;
            }
        }
    }

    private void FindNearestObject()
    {
        min = Mathf.Infinity;
        nearestObject = null;

        foreach (GameObject curObject in activeCubes)
        {
            if (curObject != null)
            {
                var distance = (curObject.transform.position - mainCamera.transform.position).sqrMagnitude;
                if (distance < min)
                {
                    min = distance;
                    nearestObject = curObject;
                }
            }
        }
    }
}