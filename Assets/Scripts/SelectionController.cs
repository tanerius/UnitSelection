using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : MonoBehaviour
{
    public RectTransform selectionBox;
    private Vector2 mouseStartPos;
    private Vector2 mouseEndPos;

    public Camera mainCamera;

    public GameObject[] cubes;

    public Selected selected;
    public int numberOfCubes;

    // Start is called before the first frame update
    void Start()
    {
        cubes = GameObject.FindGameObjectsWithTag("Selectable");
        numberOfCubes = cubes.Length;
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0))
       {
            mouseStartPos = Input.mousePosition;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var selection = hit.transform;
                if(selection.CompareTag("Selectable") == true)
                {
                     selected = selection.GetComponent<Selected>();
                    if (!selected)
                    {
                        for (int i = 0; i < numberOfCubes; i++)
                        {
                            cubes[i].GetComponent<Selected>().isSelected = false;
                        }
                    }

                    if (selection != null)
                    {
                        if (selected.isSelected)
                        {
                            selected.isSelected = false;
                        }
                        else
                        {
                            selected.isSelected = true;
                        }
                    }
                    else
                    {
                        selected.isSelected = false;
                    }
                }
                else
                {
                    for (int i = 0; i < numberOfCubes; i++)
                    {
                        cubes[i].GetComponent<Selected>().isSelected = false;
                    }
                }
            }
       }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
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

        for (int i = 0; i < numberOfCubes; i++)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(cubes[i].gameObject.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                cubes[i].GetComponent<Selected>().isSelected = true;
            }
        }
    }
}