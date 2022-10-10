using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public RectTransform selectionBox;
    private Vector2 mouseStartPos;
    private Vector2 mouseEndPos;

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

    public Selected selected;

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
        if (Input.GetMouseButtonDown(0))
        {
            mouseStartPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;
                if (selection.CompareTag("Selectable") == true)
                {
                    selected = selection.GetComponent<Selected>();
                    if (!selected)
                    {
                        for (int i = 0; i < cubesScripts.Length; i++)
                        {
                            cubesScripts[i].isSelected = false;
                        }
                    }

                    if (selection != null)
                    {
                        for (int i = 0; i < cubesScripts.Length; i++)
                        {
                            cubesScripts[i].isSelected = false;
                        }
                        selected.isSelected = true;

                    }
                    else
                    {
                        selected.isSelected = false;
                    }
                }
                else
                {
                    for (int i = 0; i < cubesScripts.Length; i++)
                    {
                        cubesScripts[i].isSelected = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < cubesScripts.Length; i++)
                {
                    cubesScripts[i].isSelected = false;
                }
            }

            ReleaseSelectionBox();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }


        if(x)
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
        if (y)
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
        if (z)
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

        for (int i = 0; i < cubesScripts.Length; i++)
        {
            thrusterScripts[i].nearestObject = nearestObject;
            if(cubesScripts[i].isSelected)
            {
                activeCubes[i] = cubes[i];
            } else
            {
                activeCubes[i] = null;
            }
        }


        FindNearestObject();

    }

    void UpdateSelectionBox(Vector3 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }

        float width = curMousePos.x - mouseStartPos.x;
        float height = curMousePos.y - mouseStartPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = mouseStartPos + new Vector2(width / 2, height / 2);
    }

    void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        for (int i = 0; i < cubesScripts.Length; i++)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(cubesScripts[i].gameObject.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                cubesScripts[i].isSelected = true;
            }
        }
    }

    private void FindNearestObject()
    {
        min = Mathf.Infinity;
        nearestObject = null;

        foreach (GameObject curObject in activeCubes)
        {
            if(curObject != null)
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